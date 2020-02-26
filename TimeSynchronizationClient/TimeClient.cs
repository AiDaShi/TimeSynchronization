using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSynchronizationClient.ClientTools;
using TimeSynchronizationClient.SetTimeTools;
using TimeSynchronizationClient.TimeOption;

namespace TimeSynchronizationClient
{
    public partial class TimeClient : Form
    {
        public TimeClient()
        {
            InitializeComponent();
            // 不安全跨线程访问
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public delegate SocketError del();
        public delegate void MessageHandle(string msg);

        private void ListedButton_Click(object sender, EventArgs e)
        {
            string ip = this.IPContent.Text.Trim();
            string portS = this.ProtContent.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(portS))
            {
                MessageBox.Show("请输入端口和Ip", "提示");
                return;
            }
            else
            {
                SocketError socketError = Request.Connect(ip, int.Parse(portS));
                if (socketError == SocketError.Success)
                {

                    this.ListedButton.Enabled = false;
                    this.GetMsgList.Items.Add("已连接到主机 "+ ip+":"+ portS);
                    Request.OnReceiveData += Request_OnReceiveData;
                    Request.OnServerClosed += Request_OnServerClosed;
                    Request.StartHeartbeat();
                    bool isSent = Request.Send(Overall.HeardPackage);
                    Task.Factory.StartNew(() =>
                    {
                        TimeSetting.Start();
                    });
                }
            }
        }

        /// <summary>
        /// server 断开
        /// </summary>
        private void Request_OnServerClosed()
        {
            this.BeginInvoke(new MessageHandle(UpdateRece), "server 已断开");
            Request.Disconnect();
            //SocketError socketError= Request.TryConnect();
            //if (socketError == SocketError.Success) 
            //{
            //    this.BeginInvoke(new MessageHandle(UpdateRece), "已再次连接到server " + "\r\n");
            //}
            del del3 = new del(Request.TryConnect);
            IAsyncResult iar2 = del3.BeginInvoke(Connect2Server, del3);

        }

        private void Connect2Server(IAsyncResult ar)
        {
            this.BeginInvoke(new MessageHandle(UpdateRece), "已连接服务器");
        }
        private void UpdateRece(string msg)
        {
            DateTime time;
            if (DateTime.TryParse(msg, out time))
            {
                SYSTEMTIME t = new SYSTEMTIME();
                t.Year = (short)time.Year;
                t.Month = (short)time.Month;
                t.Day = (short)time.Day;
                t.Hour = (short)(time.Hour - 8);
                t.Milliseconds = (short)time.Millisecond;
                t.Second = (short)time.Second;
                bool v = NewTimeWin32.SetSystemTime(ref t);
                if (v)
                {
                    this.GetMsgList.Items.Add("The Time Is Success！Now Time Is:" + DateTime.Now);
                }
                else
                {
                    this.GetMsgList.Items.Add("The Time Is Faild！Now Time:Is" + DateTime.Now);
                }
            }
            else
            {
                this.GetMsgList.Items.Add("The Time Is Faild！Can't Convert Time Object");

            }


        }
        /// <summary>
        /// 收到数据
        /// </summary>
        /// <param name="message"></param>
        private void Request_OnReceiveData(byte[] message)
        {
            string msg = Encoding.UTF8.GetString(message);
            this.BeginInvoke(new MessageHandle(UpdateRece), msg);
        }
        private void TimeClient_Load(object sender, EventArgs e)
        {
            string ip = this.IPContent.Text.Trim();
            string portS = this.ProtContent.Text.Trim();
        }

    }
}