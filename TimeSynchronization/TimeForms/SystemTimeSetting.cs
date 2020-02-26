using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSynchronization.SetTimeTools;

namespace TimeSynchronization.TimeForms
{
    public partial class SystemTimeSetting : Form
    {
        public SystemTimeSetting()
        {
            InitializeComponent();
            // 不安全跨线程访问
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private int mYear;
        private int mMonth;
        private int mDay;
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mYear = monthCalendar1.SelectionRange.Start.Year;
            mMonth = monthCalendar1.SelectionRange.Start.Month;
            mDay = monthCalendar1.SelectionRange.Start.Day;

            SYSTEMTIME t = new SYSTEMTIME();
            t.Year = (short)mYear;
            t.Month = (short)mMonth;
            t.Day = (short)mDay;
            t.Hour = (short)(dateTimePicker1.Value.Hour-8);
            t.Milliseconds = (short)dateTimePicker1.Value.Millisecond;
            t.Second = (short)dateTimePicker1.Value.Second;
            bool v = NewTimeWin32.SetSystemTime(ref t);
            if (v)
            {
                MessageBox.Show("Success!");
            }
            else
            {
                MessageBox.Show("Faild!");
            }

        }

        private void SystemTimeSetting_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => {
                try
                {
                    while (true)
                    {
                        NowTimeShowLable.Text = DateTime.Now.ToString();
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            mYear = e.Start.Year;
            mMonth = e.Start.Month;
            mDay = e.Start.Day;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
