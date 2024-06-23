using System;
using System.Windows.Forms;

namespace LaserGRBL.AddIn
{
    public abstract class AddIn
    {
        public abstract string Title { get; }

        public CommonGrblCore Core { get; private set; }

        public AddIn(CommonGrblCore core, ToolStripMenuItem menuItem) {
            menuItem.Text = Title;
            Core = core;
        }

    }

}
