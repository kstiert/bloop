using System.Collections.Generic;
using System.Linq;
using Bloop.Plugin;
using IronPython.Hosting;
using IronPython.Runtime.Types;
using Bloop.Infrastructure.Logger;

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
                try
                {
                    var py = Python.CreateRuntime();
                    var scope = py.UseFile(metadata.ExecuteFilePath);
                    var iplugin = scope.GetVariable<PythonType>("IPlugin");
                    var types = scope.GetItems().Where(kv => kv.Key != "IPlugin" && kv.Value is PythonType && iplugin.__subclasscheck__(kv.Value)).Select(kv => kv.Value);
                    if (!types.Any())
                    {
                        Log.Warn(string.Format("Couldn't load plugin {0}: didn't find the class that implement IPlugin", metadata.Name));
                        continue;
                    }

                    foreach (var pyplugin in types)
                    {
                        var instance = (IPlugin)scope.Engine.Operations.CreateInstance(pyplugin);
                        plugins.Add(new PluginPair { Plugin = instance, Metadata = metadata });
                    }
                }
                catch (System.Exception e)
                {
                    Log.Error(string.Format("Couldn't load plugin {0}: {1}", metadata.Name, e.Message));
#if (DEBUG)
                    {
                        throw;
                    }
#endif
                }

            }
            return plugins;
        }
    }
}