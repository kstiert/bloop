using System.Collections.Generic;

namespace Bloop.CommandArgs
{
    interface ICommandArg
    {
        string Command { get; }
        void Execute(IList<string> args);
    }
}
