using System.Collections.Generic;

namespace Bloop.Core.Theme
{
    interface ITheme
    {
        void ChangeTheme(string themeName);
        List<string> LoadAvailableThemes();
    }
}
