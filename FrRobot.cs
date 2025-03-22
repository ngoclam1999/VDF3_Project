using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Sunny.UI;
using System.IO;
using System.Windows.Shapes;
using MvCameraControl;
using System.Security.Cryptography;

namespace VDF3_Solution3
{
    public partial class FrRobot : Form
    {
        // Mảng chứa các TextBox hiển thị tọa độ của các điểm đã lưu
        private UITextBox[] calibrationXTextBoxes;
        private UITextBox[] calibrationYTextBoxes;
        // Danh sách 9 điểm
        private List<RobotPoint> realPoints = new List<RobotPoint>();
        // Chỉ số điểm hiện tại (0..8)
        private int currentPointIndex = 0;
        private Image OriginImage;

        public FrRobot()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea; 
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            BackgroundWorkerService.Instance.OnDataUpdated += UpdateUI_Modbus;

            // Gán mảng với các TextBox đã có trên Form (đảm bảo rằng các control này được tạo sẵn trong Designer)
            calibrationXTextBoxes = new UITextBox[]
            {
                txtPointX1, txtPointX2, txtPointX3,
                txtPointX4, txtPointX5, txtPointX6,
                txtPointX7, txtPointX8, txtPointX9
            };

            calibrationYTextBoxes = new UITextBox[]
            {
                txtPointY1, txtPointY2, txtPointY3,
                txtPointY4, txtPointY5, txtPointY6,
                txtPointY7, txtPointY8, txtPointY9
            };

            // Khởi tạo danh sách 9 điểm với giá trị mặc định (0,0)
            for (int i = 0; i < 9; i++)
            {
                realPoints.Add(new RobotPoint { x = 0, y = 0 });
            }

            // Hiển thị giá trị của điểm đầu tiên lên textBoxX, textBoxY (tọa độ hiện tại)
            UpdateCurrentPointDisplay();
        }

        // <summary>
        /// Cập nhật textBoxX và textBoxY với giá trị của điểm đang được chọn (currentPointIndex).
        /// </summary>
        private void UpdateCurrentPointDisplay()
        {
            textBoxX.Text = realPoints[currentPointIndex].x.ToString("F2", CultureInfo.InvariantCulture);
            textBoxY.Text = realPoints[currentPointIndex].y.ToString("F2", CultureInfo.InvariantCulture);
            
        }

        /// <summary>
        /// Lấy tọa độ hiện tại từ textBoxX, textBoxY, cập nhật vào danh sách và đồng thời hiển thị lên TextBox của điểm tương ứng.
        /// </summary>
        private void SaveCurrentReadingToList()
        {
            // Đọc tọa độ hiện tại từ textBoxX và textBoxY
            if (double.TryParse(textBoxX.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double _x) &&
                double.TryParse(textBoxY.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double _y))
            {
                // Lưu vào danh sách tại vị trí currentPointIndex
                realPoints[currentPointIndex].x = _x;
                realPoints[currentPointIndex].y = _y;

                // Đồng thời cập nhật giá trị lên TextBox của điểm tương ứng
                calibrationXTextBoxes[currentPointIndex].Text = _x.ToString("F6", CultureInfo.InvariantCulture);
                calibrationYTextBoxes[currentPointIndex].Text = _y.ToString("F6", CultureInfo.InvariantCulture);

                VariableRobot.calibrationArray[0,currentPointIndex] = (float)_x;
                VariableRobot.calibrationArray[1, currentPointIndex] = (float)_y;
            }
            else
            {
                MessageBox.Show("Giá trị tọa độ hiện tại không hợp lệ, hãy kiểm tra lại!");
            }
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
                uiLabel1.Text = value.ToString();
            }
            if (key == "CurrentX")
            {
                if (value != null && float.TryParse(value.ToString(), out float x))
                {
                    textBoxX.Text = (x).ToString();
                }
            }
            if (key == "CurrentY")
            {
                if (value != null && float.TryParse(value.ToString(), out float y))
                {
                    textBoxY.Text = (y).ToString();
                }
            }
           
        }
        // ================== Sự kiện các nút ==================

        private void tsbtnTeach_Click(object sender, EventArgs e)
        {
            SaveCurrentReadingToList();
            UpdateCurrentPointDisplay();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (currentPointIndex < 8)
            {
                currentPointIndex++;
                labelPointIndex.Text = "Current Point: " + (currentPointIndex + 1).ToString() + "/9";
                textBoxX.Text = calibrationXTextBoxes[currentPointIndex].Text;
                textBoxY.Text = calibrationYTextBoxes[currentPointIndex].Text;
            }
            else
            {
                MessageBox.Show("Đã ở điểm cuối cùng (9/9)!");
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (currentPointIndex > 0)
            {
                currentPointIndex--;
                labelPointIndex.Text = "Current Point: " + (currentPointIndex + 1).ToString() + "/9";
                textBoxX.Text = calibrationXTextBoxes[currentPointIndex].Text;
                textBoxY.Text = calibrationYTextBoxes[currentPointIndex].Text;
            }
            else
            {
                MessageBox.Show("Đã ở điểm đầu tiên (1/9)!");
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (picBoxCamRobot.Image == null) return;
            // Vẽ ảnh
            e.Graphics.DrawImage(picBoxCamRobot.Image, 0, 0, picBoxCamRobot.Width, picBoxCamRobot.Height);

            // Vẽ 2 đường dọc
            float x1 = picBoxCamRobot.Width / 3f;
            float x2 = 2 * picBoxCamRobot.Width / 3f;
            e.Graphics.DrawLine(Pens.Red, x1, 0, x1, picBoxCamRobot.Height);
            e.Graphics.DrawLine(Pens.Red, x2, 0, x2, picBoxCamRobot.Height);

            // Vẽ 2 đường ngang
            float y1 = picBoxCamRobot.Height / 3f;
            float y2 = 2 * picBoxCamRobot.Height / 3f;
            e.Graphics.DrawLine(Pens.Red, 0, y1, picBoxCamRobot.Width, y1);
            e.Graphics.DrawLine(Pens.Red, 0, y2, picBoxCamRobot.Width, y2);
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                var bitmap = FrSetting.hikCamera.Capture();
                if (bitmap != null)
                {
                    // Hiển thị hình ảnh hoặc lưu hình ảnh
                    picBoxCamRobot.Image = bitmap; // Giả sử bạn có một PictureBox để hiển thị hình ảnh
                    OriginImage = bitmap;
                    picBoxCamRobot.SizeMode = PictureBoxSizeMode.StretchImage;
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
        }
        private void tsbtnCalibration_Click(object sender, EventArgs e)
        {
            Calibration(null, null);
        }
        private async Task Calibration(object sender, EventArgs e)
        {
            // Lưu lại kết quả tọa độ hiện tại trước khi lưu JSON
            //SaveCurrentReadingToList();

            // Kiểm tra nếu danh sách đã có đủ 9 cặp tọa độ (thường luôn có vì đã khởi tạo 9 phần tử)
            if (realPoints.Count == 9)
            {
                // Serialize danh sách realPoints thành chuỗi JSON với định dạng đẹp (indented)
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(realPoints, Formatting.Indented);

                try
                {
                    var (cameraPoints, homographyMatrix, maxError, meanError, errorsPerPoint) = await ApiClient.SendCalibrationRequest(OriginImage, json);

                    if (homographyMatrix!= null) 
                    {
                        // lưu ma trận chuyển đổi vào biến
                        VariableRobot.R11 = homographyMatrix[0, 0];
                        VariableRobot.R12 = homographyMatrix[0, 1];
                        VariableRobot.Tx = homographyMatrix[0, 2];
                        VariableRobot.R21 = homographyMatrix[1, 0];
                        VariableRobot.R22 = homographyMatrix[1, 1];
                        VariableRobot.Ty = homographyMatrix[1, 2];
                        //Show lên kiểm tra
                        Console.WriteLine("R11" + VariableRobot.R11);
                        Console.WriteLine("R12" + VariableRobot.R12);
                        Console.WriteLine("Tx" + VariableRobot.Tx);
                        Console.WriteLine("R21" + VariableRobot.R21);
                        Console.WriteLine("R22" + VariableRobot.R22);
                        Console.WriteLine("Ty" + VariableRobot.Ty);

                        // Sao chép ảnh gốc để vẽ lên
                        Bitmap newImage = new Bitmap(OriginImage);
                        Pen pen = new Pen(Color.Green, 5);

                        // Hiển thị tọa độ
                        Console.WriteLine("Camera Points:");

                        using (Graphics g = Graphics.FromImage(newImage))
                        {
                            foreach (var point in cameraPoints)
                            {
                                Console.WriteLine($"[{point[0]}, {point[1]}]");

                                int radius = 50 / 2; // Bán kính hình tròn
                                g.DrawEllipse(pen, (int)point[0] - radius, (int)point[1] - radius, 50, 50);
                            }
                        }

                        // Cập nhật ảnh sau khi vẽ xong tất cả hình tròn
                        picBoxCamRobot.Image = newImage;
                        picBoxCamRobot.Invalidate();

                        // Hiển thị ma trận homography
                        Console.WriteLine("\nHomography Matrix:");
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                Console.Write($"{homographyMatrix[i, j]:F6}  ");
                            }
                            Console.WriteLine();
                        }


                        // Hiển thị sai số
                        Console.WriteLine($"\nMax Error: {maxError}");
                        Console.WriteLine($"Mean Error: {meanError}");
                        Console.WriteLine("Errors per point:");
                        foreach (var error in errorsPerPoint)
                        {
                            Console.WriteLine(error);
                        }
                        // tọa độ của camera là 150,300 và gốc 45 độ
                        Console.WriteLine($"Xr:{VariableRobot.R11 * 150 + VariableRobot.R12 * 300 + VariableRobot.Tx}");
                        Console.WriteLine($"Yr:{VariableRobot.R21 * 150 + VariableRobot.R22 * 300 + VariableRobot.Ty}");
                        Console.WriteLine($"theta_r: {45 + Math.Atan2(VariableRobot.R21, VariableRobot.R11) * (180 / Math.PI)}");
                    }
                }
                catch
                {

                }
            }
            else
            {
                MessageBox.Show("Danh sách chưa đầy đủ 9 cặp tọa độ!");
            }
        }
        // Lớp lưu toạ độ 2D (X, Y)
        public class RobotPoint
        {
            public double x { get; set; }
            public double y { get; set; }

            public RobotPoint(double _x = 0, double _y = 0)
            {
                x = _x;
                y = _y;
            }
        }

        private void FrRobot_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                realPoints[i].x = VariableRobot.calibrationArray[0, i];
                realPoints[i].y = VariableRobot.calibrationArray[1, i];

                // Đồng thời cập nhật giá trị lên TextBox của điểm tương ứng
                calibrationXTextBoxes[i].Text = realPoints[i].x.ToString("F6", CultureInfo.InvariantCulture);
                calibrationYTextBoxes[i].Text = realPoints[i].y.ToString("F6", CultureInfo.InvariantCulture);
            }
        }

        private void FrRobot_Activated(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                realPoints[i].x = VariableRobot.calibrationArray[0, i];
                realPoints[i].y = VariableRobot.calibrationArray[1, i];

                // Đồng thời cập nhật giá trị lên TextBox của điểm tương ứng
                calibrationXTextBoxes[i].Text = realPoints[i].x.ToString("F6", CultureInfo.InvariantCulture);
                calibrationYTextBoxes[i].Text = realPoints[i].y.ToString("F6", CultureInfo.InvariantCulture);
            }
        }
    }
}
