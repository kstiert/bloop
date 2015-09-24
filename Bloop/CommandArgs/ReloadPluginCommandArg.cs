using System.Collections.Generic;
using Bloop.Core.Plugin;

namespace Bloop.CommandArgs
{
    public class ReloadPluginCommandArg : ICommandArg
    {
        public string Command
        {
            get { return "reloadplugin"; }
        }

        public void Execute(IList<string> args)
        {
            PluginManager.Init(App.Window);
        }
    }
}
