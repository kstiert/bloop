using System.Collections.Generic;
using System.Windows.Controls;

namespace Bloop.Plugin.Sys
{
    public partial class SysSettings : UserControl
    {
        public SysSettings(List<Result> Results)
        {
            InitializeComponent();

            foreach (var Result in Results)
            {
                this.lbxCommands.Items.Add(Result);
            }
        }
    }
}
