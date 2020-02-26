using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TimeSynchronization.SocketTaskTools.Model;
using TimeSynchronization.TimeForms;

namespace TimeSynchronization.SocketTaskTools.GLB
{
    public static class Overall
    {
        public static TimeMainForm OverAllForm;

        public static object obj1 = new object();
        public static bool watchBool = false;
        public static List<OrtherResult> ListIP = new List<OrtherResult>();

       public static Dictionary<string, SocketAsyncEventArgs> lsocketEventArgs = new Dictionary<string, SocketAsyncEventArgs>();
        public static Dictionary<string, Socket> lsocket = new Dictionary<string, Socket>();
        public static string HeartPackage => "*^*--^--*^*";
        public static string CryString => "$AiDaSi$";
    }
}
