using System.Collections.Generic;

namespace Bloop.Plugin
{
    /// <summary>
    /// Public APIs that plugin can use
    /// </summary>
    public interface IPublicAPI
    {
        /// <summary>
        /// Push result to query box
        /// </summary>
        /// <param name="query"></param>
        /// <param name="plugin"></param>
        /// <param name="results"></param>
        void PushResults(Query query, PluginMetadata plugin, List<Result> results);

        /// <summary>
        /// Execute command
        /// a replacement to RUN(win+r) function
        /// </summary>
        /// <param name="cmd">command that want to execute</param>
        /// <param name="runAsAdministrator">run as administrator</param>
        /// <returns></returns>
        bool ShellRun(string cmd, bool runAsAdministrator = false);

        /// <summary>
        /// Change Bloop query
        /// </summary>
        /// <param name="query">query text</param>
        /// <param name="requery">
        /// force requery By default, Bloop will not fire query if your query is same with existing one. 
        /// Set this to true to force Bloop requerying
        /// </param>
        void ChangeQuery(string query, bool requery = false);

        /// <summary>
        /// Just change the query text, this won't raise search
        /// </summary>
        /// <param name="query"></param>
        void ChangeQueryText(string query, bool selectAll = false);

        /// <summary>
        /// Close Bloop
        /// </summary>
        void CloseApp();

        /// <summary>
        /// Hide Bloop
        /// </summary>
        void HideApp();

        /// <summary>
        /// Show Bloop
        /// </summary>
        void ShowApp();

        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="title">Message title</param>
        /// <param name="subTitle">Message subtitle</param>
        /// <param name="iconPath">Message icon path (relative path to your plugin folder)</param>
        void ShowMsg(string title, string subTitle = "", string iconPath = "");

        /// <summary>
        /// Open setting dialog
        /// </summary>
        void OpenSettingDialog(string tabName = "general");

        /// <summary>
        /// Show loading animation
        /// </summary>
        void StartLoadingBar();

        /// <summary>
        /// Stop loading animation
        /// </summary>
        void StopLoadingBar();

        /// <summary>
        /// Install Bloop plugin
        /// </summary>
        /// <param name="path">Plugin path (ends with .Bloop)</param>
        void InstallPlugin(string path);

        /// <summary>
        /// Reload all plugins
        /// </summary>
        void ReloadPlugins();

        /// <summary>
        /// Get translation of current language
        /// You need to implement IPluginI18n if you want to support multiple languages for your plugin
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetTranslation(string key);

        /// <summary>
        /// Get all loaded plugins 
        /// </summary>
        /// <returns></returns>
        List<PluginPair> GetAllPlugins();

        /// <summary>
        /// Fired after Back key down in the Bloop query box
        /// </summary>
        event BloopKeyDownEventHandler BackKeyDownEvent;

        /// <summary>
        /// Fired after global keyboard events
        /// if you want to hook something like Ctrl+R, you should use this event
        /// </summary>
        event BloopGlobalKeyboardEventHandler GlobalKeyboardEvent;

        /// <summary>
        /// Fired after drop to result item of current plugin 
        /// </summary>
        event ResultItemDropEventHandler ResultItemDropEvent;
    }
}
