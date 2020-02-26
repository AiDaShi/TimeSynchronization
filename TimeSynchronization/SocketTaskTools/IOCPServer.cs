using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using TimeSynchronization.SocketTaskTools.Model;
using TimeSynchronization.SocketTaskTools.GLB;
using TimeSynchronization.SocketTaskTools.Handle;
using Serilog;
using TimeSynchronization.GlobleLogger;

namespace TimeSynchronization.SocketTaskTools
{
    public class IOCPServer : IDisposable
    {

        string CurrentClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        const int opsToPreAlloc = 2;
        #region Fields
        /// <summary>
        /// 服务器程序允许的最大客户端连接数
        /// </summary>
        private int _maxClient;

        /// <summary>
        /// 监听Socket，用于接受客户端的连接请求
        /// </summary>
        private Socket _serverSock;

        /// <summary>
        /// 当前的连接的客户端数
        /// </summary>
        private int _clientCount;

        /// <summary>
        /// 用于每个I/O Socket操作的缓冲区大小
        /// </summary>
        private int _bufferSize = 1024;

        /// <summary>
        /// 信号量
        /// </summary>
        Semaphore _maxAcceptedClients;

        /// <summary>
        /// 缓冲区管理
        /// </summary>
        BufferManager _bufferManager;

        /// <summary>
        /// 对象池
        /// </summary>
        SocketAsyncEventArgsPool _objectPool;

        private bool disposed = false;

        #endregion

        #region Properties

        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// 监听的IP地址
        /// </summary>
        public IPAddress Address { get; private set; }


        /// <summary>
        /// 监听的端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 通信使用的编码
        /// </summary>
        public Encoding Encoding { get; set; }

        #endregion

        #region Ctors

        /// <summary>
        /// 异步IOCP SOCKET服务器
        /// </summary>
        /// <param name="listenPort">监听的端口</param>
        /// <param name="maxClient">最大的客户端数量</param>
        public IOCPServer(int listenPort, int maxClient)
            : this(IPAddress.Any, listenPort, maxClient)
        {
        }

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localEP">监听的终结点</param>
        /// <param name="maxClient">最大客户端数量</param>
        public IOCPServer(IPEndPoint localEP, int maxClient)
            : this(localEP.Address, localEP.Port, maxClient)
        {
        }

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localIPAddress">监听的IP地址</param>
        /// <param name="listenPort">监听的端口</param>
        /// <param name="maxClient">最大客户端数量</param>
        public IOCPServer(IPAddress localIPAddress, int listenPort, int maxClient)
        {
            this.Address = localIPAddress;
            this.Port = listenPort;
            this.Encoding = Encoding.Default;

            _maxClient = maxClient;

            _serverSock = new Socket(localIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _bufferManager = new BufferManager(_bufferSize * _maxClient * opsToPreAlloc, _bufferSize);
            _objectPool = new SocketAsyncEventArgsPool(_maxClient);

            _maxAcceptedClients = new Semaphore(_maxClient, _maxClient);
        }

        #endregion


        #region 初始化

        /// <summary>
        /// 初始化函数
        /// </summary>
        public void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds 
            // against memory fragmentation
            _bufferManager.InitBuffer();

            // preallocate pool of SocketAsyncEventArgs objects
            SocketAsyncEventArgs readWriteEventArg;

            for (int i = 0; i < _maxClient; i++)
            {
                //Pre-allocate a set of reusable SocketAsyncEventArgs
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                readWriteEventArg.UserToken = null;

                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
                _bufferManager.SetBuffer(readWriteEventArg);

                // add SocketAsyncEventArg to the pool
                _objectPool.Push(readWriteEventArg);
            }

        }

        #endregion

        #region Start
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (!IsRunning)
            {
                Init();
                IsRunning = true;
                IPEndPoint localEndPoint = new IPEndPoint(Address, Port);
                // 创建监听socket
                _serverSock = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //_serverSock.ReceiveBufferSize = _bufferSize;
                //_serverSock.SendBufferSize = _bufferSize;
                if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    // 配置监听socket为 dual-mode (IPv4 & IPv6) 
                    // 27 is equivalent to IPV6_V6ONLY socket option in the winsock snippet below,
                    _serverSock.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                    _serverSock.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
                }
                else
                {
                    _serverSock.Bind(localEndPoint);
                }
                // 开始监听
                _serverSock.Listen(this._maxClient);
                // 在监听Socket上投递一个接受请求。
                StartAccept(null);
            }
        }
        #endregion

        #region Stop

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _serverSock.Close();
                //TODO 关闭对所有客户端的连接
                _objectPool.Clear();
            }
        }

        #endregion


        #region Accept

        /// <summary>
        /// 从客户端开始接受一个连接操作
        /// </summary>
        private void StartAccept(SocketAsyncEventArgs asyniar)
        {
            if (asyniar == null)
            {
                asyniar = new SocketAsyncEventArgs();
                asyniar.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
            {
                //socket must be cleared since the context object is being reused
                asyniar.AcceptSocket = null;
            }
            _maxAcceptedClients.WaitOne();
            if (!_serverSock.AcceptAsync(asyniar))
            {
                ProcessAccept(asyniar);
                //如果I/O挂起等待异步则触发AcceptAsyn_Asyn_Completed事件
                //此时I/O操作同步完成，不会触发Asyn_Completed事件，所以指定BeginAccept()方法
            }
        }

        /// <summary>
        /// accept 操作完成时回调函数
        /// </summary>
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        /// <summary>
        /// 监听Socket接受处理
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {

            if (e.SocketError == SocketError.Success)
            {
                Socket s = e.AcceptSocket;//和客户端关联的socket
                if (s.Connected)
                {
                    try
                    {
                        Interlocked.Increment(ref _clientCount);//原子操作加1
                        SocketAsyncEventArgs asyniar = _objectPool.Pop();
                        asyniar.UserToken = s;

                        //存储链接客户端
                        string cstr = s.RemoteEndPoint.ToString();
                        //创建空时间
                        Overall.lsocket.Add(cstr, s);
                        Overall.ListIP.Add(new OrtherResult() { IpAddressInfo = cstr, It_Is_Time = null, Difference_Time = "0" });
                        //Overall.OverAllForm.comboBoxEdit1.Properties.Items.AddRange(new object[] { cstr });

                        //日志
                        Log4Debug(String.Format("Client {0} is Connected, Connected Count is {1}.", cstr, _clientCount), Outputoption.Info, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);

                        if (!s.ReceiveAsync(asyniar))//投递接收请求
                        {
                            ProcessReceive(asyniar);
                        }
                    }
                    catch (SocketException ex)
                    {
                        Log4Debug(String.Format("Get from Client {0} Data is, Exception Information： {1} 。", s.RemoteEndPoint, ex.ToString()), Outputoption.Info, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);
                        //TODO 异常处理
                    }
                    //投递下一个接受请求
                    StartAccept(e);
                }
            }
        }

        #endregion

        #region 发送数据


        /// <summary>  
        /// 对数据进行打包,然后再发送  
        /// </summary>  
        /// <param name="token"></param>  
        /// <param name="message"></param>  
        /// <returns></returns>  
        public bool SendMessage(Socket token, byte[] message)
        {
            bool isSuccess = false;
            if (token == null || !token.Connected)
                return isSuccess;

            try
            {
                //对要发送的消息,制定简单协议,头4字节指定包的大小,方便客户端接收(协议可以自己定)  
                byte[] buff = new byte[message.Length + 4];
                byte[] len = BitConverter.GetBytes(message.Length);
                Array.Copy(len, buff, 4);
                Array.Copy(message, 0, buff, 4, message.Length);
                //token.Socket.Send(buff);  //
                //新建异步发送对象, 发送消息  
                SocketAsyncEventArgs sendArg = new SocketAsyncEventArgs();
                sendArg.UserToken = token;
                sendArg.SetBuffer(buff, 0, buff.Length);  //将数据放置进去.  
                isSuccess = token.SendAsync(sendArg);
            }
            catch (Exception e)
            {
                Console.WriteLine("SendMessage - Error:" + e.Message);
            }
            return isSuccess;
        }


        /// <summary>
        /// 异步的发送数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="data"></param>
        public void Send(SocketAsyncEventArgs e, byte[] data)
        {

            e.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            if (e.SocketError == SocketError.Success)
            {
                Socket s = e.AcceptSocket;//e.UserToken as Socket;//e.AcceptSocket;//和客户端关联的socket
                if (s == null)
                {
                    Log4Debug("Communication anomaly！！！", Outputoption.Error, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);
                    return;
                }
                if (s.Connected)
                {

                    // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
                    _bufferManager.SetBuffer(e);
                    Array.Copy(data, 0, e.Buffer, 0, data.Length);//设置发送数据
                    e.SetBuffer(data, 0, data.Length); //设置发送数据
                    if (!s.SendAsync(e))//投递发送请求，这个函数有可能同步发送出去，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件
                    {
                        // 同步发送时处理发送完成事件
                        ProcessSend(e);
                    }
                    else
                    {
                        CloseClientSocket(e);
                    }
                }
            }
        }
        /// <summary>
        /// 同步的使用socket发送数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="timeout"></param>
        public void Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
        {
            socket.SendTimeout = 0;
            int startTickCount = Environment.TickCount;
            int sent = 0; // how many bytes is already sent
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                {
                    //throw new Exception("Timeout.");
                }
                try
                {
                    sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably full, wait and try again
                        Thread.Sleep(30);
                    }
                    else
                    {
                        throw ex; // any serious error occurr
                    }
                }
            } while (sent < size);
        }


        /// <summary>
        /// 发送完成时处理函数
        /// </summary>
        /// <param name="e">与发送完成操作相关联的SocketAsyncEventArg对象</param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket s = (Socket)e.UserToken;
                //TODO

            }
            else
            {
                CloseClientSocket(e);
            }
        }

        #endregion

        #region 接收数据


        /// <summary>
        ///接收完成时处理函数
        /// </summary>
        /// <param name="e">与接收完成操作相关联的SocketAsyncEventArg对象</param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)//if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                Socket s = (Socket)e.UserToken;
                // 检查远程主机是否关闭连接
                if (e.BytesTransferred > 0)
                {
                    //判断所有需接收的数据是否已经完成
                    if (s.Available == 0)
                    {
                        //从侦听者获取接收到的消息。 
                        //String received = Encoding.ASCII.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        //echo the data received back to the client
                        //e.SetBuffer(e.Offset, e.BytesTransferred);

                        byte[] data = new byte[e.BytesTransferred];

                        Array.Copy(e.Buffer, e.Offset, data, 0, data.Length);//从e.Buffer块中复制数据出来，保证它可重用

                        string info = Encoding.UTF8.GetString(data);
                        foreach (var item in Overall.lsocket)
                        {
                            if (item.Key == s.RemoteEndPoint.ToString() && item.Value.Connected == false)
                            {
                                Overall.lsocket[item.Key] = s;
                                break;
                            }
                        }

                        string CRYbuffString = GetentrysptionClass.GetEnTryString();

                        List<string> ls = Regex.Split(info, CRYbuffString).ToList();
                        foreach (var item in ls)
                        {
                            if (string.IsNullOrEmpty(item))
                            {
                                continue;
                            }
                            //是否是心跳包
                            if (item.Contains(Overall.HeartPackage))
                            {
                                HeartPacket(s.RemoteEndPoint.ToString());
                            }
                            else
                            {
                                //处理消息
                                HandleWebServerDataTools.TaskFinishConvertionHtml(item, s.RemoteEndPoint.ToString());
                                //ConvertQueue.queue.Enqueue(item);

                                //写个日志
                                Log4Debug(String.Format("Get from {0} Data is {1}", s.RemoteEndPoint.ToString(), item), Outputoption.Info, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);
                            }
                        }
                        //TODO 处理数据
                        //增加服务器接收的总字节数。
                    }

                    if (!s.ReceiveAsync(e))//为接收下一段数据，投递接收请求，这个函数有可能同步完成，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件
                    {
                        //同步接收时处理接收完成事件
                        ProcessReceive(e);
                    }
                }
                else
                {
                    //写个日志
                    Log4Debug(String.Format("{0} Disconnected..!", s.RemoteEndPoint.ToString()), Outputoption.Info, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);

                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void HeartPacket(string msg)
        {
            try
            {
                //Log4Debug(msg, Outputoption.Info, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                Log4Debug(ex.Message, Outputoption.Error, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion

        #region 回调函数

        /// <summary>
        /// 当Socket上的发送或接收请求被完成时，调用此函数
        /// </summary>
        /// <param name="sender">激发事件的对象</param>
        /// <param name="e">与发送或接收完成操作相关联的SocketAsyncEventArg对象</param>
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                // 确定刚刚完成的操作类型并调用关联的处理程序。
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Accept:
                        ProcessAccept(e);
                        break;
                    case SocketAsyncOperation.Receive:
                        ProcessReceive(e);
                        break;
                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (ArgumentException exception)
            {

                //如果客户端已关闭，则抛出，因此不必捕获。
                Log4Debug(exception.Message, Outputoption.Error, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion
        #region Close
        /// <summary>
        /// 关闭socket连接
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            Log4Debug(String.Format("Client {0} Disconnected!", ((Socket)e.UserToken).RemoteEndPoint.ToString()), Outputoption.Info, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);
            Socket s = e.UserToken as Socket;
            CloseClientSocket(s, e);
        }

        /// <summary>
        /// 关闭socket连接
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void CloseClientSocket(Socket s, SocketAsyncEventArgs e)
        {
            try
            {
                string NowCloseSocketInfo = s.RemoteEndPoint.ToString();
                //socket关闭时，
                foreach (var item in Overall.lsocket)
                {
                    if (item.ToString().Equals(NowCloseSocketInfo))
                    {
                        Overall.lsocket.Remove(NowCloseSocketInfo);
                        //Overall.OverAllForm.comboBoxEdit1.Properties.Items.Remove(item);
                        break;
                    }
                }
                //需要删除集合信息
                var nowlif = Overall.ListIP.FirstOrDefault(x => x.IpAddressInfo == NowCloseSocketInfo);
                if (nowlif!=null)
                {
                    Overall.ListIP.Remove(nowlif);
                }
            }
            catch (Exception ex)
            {
                Log4Debug(ex.Message, Outputoption.Error, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);
                return;
            }

            //以上附加
            try
            {
                s.Shutdown(SocketShutdown.Send);
            }
            catch (Exception ex)
            {
                //如果客户端已关闭，则抛出，因此不必捕获。
                Log4Debug(ex.Message, Outputoption.Error, Class: CurrentClassName, Method: MethodBase.GetCurrentMethod().Name);

            }
            finally
            {
                s.Close();

            }
            Interlocked.Decrement(ref _clientCount);
            _maxAcceptedClients.Release();
            _objectPool.Push(e);//SocketAsyncEventArg 对象被释放，压入可重用队列。

        }
        #endregion

        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release 
        /// both managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Stop();
                        if (_serverSock != null)
                        {
                            _serverSock = null;
                        }
                    }
                    catch (SocketException ex)
                    {
                        //TODO 事件
                    }
                }
                disposed = true;
            }
        }
        #endregion

        public void Log4Debug(string msg, Outputoption outputoption, string ServerOrClient = "server", string Class = "None", string Method = "None")
        {
            try
            {
                SerilogHelper.FileLog(outputoption, msg);
                lock (Overall.obj1)
                {
                    SerilogHelper.FormLog(msg);
                }
            }
            catch (Exception ex)
            {
                Log4Debug(ex.Message, Outputoption.Error, Class: CurrentClassName, Method: "Log4Debug");
            }
        }

    }
}
