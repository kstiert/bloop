using System;

namespace Bloop.Core.UserSettings
{
    [Serializable]
    public class CustomPluginHotkey
    {
        public string Hotkey { get; set; }
        public string ActionKeyword { get; set; }
    }
}
