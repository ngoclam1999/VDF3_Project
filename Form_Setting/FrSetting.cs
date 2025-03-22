using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MvCameraControl;
using VDF3_Solution3.Properties;
using GigE_ForceIP;

namespace VDF3_Solution3
{

    public partial class FrSetting : Form
    {
  
        public static Hikcamera hikCamera;
        public static BaslerCamera baslerCamera;
        private List<string> deviceNames;
        private CameraStatus Status;

        
        FrSettingRobot _ConfigRobot;
        FrSettingModbus _SettingModbus;
        public FrSetting()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            baslerCamera = new BaslerCamera("Basler Camera", "Local");
            Control.CheckForIllegalCrossThreadCalls = false;
            RefreshDeviceList();
        }
        private void RefreshDeviceList()
        {
            try
            {
                hikCamera = new Hikcamera("HikCamera", "ConnectionInfo"); // Thay đổi nếu cần
                //baslerCamera = new BaslerCamera("BaslerCamera", "Local");

                deviceNames = hikCamera.RefreshDeviceList();
                //deviceNames = baslerCamera.RefreshDeviceList();

                cbDeviceList.Items.Clear(); // Xóa danh sách cũ
                foreach (var device in deviceNames)
                {
                    cbDeviceList.Items.Add(device); // Thêm thiết bị vào ComboBox
                }

                if (cbDeviceList.Items.Count > 0)
                {
                    cbDeviceList.SelectedIndex = 0; // Chọn thiết bị đầu tiên nếu có
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing device list: " + ex.Message);
            }
        }
        private void btnRobot_Click(object sender, EventArgs e)
        {
            FrMain.Instance.lbTitleform.Text = "Setting/Robot";
            if (_ConfigRobot == null)
            {
                _ConfigRobot = new FrSettingRobot();
                _ConfigRobot.MdiParent = FrMain.Instance;
                _ConfigRobot.Dock = DockStyle.Fill;
                _ConfigRobot.FormClosed += _ConfigRobot_FormClosed; ;
                _ConfigRobot.Show();
            }
            else
            {
                _ConfigRobot.Activate();
            }
        }
        private void _ConfigRobot_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ConfigRobot = null;
        }
        private void btnSettingModbus_Click(object sender, EventArgs e)
        {
            FrMain.Instance.lbTitleform.Text = "Setting/Register";
            if (_SettingModbus == null)
            {
                _SettingModbus = new FrSettingModbus();
                _SettingModbus.MdiParent = FrMain.Instance;
                _SettingModbus.Dock = DockStyle.Fill;
                _SettingModbus.FormClosed += _SettingModbus_FormClosed;
                _SettingModbus.Show();
            }
            else
            {
                _SettingModbus.Activate();
            }
        }

        private void _SettingModbus_FormClosed(object sender, FormClosedEventArgs e)
        {
            _SettingModbus = null;
        }

        

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
            if (cbDeviceList.SelectedIndex == -1)
            {
                MessageBox.Show("No device selected, please select a device.");
                return;
            }

            try
            {
                int selectedIndex = cbDeviceList.SelectedIndex;
                hikCamera.Start(selectedIndex);
                MessageBox.Show("Camera connected successfully!");
                try
                {
                    if (cbDeviceList.SelectedIndex < 0) return;

                    var networkInfo = hikCamera.GetDeviceNetworkInfo(cbDeviceList.SelectedIndex);

                    //rangeLabel.Text = networkInfo.Range;
                    txtIPaddress.Text = networkInfo.IP;
                    txtSubnet.Text = networkInfo.SubnetMask;
                    txtGateway.Text = networkInfo.Gateway;
                    var DeviceInfo = hikCamera.GetDeviceInfo(cbDeviceList.Text);
                    txtDeviceBrand.Text = DeviceInfo.Manufacturer;
                    txtModel.Text = DeviceInfo.ModelName;
                    txtSerial.Text = DeviceInfo.SerialNumber;
                    txtVersion.Text = DeviceInfo.version;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to camera: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDeviceList();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                hikCamera.Stop();
                MessageBox.Show("Camera disconnected successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error disconnecting from camera: " + ex.Message);
            }
        }

        private void btnCamRead_Click(object sender, EventArgs e)
        {
            var netinfor = hikCamera.GetDeviceNetworkInfo(cbDeviceList.SelectedIndex);
            txtIPaddress.Text = netinfor.IP;
            txtSubnet.Text = netinfor.SubnetMask;
            txtGateway.Text = netinfor.Gateway;
            var DeviceInfo = hikCamera.GetDeviceInfo(cbDeviceList.Text);
            txtDeviceBrand.Text = DeviceInfo.Manufacturer;
            txtModel.Text = DeviceInfo.ModelName;
            txtSerial.Text = DeviceInfo.SerialNumber;
            txtVersion.Text = DeviceInfo.version;
        }

        private void btnCamApply_Click(object sender, EventArgs e)
        {
            string ip = txtIPaddress.Text; // Lấy IP từ textbox
            string subnet = txtSubnet.Text; // Lấy subnet từ textbox
            string gateway = txtGateway.Text; // Lấy gateway từ textbox
            int deviceIndex = cbDeviceList.SelectedIndex; // Lấy index thiết bị từ combobox
            btnDisconnect_Click(null, null);
            using (GigE_ForceIP.GigE_ForceIP frceIP = new GigE_ForceIP.GigE_ForceIP())
            {
                frceIP.ShowDialog();
            }
            MessageBox.Show("Change The Network Successfully, Please Reboot Application");
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                var bitmap = hikCamera.Capture();
                if (bitmap != null)
                {
                    // Hiển thị hình ảnh hoặc lưu hình ảnh
                    pic_campreview.Image = bitmap; // Giả sử bạn có một PictureBox để hiển thị hình ảnh
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

        private void btnCamSet_Click(object sender, EventArgs e)
        {
            btnSetGain_Click(sender, e);
            btnSetExposure_Click(sender, e);
        }

        private void btnSetExposure_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtExposure.Text, out int exposure))
            {
                hikCamera.SetExposure(exposure);
                MessageBox.Show("Exposure set successfully!");
            }
            else
            {
                MessageBox.Show("Invalid exposure value.");
            }
        }
        private void btnSetGain_Click(object sender, EventArgs e)
        {
            if (float.TryParse(txtGain.Text, out float gain))
            {
                hikCamera.SetGain(gain);
            }
            else
            {
                MessageBox.Show("Invalid gain value.");
            }
        }

        private void FrSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hikCamera != null)
            {
                hikCamera.Stop(); // Ngắt kết nối camera khi form đóng
            }
            SDKSystem.Finalize();
        }

       
    }
}
