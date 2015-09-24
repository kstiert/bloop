using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Bloop.Infrastructure.Storage;
using System.IO;

namespace Bloop.Plugin.Folder
{
    public class FolderStorage : JsonStrorage<FolderStorage>
    {
        [JsonProperty]
        public List<FolderLink> FolderLinks { get; set; }
        protected override string ConfigFolder
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        protected override string ConfigName
        {
            get { return "setting"; }
        }
    }
}
