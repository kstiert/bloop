using System.Windows;

namespace Bloop.Core.UI
{
    /// <summary>
    /// Object implement this interface will have the ability to has its own UI styles
    /// </summary>
    public interface IUIResource
    {
        ResourceDictionary GetResourceDictionary();
    }
}
