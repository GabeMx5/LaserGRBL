namespace LaserGRBL.AddIn.StableDiffusion
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnNext = new System.Windows.Forms.Button();
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.prbProgress = new System.Windows.Forms.ProgressBar();
            this.pcbImage = new System.Windows.Forms.PictureBox();
            this.pnlMain.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.ColumnCount = 1;
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.Controls.Add(this.pnlBottom, 0, 1);
            this.pnlMain.Controls.Add(this.pcbImage, 0, 0);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.RowCount = 2;
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.pnlMain.Size = new System.Drawing.Size(658, 454);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.ColumnCount = 2;
            this.pnlBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBottom.Controls.Add(this.btnNext, 1, 1);
            this.pnlBottom.Controls.Add(this.txtPrompt, 0, 0);
            this.pnlBottom.Controls.Add(this.btnGenerate, 0, 1);
            this.pnlBottom.Controls.Add(this.prbProgress, 0, 2);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(3, 327);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.RowCount = 3;
            this.pnlBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlBottom.Size = new System.Drawing.Size(652, 124);
            this.pnlBottom.TabIndex = 0;
            // 
            // btnNext
            // 
            this.btnNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNext.Location = new System.Drawing.Point(329, 67);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(320, 24);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // txtPrompt
            // 
            this.pnlBottom.SetColumnSpan(this.txtPrompt, 2);
            this.txtPrompt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrompt.Location = new System.Drawing.Point(3, 3);
            this.txtPrompt.Multiline = true;
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.Size = new System.Drawing.Size(646, 58);
            this.txtPrompt.TabIndex = 0;
            this.txtPrompt.Text = "An image of rose of color from list white, red, pink, or black";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGenerate.Location = new System.Drawing.Point(3, 67);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(320, 24);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate image";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // prbProgress
            // 
            this.pnlBottom.SetColumnSpan(this.prbProgress, 2);
            this.prbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prbProgress.Location = new System.Drawing.Point(3, 97);
            this.prbProgress.Name = "prbProgress";
            this.prbProgress.Size = new System.Drawing.Size(646, 24);
            this.prbProgress.TabIndex = 3;
            // 
            // pcbImage
            // 
            this.pcbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcbImage.Location = new System.Drawing.Point(3, 3);
            this.pcbImage.Name = "pcbImage";
            this.pcbImage.Size = new System.Drawing.Size(652, 318);
            this.pcbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbImage.TabIndex = 1;
            this.pcbImage.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(658, 454);
            this.Controls.Add(this.pnlMain);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stable diffusion";
            this.pnlMain.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlMain;
        private System.Windows.Forms.TableLayoutPanel pnlBottom;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.PictureBox pcbImage;
        private System.Windows.Forms.ProgressBar prbProgress;
    }
}