using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace LaserGRBL.AddIn
{
    public static class AddInManager
    {
        private readonly static List<AddIn> mAddIns = new List<AddIn>();

        internal static void LoadAddIns(ToolStripMenuItem addInMenuItemRoot)
        {
            bool showAddInMenu = false;
            var addInFolder = Path.Combine(GrblCore.ExePath, "AddIn");
            if (Directory.Exists(addInFolder))
            {
                var files = Directory.GetFiles(addInFolder, "*.dll"); ;
                foreach (var addInFile in files)
                {
                    Assembly assembly = Assembly.LoadFile(addInFile);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (typeof(AddIn).IsAssignableFrom(type))
                        {
                            try
                            {
                                ToolStripMenuItem addInMenuItem = new ToolStripMenuItem();
                                addInMenuItemRoot.DropDownItems.Add(addInMenuItem);
                                AddIn addIn = (AddIn)Activator.CreateInstance(type, new object[] { addInMenuItem });
                                mAddIns.Add(addIn);
                                showAddInMenu = true;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            addInMenuItemRoot.Visible = showAddInMenu;
        }

        private static void ForEveryAddIn(Action<AddIn> action)
        {
            foreach (var addIn in mAddIns)
            {
                try
                {
                    action.Invoke(addIn);
                }
                catch
                {
                }
            }
        }

        internal static void InvokeOnFileLoading(long elapsed, string filename) => ForEveryAddIn((addIn) => addIn.OnFileLoading(elapsed, filename));

        internal static void InvokeOnFileLoaded(long elapsed, string filename) => ForEveryAddIn((addIn) => addIn.OnFileLoaded(elapsed, filename));

    }

}
