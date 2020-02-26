using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSynchronization.SocketTaskTools.GLB;

namespace TimeSynchronization.SocketTaskTools.Handle
{
    public static class HandleWebServerDataTools
    {
        /// <summary>
        /// 处理内容
        /// </summary>
        /// <param name="fullname"></param>
        public static void TaskFinishConvertionHtml(string fullname,string ip)
        {
            //处理完整路径
            fullname = fullname.Replace("\0", "");
            //时间数据处理
            var OLFirst = Overall.ListIP.FirstOrDefault(x => x.IpAddressInfo == ip);
            if (OLFirst!=null)
            {
                DateTime dt;
                if (DateTime.TryParse(fullname, out dt))
                {
                    OLFirst.It_Is_Time = dt;
                    OLFirst.Difference_Time= (DateTime.Now-dt).TotalMilliseconds.ToString("0.000");
                }
            }
        }
    }
}
