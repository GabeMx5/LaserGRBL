using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.XInput;

namespace LaserGRBL.AddInTemplate
{
    public class Template: AddIn.AddIn
    {
        public override string Title => "Test LaserGRBL add-in";

        Controller controller;

        public Template(ToolStripMenuItem menuItem): base(menuItem)
        {
            ToolStripMenuItem privateItem = new ToolStripMenuItem
            {
                Text = "Private item"
            };
            privateItem.Click += PrivateItem_Click;
            menuItem.DropDownItems.Add(privateItem);
            controller = new Controller(UserIndex.One);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    State state = controller.GetState();

                    Debug.WriteLine(state.Gamepad.LeftThumbX);
                    if (state.Gamepad.LeftThumbX > 10000)
                    {
                        EnqueueCommand("$J=G91X2.0F500");
                    }
                    if (state.Gamepad.LeftThumbX < -10000)
                    {
                        EnqueueCommand("$J=G91X-2.0F500");
                    }
                    if (state.Gamepad.LeftThumbY > 10000)
                    {
                        EnqueueCommand("$J=G91Y2.0F500");
                    }
                    if (state.Gamepad.LeftThumbY < -10000)
                    {
                        EnqueueCommand("$J=G91Y-2.0F500");
                    }
                    System.Threading.Thread.Sleep(100);
                }
            });
        }

        private void PrivateItem_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        public override void OnFileLoaded(long elapsed, string filename)
        {
            //MessageBox.Show($"Loaded {filename}");
        }
    }
}
