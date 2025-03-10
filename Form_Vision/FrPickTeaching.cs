using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.IO;

namespace VDF3_Solution3
{
    
    ///<summary>
    /// Represents the form for pick teaching.
    /// </summary>
    public partial class FrPickTeaching : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrPickTeaching"/> class.
        /// </summary>
        public FrPickTeaching()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        /// <summary>
        /// Handles the Click event of the btnLoadImg control.
        /// Loads an image and draws bounding boxes on it.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            List<BoundingBox> boxes = new List<BoundingBox>()
                {
                    new BoundingBox() { X = 308, Y = 136, Width = 79, Height = 98, Angle = 0 },
                    new BoundingBox() { X = 415, Y = 263, Width = 81, Height = 99, Angle = -7 },
                    new BoundingBox() { X = 178, Y = 180, Width = 80, Height = 105, Angle = 255 }
                };
            LoadImageAndDrawBoundingBoxes(pictureBox, "C:\\Users\\trila\\Downloads\\arranged.jpg", boxes);
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tsbtnFindROI_Click(object sender, EventArgs e)
        {

        }

        private void tsbtnReadImg_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Loads an image from the specified path and draws the provided bounding boxes on it.
        /// </summary>
        /// <param name="picBox">The PictureBox control to display the image with bounding boxes.</param>
        /// <param name="imagePath">The path to the image file.</param>
        /// <param name="boundingBoxes">The list of bounding boxes to draw on the image.</param>
        public static void LoadImageAndDrawBoundingBoxes(PictureBox picBox, string imagePath, List<BoundingBox> boundingBoxes)
        {
            if (!File.Exists(imagePath))
            {
                MessageBox.Show("Không tìm thấy ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load the original image
            Image originalImage = Image.FromFile(imagePath);
            Bitmap bitmap = new Bitmap(originalImage);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                foreach (var box in boundingBoxes)
                {
                    // Calculate the center coordinates of the bounding box
                    float centerX = box.X + box.Width / 2f;
                    float centerY = box.Y + box.Height / 2f;
                    float angleRad = box.Angle * (float)Math.PI / 180f;

                    // Define the four corners of the bounding box before rotation
                    PointF[] points = new PointF[]
                    {
                            new PointF(box.X, box.Y),
                            new PointF(box.X + box.Width, box.Y),
                            new PointF(box.X + box.Width, box.Y + box.Height),
                            new PointF(box.X, box.Y + box.Height)
                    };

                    // Rotate the points around the center
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = RotatePoint(points[i], centerX, centerY, angleRad);
                    }

                    // Draw the rotated bounding box
                    using (Pen pen = new Pen(Color.Red, 2))
                    {
                        g.DrawPolygon(pen, points);
                    }
                }
            }

            // Display the image with bounding boxes on the PictureBox
            picBox.Image = bitmap;
            picBox.SizeMode = PictureBoxSizeMode.Zoom;
        }

        /// <summary>
        /// Rotates a point around a specified center by a given angle in radians.
        /// </summary>
        /// <param name="point">The point to rotate.</param>
        /// <param name="cx">The x-coordinate of the center of rotation.</param>
        /// <param name="cy">The y-coordinate of the center of rotation.</param>
        /// <param name="angleRad">The angle of rotation in radians.</param>
        /// <returns>The rotated point.</returns>
        private static PointF RotatePoint(PointF point, float cx, float cy, float angleRad)
        {
            float x = point.X - cx;
            float y = point.Y - cy;

            float newX = (float)(Math.Cos(angleRad) * x - Math.Sin(angleRad) * y + cx);
            float newY = (float)(Math.Sin(angleRad) * x + Math.Cos(angleRad) * y + cy);

            return new PointF(newX, newY);
        }
    }
}
/// <summary>
/// Represents a bounding box with position, size, and rotation angle.
/// </summary>
public class BoundingBox
{
    /// <summary>
    /// Gets or sets the x-coordinate of the top-left corner of the bounding box.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of the top-left corner of the bounding box.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Gets or sets the width of the bounding box.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the bounding box.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the rotation angle of the bounding box in degrees.
    /// </summary>
    public float Angle { get; set; }
}
