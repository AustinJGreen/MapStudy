using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MapStudy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Debug.WriteLine(string.Format("FirstChance exception: {0}, Stacktrace: {1}", e.Exception.Message, e.Exception.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(string.Format("Unhandled exception: {0}", e.ExceptionObject.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Debug.WriteLine(string.Format("Thread exception: {0}, Stacktrace: {1}", e.Exception.Message, e.Exception.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
