using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Diagnostics;

namespace VDF3_Solution3
{
    public partial class FrStartingUp : Form
    {
        private bool _EnbContinuous = false;
        private bool _EnbLive = false;
        Stopwatch stopwatch = new Stopwatch();
        public FrStartingUp()
        {
            InitializeComponent();

            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void tsbtnTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                var bitmap = FrSetting.hikCamera.Capture();
                if (bitmap != null)
                {
                    // Hiển thị hình ảnh hoặc lưu hình ảnh
                    Pic_Capture.SizeMode = PictureBoxSizeMode.Zoom;
                    Pic_Capture.Image = bitmap; // Giả sử bạn có một PictureBox để hiển thị hình ảnh
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
                stopwatch.Stop();
                Console.WriteLine($"Trigger Time: {stopwatch.ElapsedMilliseconds} ms");
                stopwatch.Reset();
            }
        }

        private void tsbtnOpenFile_Click(object sender, EventArgs e)
        {

        }

        private void tsbtnContinuousTrigger_Click(object sender, EventArgs e)
        {
            if (!_EnbContinuous)
            {
                tsbtnContinuousTrigger.BackColor = Color.Gray;
                _EnbContinuous = true;
                timerCamera.Start();
            }
            else
            {
                tsbtnContinuousTrigger.BackColor = SystemColors.ButtonHighlight;
                _EnbContinuous = false;
                timerCamera.Stop();
            }
        }

        private void tsbtnLive_Click(object sender, EventArgs e)
        {
            if(!_EnbLive)
            {
                tsbtnLive.BackColor = Color.Gray;
                FrSetting.hikCamera.StartLive();
                var bitmap = FrSetting.hikCamera.Capture();
                if (bitmap != null)
                {
                    // Hiển thị hình ảnh hoặc lưu hình ảnh
                    Pic_Capture.Image = bitmap; // Giả sử bạn có một PictureBox để hiển thị hình ảnh
                }
                else
                {
                    MessageBox.Show("No image captured.");
                }
                _EnbLive = true;
            }
            else
            {
                tsbtnLive.BackColor = SystemColors.ButtonHighlight;
                FrSetting.hikCamera.StopLive();
                _EnbLive = false;
            }    
            
        }

        private void timerCamera_Tick(object sender, EventArgs e)
        {
            tsbtnTrigger_Click(null, null);
        }
    }
}
