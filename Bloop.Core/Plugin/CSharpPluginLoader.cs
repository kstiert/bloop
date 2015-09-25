using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bloop.Infrastructure.Logger;
using Bloop.Plugin;
using System.IO;
using System.Xml;

namespace Bloop.Core.Plugin
{
    internal class CSharpPluginLoader : IPluginLoader
    {
        public IEnumerable<PluginPair> LoadPlugin(List<PluginMetadata> pluginMetadatas)
        {
            var plugins = new List<PluginPair>();
            List<PluginMetadata> CSharpPluginMetadatas = pluginMetadatas.Where(o => o.Language.ToUpper() == AllowedLanguage.CSharp.ToUpper()).ToList();

            foreach (PluginMetadata metadata in CSharpPluginMetadatas)
            {
                try
                {
                    // Check if the plugin has an app.config
                    if(File.Exists(metadata.ExecuteFilePath + ".config"))
                    {
                        using (var reader = XmlReader.Create(metadata.ExecuteFilePath + ".config"))
                        {
                            // Can't do redirects properly, so we're gonna fake it.
                            while(reader.ReadToFollowing("assemblyIdentity"))
                            {
                                var name = reader.GetAttribute("name");
                                this.RedirectAssembly(name, metadata.PluginDirectory);
                            }
                        }  
                    }

                    Assembly asm = Assembly.Load(AssemblyName.GetAssemblyName(metadata.ExecuteFilePath));
                    List<Type> types = asm.GetTypes().Where(o => o.IsClass && !o.IsAbstract &&  o.GetInterfaces().Contains(typeof(IPlugin))).ToList();
                    if (types.Count == 0)
                    {
                        Log.Warn(string.Format("Couldn't load plugin {0}: didn't find the class that implement IPlugin", metadata.Name));
                        continue;
                    }

                    foreach (Type type in types)
                    {
                        PluginPair pair = new PluginPair()
                        {
                            Plugin = Activator.CreateInstance(type) as IPlugin,
                            Metadata = metadata
                        };

                        plugins.Add(pair);
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

        private void RedirectAssembly(string shortName, string path)
        {
            ResolveEventHandler handler = null;

            handler = (sender, args) => {
                var requestedAssembly = new AssemblyName(args.Name);
                if (requestedAssembly.Name != shortName)
                    return null;

                AppDomain.CurrentDomain.AssemblyResolve -= handler;
                // Load the assembly of this name from the plugin's directory.
                return Assembly.LoadFrom(Path.Combine(path, shortName + ".dll"));
            };
            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }
    }
}