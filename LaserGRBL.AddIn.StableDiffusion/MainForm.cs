using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LaserGRBL.AddIn.StableDiffusion
{
    public partial class MainForm : Form
    {
        private Main mAddIn;
        private int mLastFileIndex = 0;
        private string mSelectedFile = null;

        public MainForm(Main main)
        {
            InitializeComponent();
            mAddIn = main;
            if (!Directory.Exists(mAddIn.TempFolder)) Directory.CreateDirectory(mAddIn.TempFolder);
            string[] files = Directory.GetFiles(mAddIn.TempFolder, "*.png");
            foreach (string file in files)
            {
                mLastFileIndex = Math.Max(mLastFileIndex, int.Parse(Path.GetFileNameWithoutExtension(file)));
            }
        }

        public string Filename => mSelectedFile;

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            StableDiffusionClient.GenerateImage(txtPrompt.Text, CompletedCallback, ProgressCallback);
        }

        private void CompletedCallback(Bitmap bmp)
        {
            BeginInvoke(new Action(() => {
                mSelectedFile = Path.Combine(mAddIn.TempFolder, $"{++mLastFileIndex}.png");
                bmp.Save(mSelectedFile);
                if (pcbImage.Image != null) pcbImage.Image.Dispose();
                pcbImage.Image = bmp;
            }));
        }

        private void ProgressCallback(StableDiffusionClient.AIProgress progress)
        {
            BeginInvoke(new Action(() =>
            {
                prbProgress.Value = Convert.ToInt32(progress.progress * 100);
            }));
        }
    }
}
