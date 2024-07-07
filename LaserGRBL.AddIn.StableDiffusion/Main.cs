using System.Threading.Tasks;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace LaserGRBL.AddIn.StableDiffusion
{
    public class Main : AddIn
    {

        public override string Title => "Stable diffusion";

        public string TempFolder = Path.Combine(Path.GetTempPath(), "LaserGRBL_AI");

        public Main(CommonGrblCore core, ToolStripMenuItem menuItem) : base(core, menuItem)
        {
            CommonImageButton btnGenerateImageAI = core.AddButton("mdi-atom", "Generate image with AI");
            btnGenerateImageAI.Enabled = true;
            btnGenerateImageAI.Click += BtnGenerateImageAI_Click;
        }

        private void BtnGenerateImageAI_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm(this);
            if (form.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(form.Filename))
            {
                Core.OpenFile(form.Filename);
            }
        }

    }

}
