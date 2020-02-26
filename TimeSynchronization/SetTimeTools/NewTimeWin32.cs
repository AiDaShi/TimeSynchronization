using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TimeSynchronization.SetTimeTools
{
    public static class NewTimeWin32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public extern static bool SetSystemTime(ref SYSTEMTIME sysTime);
    }
}
