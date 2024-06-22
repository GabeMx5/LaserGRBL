using System;
using System.Windows.Forms;

namespace LaserGRBL.AddInTemplate
{
    public partial class FormConfig : Form
    {
        private Template mAddIn;

        public FormConfig()
        {
            InitializeComponent();
        }

        public void ShowDialog(Template addIn)
        {
            mAddIn = addIn;
            ShowDialog();
        } 

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            lblData.Text = $"X: {mAddIn.Data.X}\r\nY: {mAddIn.Data.Y}\r\nPower: {mAddIn.Data.Power}";
        }
    }
}
