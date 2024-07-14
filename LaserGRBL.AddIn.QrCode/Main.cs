using System;
using System.Windows.Forms;

namespace LaserGRBL.AddIn.QrCode
{
    public class Main : AddIn
    {
        public override string Title => "QrCode";

        public Main(CommonGrblCore core, ToolStripMenuItem menuItem) : base(core, menuItem)
        {
            CommonImageButton btnGenerateQrCode = core.AddButton("mdi-qrcode", "Generate QR code");
            btnGenerateQrCode.Enabled = true;
            btnGenerateQrCode.Click += BtnGenerateBarCode_Click;
        }

        private void BtnGenerateBarCode_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm(this);
            if (form.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(form.Filename))
            {
                Core.OpenFile(form.Filename);
            }
        }

    }

}
