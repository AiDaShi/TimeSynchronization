using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TimeSynchronizationClient.ClientTools
{
    public class client
    {

        public TcpClient _client;

        public int port;

        public IPAddress remote;

        public client(int port, IPAddress remote)
        {

            this.port = port;
            this.remote = remote;
        }

        public void connect()
        {
            this._client = new TcpClient();
            _client.Connect(remote, port);
        }
        public void disconnect()
        {
            _client.Close();
        }
        public void send(string msg)
        {
            byte[] data = Encoding.Default.GetBytes(msg);
            try
            {
                _client.GetStream().Write(data, 0, data.Length);
                Overall.OverAllForm.GetMsgList.Items.Add(msg);
            }
            catch (Exception exception)
            {
                Overall.OverAllForm.GetMsgList.Items.Add($"【Error】：{exception.Message}");
            }
        }
        public void Conntestparck()
        {
            byte[] data = Encoding.Default.GetBytes(Overall.HeardPackage);
            try
            {
                _client.GetStream().Write(data, 0, data.Length);
                Overall.OverAllForm.GetMsgList.Items.Add("【Success】：successful connection");

            }
            catch (Exception exception)
            {
                Overall.OverAllForm.GetMsgList.Items.Add("【Error】：connection failure");
            }
        }
    }
}
