namespace openCV
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            btnStartCamera = new Button();
            btnStopCamera = new Button();
            btnSaveSnapshot = new Button();
            cbMode = new ComboBox();
            lblMode = new Label();
            numParam1 = new NumericUpDown();
            lblParam1 = new Label();
            numParam2 = new NumericUpDown();
            lblParam2 = new Label();
            numCameraIndex = new NumericUpDown();
            lblCamera = new Label();
            pbOriginal = new PictureBox();
            pbProcessed = new PictureBox();
            lblOriginal = new Label();
            lblProcessed = new Label();
            lblFps = new Label();
            SuspendLayout();
            // 
            // btnStartCamera
            // 
            btnStartCamera.Location = new Point(29, 52);
            btnStartCamera.Name = "btnStartCamera";
            btnStartCamera.Size = new Size(110, 29);
            btnStartCamera.TabIndex = 0;
            btnStartCamera.Text = "Kamera Başlat";
            btnStartCamera.UseVisualStyleBackColor = true;
            btnStartCamera.Click += btnStartCamera_Click;
            // 
            // btnStopCamera
            // 
            btnStopCamera.Location = new Point(150, 52);
            btnStopCamera.Name = "btnStopCamera";
            btnStopCamera.Size = new Size(110, 29);
            btnStopCamera.TabIndex = 1;
            btnStopCamera.Text = "Kamera Durdur";
            btnStopCamera.UseVisualStyleBackColor = true;
            btnStopCamera.Click += btnStopCamera_Click;
            // 
            // btnSaveSnapshot
            // 
            btnSaveSnapshot.Location = new Point(271, 52);
            btnSaveSnapshot.Name = "btnSaveSnapshot";
            btnSaveSnapshot.Size = new Size(110, 29);
            btnSaveSnapshot.TabIndex = 2;
            btnSaveSnapshot.Text = "Kare Kaydet";
            btnSaveSnapshot.UseVisualStyleBackColor = true;
            btnSaveSnapshot.Click += btnSaveSnapshot_Click;
            // 
            // cbMode
            // 
            cbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMode.FormattingEnabled = true;
            cbMode.Location = new Point(29, 100);
            cbMode.Name = "cbMode";
            cbMode.Size = new Size(220, 28);
            cbMode.TabIndex = 3;
            cbMode.SelectedIndexChanged += cbMode_SelectedIndexChanged;
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.Location = new Point(29, 80);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(72, 20);
            lblMode.TabIndex = 4;
            lblMode.Text = "İşlem Seç";
            // 
            // numParam1
            // 
            numParam1.Location = new Point(29, 150);
            numParam1.Name = "numParam1";
            numParam1.Size = new Size(90, 27);
            numParam1.TabIndex = 5;
            numParam1.ValueChanged += numParam_ValueChanged;
            // 
            // lblParam1
            // 
            lblParam1.AutoSize = true;
            lblParam1.Location = new Point(29, 130);
            lblParam1.Name = "lblParam1";
            lblParam1.Size = new Size(70, 20);
            lblParam1.TabIndex = 6;
            lblParam1.Text = "Düşük Eşik";
            // 
            // numParam2
            // 
            numParam2.Location = new Point(140, 150);
            numParam2.Name = "numParam2";
            numParam2.Size = new Size(90, 27);
            numParam2.TabIndex = 7;
            numParam2.ValueChanged += numParam_ValueChanged;
            // 
            // lblParam2
            // 
            lblParam2.AutoSize = true;
            lblParam2.Location = new Point(140, 130);
            lblParam2.Name = "lblParam2";
            lblParam2.Size = new Size(80, 20);
            lblParam2.TabIndex = 8;
            lblParam2.Text = "Yüksek Eşik";
            // 
            // numCameraIndex
            // 
            numCameraIndex.Location = new Point(271, 100);
            numCameraIndex.Name = "numCameraIndex";
            numCameraIndex.Size = new Size(110, 27);
            numCameraIndex.TabIndex = 9;
            // 
            // lblCamera
            // 
            lblCamera.AutoSize = true;
            lblCamera.Location = new Point(271, 80);
            lblCamera.Name = "lblCamera";
            lblCamera.Size = new Size(69, 20);
            lblCamera.TabIndex = 10;
            lblCamera.Text = "Kamera #";
            // 
            // pbOriginal
            // 
            pbOriginal.Location = new Point(29, 200);
            pbOriginal.Name = "pbOriginal";
            pbOriginal.Size = new Size(360, 240);
            pbOriginal.TabIndex = 11;
            pbOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pbOriginal.BorderStyle = BorderStyle.FixedSingle;
            // 
            // pbProcessed
            // 
            pbProcessed.Location = new Point(410, 200);
            pbProcessed.Name = "pbProcessed";
            pbProcessed.Size = new Size(360, 240);
            pbProcessed.TabIndex = 12;
            pbProcessed.SizeMode = PictureBoxSizeMode.Zoom;
            pbProcessed.BorderStyle = BorderStyle.FixedSingle;
            // 
            // lblOriginal
            // 
            lblOriginal.AutoSize = true;
            lblOriginal.Location = new Point(29, 452);
            lblOriginal.Name = "lblOriginal";
            lblOriginal.Size = new Size(114, 20);
            lblOriginal.TabIndex = 13;
            lblOriginal.Text = "Orijinal Görüntü";
            // 
            // lblProcessed
            // 
            lblProcessed.AutoSize = true;
            lblProcessed.Location = new Point(410, 452);
            lblProcessed.Name = "lblProcessed";
            lblProcessed.Size = new Size(119, 20);
            lblProcessed.TabIndex = 14;
            lblProcessed.Text = "İşlenmiş Görüntü";
            // 
            // lblFps
            // 
            lblFps.AutoSize = true;
            lblFps.Location = new Point(700, 20);
            lblFps.Name = "lblFps";
            lblFps.Size = new Size(60, 20);
            lblFps.TabIndex = 15;
            lblFps.Text = "FPS: 0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(lblFps);
            Controls.Add(lblProcessed);
            Controls.Add(lblOriginal);
            Controls.Add(pbProcessed);
            Controls.Add(pbOriginal);
            Controls.Add(lblCamera);
            Controls.Add(numCameraIndex);
            Controls.Add(lblParam2);
            Controls.Add(numParam2);
            Controls.Add(lblParam1);
            Controls.Add(numParam1);
            Controls.Add(lblMode);
            Controls.Add(cbMode);
            Controls.Add(btnSaveSnapshot);
            Controls.Add(btnStopCamera);
            Controls.Add(btnStartCamera);
            Name = "Form1";
            Text = "OpenCV - Canlı İşleme";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStartCamera;
        private Button btnStopCamera;
        private Button btnSaveSnapshot;
        private ComboBox cbMode;
        private Label lblMode;
        private NumericUpDown numParam1;
        private Label lblParam1;
        private NumericUpDown numParam2;
        private Label lblParam2;
        private NumericUpDown numCameraIndex;
        private Label lblCamera;
        private PictureBox pbOriginal;
        private PictureBox pbProcessed;
        private Label lblOriginal;
        private Label lblProcessed;
        private Label lblFps;
    }
}
