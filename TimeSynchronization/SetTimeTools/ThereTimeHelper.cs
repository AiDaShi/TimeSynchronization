using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSynchronization.SetTimeTools
{
    public class ThereTimeHelper
    {

        /// <summary>
        /// 设置系统时间   针对于旧Windows系统，如Windows XP,win2003,win2008
        /// </summary>
        private void setSystemDate(DateTime currentTime)
        {
            SystemTime sysTime = new SystemTime();
            sysTime.year = Convert.ToUInt16(currentTime.Year);
            sysTime.month = Convert.ToUInt16(currentTime.Month);
            sysTime.day = Convert.ToUInt16(currentTime.Day);
            sysTime.dayOfWeek = Convert.ToUInt16(currentTime.DayOfWeek);
            sysTime.minute = Convert.ToUInt16(currentTime.Minute);
            sysTime.second = Convert.ToUInt16(currentTime.Second);
            sysTime.milliseconds = Convert.ToUInt16(currentTime.Millisecond);

            //SetSystemTime()默认设置的为UTC时间，设定时比北京时间多了8个小时。  
            //int nBeijingHour = currentTime.Hour - 8
            int nBeijingHour = currentTime.Hour;
            if (nBeijingHour < 0)
            {
                nBeijingHour = 24;
                sysTime.day = Convert.ToUInt16(currentTime.Day - 1);
                sysTime.dayOfWeek = Convert.ToUInt16(currentTime.DayOfWeek - 1);
            }
            else
            {
                sysTime.day = Convert.ToUInt16(currentTime.Day);
                sysTime.dayOfWeek = Convert.ToUInt16(currentTime.DayOfWeek);
            }
            sysTime.hour = Convert.ToUInt16(nBeijingHour);
            //设置系统时间
            //SetSystemTime(ref sysTime);
            TimeWin32.SetLocalTime(ref sysTime);
        }
    }
}
