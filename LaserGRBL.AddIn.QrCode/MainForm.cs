using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LaserGRBL.AddIn.QrCode
{

    public partial class MainForm : Form
    {
        private readonly string mBaseFolder;
        private readonly List<string> mImages = new List<string>();
        private int mIndex = 0;
        public string Filename { get; private set; }

        public MainForm(Main main)
        {
            InitializeComponent();
            // get the folder where the images are stored
            Assembly assembly = Assembly.GetCallingAssembly();
            mBaseFolder = Path.Combine(Path.GetDirectoryName(assembly.Location), "Images");
            if (!Directory.Exists(mBaseFolder))
            {
                Directory.CreateDirectory(mBaseFolder);
            }
            // get all images in the folder ordered by creation date
            DirectoryInfo info = new DirectoryInfo(mBaseFolder);
            FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                mImages.Add(file.FullName);
            }
            // load the last image
            SelectImage(mImages.Count - 1);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(txtPrompt.Text, QRCodeGenerator.ECCLevel.Q))
            using (BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData))
            {
                byte[] qrCodeImage = qrCode.GetGraphic(20);
                using (var ms = new MemoryStream(qrCodeImage))
                {
                    Filename = Path.Combine(mBaseFolder, Path.ChangeExtension(Path.GetRandomFileName(), ".jpg"));
                    Image img = Image.FromStream(ms);
                    img.SetComments(txtPrompt.Text);
                    img.SetCopyright();
                    img.Save(Filename, ImageFormat.Jpeg);
                    mImages.Add(Filename);
                    SelectImage(mImages.Count - 1);
                }
            }
        }

        private void SelectImage(int index)
        {
            if (index < 0 || index >= mImages.Count)
            {
                return;
            }
            btnPrevImage.Enabled = index > 0;
            btnNextImage.Enabled = index < mImages.Count - 1;
            mIndex = index;
            Filename = mImages[mIndex];
            Image image = Image.FromFile(Filename);
            txtPrompt.Text = image.GetComments();
            txtPrompt.ReadOnly = true;
            pcbImage.Image = image;
        }

        private void btnPrevImage_Click(object sender, EventArgs e) => SelectImage(mIndex - 1);

        private void btnNextImage_Click(object sender, EventArgs e) => SelectImage(mIndex + 1);

        private void txtPrompt_TextChanged(object sender, EventArgs e)
        {
            pcbImage.Image = null;
        }

        private void txtPrompt_Click(object sender, EventArgs e)
        {
            txtPrompt.ReadOnly = false;
        }
    }
}
