using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDF3_Solution3
{
    public partial class FrDetection : Form
    {
        private float scaleFactor = 1.0f; // Hệ số phóng to/thu nhỏ
        private int rotationAngle = 0; // Góc xoay của ảnh
        private System.Drawing.Image originalImage; // Ảnh gốc
        private Point mouseDownLocation;
        private List<string> imageFiles = new List<string>(); // Danh sách ảnh trong thư mục
        private int currentIndex = -1; // Vị trí ảnh hiện tại
        private bool Selected_BoundBox = false;

        private List<BoundingBox> boundingBoxes = new List<BoundingBox>();
        private Point startPoint;
        private Point centerPoint;
        private Rectangle currentRectangle;
        private BoundingBox selectedBox = null;
        private bool isDragging = false;
        private bool isRotating = false;
        private bool isResizing = false;
        private Point resizeStartPoint;
        private Rectangle resizeRectangle;
        private int handleSize = 10;
        public FrDetection()
        {
            InitializeComponent();

            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Chặn cuộn dọc, chỉ cho phép cuộn ngang
            flowPanel.VerticalScroll.Maximum = 0;
            flowPanel.VerticalScroll.Visible = false;
            flowPanel.HorizontalScroll.Visible = true;
            flowPanel.AutoScroll = true;

            // Ngăn cuộn dọc nếu có
            flowPanel.Scroll += (s, e) =>
            {
                flowPanel.VerticalScroll.Value = 0;
            };
        }

        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            SelectAndLoadImages();
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {

        }

        private void tsbtnBoundTool_Click(object sender, EventArgs e)
        {
            if (!Selected_BoundBox)
            {
                Selected_BoundBox = true;
                tsbtnBoundTool.BackColor = Color.Gray;
            }
            else
            {
                Selected_BoundBox = false;
                tsbtnBoundTool.BackColor = Color.White;
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ZoomImage(1.2f);
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ZoomImage(0.8f);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetImage();
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            RotateImage(90);
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            RotateImage(-90);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (imageFiles.Count == 0) return;

            currentIndex = 0;
            LoadImage(imageFiles[currentIndex]);
            UpdateNavigationButtons();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (imageFiles.Count == 0) return;

            if (currentIndex > 0)
            {
                currentIndex--;
                LoadImage(imageFiles[currentIndex]);
            }

            UpdateNavigationButtons();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (imageFiles.Count == 0) return;

            if (currentIndex < imageFiles.Count - 1)
            {
                currentIndex++;
                LoadImage(imageFiles[currentIndex]);
            }

            UpdateNavigationButtons();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (imageFiles.Count == 0) return;

            currentIndex = imageFiles.Count - 1;
            LoadImage(imageFiles[currentIndex]);
            UpdateNavigationButtons();
        }

        private void SelectAndLoadImages()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadThumbnails(dialog.SelectedPath);
                }
            }
        }

        private void LoadThumbnails(string directory)
        {
            flowPanel.Controls.Clear(); // Xóa các ảnh cũ trước khi tải mới
            imageFiles = Directory.GetFiles(directory, "*.*")
                                            .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                        f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                        f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                        f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                                            .ToList();
            currentIndex = -1;

            foreach (string file in imageFiles)
            {
                PictureBox thumb = new PictureBox
                {
                    Image = LoadImageWithoutLocking(file).GetThumbnailImage(80, 80, null, IntPtr.Zero),
                    Width = 90,
                    Height = 90,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = file // Lưu đường dẫn ảnh vào Tag
                };
                thumb.Click += (s, e) => LoadImage(file); // Gán sự kiện click
                flowPanel.Controls.Add(thumb);
            }
            UpdateNavigationButtons(); // Cập nhật trạng thái nút
        }

        // Hàm load ảnh không khóa file
        private System.Drawing.Image LoadImageWithoutLocking(string path)
        {
            using (var img = System.Drawing.Image.FromFile(path))
            {
                return new Bitmap(img);
            }
        }


        private void LoadImage(string filePath)
        {
            originalImage = System.Drawing.Image.FromFile(filePath);
            pictureBox.Image = (System.Drawing.Image)originalImage.Clone(); // Hiển thị ảnh gốc
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            rotationAngle = 0;
            scaleFactor = 1.0f;
            CenterImage(pnlContaner, pictureBox);

            currentIndex = imageFiles.IndexOf(filePath);

            // Tô viền xanh thumbnail tương ứng
            foreach (PictureBox thumb in flowPanel.Controls)
            {
                if (thumb.Tag.ToString() == filePath)
                {
                    thumb.BorderStyle = BorderStyle.Fixed3D; // Tô viền xanh
                    thumb.BackColor = Color.Green;
                }
                else
                {
                    thumb.BorderStyle = BorderStyle.FixedSingle; // Đặt lại viền bình thường
                    thumb.BackColor = SystemColors.Control;
                }
            }

            UpdateNavigationButtons();
        }

        // Cập nhật trạng thái của các nút điều hướng
        private void UpdateNavigationButtons()
        {
            btnPrevious.Enabled = currentIndex > 0;
            btnFirst.Enabled = currentIndex > 0;
            btnNext.Enabled = currentIndex < imageFiles.Count - 1;
            btnLast.Enabled = currentIndex < imageFiles.Count - 1;
        }

        private void RotateImage(int angle)
        {
            if (pictureBox.Image == null) return;
            rotationAngle = (rotationAngle + angle) % 360;
            Console.WriteLine(rotationAngle);
            pictureBox.Image = new Bitmap(originalImage);
            switch (Math.Abs(rotationAngle))
            {
                case 90:
                    pictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 180:
                    pictureBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 270:
                    pictureBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
            }
            pictureBox.Refresh(); // Cập nhật lại giao diện
            KeepImageInPanel(pnlContaner, pictureBox);

        }

        private void ZoomImage(float factor)
        {
            if (pictureBox.Image == null) return;
            scaleFactor *= factor;
            pictureBox.Width = (int)(pictureBox.Image.Width * scaleFactor);
            pictureBox.Height = (int)(pictureBox.Image.Height * scaleFactor);
            KeepImageInPanel(pnlContaner, pictureBox);

        }

        private void ResetImage()
        {

            if (originalImage == null) return;
            pictureBox.Image = (System.Drawing.Image)originalImage.Clone();
            rotationAngle = 0;
            scaleFactor = 1.0f;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CenterImage(pnlContaner, pictureBox);
        }

        // Giữ ảnh luôn trong Panel khi zoom hoặc di chuyển
        private void KeepImageInPanel(Sunny.UI.UIPanel pnl, PictureBox pic)
        {
            if (pic.Image == null) return;

            // Đảm bảo ảnh không ra khỏi Panel
            int x = Math.Max(pnl.Width - pic.Width, 0) / 2;
            int y = Math.Max(pnl.Height - pic.Height, 0) / 2;

            pic.Location = new Point(x, y);
        }

        // Căn giữa ảnh khi reset hoặc load
        private void CenterImage(Sunny.UI.UIPanel pnlContaner, PictureBox Img)
        {
            if (Img.Image == null) return;

            int x = (pnlContaner.Width - Img.Width) / 2;
            int y = (pnlContaner.Height - Img.Height) / 2;

            Img.Location = new Point(x, y);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Selected_BoundBox)
            {
                //Scroll image
                if (e.Button == MouseButtons.Left && pictureBox.Image != null)
                {
                    int newX = pictureBox.Left + (e.X - mouseDownLocation.X);
                    int newY = pictureBox.Top + (e.Y - mouseDownLocation.Y);

                    // Giữ ảnh không vượt ra khỏi panel
                    newX = Math.Min(0, Math.Max(pnlContaner.Width - pictureBox.Width, newX));
                    newY = Math.Min(0, Math.Max(pnlContaner.Height - pictureBox.Height, newY));

                    pictureBox.Location = new Point(newX, newY);
                }
            }
            else
            {
                //Bounding Box
                if (isRotating && selectedBox != null)
                {
                    // Tính góc xoay dựa trên vị trí chuột và điểm trung tâm
                    float centerX = selectedBox.X + selectedBox.Width / 2;
                    float centerY = selectedBox.Y + selectedBox.Height / 2;

                    // Tính góc giữa điểm chuột và trung tâm
                    float deltaX = e.X - centerX;
                    float deltaY = e.Y - centerY;
                    float angle = (float)(Math.Atan2(deltaY, deltaX) * 180.0 / Math.PI);

                    // Điều chỉnh góc để có được góc xoay mong muốn
                    selectedBox.Angle = angle + 90; // Cộng thêm 90 độ để điều chỉnh hướng
                                                    // Cập nhật tọa độ các đỉnh
                    UpdateBoundingBoxVertices(selectedBox);

                    // Cập nhật ListBox
                    UpdateListBox();
                    pictureBox.Invalidate();
                }
                else if (isDragging && selectedBox != null)
                {
                    int dx = e.X - startPoint.X;
                    int dy = e.Y - startPoint.Y;
                    selectedBox.X += dx;
                    selectedBox.Y += dy;
                    startPoint = e.Location;
                    UpdateBoundingBoxVertices(selectedBox);
                    // Cập nhật ListBox
                    UpdateListBox();
                    pictureBox.Invalidate();
                }
                else if (e.Button == MouseButtons.Left)
                {
                    currentRectangle.Width = e.X - startPoint.X;
                    currentRectangle.Height = e.Y - startPoint.Y;
                    pictureBox.Invalidate();
                }
            }
        }

        private void UpdateBoundingBoxVertices(BoundingBox box)
        {
            // Tính tọa độ tâm của Bounding Box
            float centerX = box.X + box.Width / 2f;
            float centerY = box.Y + box.Height / 2f;

            // Chuyển góc từ độ sang radian
            float angleRad = box.Angle * (float)Math.PI / 180f;

            // Tính các đỉnh ban đầu (trước khi xoay)
            PointF topLeft = new PointF(box.X, box.Y);
            PointF topRight = new PointF(box.X + box.Width, box.Y);
            PointF bottomLeft = new PointF(box.X, box.Y + box.Height);
            PointF bottomRight = new PointF(box.X + box.Width, box.Y + box.Height);

            // Tính các đỉnh sau khi xoay
            box.Vertices = new List<PointF>
            {
                RotatePoint(topLeft, centerX, centerY, angleRad),
                RotatePoint(topRight, centerX, centerY, angleRad),
                RotatePoint(bottomLeft, centerX, centerY, angleRad),
                RotatePoint(bottomRight, centerX, centerY, angleRad)
            };
        }

        // Hàm xoay một điểm quanh tâm
        private PointF RotatePoint(PointF point, float cx, float cy, float angleRad)
        {
            float x = point.X;
            float y = point.Y;

            float newX = (float)(Math.Cos(angleRad) * (x - cx) - Math.Sin(angleRad) * (y - cy) + cx);
            float newY = (float)(Math.Sin(angleRad) * (x - cx) + Math.Cos(angleRad) * (y - cy) + cy);

            return new PointF(newX, newY);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Selected_BoundBox)
            {
                //Scroll image
                if (e.Button == MouseButtons.Left)
                {
                    mouseDownLocation = e.Location;
                }
            }
            else
            {
                //Bounding Box
                if (pictureBox.Image == null)
                {
                    MessageBox.Show("No image loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (e.Button == MouseButtons.Left)
                {
                    selectedBox = GetBoundingBoxAtPoint(e.Location);
                    if (selectedBox != null)
                    {
                        Point rotationPoint = GetRotationPoint(selectedBox);
                        if (IsNearRotationPoint(e.Location, rotationPoint))
                        {
                            isRotating = true;
                            isDragging = false;
                        }
                        else
                        {
                            isDragging = true;
                            isRotating = false;
                            startPoint = e.Location;
                        }
                    }
                    else
                    {
                        startPoint = e.Location;
                        currentRectangle = new Rectangle(startPoint.X, startPoint.Y, 0, 0);
                    }
                    pictureBox.Invalidate();
                }
            }
        }
        private BoundingBox GetBoundingBoxAtPoint(Point point)
        {
            foreach (var box in boundingBoxes)
            {
                // Kiểm tra nếu điểm click nằm trong vùng điểm xoay
                Point rotationPoint = GetRotationPoint(box);
                if (Math.Abs(point.X - rotationPoint.X) <= 5 &&
                    Math.Abs(point.Y - rotationPoint.Y) <= 5)
                {
                    return box;
                }

                // Kiểm tra nếu điểm click nằm trong bounding box
                if (box.X <= point.X && point.X <= box.X + box.Width &&
                    box.Y <= point.Y && point.Y <= box.Y + box.Height)
                {
                    return box;
                }
            }
            return null;
        }


        private Point GetRotationPoint(BoundingBox box)
        {
            // Tính điểm trung tâm
            float centerX = box.X + box.Width / 2f;
            float centerY = box.Y + box.Height / 2f;

            // Tính toán điểm xoay dựa trên góc hiện tại
            float angle = box.Angle * (float)Math.PI / 180f; // Chuyển đổi độ sang radian
            float distance = box.Height / 2f + 15; // Khoảng cách từ tâm đến điểm xoay

            // Tính tọa độ điểm xoay sau khi xoay
            float rotatedX = centerX + (float)Math.Sin(angle) * distance;
            float rotatedY = centerY - (float)Math.Cos(angle) * distance;
            return new Point((int)rotatedX, (int)rotatedY);
        }


        private bool IsNearRotationPoint(Point mousePoint, Point rotationPoint, int threshold = 10)
        {
            // Kiểm tra xem chuột có gần điểm xoay không
            int dx = mousePoint.X - rotationPoint.X;
            int dy = mousePoint.Y - rotationPoint.Y;
            return Math.Sqrt(dx * dx + dy * dy) <= threshold;
        }

        private bool IsMouseOnRotateHandle(BoundingBox box, Point mouseLocation)
        {
            int handleSize = 10; // Kích thước điểm điều khiển

            // Tính tọa độ tâm của bounding box
            float centerX = box.X + box.Width / 2;
            float centerY = box.Y + box.Height / 2;

            // Tính tọa độ của điểm điều khiển xoay (trên trục y phía trên bounding box)
            float rotateHandleX = centerX;
            float rotateHandleY = centerY - box.Height / 2 - 15;

            // Nếu bounding box có góc xoay, cần tính lại tọa độ điểm xoay
            if (box.Angle != 0)
            {
                double angleRad = box.Angle * Math.PI / 180.0; // Chuyển góc sang radian
                float offsetX = 0;
                float offsetY = -(box.Height / 2 + 15); // Khoảng cách từ tâm đến điểm xoay

                // Áp dụng phép xoay
                rotateHandleX = centerX + (float)(offsetX * Math.Cos(angleRad) - offsetY * Math.Sin(angleRad));
                rotateHandleY = centerY + (float)(offsetX * Math.Sin(angleRad) + offsetY * Math.Cos(angleRad));
                Console.WriteLine($"Rotate Handle: ({rotateHandleX}, {rotateHandleY})");
                Console.WriteLine($"Mouse Location: ({mouseLocation.X}, {mouseLocation.Y})");
            }

            // Kiểm tra xem chuột có nằm gần điểm xoay không
            return true;
        }
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Graphics g = e.Graphics;

            if (currentRectangle != Rectangle.Empty)
            {
                e.Graphics.DrawRectangle(Pens.Red, currentRectangle);
            }

            foreach (var box in boundingBoxes)
            {
                // Lưu trạng thái transform
                var state = e.Graphics.Save();

                float scaleX = pictureBox.Width / originalImage.Width;
                float scaleY = pictureBox.Height / originalImage.Height;
                // Tính điểm trung tâm
                float centerX = (box.X + box.Width / 2) * scaleX;
                float centerY = (box.Y + box.Height / 2) * scaleY;

                // Áp dụng transform cho bounding box
                e.Graphics.TranslateTransform(centerX, centerY);
                e.Graphics.RotateTransform(box.Angle);

                // Vẽ bounding box
                e.Graphics.DrawRectangle(Pens.Blue,
                    (-box.Width / 2f) * scaleX,
                    (-box.Height / 2f) * scaleY,
                    box.Width * scaleX,
                    box.Height * scaleY);

                // Vẽ điểm xoay (điểm đỏ) với cùng transform
                using (SolidBrush brush = new SolidBrush(Color.YellowGreen))
                {
                    e.Graphics.FillEllipse(brush,
                        0,              // Điểm giữa theo chiều ngang
                        -box.Height / 2 - 15,  // Điểm trên cùng của box
                        10, 10);
                }

                if (box == selectedBox)
                {
                    g.DrawRectangle(Pens.White,
                    -box.Width / 2f,
                    -box.Height / 2f,
                    box.Width,
                    box.Height);

                    // Vẽ hình chữ nhật nhỏ (handle) ở góc trên bên trái của hình chữ nhật lớn
                    g.DrawRectangle(Pens.White,
                                    -box.Width / 2f - handleSize / 2f,
                                    -box.Height / 2f - handleSize / 2f,
                                    handleSize,
                                    handleSize);

                    // Vẽ hình chữ nhật nhỏ (handle) ở góc trên bên phải của hình chữ nhật lớn
                    g.DrawRectangle(Pens.White,
                                    box.Width / 2f - handleSize / 2f,
                                    -box.Height / 2f - handleSize / 2f,
                                    handleSize,
                                    handleSize);

                    // Vẽ hình chữ nhật nhỏ (handle) ở góc dưới bên trái của hình chữ nhật lớn
                    g.DrawRectangle(Pens.White,
                                    -box.Width / 2f - handleSize / 2f,
                                    box.Height / 2f - handleSize / 2f,
                                    handleSize,
                                    handleSize);

                    // Vẽ hình chữ nhật nhỏ (handle) ở góc dưới bên phải của hình chữ nhật lớn
                    g.DrawRectangle(Pens.White,
                                    box.Width / 2f - handleSize / 2f,
                                    box.Height / 2f - handleSize / 2f,
                                    handleSize,
                                    handleSize);

                    // Vẽ hình chữ nhật nhỏ (handle) ở điểm giữa trên (Top Center)
                    g.DrawRectangle(Pens.White,
                                    0, // X = 0 vì là giữa
                                    -box.Height / 2f - handleSize / 2f, // Y = Top Center
                                    handleSize,
                                    handleSize);

                    // Vẽ hình chữ nhật nhỏ (handle) ở điểm giữa trái (Left Center)
                    g.DrawRectangle(Pens.White,
                                    -box.Width / 2f - handleSize / 2f, // X = Left Center
                                    0, // Y = 0 vì là giữa
                                    handleSize,
                                    handleSize);

                    // Vẽ hình chữ nhật nhỏ (handle) ở điểm giữa phải (Right Center)
                    g.DrawRectangle(Pens.White,
                                    box.Width / 2f - handleSize / 2f, // X = Right Center
                                    0, // Y = 0 vì là giữa
                                    handleSize,
                                    handleSize);

                    // Vẽ hình chữ nhật nhỏ (handle) ở điểm giữa dưới (Bottom Center)
                    g.DrawRectangle(Pens.White,
                                    0, // X = 0 vì là giữa
                                    box.Height / 2f - handleSize / 2f, // Y = Bottom Center
                                    handleSize,
                                    handleSize);
                }

                // Khôi phục transform
                UpdateListBox();
                e.Graphics.Restore(state);
            }
        }
        private void UpdateListBox()
        {
            uiListBox1.Items.Clear(); // Xóa toàn bộ nội dung cũ trong ListBox

            foreach (var box in boundingBoxes)
            {
                // Hiển thị thông tin chính của Bounding Box
                uiListBox1.Items.Add($"ImagePath: {box.imgpath}");
            }
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var box in boundingBoxes)
            {
                // Kiểm tra nếu chuột nhấp vào bên trong BBox
                if (e.X >= box.X && e.X <= box.X + box.Width &&
                    e.Y >= box.Y && e.Y <= box.Y + box.Height)
                {
                    selectedBox = box; // Lưu BBox đang được chọn
                    pictureBox.Invalidate(); // Vẽ lại để hiển thị trạng thái chọn
                    return;
                }
            }

            selectedBox = null; // Không chọn BBox nào
            pictureBox.Invalidate();
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox.Image == null)
            {
                MessageBox.Show("Please select a image before drawing a bounding box.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Không thêm Bounding Box nếu nhãn chưa được chọn
            }
            if (isDragging)
            {
                isDragging = false; // Kết thúc kéo
            }
            else if (isRotating)
            {
                isRotating = false; // Kết thúc xoay
            }
            else if (currentRectangle.Width > 0 && currentRectangle.Height > 0)
            {
                string imgpath = imageFiles[currentIndex];
                Console.WriteLine(imgpath); 
                boundingBoxes.Add(new BoundingBox
                {
                    imgpath = imageFiles[currentIndex],
                    X = currentRectangle.X,
                    Y = currentRectangle.Y,
                    Width = currentRectangle.Width,
                    Height = currentRectangle.Height,
                    Angle = 0, // Góc ban đầu
                    Vertices = new List<PointF>
                    {
                        new PointF(currentRectangle.X, currentRectangle.Y), // Góc trên bên trái
                        new PointF(currentRectangle.X + currentRectangle.Width, currentRectangle.Y), // Góc trên bên phải
                        new PointF(currentRectangle.X + currentRectangle.Width, currentRectangle.Y + currentRectangle.Height), // Góc dưới bên phải
                        new PointF(currentRectangle.X, currentRectangle.Y + currentRectangle.Height) // Góc dưới bên trái
                    }
                }); ;


            }

            currentRectangle = Rectangle.Empty;

            pictureBox.Invalidate();
        }

        private void uiListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiListBox1.SelectedIndex == -1)
            {
                selectedBox = null;
                pictureBox.Invalidate();
                return;
            }
            selectedBox = boundingBoxes[uiListBox1.SelectedIndex];
            pictureBox.Invalidate();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedBox != null)
            {
                boundingBoxes.Remove(selectedBox); // Xóa BBox khỏi danh sách
                selectedBox = null; // Bỏ chọn
                pictureBox.Invalidate(); // Vẽ lại giao diện
            }
        }

        private void btnTraning_Click(object sender, EventArgs e)
        {

        }
    }

    public class BoundingBox
    {
        public string imgpath { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Angle { get; set; }
        public List<PointF> Vertices { get; set; } = new List<PointF>(); // Danh sách tọa độ các đỉnh
    }
}
