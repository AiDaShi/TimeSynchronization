using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TimeSynchronization.GlobleLogger;

namespace TimeSynchronization.TimeForms
{
    public partial class SettingServer : Form
    {
        public SettingServer()
        {
            InitializeComponent();
            // 不安全跨线程访问
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        string TheIpOld;
        string ThePortOld;
        private void SettingServer_Load(object sender, EventArgs e)
        {
            TheIpOld = ServerVali.IP;
            ThePortOld = ServerVali.Port.ToString();
            IpText.Text = TheIpOld;
            PortText.Text = ThePortOld;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IpText.Text.Length<=0)
            {
                MessageBox.Show("请从新填写Ip！");
                IpText.Text = TheIpOld;
                PortText.Text = ThePortOld;
                return;
            }
            if (PortText.Text.Length <= 0)
            {
                MessageBox.Show("请从新填写Port！");
                IpText.Text = TheIpOld;
                PortText.Text = ThePortOld;
                return;
            }
            int portout;
            if (!int.TryParse(PortText.Text,out portout))
            {
                MessageBox.Show("请输入正确的Port！");
                IpText.Text = TheIpOld;
                PortText.Text = ThePortOld;
                return;
            }

            ServerVali.IP = IpText.Text;
            ServerVali.Port = portout;
            MessageBox.Show("保存成功！");
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
