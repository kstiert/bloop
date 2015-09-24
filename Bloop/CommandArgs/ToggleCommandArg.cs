using System.Collections.Generic;

namespace Bloop.CommandArgs
{
    public class ToggleCommandArg:ICommandArg
    {
        public string Command
        {
            get { return "toggle"; }
        }

        public void Execute(IList<string> args)
        {
            App.Window.ToggleBloop();
        }
    }
}
