using SharpDX.XInput;
using System;
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

        public Template(ToolStripMenuItem menuItem): base(menuItem)
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

                while (true)
                {
                    if (mController.IsConnected)
                    {
                        State state = mController.GetState();
                        Data.BeginUpdate();
                        Data.X = MapValue(state.Gamepad.LeftThumbX);
                        Data.Y = MapValue(state.Gamepad.LeftThumbY);

                        double distance = Quantize(Math.Sqrt(Math.Pow(Data.X, 2) + Math.Pow(Data.Y, 2)));
                        if (distance > 1) distance = 1;

                        Data.Power = Math.Round(state.Gamepad.RightTrigger / 256.0, 1);
                        if (Data.IsChanged)
                        {
                            SendImmediate(0x85);
                            if (Data.IsPowerChanged) EnqueueCommand($"S{ScaleValue(Data.Power, 1000)}");
                            if (!Data.IsZeroPosition) EnqueueCommand($"$J=G91X{Data.X * 5000}Y{Data.Y * 5000}F{ScaleValue(distance, 5000)}");
                        }
                    }
                    System.Threading.Thread.Sleep(200);
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
            return Math.Round(value * (Config.MaxSteps * 1f), 0) / Config.MaxSteps;
        }

        private void PrivateItem_Click(object sender, System.EventArgs e)
        {
            using (FormConfig form = new FormConfig())
            {
                form.ShowDialog(this);
            }
        }

        public override void OnFileLoaded(long elapsed, string filename)
        {
        }
    }
}
