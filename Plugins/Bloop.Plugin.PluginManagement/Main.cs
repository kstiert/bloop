using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Bloop.Plugin.PluginManagement
{
    public class Main : IPlugin,IPluginI18n
    {
        private PluginInitContext context;

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            if (string.IsNullOrEmpty(query.Search))
            {
                results.Add(new Result("uninstall <pluginName>", "Images\\plugin.png", "uninstall plugin")
                {
                    Action = e => ChangeToUninstallCommand()
                });
                results.Add(new Result("list", "Images\\plugin.png", "list plugins installed")
                {
                    Action = e => ChangeToListCommand()
                });
                return results;
            }

            if (!string.IsNullOrEmpty(query.FirstSearch))
            {
                bool hit = false;
                switch (query.FirstSearch.ToLower())
                {
                    case "list":
                        hit = true;
                        results = ListInstalledPlugins();
                        break;

                    case "uninstall":
                        hit = true;
                        results = UnInstallPlugins(query);
                        break;
                }

                if (!hit)
                {
                    if ("uninstall".Contains(query.FirstSearch.ToLower()))
                    {
                        results.Add(new Result("uninstall <pluginName>", "Images\\plugin.png", "uninstall plugin")
                        {
                            Action = e => ChangeToUninstallCommand()
                        });
                    }
                    if ("list".Contains(query.FirstSearch.ToLower()))
                    {
                        results.Add(new Result("list", "Images\\plugin.png", "list plugins installed")
                        {
                            Action = e => ChangeToListCommand()
                        });
                    }
                }
            }

            return results;
        }

        private bool ChangeToListCommand()
        {
            if (context.CurrentPluginMetadata.ActionKeyword == "*")
            {
                context.API.ChangeQuery("list ");
            }
            else
            {
                context.API.ChangeQuery(string.Format("{0} list ", context.CurrentPluginMetadata.ActionKeyword));
            }
            return false;
        }

        private bool ChangeToUninstallCommand()
        {
            if (context.CurrentPluginMetadata.ActionKeyword == "*")
            {
                context.API.ChangeQuery("uninstall ");
            }
            else
            {
                context.API.ChangeQuery(string.Format("{0} uninstall ", context.CurrentPluginMetadata.ActionKeyword));
            }
            return false;
        }

        private List<Result> UnInstallPlugins(Query query)
        {
            List<Result> results = new List<Result>();
            List<PluginMetadata> allInstalledPlugins = context.API.GetAllPlugins().Select(o => o.Metadata).ToList();
            if (!string.IsNullOrEmpty(query.SecondSearch))
            {
                allInstalledPlugins =
                    allInstalledPlugins.Where(o => o.Name.ToLower().Contains(query.SecondSearch.ToLower())).ToList();
            }

            foreach (PluginMetadata plugin in allInstalledPlugins)
            {
                var plugin1 = plugin;
                results.Add(new Result()
                {
                    Title = plugin.Name,
                    SubTitle = plugin.Description,
                    IcoPath = plugin.FullIcoPath,
                    Action = e =>
                    {
                        UnInstallPlugin(plugin1);
                        return false;
                    }
                });
            }
            return results;
        }

        private void UnInstallPlugin(PluginMetadata plugin)
        {
            string content = string.Format("Do you want to uninstall following plugin?\r\n\r\nName: {0}\r\nVersion: {1}\r\nAuthor: {2}", plugin.Name, plugin.Version, plugin.Author);
            if (MessageBox.Show(content, "Bloop", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Create(Path.Combine(plugin.PluginDirectory, "NeedDelete.txt")).Close();
                if (MessageBox.Show(
                    "You have uninstalled plugin " + plugin.Name + " successfully.\r\n Restart Bloop to take effect?",
                    "Install plugin",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.Arguments = "/C ping 127.0.0.1 -n 1 && \"" + Application.ExecutablePath + "\"";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.CreateNoWindow = true;
                    Info.FileName = "cmd.exe";
                    Process.Start(Info);
                    context.API.CloseApp();
                }
            }
        }

        private List<Result> ListInstalledPlugins()
        {
            List<Result> results = new List<Result>();
            foreach (PluginMetadata plugin in context.API.GetAllPlugins().Select(o => o.Metadata))
            {
                results.Add(new Result()
                {
                    Title = plugin.Name + " - " + plugin.ActionKeyword,
                    SubTitle = plugin.Description,
                    IcoPath = plugin.FullIcoPath
                });
            }
            return results;
        }

        public void Init(PluginInitContext context)
        {
            this.context = context;
        }

        public string GetLanguagesFolder()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Languages");
        }

        public string GetTranslatedPluginTitle()
        {
            return context.API.GetTranslation("Bloop_plugin_plugin_management_plugin_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return context.API.GetTranslation("Bloop_plugin_plugin_management_plugin_description");
        }
    }
}
