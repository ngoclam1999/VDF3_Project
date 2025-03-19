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

namespace VDF3_Solution3
{
    public partial class FrRun : Form
    {
        private Image originalImage;
        private string body_ApiResponse;
        private List<BoundingBox> BoundingBoxes = new List<BoundingBox>();
        private Transform imageTransform = new Transform(); // Thêm biến transform cho ảnh
        private List<CoordinateData> cameraCoordinates = new List<CoordinateData>();
        private List<CoordinateData> robotCoordinates = new List<CoordinateData>();

        public FrRun()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            SetupListView();
        }

        private void SetupListView()
        {
            // Tạo ListView
            lwCameraRobot.View = View.Details;
            lwCameraRobot.FullRowSelect = true;
            lwCameraRobot.GridLines = true;
            lwCameraRobot.OwnerDraw = true;
          
            lwCameraRobot.DrawColumnHeader += ListView_DrawColumnHeader;
            // Thêm các cột
            lwCameraRobot.Columns.Add("CAM (x)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("CAM (y)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("CAM (θ)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("ROBOT (x)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("ROBOT (y)", 80, HorizontalAlignment.Center);
            lwCameraRobot.Columns.Add("ROBOT (θ)", 80, HorizontalAlignment.Center);
        }

        // Vẽ tiêu đề cột căn giữa
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
                    // Hiển thị hình ảnh hoặc lưu hình ảnh
                    PicRunRobot.Image = bitmap; // Giả sử bạn có một PictureBox để hiển thị hình ảnh
                    originalImage = bitmap;
                    PicRunRobot.SizeMode = PictureBoxSizeMode.Zoom;
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
                    body_ApiResponse = await ApiClient.MatchTemplate(PicRunRobot.Image, pictemplateRunning.Image, 0.5f);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error capturing image: " + ex.Message);
                }
                finally
                {
                    if (body_ApiResponse != null)
                    {
                        Console.WriteLine("body_ApiResponse: " + body_ApiResponse);
                        LoadBoundingBoxesFromJson(body_ApiResponse);
                    }
                }
            }
        }
        private void LoadBoundingBoxesFromJson(string jsonString)
        {
            try
            {
                MatchResult result = JsonConvert.DeserializeObject<MatchResult>(jsonString);

                if (result != null && result.matches != null)
                {
                    // Xóa dữ liệu cũ nếu cần
                    BoundingBoxes.Clear();
                    lwCameraRobot.Items.Clear();
                    cameraCoordinates.Clear();
                    robotCoordinates.Clear();

                    foreach (var match in result.matches)
                    {
                        // Lấy kích thước của template ảnh làm kích thước bounding box
                        int boxWidth = pictemplateRunning.Image.Width;
                        int boxHeight = pictemplateRunning.Image.Height;
                        // Nếu giá trị x, y trong JSON là tâm, có thể điều chỉnh (ví dụ: trừ đi 1/2 kích thước) nếu cần.
                        int topLeftX = match.x;
                        int topLeftY = match.y;

                        BoundingBox newBox = new BoundingBox
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
                                new PointF(topLeftX + boxWidth, topLeftY),                           // Top-right
                                new PointF(topLeftX + boxWidth, topLeftY + boxHeight),                 // Bottom-right
                                new PointF(topLeftX, topLeftY + boxHeight)                             // Bottom-left
                            }
                        };

                        // Cập nhật các đỉnh sau khi áp dụng góc xoay và ma trận biến đổi ảnh
                        UpdateBoundingBoxVertices(newBox);
                        BoundingBoxes.Add(newBox);

                        // Tính tọa độ trung tâm camera (giả sử giá trị trong JSON là tâm)
                        double CameraX = newBox.X + boxWidth / 2.0;
                        double CameraY = newBox.Y + boxHeight / 2.0;

                        // Chuyển đổi tọa độ camera sang tọa độ robot theo ma trận biến đổi
                        double robotX = VariableRobot.R11 * CameraX + VariableRobot.R12 * CameraY + VariableRobot.Tx;
                        double robotY = VariableRobot.R21 * CameraX + VariableRobot.R22 * CameraY + VariableRobot.Ty;
                        double robotAngle = newBox.Angle + Math.Atan2(VariableRobot.R21, VariableRobot.R11) * (180 / Math.PI);

                        // Lưu tọa độ vào 2 list
                        CoordinateData camData = new CoordinateData(CameraX, CameraY, newBox.Angle);
                        CoordinateData robData = new CoordinateData(robotX, robotY, robotAngle);
                        cameraCoordinates.Add(camData);
                        robotCoordinates.Add(robData);

                        // Tạo ListViewItem với 6 cột: CAM (x), CAM (y), CAM (θ), ROBOT (x), ROBOT (y), ROBOT (θ)
                        ListViewItem lvItem = new ListViewItem(new string[]
                        {
                            CameraX.ToString("F2"),
                            CameraY.ToString("F2"),
                            newBox.Angle.ToString("F2"),
                            robotX.ToString("F2"),
                            robotY.ToString("F2"),
                            robotAngle.ToString("F2")
                        });
                        lwCameraRobot.Items.Add(lvItem);
                    }
                    // Sau khi load xong, vẽ lại ảnh (với bounding box nếu có)
                    PicRunRobot.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error parsing JSON: " + ex.Message);
            }
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

        private void tsbtnTrigger_Click(object sender, EventArgs e)
        {
            Trigger(null, null);
        }
        public class CoordinateData
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Angle { get; set; }

            public CoordinateData(double x, double y, double angle)
            {
                X = x;
                Y = y;
                Angle = angle;
            }

            public override string ToString()
            {
                return $"({X}, {Y}, {Angle})";
            }
        }

    }
}
