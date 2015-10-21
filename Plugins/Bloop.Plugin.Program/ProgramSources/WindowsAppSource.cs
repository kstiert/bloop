using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;

namespace Bloop.Plugin.Program.ProgramSources
{
    [Serializable]
    public class WindowsAppSource : AbstractProgramSource
    {
        public WindowsAppSource(ProgramSource source)
        {
            this.BonusPoints = source.BonusPoints;
        }

        public override List<Program> LoadPrograms()
        {
            var programs = new List<Program>();
            using(var packages = Registry.ClassesRoot.OpenSubKey(@"\Extensions\ContractId\Windows.Protocol\PackageId"))
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
                                Title = ResolveResource((string)classId.GetValue("DisplayName")),
                                ExecutePath = properties.GetValue("Name") + "://",
                                IcoPath = ResolveResource((string)classId.GetValue("Icon"))
                            });
                        }
                    }
                }
            }
            return programs;
        }

        [DllImport("shlwapi.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false, ThrowOnUnmappableChar = true)]
        private static extern int SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved);

        private static string ResolveResource(string resource)
        {
            var outBuff = new StringBuilder(1024);
            int result = SHLoadIndirectString(resource, outBuff, outBuff.Capacity, IntPtr.Zero);
            return outBuff.ToString();
        }
    }
}
