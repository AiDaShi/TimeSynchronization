using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TimeSynchronizationClient.SetTimeTools
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public short Year; // 年
        public short Month; // 月
        public short DayOfWeek; // 星期
        public short Day; // 日
        public short Hour; // 时
        public short Minute; // 分
        public short Second; // 秒
        public short Milliseconds; // 毫秒
    }
}
