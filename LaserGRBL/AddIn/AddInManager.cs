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
        private readonly static List<AddIn> mAddIns = new List<AddIn>();

        internal static void LoadAddIns(ToolStripMenuItem addInMenuItemRoot, GrblCore core)
        {
            bool showAddInMenu = false;
            var addInFolder = Path.Combine(GrblCore.ExePath, "AddIn");
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
                filename = Path.Combine(addInFolder, filename);
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
            // check if the AddIn folder exists
            if (Directory.Exists(addInFolder))
            {
                // load all assemblies from the AddIn folder
                var files = Directory.GetFiles(addInFolder, "*.dll");
                foreach (var addInFile in files)
                {
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
                                AddIn addIn = (AddIn)Activator.CreateInstance(type, new object[] { addInMenuItem });
                                addIn.OnEnqueueCommand += (command) => core.EnqueueCommand(new GrblCommand(command));
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
