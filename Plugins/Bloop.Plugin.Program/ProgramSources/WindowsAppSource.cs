using Microsoft.Win32;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Bloop.Plugin.Program.ProgramSources
{
    public class WindowsAppSource : AbstractProgramSource
    {
        public override List<Program> LoadPrograms()
        {
            var programs = new List<Program>();
            using(var packages = Registry.ClassesRoot.OpenSubKey(@"HKEY_CLASSES_ROOT\Extensions\ContractId\Windows.Protocol\PackageId"))
            {
                foreach(var package in packages.GetSubKeyNames().Select(key => packages.OpenSubKey(key+ "\\ActivatableClassId")).Where(key => key != null))
                {
                    foreach(var classId in package.GetSubKeyNames().Select(key => package.OpenSubKey(key)).Where(key => key != null))
                    {
                        var properties = classId.OpenSubKey("CustomProperties");
                        if(properties != null)
                        {
                            programs.Add(new Program
                            {
                                Title = (string)classId.GetValue("DisplayName"),
                                ExecutePath = properties.GetValue("Name") + "://"
                            });
                        }
                    }
                }
            }
            return programs;
        }
    }
}
