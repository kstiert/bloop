using System.Collections.Generic;
using Bloop.Plugin;

namespace Bloop.Core.Plugin
{
    internal interface IPluginLoader
    {
        IEnumerable<PluginPair> LoadPlugin(List<PluginMetadata> pluginMetadatas);
    }
}
