using System.Collections.Generic;

namespace Bloop.Plugin
{
    public interface IPlugin
    {
        List<Result> Query(Query query);
        void Init(PluginInitContext context);
    }
}