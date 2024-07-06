using LaserGRBL.AddIn;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LaserGRBL.AddInTemplate
{
    public class Template: AddIn.AddIn
    {
        public override string Title => "Test LaserGRBL add-in";

        private Controller mController;

        public Config Config { get; private set; } = new Config();
        public Data Data { get; private set; } = new Data();

        public Template(CommonGrblCore core, ToolStripMenuItem menuItem): base(core, menuItem)
        {
            CommonImageButton button = core.AddButton("mdi-gamepad", "Open gamepad form");
            button.Enabled = true;
            button.Click += PrivateItem_Click;
            CommonImageButton btnGenerateImageAI = core.AddButton("mdi-atom", "Generate image with AI");
            btnGenerateImageAI.Enabled = true;
            btnGenerateImageAI.Click += InvokeAI;
            ToolStripMenuItem privateItem = new ToolStripMenuItem
            {
                Text = "Config"
            };
            privateItem.Click += PrivateItem_Click;
            menuItem.DropDownItems.Add(privateItem);
            mController = new Controller(UserIndex.One);
            Task.Factory.StartNew(() =>
            {
                List<ButtonVar> buttons = new List<ButtonVar>() {
                    new ButtonVar(GamepadButtonFlags.A, "M3 S[$30*10/100]\r\nG1 F1000", "M5 S0\r\nG0")
                };
                while (true)
                {
                    if (mController.IsConnected)
                    {
                        State state = mController.GetState();

                        Data.X.Value = MapValue(state.Gamepad.LeftThumbX);
                        Data.Y.Value = MapValue(state.Gamepad.LeftThumbY);
                        Data.Power.Value = Math.Round(state.Gamepad.RightTrigger / 256.0, 1);

                        List<string> commands = new List<string>();
                        foreach (ButtonVar btn in buttons)
                        {
                            btn.Buttons = state.Gamepad.Buttons;
                            string command = btn.CurrentCommand;
                            if (string.IsNullOrEmpty(command)) commands.Add(command);
                        }

                        double distance = Quantize(Math.Sqrt(Math.Pow(Data.X.Value, 2) + Math.Pow(Data.Y.Value, 2)));
                        if (distance > 1) distance = 1;

                        if (Data.IsChanged || commands.Count > 0)
                        {
                            foreach (string command in commands)
                            {
                                Core.ContinuousJogAbort();
                                Core.ExecuteCustomCode(command);
                            }
                            if (!Data.IsZeroPosition || commands.Count > 0)
                            {
                                PointF target = new PointF((float)Data.X.Value * 5000, (float)Data.Y.Value * 5000);
                                Core.ContinuousJogToPosition(target, (float)ScaleValue(distance, 5000));
                            }
                            else
                            {
                                Core.ContinuousJogAbort();
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(50);
                }
            });
        }

        private double ScaleValue(double value, double max)
        {
            value = (1 - Math.Cos(value * Math.PI)) / 2;
            return Math.Round(value * max, 0);
        }

        private double MapValue(short value)
        {
            double result = 0;
            if (value > Config.Tolerance)
            {
                result = (value - Config.Tolerance) / (short.MaxValue - Config.Tolerance * 2);
            }
            else if (value < -Config.Tolerance)
            {
                result = (value + Config.Tolerance) / (short.MaxValue - Config.Tolerance * 2);
            }
            if (result > 1) result = 1;
            if (result < -1) result = -1;
            result = Quantize(result);
            return result;
        }

        private double Quantize(double value)
        {
            return Math.Round(value * Config.MaxSteps, 0) / Config.MaxSteps;
        }

        private void PrivateItem_Click(object sender, EventArgs e)
        {
            using (FormConfig form = new FormConfig())
            {
                form.ShowDialog(this);
            }
        }

        private void InvokeAI(object sender, EventArgs e)
        {

            AI.GenerateImage("Hello, world!", (filename) =>
            {
                Core.OpenFile(filename);
            });
        }

    }

}
