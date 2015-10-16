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
                var iplugin = scope.GetVariable("IPlugin");
                dynamic plugin = scope.GetItems().Select(i => (i.Value as PythonType)).FirstOrDefault(i => i != null && i.__subclasscheck__(iplugin));
                plugins.Add(new PluginPair { Metadata = metadata, Plugin = plugin });
            }
            return plugins;
        }
    }
}