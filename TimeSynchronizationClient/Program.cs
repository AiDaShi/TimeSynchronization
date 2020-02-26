using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSynchronizationClient.DumpTools;

namespace TimeSynchronizationClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Overall.OverAllForm = new TimeClient();
            Application.Run(Overall.OverAllForm);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string dumpFile = System.IO.Path.Combine(System.Environment.CurrentDirectory, string.Format("crash-dump-{0}.dmp", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
            MiniDump.Write(dumpFile);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string dumpFile = System.IO.Path.Combine(System.Environment.CurrentDirectory, string.Format("thread-dump-{0}.dmp", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
            MiniDump.Write(dumpFile);
        }
    }
}
