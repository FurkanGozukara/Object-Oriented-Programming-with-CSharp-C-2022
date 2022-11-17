using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace lecture_7
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            //i am assigning delegate UnhandledExceptionEventHandler to UnhandledException
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleUnhandledExceptions);

            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Application_DispatcherUnhandledException);

            TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(Application_DispatcherUnhandledException2);
        }

        public static void writeMessage(Exception e, string srWhichHandle)
        {
            //.? means null check. if not null
            //string srMsg;
            //if (e.InnerException != null)
            //    srMsg = e.InnerException?.Message;

            string srMsg = e.InnerException?.Message;
            //2019103039
            if (!string.IsNullOrEmpty(srMsg))
            {
                srMsg += "\r\n\r\nStack\r\n" + e.InnerException?.StackTrace;
            }

            if (string.IsNullOrEmpty(srMsg))
            {
                srMsg = e.Message + "\r\n\r\nStack\r\n" + e.StackTrace;
            }

            srMsg += $"\r\n\r\n****{srWhichHandle}*****\r\n\r\n";

            File.AppendAllText("global_errors.txt", srMsg);

        }

        private static void Application_DispatcherUnhandledException2(object o, UnobservedTaskExceptionEventArgs e)
        {
            writeMessage(e.Exception, "UnobservedTaskException");

        }

        private static void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            writeMessage(e.Exception, "DispatcherUnhandledExceptionEventHandler");
        }

        private static void HandleUnhandledExceptions(object sender, UnhandledExceptionEventArgs args)
        {

            Exception e = (Exception)args.ExceptionObject;

            writeMessage(e, "UnhandledExceptionEventHandler");
        }
    }
}
