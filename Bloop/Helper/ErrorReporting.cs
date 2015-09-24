using System;
using System.Windows.Threading;
using Bloop.Core.Exception;
using Bloop.Infrastructure.Logger;

namespace Bloop.Helper
{
    public static class ErrorReporting
    {
        public static void Report(Exception e)
        {
            Log.Error(ExceptionFormatter.FormatExcpetion(e));
        }

        public static void UnhandledExceptionHandle(object sender, UnhandledExceptionEventArgs e)
        {
            //handle non-ui thread exceptions
            App.Window.Dispatcher.Invoke(new Action(() =>
            {
                Report((Exception)e.ExceptionObject);
                if (!(e.ExceptionObject is BloopException))
                {
                    Environment.Exit(0);
                }
            }));
        }

        public static void DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //handle ui thread exceptions
            Report(e.Exception);
            //prevent crash
            e.Handled = true;
        }
    }
}
