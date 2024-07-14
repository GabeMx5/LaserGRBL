using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LaserGRBL.AddIn
{
    public static class AddInManager
    {
        // add-in list
        private readonly static List<AddIn> mAddIns = new List<AddIn>();
        // current add-in folder
        private static string mCurrentAddInFolder;

        internal static void LoadAddIns(ToolStripMenuItem addInMenuItemRoot, GrblCore core)
        {
            // init flag
            bool showAddInMenu = false;
            // resolve all assemblies from the AddIn folder
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                // ignore resources
                if (args.Name.Contains(".resources")) return null;
                // check if the assembly is already loaded
                Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                if (assembly != null) return assembly;
                // load the assembly from the AddIn folder
                string filename = args.Name.Split(',')[0] + ".dll".ToLower();
                filename = Path.Combine(mCurrentAddInFolder, filename);
                if (File.Exists(filename))
                {
                    try
                    {
                        return Assembly.LoadFrom(filename);
                    }
                    catch
                    {
                        return null;
                    }
                }
                return null;
            };
            // get the AddIn folder
            var addInFolder = Path.Combine(GrblCore.DataPath, "AddIn");
            // check if the AddIn folder exists
            if (Directory.Exists(addInFolder))
            {
                // load all assemblies from the AddIn folder
                var files = Directory.GetFiles(addInFolder, "*.dll", SearchOption.AllDirectories);
                foreach (var addInFile in files)
                {
                    // get current folder
                    mCurrentAddInFolder = Path.GetDirectoryName(addInFile);
                    // load the assembly
                    Assembly assembly = Assembly.LoadFile(addInFile);
                    foreach (Type type in assembly.GetTypes())
                    {
                        // check if the type is an AddIn
                        if (typeof(AddIn).IsAssignableFrom(type))
                        {
                            try
                            {
                                // prepare the menu item
                                ToolStripMenuItem addInMenuItem = new ToolStripMenuItem();
                                addInMenuItemRoot.DropDownItems.Add(addInMenuItem);
                                // create the AddIn instance
                                AddIn addIn = (AddIn)Activator.CreateInstance(type, new object[] { core, addInMenuItem });
                                // add the AddIn to the list
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
            // show the AddIn menu if some AddIns are loaded
            addInMenuItemRoot.Visible = showAddInMenu;
        }

    }

}
