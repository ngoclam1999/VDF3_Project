using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageProcessingLib;

namespace VDF3_Solution3
{
    
    public partial class FrPickTeaching : Form
    {
        private ImageProcessor imageProcessor;
        private ROI selectedROI;
        private bool isSelecting = false;
        private Point startPoint;
        public FrPickTeaching()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imageProcessor = new ImageProcessor(ofd.FileName);
                    pictureBox.Image = imageProcessor.GetImage();
                }
            }
        }
        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (imageProcessor == null) return;

            isSelecting = true;
            startPoint = e.Location;
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                int x = Math.Min(startPoint.X, e.X);
                int y = Math.Min(startPoint.Y, e.Y);
                int width = Math.Abs(e.X - startPoint.X);
                int height = Math.Abs(e.Y - startPoint.Y);
                selectedROI = new ROI(x, y, width, height);
                pictureBox.Invalidate();
            }
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            isSelecting = false;
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            if (selectedROI != null)
            {
                e.Graphics.DrawRectangle(Pens.Blue, new Rectangle(selectedROI.X, selectedROI.Y, selectedROI.Width, selectedROI.Height));
            }
        }
        private void tsbtnFindROI_Click(object sender, EventArgs e)
        {
            if (imageProcessor == null || selectedROI == null)
            {
                MessageBox.Show("Vui lòng chọn vùng ROI trước!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap resultImage = imageProcessor.FindMatchingRegions(selectedROI);
            pictureBox.Image = resultImage;

            // Hiển thị kết quả lên ListView
            lstResults.Items.Clear();
            foreach (var region in imageProcessor.DetectedRegions)
            {
                ListViewItem item = new ListViewItem(new[]
                {
                    region.Center.X.ToString("0"),
                    region.Center.Y.ToString("0"),
                    imageProcessor.DetectedScores[region].ToString("0.00")
                });
                lstResults.Items.Add(item);
            }
        }
    }
}
