using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace openCV
{
    public partial class Form1 : Form
    {
        private VideoCapture? capture = null;
        private Mat? frame = null;
        private System.Windows.Forms.Timer timer;
        private bool isCameraRunning = false;
        private Stopwatch fpsTimer = new Stopwatch();
        private int frameCount = 0;

        // Son iþlenmiþ görüntünün güvenli saklamasý
        private Bitmap? lastProcessedBitmap = null;
        private readonly object imageLock = new object();

        public Form1()
        {
            InitializeComponent();

            // Buton/metin baþlangýç ayarlarý (Designer ile eþleþir)
            btnStartCamera.Text = "Kamera Baþlat";
            btnStopCamera.Text = "Kamera Durdur";
            btnStopCamera.Enabled = false;
            btnSaveSnapshot.Text = "Kare Kaydet";

            cbMode.Items.AddRange(new object[] { "None", "Grayscale", "Canny", "Threshold", "AdaptiveThreshold", "Blur" });
            cbMode.SelectedIndex = 1;

            numCameraIndex.Minimum = 0;
            numCameraIndex.Maximum = 10;
            numCameraIndex.Value = 0;

            frame = new Mat();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 33; // ~30 FPS
            timer.Tick += Timer_Tick;

            // Kullanýcýya açýklama için ToolTip
            var tt = new ToolTip();
            tt.SetToolTip(cbMode, "Ýþlem seçin: Grayscale, Canny, Threshold, AdaptiveThreshold, Blur");
            tt.SetToolTip(numParam1, "Canny: Düþük Eþik / Threshold: Eþik deðeri / Adaptive: Blok boyutu / Blur: Filtre boyutu");
            tt.SetToolTip(numParam2, "Canny: Yüksek Eþik / Adaptive: C deðeri");

            UpdateControlsVisibility();
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            StopCameraStream();

            try { frame?.Dispose(); } catch { }
            try { timer?.Dispose(); } catch { }
            try { capture?.Dispose(); } catch { }

            lock (imageLock)
            {
                try { lastProcessedBitmap?.Dispose(); } catch { }
                lastProcessedBitmap = null;
            }
        }

        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            if (isCameraRunning) return;

            try
            {
                int camIndex = (int)numCameraIndex.Value;
                capture?.Release();
                capture?.Dispose();
                capture = new VideoCapture(camIndex);

                if (!capture.IsOpened())
                {
                    MessageBox.Show("Web kamera açýlamadý. Kamera indeksini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    capture?.Dispose();
                    capture = null;
                    return;
                }

                timer.Start();
                isCameraRunning = true;
                btnStartCamera.Enabled = false;
                btnStopCamera.Enabled = true;
                fpsTimer.Restart();
                frameCount = 0;
                lblFps.Text = "FPS: 0";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kamera baþlatýlamadý: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            StopCameraStream();
        }

        private void StopCameraStream()
        {
            try { timer?.Stop(); } catch { }

            if (capture != null)
            {
                try { capture.Release(); } catch { }
                try { capture.Dispose(); } catch { }
                capture = null;
            }

            DisposePictureBoxImage(pbOriginal);
            DisposePictureBoxImage(pbProcessed);

            lock (imageLock)
            {
                try { lastProcessedBitmap?.Dispose(); } catch { }
                lastProcessedBitmap = null;
            }

            isCameraRunning = false;
            btnStartCamera.Enabled = true;
            btnStopCamera.Enabled = false;
            fpsTimer.Reset();
            lblFps.Text = "FPS: 0";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (capture == null || !capture.IsOpened()) return;
            if (frame == null) frame = new Mat();
            if (!capture.Read(frame) || frame.Empty()) return;

            // Orijinal görüntü
            Bitmap originalBmp = BitmapConverter.ToBitmap(frame);
            SetPictureBoxImage(pbOriginal, originalBmp);

            // Ýþlenmiþ görüntü
            Bitmap processedBmp = ProcessFrame(frame);
            SetPictureBoxImage(pbProcessed, processedBmp);

            // FPS hesaplama
            frameCount++;
            if (fpsTimer.ElapsedMilliseconds >= 1000)
            {
                lblFps.Text = $"FPS: {frameCount}";
                frameCount = 0;
                fpsTimer.Restart();
            }
        }

        private Bitmap ProcessFrame(Mat src)
        {
            string mode = cbMode.SelectedItem?.ToString() ?? "None";

            switch (mode)
            {
                case "Grayscale":
                    using (Mat gray = new Mat())
                    {
                        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                        return BitmapConverter.ToBitmap(gray);
                    }

                case "Canny":
                    using (Mat gray = new Mat())
                    using (Mat edges = new Mat())
                    {
                        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                        int th1 = (int)numParam1.Value; // Düþük Eþik
                        int th2 = (int)numParam2.Value; // Yüksek Eþik
                        Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(5, 5), 1.5);
                        Cv2.Canny(gray, edges, th1, th2);
                        return BitmapConverter.ToBitmap(edges);
                    }

                case "Threshold":
                    using (Mat gray = new Mat())
                    using (Mat th = new Mat())
                    {
                        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                        double t = (double)numParam1.Value; // Eþik
                        Cv2.Threshold(gray, th, t, 255, ThresholdTypes.Binary);
                        return BitmapConverter.ToBitmap(th);
                    }

                case "AdaptiveThreshold":
                    using (Mat gray = new Mat())
                    using (Mat th = new Mat())
                    {
                        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                        int blockSize = (int)numParam1.Value;
                        if (blockSize % 2 == 0) blockSize = Math.Max(3, blockSize + 1);
                        int C = (int)numParam2.Value;
                        Cv2.AdaptiveThreshold(gray, th, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, blockSize, C);
                        return BitmapConverter.ToBitmap(th);
                    }

                case "Blur":
                    using (Mat blurred = new Mat())
                    {
                        int k = (int)numParam1.Value;
                        if (k % 2 == 0) k = Math.Max(1, k + 1);
                        Cv2.GaussianBlur(src, blurred, new OpenCvSharp.Size(k, k), 0);
                        return BitmapConverter.ToBitmap(blurred);
                    }

                default:
                    return BitmapConverter.ToBitmap(src);
            }
        }

        // Güvenli atama: pb.Image olarak 32bpp baðýmsýz bir Bitmap koyar ve
        // pbProcessed için lastProcessedBitmap alanýný günceller.
        private void SetPictureBoxImage(PictureBox pb, Bitmap bmp)
        {
            if (bmp == null) return;
            Bitmap? safe = null;
            try
            {
                safe = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(safe))
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);

                lock (imageLock)
                {
                    var old = pb.Image as Bitmap;
                    pb.Image = safe;
                    old?.Dispose();

                    if (pb == pbProcessed)
                    {
                        try { lastProcessedBitmap?.Dispose(); } catch { }
                        lastProcessedBitmap = (Bitmap)safe.Clone();
                    }

                    safe = null; // pb.Image artýk safe'i refere ediyor
                }
            }
            finally
            {
                try { bmp.Dispose(); } catch { }
                try { safe?.Dispose(); } catch { }
            }
        }

        private void DisposePictureBoxImage(PictureBox pb)
        {
            lock (imageLock)
            {
                var old = pb.Image as Bitmap;
                pb.Image = null;
                old?.Dispose();
            }
        }

        private void btnSaveSnapshot_Click(object sender, EventArgs e)
        {
            Bitmap? toSave = null;

            lock (imageLock)
            {
                if (lastProcessedBitmap != null)
                {
                    try { toSave = (Bitmap)lastProcessedBitmap.Clone(); } catch { toSave = null; }
                }
                else if (pbOriginal.Image is Bitmap b)
                {
                    try { toSave = (Bitmap)b.Clone(); } catch { toSave = null; }
                }
            }

            if (toSave == null)
            {
                MessageBox.Show("Kaydetmek için geçerli bir görüntü yok veya görüntü hatalý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg";
                sfd.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "snapshot.png");
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var ext = Path.GetExtension(sfd.FileName).ToLowerInvariant();
                        var format = (ext == ".jpg" || ext == ".jpeg") ? ImageFormat.Jpeg : ImageFormat.Png;
                        toSave.Save(sfd.FileName, format);
                        MessageBox.Show("Kaydetme baþarýlý.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Kaydetme sýrasýnda hata:\n" + ex.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally { toSave.Dispose(); }
                }
                else { toSave.Dispose(); }
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlsVisibility();
        }

        private void numParam_ValueChanged(object? sender, EventArgs e)
        {
            // Parametre deðiþtiðinde timer üzerinden güncellenecek
        }

        private void UpdateControlsVisibility()
        {
            string mode = cbMode.SelectedItem?.ToString() ?? "None";

            // Önce param kontrollerini gizle
            lblParam1.Visible = numParam1.Visible = false;
            lblParam2.Visible = numParam2.Visible = false;

            switch (mode)
            {
                case "Canny":
                    lblParam1.Text = "Düþük Eþik";
                    lblParam2.Text = "Yüksek Eþik";
                    lblParam1.Visible = numParam1.Visible = true;
                    lblParam2.Visible = numParam2.Visible = true;
                    numParam1.Minimum = 0; numParam1.Maximum = 500; numParam1.Value = 50;
                    numParam2.Minimum = 0; numParam2.Maximum = 500; numParam2.Value = 150;
                    break;

                case "Threshold":
                    lblParam1.Text = "Eþik";
                    lblParam1.Visible = numParam1.Visible = true;
                    numParam1.Minimum = 0; numParam1.Maximum = 255; numParam1.Value = 127;
                    break;

                case "AdaptiveThreshold":
                    lblParam1.Text = "Blok Boyutu (tek)";
                    lblParam2.Text = "C deðeri";
                    lblParam1.Visible = numParam1.Visible = true;
                    lblParam2.Visible = numParam2.Visible = true;
                    numParam1.Minimum = 3; numParam1.Maximum = 99; numParam1.Value = 11;
                    numParam2.Minimum = -50; numParam2.Maximum = 50; numParam2.Value = 2;
                    break;

                case "Blur":
                    lblParam1.Text = "Filtre Boyutu (tek)";
                    lblParam1.Visible = numParam1.Visible = true;
                    numParam1.Minimum = 1; numParam1.Maximum = 31; numParam1.Value = 5;
                    break;

                default:
                    break;
            }
        }

        private void LblOriginal_Click(object sender, EventArgs e)
        {
            
        }
    }
}