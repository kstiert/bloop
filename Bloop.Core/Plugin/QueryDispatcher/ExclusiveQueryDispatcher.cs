using System.Collections.Generic;
using Bloop.Plugin;

namespace Bloop.Core.Plugin.QueryDispatcher
{
    public class ExclusiveQueryDispatcher : BaseQueryDispatcher
    {
        protected override List<PluginPair> GetPlugins(Query query)
        {
            List<PluginPair> pluginPairs = new List<PluginPair>();
            var exclusivePluginPair = PluginManager.GetExclusivePlugin(query) ??
                                      PluginManager.GetActionKeywordPlugin(query);
            if (exclusivePluginPair != null)
            {
                pluginPairs.Add(exclusivePluginPair);
            }

            return pluginPairs;
        }


    
    }
}
