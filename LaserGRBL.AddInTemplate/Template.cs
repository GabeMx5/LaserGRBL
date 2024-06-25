using LaserGRBL.AddIn;
using SharpDX.XInput;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            ToolStripMenuItem privateItem = new ToolStripMenuItem
            {
                Text = "Config"
            };
            privateItem.Click += PrivateItem_Click;
            menuItem.DropDownItems.Add(privateItem);
            mController = new Controller(UserIndex.One);
            Task.Factory.StartNew(() =>
            {
                ChangingVar<bool> buttonA = new ChangingVar<bool>();
                while (true)
                {
                    if (mController.IsConnected)
                    {
                        State state = mController.GetState();
                        buttonA.Value = (state.Gamepad.Buttons & GamepadButtonFlags.A) != 0;
                        Data.X.Value = MapValue(state.Gamepad.LeftThumbX);
                        Data.Y.Value = MapValue(state.Gamepad.LeftThumbY);
                        if (buttonA.Value && !buttonA.PreviousValue)
                        {
                            Core.ExecuteCustombutton("M3 S[$30*10/100]\r\nG1 F1000");
                        }
                        if (!buttonA.Value && buttonA.PreviousValue)
                        {
                            Core.ExecuteCustombutton("M5 S0\r\nG0");
                        }
                        double distance = Quantize(Math.Sqrt(Math.Pow(Data.X.Value, 2) + Math.Pow(Data.Y.Value, 2)));
                        if (distance > 1) distance = 1;

                        Data.Power.Value = Math.Round(state.Gamepad.RightTrigger / 256.0, 1);
                        if (Data.IsChanged)
                        {
                            if (!Data.IsZeroPosition)
                            {
                                PointF target = new PointF((float)Data.X.Value * 5000, (float)Data.Y.Value * 5000);
                                Core.JogToPosition(target, (float)ScaleValue(distance, 5000));
                            }
                            else
                            {
                                Core.JogAbort();
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

        private void PrivateItem_Click(object sender, System.EventArgs e)
        {
            using (FormConfig form = new FormConfig())
            {
                form.ShowDialog(this);
            }
        }

    }

}
