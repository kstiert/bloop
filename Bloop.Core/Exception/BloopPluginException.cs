namespace Bloop.Core.Exception
{
    public class BloopPluginException : BloopException
    {
        public string PluginName { get; set; }

        public BloopPluginException(string pluginName,System.Exception e)
            : base(e.Message,e)
        {
            PluginName = pluginName;
        }
    }
}
