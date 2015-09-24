using System.Collections.Generic;

namespace Bloop.Plugin.Features
{
    public interface IContextMenu
    {
        List<Result> LoadContextMenus(Result selectedResult);
    }
}