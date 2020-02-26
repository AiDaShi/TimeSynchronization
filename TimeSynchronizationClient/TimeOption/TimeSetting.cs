using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeSynchronizationClient.ClientTools;

namespace TimeSynchronizationClient.TimeOption
{
    public static class TimeSetting
    {
        public static bool IsStart = false;
        public static int secound = 10;
        public static void Start()
        {
            try
            {
                IsStart = true;
                Overall.OverAllForm.GetMsgList.Items.Add("时间发送启动");
                while (IsStart)
                {
                    Thread.Sleep(secound * 1000);
                    bool isSent = Request.Send(DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //TODO
                Overall.OverAllForm.GetMsgList.Items.Add("时间发送中止");
                IsStart = false;
            }
        }
        public static void Stop() 
        {
            IsStart = false;
        }

    }
}
