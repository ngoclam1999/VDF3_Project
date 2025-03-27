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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using MvCameraControl;

namespace VDF3_Solution3
{
    public partial class FrDetection : Form
    {
        private Transform imageTransform = new Transform(); // Thêm biến transform cho ảnh
        private Transform boxTransform = new Transform();   // Transform cho bounding box
        private float scaleFactor = 1.0f; // Hệ số phóng to/thu nhỏ
        private int rotationAngle = 0; // Góc xoay của ảnh
        private System.Drawing.Image originalImage; // Ảnh gốc
        private System.Drawing.Point mouseDownLocation;
        private List<string> imageFiles = new List<string>(); // Danh sách ảnh trong thư mục
        private int currentIndex = -1; // Vị trí ảnh hiện tại
        private bool Selected_BoundBox = false;
        private bool Resize_BoundBox = false;
        private bool Rotate_BoundBox = false;
        string savePath;
        private System.Drawing.Point imageMouseDownLocation;  // Dùng cho chế độ cuộn ảnh
        string body_ApiResponse;
        private enum ControlPointType
        {
            None,
            TopLeft,
            TopCenter,
            TopRight,
            MiddleLeft,
            MiddleRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }
        private ControlPointType resizingHandle = ControlPointType.None;
        private List<BoundingBox> BoundingBoxes = new List<BoundingBox>();
        private System.Drawing.PointF startPoint;
        private System.Drawing.PointF centerPoint;
        private Rectangle currentRectangle;
        private Rectangle _ReadApiRectangle;
        private BoundingBox selectedBox = null;
        private bool isDragging = false;
        private bool isRotating = false;
        private bool isResizing = false;
        private System.Drawing.Point resizeStartPoint;
        private Rectangle resizeRectangle;
        private int handleSize = 10;
        // Biến cho xoay bounding box
        private float initialBoxAngle = 0f;
        private PointF startRotationPoint;
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
            try
            {
                var bitmap = FrSetting.hikCamera.Capture();
                if (bitmap != null)
                {
                    // Hiển thị hình ảnh hoặc lưu hình ảnh
                    pictureBox.Image = bitmap; // Giả sử bạn có một PictureBox để hiển thị hình ảnh
                    originalImage = bitmap;
                    pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                }
                else
                {
                    MessageBox.Show("No image captured.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error capturing image: " + ex.Message);
            }
            finally
            {

            }
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
                                                        //f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
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
            if (imageFiles == null)
            {
                originalImage = pictureBox.Image;
            }
            else
            {
                originalImage = System.Drawing.Image.FromFile(filePath);
            }

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
                    SystemMode.PresentImagePath = filePath;
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

        // --- Các phương thức xử lý ảnh bằng Transform ---

        private void RotateImage(float angle)
        {
            if (pictureBox.Image == null) return;

            // Tính tâm của ảnh gốc
            float centerX = originalImage.Width / 2f;
            float centerY = originalImage.Height / 2f;
            imageTransform.Rotate(angle, centerX, centerY);
            ApplyImageTransformation();
        }

        private void ZoomImage(float factor)
        {
            float centerX = pictureBox.Width / 2f;
            float centerY = pictureBox.Height / 2f;
            imageTransform.Scale(factor, factor, centerX, centerY);
            ApplyImageTransformation();
        }

        private void ApplyImageTransformation()
        {
            if (originalImage == null) return;

            // Tạo ảnh mới theo kích thước ảnh gốc và áp dụng ma trận biến đổi
            Bitmap transformedImage = new Bitmap(originalImage.Width, originalImage.Height);
            using (Graphics g = Graphics.FromImage(transformedImage))
            {
                imageTransform.Apply(g);
                g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
            }
            pictureBox.Image = transformedImage;
            pictureBox.Invalidate();
        }

        private void ResetImage()
        {
            if (originalImage == null) return;

            imageTransform.Reset();
            pictureBox.Invalidate();
        }


        // Giữ ảnh luôn trong Panel khi zoom hoặc di chuyển
        private void KeepImageInPanel(Sunny.UI.UIPanel pnl, PictureBox pic)
        {
            if (pic.Image == null) return;

            // Đảm bảo ảnh không ra khỏi Panel
            int x = Math.Max(pnl.Width - pic.Width, 0) / 2;
            int y = Math.Max(pnl.Height - pic.Height, 0) / 2;

            pic.Location = new System.Drawing.Point(x, y);
        }

        // Căn giữa ảnh khi reset hoặc load
        private void CenterImage(Sunny.UI.UIPanel pnlContaner, PictureBox Img)
        {
            if (Img.Image == null) return;

            int x = (pnlContaner.Width - Img.Width) / 2;
            int y = (pnlContaner.Height - Img.Height) / 2;

            Img.Location = new System.Drawing.Point(x, y);
        }
        //------------------------------------------------------------------
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (originalImage == null) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Áp dụng ma trận biến đổi cho graphics trước khi vẽ ảnh và bounding box
            imageTransform.Apply(e.Graphics);
            e.Graphics.DrawImage(originalImage, 0, 0);
            DrawBoundingBoxes(e.Graphics);
        }

        private void DrawBoundingBoxes(Graphics g)
        {
            foreach (var box in BoundingBoxes)
            {
                PointF[] points = box.Vertices.ToArray();
                // Vẽ bounding box
                g.DrawPolygon(Pens.Blue, points);

                // Nếu bounding box đang được chọn thì vẽ thêm viền nổi bật, các handle resize và handle xoay.
                if (selectedBox != null && box == selectedBox)
                {
                    // Vẽ viền nổi bật (màu trắng, độ dày 2)
                    using (Pen selPen = new Pen(Color.White, 2))
                    {
                        g.DrawPolygon(selPen, points);
                    }

                    int handleSize = 20;
                    // Vẽ các handle resize ở các đỉnh
                    foreach (PointF pt in points)
                    {
                        RectangleF handleRect = new RectangleF(pt.X - handleSize / 2, pt.Y - handleSize / 2, handleSize, handleSize);
                        using (Brush handleBrush = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(handleBrush, handleRect);
                        }
                        g.DrawRectangle(Pens.Black, (int)handleRect.X, (int)handleRect.Y, (int)handleRect.Width, (int)handleRect.Height);
                    }

                    // Vẽ handle resize ở các trung điểm của cạnh
                    for (int i = 0; i < points.Length; i++)
                    {
                        PointF p1 = points[i];
                        PointF p2 = points[(i + 1) % points.Length];
                        PointF midPoint = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                        RectangleF midHandleRect = new RectangleF(midPoint.X - handleSize / 2, midPoint.Y - handleSize / 2, handleSize, handleSize);
                        using (Brush midBrush = new SolidBrush(Color.Yellow))
                        {
                            g.FillRectangle(midBrush, midHandleRect);
                        }
                        g.DrawRectangle(Pens.Black, (int)midHandleRect.X, (int)midHandleRect.Y, (int)midHandleRect.Width, (int)midHandleRect.Height);
                    }

                    // Vẽ handle xoay (RotationHandle) ở vị trí tính từ đỉnh TopRight với offset 10 pixel
                    PointF rotationHandle = GetRotationHandle(selectedBox);
                    RectangleF rotHandleRect = new RectangleF(rotationHandle.X - handleSize / 2, rotationHandle.Y - handleSize / 2, handleSize, handleSize);
                    using (Brush rotBrush = new SolidBrush(Color.Red))
                    {
                        g.FillRectangle(rotBrush, rotHandleRect);
                    }
                    g.DrawRectangle(Pens.Black, (int)rotHandleRect.X, (int)rotHandleRect.Y, (int)rotHandleRect.Width, (int)rotHandleRect.Height);
                }
            }
        }

        // --- Xử lý các sự kiện chuột ---

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Nếu đang ở chế độ bounding box thì xử lý vẽ/chỉnh sửa bounding box
            if (Selected_BoundBox)
            {
                // Chuyển tọa độ chuột từ PictureBox sang hệ tọa độ ảnh gốc
                PointF[] pts = new PointF[] { e.Location };
                Matrix invMatrix = imageTransform.Matrix.Clone();
                invMatrix.Invert();
                invMatrix.TransformPoints(pts);
                PointF transformedPoint = pts[0];

                // Kiểm tra xem có bounding box nào được chọn không
                selectedBox = GetBoundingBoxAtPoint(transformedPoint);
                if (selectedBox != null)
                {
                    // Nếu bật Resize_BoundBox, kiểm tra xem chuột nhấp vào handle resize chưa
                    if (Resize_BoundBox)
                    {
                        ControlPointType handle = GetResizeHandleUnderMouse(selectedBox, transformedPoint);
                        if (handle != ControlPointType.None)
                        {
                            isResizing = true;
                            isRotating = false;
                            isDragging = false;
                            resizingHandle = handle;
                            resizeStartPoint = Point.Round(transformedPoint);
                            return;
                        }
                    }
                    // Nếu không resize và bật Rotate_BoundBox, kiểm tra xem nhấp vào handle xoay chưa
                    if (Rotate_BoundBox && !Resize_BoundBox)
                    {
                        PointF rotationHandle = GetRotationHandle(selectedBox);
                        if (DistanceBetweenPoints(transformedPoint, rotationHandle) < 20)
                        {
                            isRotating = true;
                            isResizing = false;
                            isDragging = false;
                            startRotationPoint = transformedPoint;
                            initialBoxAngle = selectedBox.Angle;
                            return;
                        }
                    }
                    // Nếu không nhấp vào handle xoay/resize, bật chế độ kéo di chuyển bounding box
                    isDragging = true;
                    isRotating = false;
                    isResizing = false;
                    startPoint = transformedPoint;
                }
                else
                {
                    // Nếu không nhấp vào bounding box nào, bắt đầu vẽ bounding box mới
                    startPoint = transformedPoint;
                    currentRectangle = new Rectangle((int)startPoint.X, (int)startPoint.Y, 0, 0);
                }
            }
            else
            {
                // Chế độ cuộn ảnh: lưu vị trí nhấn chuột
                imageMouseDownLocation = e.Location;
            }
        }

        // MouseMove: Tùy chế độ, nếu Selected_BoundBox true thì cập nhật bounding box (di chuyển hoặc vẽ mới),
        // còn nếu false thì cập nhật vị trí PictureBox để cuộn ảnh.
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Selected_BoundBox)
            {
                // Chuyển đổi tọa độ chuột
                PointF[] pts = new PointF[] { e.Location };
                Matrix invMatrix = imageTransform.Matrix.Clone();
                invMatrix.Invert();
                invMatrix.TransformPoints(pts);
                PointF transformedPoint = pts[0];
                // Kiểm tra handle resize
                if (Resize_BoundBox && selectedBox != null)
                {
                    ControlPointType handle = GetResizeHandleUnderMouse(selectedBox, transformedPoint);
                    if (handle != ControlPointType.None)
                    {
                        // Đổi con trỏ chuột theo hướng resize
                        switch (handle)
                        {
                            case ControlPointType.TopLeft:
                            case ControlPointType.BottomRight:
                                pictureBox.Cursor = Cursors.SizeNWSE;
                                break;
                            case ControlPointType.TopRight:
                            case ControlPointType.BottomLeft:
                                pictureBox.Cursor = Cursors.SizeNESW;
                                break;
                            case ControlPointType.TopCenter:
                            case ControlPointType.BottomCenter:
                                pictureBox.Cursor = Cursors.SizeNS;
                                break;
                            case ControlPointType.MiddleLeft:
                            case ControlPointType.MiddleRight:
                                pictureBox.Cursor = Cursors.SizeWE;
                                break;
                            default:
                                pictureBox.Cursor = Cursors.Default;
                                break;
                        }
                        return; // Nếu đã phát hiện handle resize, không cần kiểm tra tiếp
                    }
                }

                // Kiểm tra handle xoay
                if (Rotate_BoundBox && selectedBox != null && !Resize_BoundBox)
                {
                    PointF rotationHandle = GetRotationHandle(selectedBox);
                    if (DistanceBetweenPoints(transformedPoint, rotationHandle) < 20)
                    {
                        pictureBox.Cursor = Cursors.Hand;
                        return;
                    }
                }

                // Nếu không ở trên handle, đặt lại con trỏ về mặc định
                pictureBox.Cursor = Cursors.Default;
                if (isResizing && selectedBox != null)
                {
                    float dx = transformedPoint.X - resizeStartPoint.X;
                    float dy = transformedPoint.Y - resizeStartPoint.Y;

                    // Cập nhật kích thước bounding box theo handle được chọn
                    switch (resizingHandle)
                    {
                        case ControlPointType.TopLeft:
                            selectedBox.X += (int)dx;
                            selectedBox.Y += (int)dy;
                            selectedBox.Width -= (int)dx;
                            selectedBox.Height -= (int)dy;
                            break;
                        case ControlPointType.TopCenter:
                            selectedBox.Y += (int)dy;
                            selectedBox.Height -= (int)dy;
                            break;
                        case ControlPointType.TopRight:
                            selectedBox.Y += (int)dy;
                            selectedBox.Width += (int)dx;
                            selectedBox.Height -= (int)dy;
                            break;
                        case ControlPointType.MiddleLeft:
                            selectedBox.X += (int)dx;
                            selectedBox.Width -= (int)dx;
                            break;
                        case ControlPointType.MiddleRight:
                            selectedBox.Width += (int)dx;
                            break;
                        case ControlPointType.BottomLeft:
                            selectedBox.X += (int)dx;
                            selectedBox.Width -= (int)dx;
                            selectedBox.Height += (int)dy;
                            break;
                        case ControlPointType.BottomCenter:
                            selectedBox.Height += (int)dy;
                            break;
                        case ControlPointType.BottomRight:
                            selectedBox.Width += (int)dx;
                            selectedBox.Height += (int)dy;
                            break;
                        default:
                            break;
                    }
                    // Cập nhật lại các đỉnh gốc dựa trên X, Y, Width, Height mới
                    selectedBox.OriginalVertices = new List<PointF>
                    {
                        new PointF(selectedBox.X, selectedBox.Y),
                        new PointF(selectedBox.X + selectedBox.Width, selectedBox.Y),
                        new PointF(selectedBox.X + selectedBox.Width, selectedBox.Y + selectedBox.Height),
                        new PointF(selectedBox.X, selectedBox.Y + selectedBox.Height)
                    };
                    UpdateBoundingBoxVertices(selectedBox);
                    resizeStartPoint = Point.Round(transformedPoint);
                    UpdateListBox();
                }
                else if (isRotating && selectedBox != null)
                {
                    float cx = selectedBox.X + selectedBox.Width / 2f;
                    float cy = selectedBox.Y + selectedBox.Height / 2f;
                    float startAngle = (float)Math.Atan2(startRotationPoint.Y - cy, startRotationPoint.X - cx);
                    float currentAngle = (float)Math.Atan2(transformedPoint.Y - cy, transformedPoint.X - cx);
                    float deltaAngle = (currentAngle - startAngle) * 180f / (float)Math.PI;
                    selectedBox.Angle = initialBoxAngle + deltaAngle;
                    UpdateBoundingBoxVertices(selectedBox);
                    UpdateListBox();
                }
                else if (isDragging && selectedBox != null)
                {
                    float dx = transformedPoint.X - startPoint.X;
                    float dy = transformedPoint.Y - startPoint.Y;
                    for (int i = 0; i < selectedBox.OriginalVertices.Count; i++)
                    {
                        selectedBox.OriginalVertices[i] = new PointF(
                            selectedBox.OriginalVertices[i].X + dx,
                            selectedBox.OriginalVertices[i].Y + dy);
                    }
                    UpdateBoundingBoxVertices(selectedBox);
                    startPoint = transformedPoint;
                    UpdateListBox();
                }
                else
                {
                    // Nếu không ở chế độ chỉnh sửa bounding box, cập nhật kích thước bounding box mới đang vẽ
                    currentRectangle.Width = (int)(transformedPoint.X - startPoint.X);
                    currentRectangle.Height = (int)(transformedPoint.Y - startPoint.Y);
                }
                pictureBox.Invalidate();
            }
            else
            {
                // Chế độ cuộn ảnh
                if (e.Button == MouseButtons.Left && pictureBox.Image != null)
                {
                    int newX = pictureBox.Left + (e.X - imageMouseDownLocation.X);
                    int newY = pictureBox.Top + (e.Y - imageMouseDownLocation.Y);
                    pictureBox.Location = new Point(newX, newY);
                }
            }
        }


        // MouseUp: Tùy chế độ, nếu Selected_BoundBox true thì xử lý kết thúc thao tác vẽ/chỉnh sửa bounding box,
        // còn nếu false thì không làm gì thêm.
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (Selected_BoundBox)
            {
                if (pictureBox.Image == null)
                {
                    MessageBox.Show("Please select an image before drawing a bounding box.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (isDragging)
                {
                    isDragging = false;
                }
                else if (isResizing)
                {
                    isResizing = false;
                }
                else if (isRotating)
                {
                    isRotating = false;
                }
                else if (currentRectangle.Width != 0 && currentRectangle.Height != 0)
                {
                    // Tạo bounding box mới từ currentRectangle
                    string _imgpath = imageFiles == null ? null : SystemMode.PresentImagePath;
                    BoundingBox newBox = new BoundingBox
                    {
                        imgpath = _imgpath,
                        X = currentRectangle.X,
                        Y = currentRectangle.Y,
                        Width = currentRectangle.Width,
                        Height = currentRectangle.Height,
                        Angle = 0,
                        OriginalVertices = new List<PointF>
                        {
                            new PointF(currentRectangle.X, currentRectangle.Y),
                            new PointF(currentRectangle.X + currentRectangle.Width, currentRectangle.Y),
                            new PointF(currentRectangle.X + currentRectangle.Width, currentRectangle.Y + currentRectangle.Height),
                            new PointF(currentRectangle.X, currentRectangle.Y + currentRectangle.Height)
                        }
                    };
                    UpdateBoundingBoxVertices(newBox);
                    BoundingBoxes.Add(newBox);
                }
                currentRectangle = Rectangle.Empty;
                pictureBox.Invalidate();
                UpdateListBox();
            }
            // Nếu chế độ cuộn ảnh thì không cần xử lý gì thêm.
        }


        // ------------------- Hàm hỗ trợ cho bounding box -------------------

        private BoundingBox GetBoundingBoxAtPoint(PointF point)
        {
            foreach (var box in BoundingBoxes)
            {
                if (IsPointInsideBoundingBox(point, box))
                    return box;
            }
            return null;
        }

        private bool IsPointInsideBoundingBox(PointF point, BoundingBox box)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(box.Vertices.ToArray());
            return path.IsVisible(point);
        }

        private void UpdateBoundingBoxVertices(BoundingBox box)
        {
            // Tính bounding rectangle từ các điểm gốc
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            foreach (var pt in box.OriginalVertices)
            {
                if (pt.X < minX) minX = pt.X;
                if (pt.Y < minY) minY = pt.Y;
                if (pt.X > maxX) maxX = pt.X;
                if (pt.Y > maxY) maxY = pt.Y;
            }
            // Tính tâm dựa trên bounding rectangle
            PointF center = new PointF((minX + maxX) / 2, (minY + maxY) / 2);

            // Tạo ma trận xoay với góc box.Angle xoay quanh tâm vừa tính
            Matrix rotationMatrix = new Matrix();
            rotationMatrix.RotateAt(box.Angle, center);

            // Áp dụng ma trận xoay lên các điểm gốc
            PointF[] rotatedPoints = box.OriginalVertices.ToArray();
            rotationMatrix.TransformPoints(rotatedPoints);

            // Áp dụng biến đổi ảnh (imageTransform) lên các điểm sau xoay
            List<PointF> finalVertices = new List<PointF>();
            foreach (var pt in rotatedPoints)
            {
                PointF[] temp = new PointF[] { pt };
                imageTransform.Matrix.TransformPoints(temp);
                finalVertices.Add(temp[0]);
            }
            box.Vertices = finalVertices;
        }

        // Hàm trả về handle resize dưới chuột nếu có
        private ControlPointType GetResizeHandleUnderMouse(BoundingBox box, PointF mousePoint)
        {
            foreach (ControlPointType handle in Enum.GetValues(typeof(ControlPointType)))
            {
                if (handle == ControlPointType.None)
                    continue;
                Rectangle handleRect = GetHandleRectangle(box, handle);
                if (handleRect.Contains(Point.Round(mousePoint)))
                    return handle;
            }
            return ControlPointType.None;
        }

        private Rectangle GetHandleRectangle(BoundingBox box, ControlPointType handleType)
        {
            /*
            Point handleCenter = GetHandleCenter(box, handleType);
            int handleSize = 20;
            return new Rectangle(handleCenter.X - handleSize / 2, handleCenter.Y - handleSize / 2, handleSize, handleSize);
        */
            Point handleCenter = GetHandleCenter(box, handleType);
            int drawnHandleSize = 12; // Kích thước gốc dùng để vẽ handle
            int extraMargin = 10;     // Mở rộng khu vực nhấp thêm 10 pixel từ mỗi bên
            int totalSize = drawnHandleSize + 2 * extraMargin; // Tổng kích thước mới
            return new Rectangle(handleCenter.X - totalSize / 2, handleCenter.Y - totalSize / 2, totalSize, totalSize);
        }

        private Point GetHandleCenter(BoundingBox box, ControlPointType handleType)
        {
            float centerX = box.X + box.Width / 2f;
            float centerY = box.Y + box.Height / 2f;
            float x = 0, y = 0;
            switch (handleType)
            {
                case ControlPointType.TopLeft:
                    x = box.X;
                    y = box.Y;
                    break;
                case ControlPointType.TopCenter:
                    x = centerX;
                    y = box.Y;
                    break;
                case ControlPointType.TopRight:
                    x = box.X + box.Width;
                    y = box.Y;
                    break;
                case ControlPointType.MiddleLeft:
                    x = box.X;
                    y = centerY;
                    break;
                case ControlPointType.MiddleRight:
                    x = box.X + box.Width;
                    y = centerY;
                    break;
                case ControlPointType.BottomLeft:
                    x = box.X;
                    y = box.Y + box.Height;
                    break;
                case ControlPointType.BottomCenter:
                    x = centerX;
                    y = box.Y + box.Height;
                    break;
                case ControlPointType.BottomRight:
                    x = box.X + box.Width;
                    y = box.Y + box.Height;
                    break;
                default:
                    break;
            }
            return new Point((int)x, (int)y);
        }

        // Hàm trả về handle xoay: dựa vào đỉnh TopRight của bounding box trước khi xoay, sau đó áp dụng xoay và offset 10 pixel
        private PointF GetRotationHandle(BoundingBox box)
        {
            if (box.Vertices != null && box.Vertices.Count == 4)
            {
                // Sử dụng đỉnh thứ 2 (index 1) là top-right
                PointF topRight = box.Vertices[1];
                // Tính tâm của bounding box dựa trên các đỉnh
                float cx = 0, cy = 0;
                foreach (PointF pt in box.Vertices)
                {
                    cx += pt.X;
                    cy += pt.Y;
                }
                cx /= box.Vertices.Count;
                cy /= box.Vertices.Count;

                // Tính vector từ tâm đến topRight
                float vx = topRight.X - cx;
                float vy = topRight.Y - cy;
                float len = (float)Math.Sqrt(vx * vx + vy * vy);
                if (len != 0)
                {
                    vx /= len;
                    vy /= len;
                }
                // Dịch handle ra ngoài 20 pixel theo hướng từ tâm đến topRight
                topRight.X += vx * 20;
                topRight.Y += vy * 20;
                return topRight;
            }
            else
            {
                // Nếu chưa có vertices hoặc không đủ, dùng cách tính cũ dựa trên box.X, box.Y, Width, Height và Angle.
                PointF topRight = new PointF(box.X + box.Width, box.Y);
                float cx = box.X + box.Width / 2f;
                float cy = box.Y + box.Height / 2f;
                Matrix m = new Matrix();
                m.RotateAt(box.Angle, new PointF(cx, cy));
                PointF[] pts = new PointF[] { topRight };
                m.TransformPoints(pts);
                float vx = pts[0].X - cx;
                float vy = pts[0].Y - cy;
                float len = (float)Math.Sqrt(vx * vx + vy * vy);
                if (len != 0)
                {
                    vx /= len;
                    vy /= len;
                }
                pts[0].X += vx * 20;
                pts[0].Y += vy * 20;
                return pts[0];
            }
        }

        // Hàm tính khoảng cách giữa 2 điểm
        private float DistanceBetweenPoints(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private void tsbtnEditBoudingBox_Click(object sender, EventArgs e)
        {
            if (!Resize_BoundBox)
            {
                Resize_BoundBox = true;
                tsbtnEditBoudingBox.BackColor = Color.Gray;
            }
            else
            {
                Resize_BoundBox = false;
                tsbtnEditBoudingBox.BackColor = Color.White;
            }
        }

        private void tsbtnRotateBouding_Click(object sender, EventArgs e)
        {
            if (!Rotate_BoundBox)
            {
                Rotate_BoundBox = true;
                tsbtnRotateBouding.BackColor = Color.Gray;
            }
            else
            {
                Rotate_BoundBox = false;
                tsbtnRotateBouding.BackColor = Color.White;
            }
        }

        // ------------------- Hàm cập nhật dữ liệu BoundingBox vào ListBox -------------------
        private void UpdateListBox()
        {
            // Giả sử listbox của bạn có tên uiListBox1
            uiListBox1.Items.Clear();
            foreach (var box in BoundingBoxes)
            {
                uiListBox1.Items.Add($"Image: {box.imgpath}, X: {box.X}, Y: {box.Y}, W: {box.Width}, H: {box.Height}, Angle: {box.Angle:F2}");
            }
        }
        public static Bitmap CropAndRotateImage(Image originalImage, BoundingBox selectedBox, float angle)
        {
            // Tính tâm của bounding box được chọn
            float centerX = selectedBox.X + selectedBox.Width / 2f;
            float centerY = selectedBox.Y + selectedBox.Height / 2f;
            PointF boxCenter = new PointF(centerX, centerY);

            // Tạo Bitmap mới với kích thước bằng bounding box
            Bitmap croppedBitmap = new Bitmap(Math.Abs(selectedBox.Width), Math.Abs(selectedBox.Height));
            croppedBitmap.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                // Thiết lập chất lượng vẽ
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Đưa gốc tọa độ Graphics về tâm của ảnh đích
                g.TranslateTransform(croppedBitmap.Width / 2f, croppedBitmap.Height / 2f);

                // Xoay theo hướng ngược lại với góc của bounding box để “thẳng” ảnh crop ra
                g.RotateTransform(-angle);

                // Dịch chuyển để căn chỉnh tâm của bounding box tại gốc tọa độ (0,0)
                g.TranslateTransform(-boxCenter.X, -boxCenter.Y);

                // Vẽ ảnh gốc vào ảnh mới, phần vẽ sẽ chỉ lấy vùng được xác định bởi bounding box
                g.DrawImage(originalImage, new Point(0, 0));
            }

            return croppedBitmap;
        }
        private void btnPickTemplate_Click(object sender, EventArgs e)
        {
            if (selectedBox != null)
            {
                // Giả sử originalImage là Image gốc, selectedBox là bounding box đã chọn và angle là góc xoay (độ)
                Bitmap result = CropAndRotateImage(originalImage, selectedBox,selectedBox.Angle);

                // Hiển thị ảnh crop ra lên PictureBox (ví dụ: picTemplate)
                picTemplate.Image = result;
                SystemMode.ImgTemplate = result;

                // lưu ảnh template vào project
                FrProject.calibService.UpdateTemplateImage(result);
                FrProject.Instance.SaveProject();

                // Lưu ảnh crop ra nếu cần
                string templateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
                if (!Directory.Exists(templateFolder))
                {
                    Directory.CreateDirectory(templateFolder);
                }
                string templatePath = Path.Combine(templateFolder, $"template{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
                result.Save(templatePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                // Nếu không có bounding box nào được chọn,
                // mở hộp thoại để chọn ảnh làm template
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image templateImage = Image.FromFile(ofd.FileName);
                    SystemMode.PresentTemplatePath = ofd.FileName;
                    picTemplate.Image = templateImage;
                    SystemMode.ImgTemplate = templateImage;

                    // lưu ảnh template vào project
                    FrProject.calibService.UpdateTemplateImage(templateImage);
                    FrProject.Instance.SaveProject();
                }
            }
        }

        private void uiListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nếu một mục được chọn, cập nhật selectedBox theo index của danh sách BoundingBoxes
            if (uiListBox1.SelectedIndex >= 0 && uiListBox1.SelectedIndex < BoundingBoxes.Count)
            {
                selectedBox = BoundingBoxes[uiListBox1.SelectedIndex];
            }
            else
            {
                selectedBox = null;
            }
            // Cập nhật lại giao diện hiển thị bounding box được chọn
            pictureBox.Invalidate();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedBox != null)
            {
                // Xóa bounding box được chọn khỏi danh sách
                BoundingBoxes.Remove(selectedBox);
                selectedBox = null;
                // Cập nhật lại ListBox để phản ánh danh sách mới
                UpdateListBox();
                // Vẽ lại PictureBox để bounding box bị xóa được cập nhật
                pictureBox.Invalidate();
            }
        }

        private async Task Training(object sender, EventArgs e)
        {
            string imagePath = SystemMode.PresentImagePath;  // Ảnh lớn
            Console.WriteLine(imagePath);
            string templatePath = savePath;  // Ảnh mẫu
            Console.WriteLine(templatePath);
            try
            {
                body_ApiResponse = await ApiClient.MatchTemplate(pictureBox.Image, picTemplate.Image, 0.2f);
                //body_ApiResponse = await ApiClient.MatchTemplate(SystemMode.PresentImagePath, SystemMode.PresentTemplatePath, 0.6f);
            }
            catch
            {

            }
            finally
            {
                if (body_ApiResponse != null)
                {
                    Console.WriteLine("body_ApiResponse: " + body_ApiResponse);
                    // DrawRotatedRectangles(body_ApiResponse, imagePath, templatePath);
                    LoadBoundingBoxesFromJson(body_ApiResponse);
                }
            }

        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            Training(null,null);
        }

        private void LoadBoundingBoxesFromJson(string jsonString)
        {
            try
            {
                MatchResult result = JsonConvert.DeserializeObject<MatchResult>(jsonString);

                if (result != null && result.matches != null)
                {
                    foreach (var match in result.matches)
                    {
                        // Giả sử kích thước mặc định của bounding box là 50x50
                        int boxWidth = SystemMode.ImgTemplate.Width;
                        int boxHeight = SystemMode.ImgTemplate.Height;
                        // Nếu giá trị x, y trong JSON là tâm, ta cần trừ đi một nửa kích thước để lấy tọa độ top-left
                        int topLeftX = match.x;
                        int topLeftY = match.y;

                        BoundingBox newBox = new BoundingBox
                        {
                            imgpath = null, // Bạn có thể cập nhật nếu cần
                            X = topLeftX,
                            Y = topLeftY,
                            Width = boxWidth,
                            Height = boxHeight,
                            Angle = match.angle, // Góc quay từ JSON
                            OriginalVertices = new List<PointF>
                    {
                        new PointF(topLeftX, topLeftY),                                     // Top-left
                        new PointF(topLeftX + boxWidth, topLeftY),                           // Top-right
                        new PointF(topLeftX + boxWidth, topLeftY + boxHeight),                 // Bottom-right
                        new PointF(topLeftX, topLeftY + boxHeight)                             // Bottom-left
                    }
                        };

                        // Cập nhật các đỉnh sau khi áp dụng góc xoay và ma trận biến đổi ảnh
                        UpdateBoundingBoxVertices(newBox);
                        BoundingBoxes.Add(newBox);
                    }
                    UpdateListBox();
                    pictureBox.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error parsing JSON: " + ex.Message);
            }
        }

        // Hàm xử lý nút xoay trái (thủ công) theo số độ nhập từ TextBox
        private void btnManualRotateLeft_Click(object sender, EventArgs e)
        {
            if (selectedBox != null)
            {
                if (float.TryParse(txtRotateDegrees.Text, out float degrees))
                {
                    // Xoay trái: trừ số độ nhập
                    selectedBox.Angle -= degrees;
                    // Cập nhật lại các đỉnh của bounding box sau khi xoay
                    UpdateBoundingBoxVertices(selectedBox);
                    UpdateListBox(); // Cập nhật listbox nếu cần
                    pictureBox.Invalidate();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không có bounding box nào được chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Hàm xử lý nút xoay phải (thủ công) theo số độ nhập từ TextBox
        private void btnManualRotateRight_Click(object sender, EventArgs e)
        {
            if (selectedBox != null)
            {
                if (float.TryParse(txtRotateDegrees.Text, out float degrees))
                {
                    // Xoay phải: cộng số độ nhập
                    selectedBox.Angle += degrees;
                    // Cập nhật lại các đỉnh của bounding box sau khi xoay
                    UpdateBoundingBoxVertices(selectedBox);
                    UpdateListBox(); // Cập nhật listbox nếu cần
                    pictureBox.Invalidate();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không có bounding box nào được chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsbtnSaveImage_Click(object sender, EventArgs e)
        {
            int result;

            try
            {
                ImageFormatInfo imageFormatInfo;
                imageFormatInfo.FormatType = ImageFormatType.Jpeg;

                imageFormatInfo.JpegQuality = 80;

                result = FrSetting.hikCamera.SaveImage(imageFormatInfo);
                if (result != MvError.MV_OK)
                {
                    MessageBox.Show("Save Image Fail!");
                    return;
                }
                else
                {
                    MessageBox.Show("Save Image Succeed!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Image Failed, " + ex.Message);
                return;
            }
        }

        private void FrDetection_Load(object sender, EventArgs e)
        {
            if (SystemMode.ImgTemplate != null)
                picTemplate.Image = SystemMode.ImgTemplate;
            else
                Console.WriteLine("Template image not found.");
        }

        private void FrDetection_Activated(object sender, EventArgs e)
        {
            if (SystemMode.ImgTemplate != null)
                picTemplate.Image = SystemMode.ImgTemplate;
            else
                Console.WriteLine("Template image not found.");
        }
    }

    public class BoundingBox
    {
        public string imgpath { get; set; }
        public int X, Y, Width, Height;
        public float Angle;
        public List<PointF> OriginalVertices { get; set; }
        public List<PointF> Vertices { get; set; }

        public BoundingBox()
        {
            OriginalVertices = new List<PointF>();
            Vertices = new List<PointF>();
        }
    }
    public class MatchData
    {
        public int x { get; set; }
        public int y { get; set; }
        public float angle { get; set; }
        public float scale { get; set; }
        public float score { get; set; }
    }

    public class MatchResult
    {
        public int count { get; set; }
        public List<MatchData> matches { get; set; }
    }
}
