using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSynchronization.Excel;
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

                    SerilogHelper.Log4Debug(item.IpAddressInfo + " happened one Error . Msg: " + ex.Message, Outputoption.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                if (this.dataGridView1.Rows.Count > 0)
                {

                    //OpenFileDialog fileDialog = new OpenFileDialog();
                    ////fileDialog.Filter = "Excel(97-2003)|*.xls|Excel(2007-2013)|*.xlsx";
                    //fileDialog.Filter = "Excel|*.xls|Excel|*.xlsx";
                    //if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    //{
                    //    return;
                    //}
                    //if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    //{
                    //    return;
                    //}
                    if (Directory.Exists(ServerVali.FilesExplot))
                    {
                        string onefile = ServerVali.FilesExplot + "\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        File.Create(onefile).Close();
                        ExcelHelper.ExportToExcel(this.dataGridView1.ToDataTable(), onefile);
                    }//Run
                }
            });
        }
    }
}
