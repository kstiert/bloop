using System.Windows.Controls;

namespace Bloop.Plugin
{
    public interface ISettingProvider
    {
        Control CreateSettingPanel();
    }
}
