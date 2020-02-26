using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSynchronization.SocketTaskTools.GLB;

namespace TimeSynchronization.SocketTaskTools.Model
{
    public static class GetentrysptionClass
    {
        public static string EnTryString;
        public static string GetEnTryString()
        {
            if (string.IsNullOrEmpty(EnTryString))
            {
                byte[] CRYbuff = Encoding.UTF8.GetBytes(Overall.CryString);
                CRYbuff = BitConverter.GetBytes(CRYbuff.Length);
                EnTryString = Encoding.UTF8.GetString(CRYbuff);
            }
            return EnTryString;
        }
    }
}
