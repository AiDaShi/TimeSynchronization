using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSynchronization.SocketTaskTools.GLB;

namespace TimeSynchronization.GlobleLogger
{
    public static class SerilogHelper
    {
        public static void FileLog(Outputoption outputoption, string msg)
        {
            switch (outputoption)
            {
                case Outputoption.Info:
                    Log.Logger.Information(msg);
                    break;
                case Outputoption.Debug:
                    Log.Logger.Debug(msg);
                    break;
                case Outputoption.Error:
                    Log.Logger.Error(msg);
                    break;
                case Outputoption.Warning:
                    Log.Logger.Warning(msg);
                    break;
                case Outputoption.Verbose:
                    Log.Logger.Verbose(msg);
                    break;
                default:
                    break;
            }
        }


        public static void Log4Debug(string msg, Outputoption outputoption, string ServerOrClient = "server", string Class = "None", string Method = "None")
        {
            try
            {
                FileLog(outputoption, msg);
                lock (Overall.obj1)
                {
                    FormLog(msg);
                }
            }
            catch (Exception ex)
            {
                Log4Debug(ex.Message, Outputoption.Error);
            }
        }
        public static void FormLog(string msg) 
        {
            if (Overall.OverAllForm.LogList.Items.Count>50)
            {
                Overall.OverAllForm.LogList.Items.Clear();
            }
            Overall.OverAllForm.LogList.Items.Add(msg);
        }
    }
}
