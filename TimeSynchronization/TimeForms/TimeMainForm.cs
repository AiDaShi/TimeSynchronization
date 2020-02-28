using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSynchronization.GlobleLogger;
using TimeSynchronization.SocketTaskTools;
using TimeSynchronization.SocketTaskTools.GLB;

namespace TimeSynchronization.TimeForms
{
    public partial class TimeMainForm : Form
    {
        public TimeMainForm()
        {
            InitializeComponent();
            // 不安全跨线程访问
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public IOCPServer server;
        private void StartServerBtn_Click(object sender, EventArgs e)
        {
            if ((TheServerStartEnum)this.StartServerBtn.Tag == TheServerStartEnum.Stop)
            {
                #region 启动服务器
                try
                {
                    MethodStartServer();
                }
                catch (Exception ex)
                {
                    SerilogHelper.FileLog(Outputoption.Error, ex.Message);
                }
                #endregion
                this.StartServerBtn.BackColor = System.Drawing.Color.LightGreen;
                this.StartServerBtn.Text = "Stop";
                this.StartServerBtn.Tag = TheServerStartEnum.Start;

            }
            else
            {
                #region 停止服务器
                try
                {
                    MethodStopServer();
                }
                catch (Exception ex)
                {
                    SerilogHelper.FileLog(Outputoption.Error, ex.Message);
                }
                #endregion
                this.StartServerBtn.BackColor = System.Drawing.Color.Red;
                this.StartServerBtn.Text = "Start";
                this.StartServerBtn.Tag = TheServerStartEnum.Stop;
            }
        }
        private void MethodStopServer()
        {
            if (server != null)
            {
                Overall.lsocket.Clear();
                server.Stop();
                server.Dispose();
                //this.GetMsgList.Items.Add("The Server is Close...");
                SerilogHelper.Log4Debug("The server is Close ....", Outputoption.Info);
                this.SettingBtn.Enabled = true;
            }
            Overall.ListIP.Clear();
        }

        private void MethodStartServer() 
        {
            try
            {
                string HereIp = ServerVali.IP;
                int HereProt = ServerVali.Port;
                server = new IOCPServer(IPAddress.Parse(HereIp),HereProt, 1024);
                server.Start();
                SerilogHelper.Log4Debug("The Server is Start ....", Outputoption.Info);
                this.SettingBtn.Enabled = false;
            }
            catch (Exception ex)
            {
                SerilogHelper.Log4Debug(ex.Message, Outputoption.Error);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.SettingBtn.Enabled = true;
            }
        }

        private void TimeMainForm_Load(object sender, EventArgs e)
        {
            ServerVali.IP = ConfigurationManager.ConnectionStrings["Ip"].ConnectionString;
            ServerVali.FilesExplot = ConfigurationManager.ConnectionStrings["FilesExplot"].ConnectionString;
            int portout;
            //默认端口 4850
            ServerVali.Port = 4850;
            if (int.TryParse(ConfigurationManager.ConnectionStrings["Port"].ConnectionString, out portout))
            {
                ServerVali.Port = portout;
            }
            this.StartServerBtn.Tag = TheServerStartEnum.Stop;
            this.StartServerBtn.BackColor = System.Drawing.Color.Red;
            this.StartServerBtn.Text = "Start";
        }

        private void SettingBtn_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                SettingServer settingServer = new SettingServer();
                settingServer.ShowDialog();
            });
        }

        private void ChildName_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                ChildForm settingServer = new ChildForm();
                settingServer.ShowDialog();
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                SystemTimeSetting settingTime = new SystemTimeSetting();
                settingTime.ShowDialog();
            });
        }
    }
}
