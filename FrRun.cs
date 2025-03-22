using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using static VDF3_Solution3.FrDetection;
using OpenCvSharp.Flann;
using Basler.Pylon;
using System.Web.UI.WebControls;
using Sunny.UI;

namespace VDF3_Solution3
{
    public partial class FrRun : Form
    {
        private System.Drawing.Image originalImage;
        private System.Drawing.Point imageMouseDownLocation;  // Dùng cho chế độ cuộn ảnh
        private string body_ApiResponse;
        private List<BoundingBoxRun> BoundingBoxes = new List<BoundingBoxRun>();
        private Transform imageTransform = new Transform(); // Biến transform cho ảnh
        private List<CoordinateData> cameraCoordinates = new List<CoordinateData>();
        private List<CoordinateData> robotCoordinates = new List<CoordinateData>();
        private List<ListviewCoordinates> cameraRobotCoordinates = new List<ListviewCoordinates>();

        public FrRun()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            SetupListView();
            BackgroundWorkerService.Instance.OnDataUpdated += UpdateUI_Modbus;
        }
        private void UpdateUI_Modbus(string key, object value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI_Modbus(key, value)));
                return;
            }
            if (key == "RbtMode")
            {
                
            }
            if (key == "CurrentX")
            {
                if (value != null && float.TryParse(value.ToString(), out float x))
                {
                    
                }
            }
            if (key == "CurrentY")
            {
                if (value != null && float.TryParse(value.ToString(), out float y))
                {
                    
                }
            }

        }

        private void SetupListView()
        {
            // Tạo ListView
            lwCameraRobot.View = System.Windows.Forms.View.Details;
            lwCameraRobot.FullRowSelect = true;
            lwCameraRobot.GridLines = true;
            // Thêm các cột
            lwCameraRobot.Columns.Add("CAM (x)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("CAM (y)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("CAM (θ)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("ROBOT (x)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("ROBOT (y)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("ROBOT (θ)", 80, HorizontalAlignment.Center);
        }

        // Vẽ tiêu đề cột căn giữa (nếu dùng OwnerDraw)
        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
                e.Graphics.DrawString(e.Header.Text, e.Font, Brushes.Black, e.Bounds, sf);
            }
        }

        private async Task Trigger(object sender, EventArgs e)
        {
            try
            {
                var bitmap = FrSetting.hikCamera.Capture();
                if (bitmap != null)
                {
                    originalImage = bitmap;
                    // Hiển thị full ảnh trong PictureBox với scale tự động
                    FitImageToPictureBox();
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
                try
                {
                    body_ApiResponse = await ApiClient.MatchTemplate(originalImage, SystemMode.ImgTemplate, 0.3f);
                    if (body_ApiResponse != null)
                    {
                        Console.WriteLine("body_ApiResponse: " + body_ApiResponse);
                        LoadBoundingBoxesFromJson(body_ApiResponse);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error capturing image: " + ex.Message);
                }
            }
        }

        // Tính toán scale tự động để full ảnh vừa với PictureBox
        private void FitImageToPictureBox()
        {
            if (originalImage == null) return;

            float scaleX = (float)PicRunRobot.Width / originalImage.Width;
            float scaleY = (float)PicRunRobot.Height / originalImage.Height;
            float scale = Math.Min(scaleX, scaleY);

            // Reset lại transform và áp dụng scale
            imageTransform.Reset();

            // Tính toán offset để canh giữa ảnh trong PictureBox
            float offsetX = (PicRunRobot.Width - originalImage.Width * scale) / 2;
            float offsetY = (PicRunRobot.Height - originalImage.Height * scale) / 2;

            // Áp dụng scale và dịch chuyển (áp dụng tại gốc tọa độ)
            imageTransform.Scale(scale, scale, 0, 0);
            imageTransform.Translate(offsetX, offsetY);

            ApplyImageTransformation();
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
                        // Lấy kích thước của template ảnh làm kích thước bounding box
                        int boxWidth = SystemMode.ImgTemplate.Width;
                        int boxHeight = SystemMode.ImgTemplate.Height;
                        Console.WriteLine($"WxH:{boxWidth},{boxHeight}");
                        // Nếu giá trị x, y trong JSON là tâm, có thể điều chỉnh (ví dụ: trừ đi 1/2 kích thước) nếu cần.
                        int topLeftX = match.x;
                        int topLeftY = match.y;

                        BoundingBoxRun newBox = new BoundingBoxRun
                        {
                            imgpath = null,
                            X = topLeftX,
                            Y = topLeftY,
                            Width = boxWidth,
                            Height = boxHeight,
                            Angle = match.angle,
                            OriginalVertices = new List<PointF>
                            {
                                new PointF(topLeftX, topLeftY),                                     // Top-left
                                new PointF(topLeftX + boxWidth, topLeftY),                          // Top-right
                                new PointF(topLeftX + boxWidth, topLeftY + boxHeight),              // Bottom-right
                                new PointF(topLeftX, topLeftY + boxHeight)                          // Bottom-left
                            }
                        };

                        // Cập nhật các đỉnh sau khi xoay (chỉ xoay theo góc, không áp dụng imageTransform nữa)
                        UpdateBoundingBoxVertices(newBox);
                        BoundingBoxes.Add(newBox);

                        // Tính tọa độ trung tâm camera (giả sử giá trị trong JSON là tâm)
                        float CameraX = newBox.X + boxWidth / 2f;
                        float CameraY = newBox.Y + boxHeight / 2f;

                        // Chuyển đổi tọa độ camera sang tọa độ robot theo ma trận biến đổi
                        float robotX = VariableRobot.R11 * CameraX + VariableRobot.R12 * CameraY + VariableRobot.Tx;
                        float robotY = VariableRobot.R21 * CameraX + VariableRobot.R22 * CameraY + VariableRobot.Ty;
                        float robotAngle = newBox.Angle + (float)(Math.Atan2(VariableRobot.R21, VariableRobot.R11) * (180.0 / Math.PI));

                        // Lưu tọa độ vào 2 list
                        CoordinateData camData = new CoordinateData(CameraX, CameraY, newBox.Angle);
                        CoordinateData robData = new CoordinateData(robotX, robotY, robotAngle);
                        cameraCoordinates.Add(camData);
                        robotCoordinates.Add(robData);

                        // Tạo ListViewItem với 6 cột: CAM (x), CAM (y), CAM (θ), ROBOT (x), ROBOT (y), ROBOT (θ)
                        cameraRobotCoordinates.Add(new ListviewCoordinates
                        {
                            Cx = CameraX,
                            Cy = CameraY,
                            Cangle = newBox.Angle,
                            Rx = robotX,
                            Ry = robotY,
                            Rangle = robotAngle
                        });
                    }
                    // Hiển thị dữ liệu lên ListView
                    lwCameraRobot.Items.Clear();
                    SetupListView();
                    foreach (var item in cameraRobotCoordinates)
                    {
                        ListViewItem listItem = new ListViewItem(new string[]
                        {
                            item.Cx.ToString(),
                            item.Cy.ToString(),
                            item.Cangle.ToString(),
                            item.Rx.ToString(),
                            item.Ry.ToString(),
                            item.Rangle.ToString()
                        });
                        lwCameraRobot.Items.Add(listItem);
                    }
                    // Sau khi load xong, vẽ lại ảnh (với bounding box nếu có)
                    PicRunRobot.Invalidate();
                }

                for(int i = 0; i < Math.Min(cameraRobotCoordinates.Count,15); i++)
                {
                    InvokeService.SendData("CountPoint", cameraRobotCoordinates.Count);
                    InvokeService.SendData($"PointX{i+1}", cameraRobotCoordinates[i].Rx);
                    InvokeService.SendData($"PointY{i+1}", cameraRobotCoordinates[i].Ry);
                    InvokeService.SendData($"Angle{i+1}", cameraRobotCoordinates[i].Rangle);
                }    

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error parsing JSON: " + ex.Message);
            }
        }

        // Chỉ xoay các điểm theo góc, để Graphics transform (ở Paint) xử lý phần scale, translate
        private void UpdateBoundingBoxVertices(BoundingBoxRun box)
        {
            // Tính bounding rectangle từ các điểm gốc
            float minX = box.OriginalVertices.Min(pt => pt.X);
            float minY = box.OriginalVertices.Min(pt => pt.Y);
            float maxX = box.OriginalVertices.Max(pt => pt.X);
            float maxY = box.OriginalVertices.Max(pt => pt.Y);
            PointF center = new PointF((minX + maxX) / 2, (minY + maxY) / 2);

            // Tạo ma trận xoay với góc box.Angle xoay quanh tâm vừa tính
            Matrix rotationMatrix = new Matrix();
            rotationMatrix.RotateAt(box.Angle, center);

            // Áp dụng ma trận xoay lên các điểm gốc
            PointF[] rotatedPoints = box.OriginalVertices.ToArray();
            rotationMatrix.TransformPoints(rotatedPoints);

            // Gán các điểm đã xoay (vẫn ở hệ tọa độ gốc) cho box.Vertices
            box.Vertices = rotatedPoints.ToList();
        }

        private void tsbtnTrigger_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu cũ nếu cần
            cameraRobotCoordinates.Clear();
            BoundingBoxes.Clear();
            cameraCoordinates.Clear();
            robotCoordinates.Clear(); 
            foreach (var item in cameraRobotCoordinates)
            {
                ListViewItem listItem = new ListViewItem(new string[]
                {
                            item.Cx.ToString(),
                            item.Cy.ToString(),
                            item.Cangle.ToString(),
                            item.Rx.ToString(),
                            item.Ry.ToString(),
                            item.Rangle.ToString()
                });
                lwCameraRobot.Items.Add(listItem);
            }
            Trigger(null, null);
        }

        public class CoordinateData
        {
            public float x { get; set; }
            public float y { get; set; }
            public float angle { get; set; }

            public CoordinateData(float _x, float _y, float _angle)
            {
                x = _x;
                y = _y;
                angle = _angle;
            }

            public override string ToString()
            {
                return $"({x}, {y}, {angle})";
            }
        }

        public class BoundingBoxRun
        {
            public string imgpath { get; set; }
            public int X, Y, Width, Height;
            public float Angle;
            public List<PointF> OriginalVertices { get; set; }
            public List<PointF> Vertices { get; set; }

            public BoundingBoxRun()
            {
                OriginalVertices = new List<PointF>();
                Vertices = new List<PointF>();
            }
        }

        private void tsbtnStart_Click(object sender, EventArgs e)
        {
            // Code xử lý khi bấm Start (nếu cần)
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
            PicRunRobot.Image = transformedImage;
            PicRunRobot.Invalidate();
        }

        private void ResetImage()
        {
            if (originalImage == null) return;

            imageTransform.Reset();
            ApplyImageTransformation();
        }

        private void tsbtnStop_Click(object sender, EventArgs e)
        {
            // Code xử lý khi bấm Stop (nếu cần)
        }

        public class ListviewCoordinates
        {
            public float Cx { get; set; }
            public float Cy { get; set; }
            public float Cangle { get; set; }
            public float Rx { get; set; }
            public float Ry { get; set; }
            public float Rangle { get; set; }
        }

        private void PicRunRobot_Paint(object sender, PaintEventArgs e)
        {
            if (originalImage == null) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Áp dụng ma trận biến đổi cho graphics (để ảnh và bounding box được scale, translate đồng bộ)
            imageTransform.Apply(e.Graphics);
            e.Graphics.DrawImage(originalImage, 0, 0);
            DrawBoundingBoxes(e.Graphics);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Chế độ cuộn ảnh: lưu vị trí nhấn chuột
            imageMouseDownLocation = e.Location;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            // Chế độ cuộn ảnh
            if (e.Button == MouseButtons.Left && PicRunRobot.Image != null)
            {
                int newX = PicRunRobot.Left + (e.X - imageMouseDownLocation.X);
                int newY = PicRunRobot.Top + (e.Y - imageMouseDownLocation.Y);
                PicRunRobot.Location = new Point(newX, newY);
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            PicRunRobot.Invalidate();
        }

        private void DrawBoundingBoxes(Graphics g)
        {
            foreach (var box in BoundingBoxes)
            {
                // Các điểm của bounding box đang ở hệ tọa độ gốc, sẽ được chuyển đổi bởi e.Graphics (với imageTransform)
                PointF[] points = box.Vertices.ToArray();
                g.DrawPolygon(Pens.Blue, points);

                // Tính và vẽ điểm trung tâm của bounding box
                float centerX = points.Average(pt => pt.X);
                float centerY = points.Average(pt => pt.Y);
                float radius = 3; // Bán kính điểm trung tâm
                g.FillEllipse(Brushes.Red, centerX - radius, centerY - radius, radius * 2, radius * 2);
            }
        }
    }
}
