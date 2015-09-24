using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Control = System.Windows.Controls.Control;

namespace Bloop.Plugin.Folder
{
    public class FolderPlugin : IPlugin, ISettingProvider, IPluginI18n
    {
        private static List<string> driveNames;
        private PluginInitContext context;

        public Control CreateSettingPanel()
        {
            return new FileSystemSettings(context.API);
        }

        public void Init(PluginInitContext context)
        {
            this.context = context;
            this.context.API.BackKeyDownEvent += ApiBackKeyDownEvent;

            if (driveNames == null)
            {
                driveNames = DriveInfo.GetDrives().Select(d => d.Name.ToLower()).ToList();
            }

            if (FolderStorage.Instance.FolderLinks == null)
            {
                FolderStorage.Instance.FolderLinks = new List<FolderLink>();
                FolderStorage.Instance.Save();
            }
        }

        private static List<Result> GetContextMenusForFileDrop(Result targetResult, List<string> files)
        {
            List<Result> contextMenus = new List<Result>();
            string folderPath = ((FolderLink) targetResult.ContextData).Path;
            contextMenus.Add(new Result()
            {
                Title = "Copy to this folder",
                IcoPath = "Images/copy.png",
                Action = _ =>
                {
                    MessageBox.Show("Copy");
                    return true;
                }
            });
            return contextMenus;
        }

        private void ApiBackKeyDownEvent(BloopKeyDownEventArgs e)
        {
            string query = e.Query;
            if (Directory.Exists(query))
            {
                if (query.EndsWith("\\"))
                {
                    query = query.Remove(query.Length - 1);
                }

                if (query.Contains("\\"))
                {
                    int index = query.LastIndexOf("\\");
                    query = query.Remove(index) + "\\";
                }

                context.API.ChangeQuery(query);
            }
        }

        public List<Result> Query(Query query)
        {
            string input = query.Search.ToLower();

            List<FolderLink> userFolderLinks = FolderStorage.Instance.FolderLinks.Where(
                x => x.Nickname.StartsWith(input, StringComparison.OrdinalIgnoreCase)).ToList();
            List<Result> results =
                userFolderLinks.Select(
                    item => new Result(item.Nickname, "Images/folder.png", "Ctrl + Enter to open the directory")
                    {
                        Action = c =>
                        {
                            if (c.SpecialKeyState.CtrlPressed)
                            {
                                try
                                {
                                    Process.Start(item.Path);
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Could not start " + item.Path);
                                    return false;
                                }
                            }
                            context.API.ChangeQuery(item.Path + "\\");
                            return false;
                        },
                        ContextData = item
                    }).ToList();

            if (driveNames != null && !driveNames.Any(input.StartsWith))
            {
                return results;
            }

            string filter = null;
            if (!input.EndsWith("\\"))
            {
                filter = input.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.None).Last();
                input = input.Replace(filter, string.Empty);
            }
            results.AddRange(QueryInternal_Directory_Exists(input, filter));

            return results;
        }

        private List<Result> QueryInternal_Directory_Exists(string rawQuery, string filter)
        {
            var results = new List<Result>();
            if (!Directory.Exists(rawQuery)) return results;

            //Add children directories
            foreach (DirectoryInfo dir in new DirectoryInfo(rawQuery).GetDirectories().Where(d => filter == null || d.Name.ToLower().Contains(filter)))
            {
                if ((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;

                DirectoryInfo dirCopy = dir;
                var result = new Result(dir.Name, "Images/folder.png", "Ctrl + Enter to open the directory")
                {
                    Action = c =>
                    {
                        if (c.SpecialKeyState.CtrlPressed)
                        {
                            try
                            {
                                Process.Start(dirCopy.FullName);
                                return true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Could not start " + dirCopy.FullName);
                                return false;
                            }
                        }
                        context.API.ChangeQuery(dirCopy.FullName + "\\");
                        return false;
                    }
                };

                results.Add(result);
            }

            //Add children files
            foreach (FileInfo file in new DirectoryInfo(rawQuery).GetFiles().Where(f => filter == null || f.Name.ToLower().Contains(filter)))
            {
                if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;

                string filePath = file.FullName;
                var result = new Result(Path.GetFileName(filePath))
                {
                    Action = c =>
                    {
                        try
                        {
                            Process.Start(filePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Could not start " + filePath);
                        }

                        return true;
                    },
                    IcoPath = filePath
                };

                results.Add(result);
            }

            // If there's exactly one result that's almost certainly what the user wants
            if (results.Count != 1)
            {
                results.Add(new Result("Open current directory", "Images/folder.png")
                {
                    Score = 10000,
                    Action = c =>
                    {
                        Process.Start(rawQuery);
                        return true;
                    }
                });
            }

            return results;
        }

        public string GetLanguagesFolder()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Languages");
        }

        public string GetTranslatedPluginTitle()
        {
            return context.API.GetTranslation("Bloop_plugin_folder_plugin_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return context.API.GetTranslation("Bloop_plugin_folder_plugin_description");
        }
    }
}