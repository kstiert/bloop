using System;
using System.Collections.Generic;
using System.Linq;
using Bloop.CommandArgs;
using Bloop.Helper;
using Application = System.Windows.Application;
using StartupEventArgs = System.Windows.StartupEventArgs;

namespace Bloop
{
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "Bloop_Unique_Application_Mutex";
        public static MainWindow Window { get; private set; }

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();
                SingleInstance<App>.Cleanup();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += ErrorReporting.DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += ErrorReporting.UnhandledExceptionHandle;

            Window = new MainWindow();
            CommandArgsFactory.Execute(e.Args.ToList());
        }

        public bool OnActivate(IList<string> args)
        {
            CommandArgsFactory.Execute(args);
            return true;
        }
    }
}
