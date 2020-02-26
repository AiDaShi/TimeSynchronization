using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSynchronization.GlobleLogger;
using TimeSynchronization.SocketTaskTools.GLB;

namespace TimeSynchronization.TimeForms
{
    public partial class ChildForm : Form
    {
        public ChildForm()
        {
            InitializeComponent();
            // 不安全跨线程访问
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void ChildForm_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = Overall.ListIP;

            Task.Factory.StartNew(() =>
            {
                this.dataGridView1.Refresh();
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //发送数据
            foreach (var item in Overall.ListIP)
            {
                try
                {
                    var one = Overall.lsocket.FirstOrDefault(x => x.Key == item.IpAddressInfo).Value;
                    if (one != null)
                    {
                        byte[] buff = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
                        Overall.OverAllForm.server.SendMessage(one, buff);
                        SerilogHelper.Log4Debug(item.IpAddressInfo + "Sync Time...", Outputoption.Info);
                    }
                    else
                    {
                        SerilogHelper.Log4Debug(item.IpAddressInfo + "Build Socket Faild！", Outputoption.Error);
                    }
                }
                catch (Exception ex)
                {

                    SerilogHelper.Log4Debug(item.IpAddressInfo + " happened one Error . Msg: "+ex.Message, Outputoption.Error);
                }
            }
        }
    }
}
