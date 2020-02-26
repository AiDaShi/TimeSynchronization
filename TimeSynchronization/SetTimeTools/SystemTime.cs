using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TimeSynchronization.SetTimeTools
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        [MarshalAs(UnmanagedType.U2)]
        internal ushort year; // 年
        [MarshalAs(UnmanagedType.U2)]
        internal ushort month; // 月
        [MarshalAs(UnmanagedType.U2)]
        internal ushort dayOfWeek; // 星期
        [MarshalAs(UnmanagedType.U2)]
        internal ushort day; // 日
        [MarshalAs(UnmanagedType.U2)]
        internal ushort hour; // 时
        [MarshalAs(UnmanagedType.U2)]
        internal ushort minute; // 分
        [MarshalAs(UnmanagedType.U2)]
        internal ushort second; // 秒
        [MarshalAs(UnmanagedType.U2)]
        internal ushort milliseconds; // 毫秒
    }


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
