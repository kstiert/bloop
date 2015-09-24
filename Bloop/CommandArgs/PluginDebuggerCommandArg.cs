using System.Collections.Generic;
using Bloop.Core.Plugin;

namespace Bloop.CommandArgs
{
    public class PluginDebuggerCommandArg : ICommandArg
    {
        public string Command
        {
            get { return "plugindebugger"; }
        }

        public void Execute(IList<string> args)
        {
            if (args.Count > 0)
            {
                var pluginFolderPath = args[0];
                PluginManager.ActivatePluginDebugger(pluginFolderPath);
            }
        }
    }
}
