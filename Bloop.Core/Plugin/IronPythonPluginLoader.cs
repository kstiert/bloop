using System.Collections.Generic;
using System.Linq;
using Bloop.Plugin;
using IronPython.Hosting;
using IronPython.Runtime.Types;

namespace Bloop.Core.Plugin
{
    public class IronPythonPluginLoader : IPluginLoader
    {
        public IEnumerable<PluginPair> LoadPlugin(List<PluginMetadata> pluginMetadatas)
        {
            var plugins = new List<PluginPair>();
            List<PluginMetadata> PythonPluginMetadatas = pluginMetadatas.Where(o => o.Language.ToUpper() == AllowedLanguage.IronPython.ToUpper()).ToList();

            foreach (var metadata in PythonPluginMetadatas)
            {
                var py = Python.CreateRuntime();
                var scope = py.UseFile(metadata.ExecuteFilePath);
                var iplugin = scope.GetVariable<PythonType>("IPlugin");
                var pyplugin = scope.GetItems().Where(kv => kv.Key != "IPlugin").Select(kv => kv.Value).FirstOrDefault(v => v is PythonType && iplugin.__subclasscheck__(v));
                if (pyplugin != null)
                {
                    plugins.Add( new PluginPair { Plugin = (IPlugin)scope.Engine.Operations.CreateInstance(pyplugin), Metadata = metadata });
                }
            }
            return plugins;
        }
    }
}