﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BackgroundWorkerService;


namespace VDF3_Solution3
{
    public partial class FrSettingRobot : Form
    {
        FrSetting _ConfigCamera;
        FrSettingModbus _SettingModbus;
        public FrSettingRobot()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            BackgroundWorkerService.Instance.OnDataUpdated += UpdateUI;
            Control.CheckForIllegalCrossThreadCalls = false;
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
        private void btnCamera_Click(object sender, EventArgs e)
        {
            FrMain.Instance.lbTitleform.Text = "Setting/Camera";
            if (_ConfigCamera == null)
            {
                _ConfigCamera = new FrSetting();
                _ConfigCamera.MdiParent = FrMain.Instance;
                _ConfigCamera.Dock = DockStyle.Fill;
                _ConfigCamera.FormClosed += _ConfigCamera_FormClosed; ;
                _ConfigCamera.Show();
            }
            else
            {
                _ConfigCamera.Activate();
            }
        }

        private void _ConfigCamera_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ConfigCamera = null;
        }

        private void rdiVacum_CheckedChanged(object sender, EventArgs e)
        {
            if(rdiVacum.Checked)
            {
                SystemMode.ToolMode = 1;
                txtVaCumD.Enabled = true;
                btnSaveVacum.Enabled = true;
            }
            else
            {
                SystemMode.ToolMode = 0;
                txtVaCumD.Enabled = false;
                txtGripD1.Enabled = false;
                txtGripD2.Enabled = false;
                txtGripA.Enabled = false;
                btnSaveGripper.Enabled = false;
                btnSaveVacum.Enabled = false;
            }    
            Console.WriteLine(SystemMode.ToolMode);
        }

        private void rdiGripper_CheckedChanged(object sender, EventArgs e)
        {
            if (rdiGripper.Checked)
            {
                SystemMode.ToolMode = 2;
                txtGripD1.Enabled = true;
                txtGripD2.Enabled = true;
                txtGripA.Enabled = true;
                btnSaveGripper.Enabled = true;
            }
            else
            {
                SystemMode.ToolMode = 0;
                txtVaCumD.Enabled = false;
                txtGripD1.Enabled = false;
                txtGripD2.Enabled = false;
                txtGripA.Enabled = false;
                btnSaveGripper.Enabled = false;
                btnSaveVacum.Enabled = false;
            }
            Console.WriteLine(SystemMode.ToolMode);
        }

        void refresh()
        {
            if(SystemMode.ToolMode == 1)
            {
                txtVaCumD.Enabled = true;
                btnSaveVacum.Enabled = true;
                txtVaCumD.Text = SystemMode.VacumPad_Diameter.ToString();
            }
            else if (SystemMode.ToolMode == 2)
            {
                txtGripD1.Enabled = true;
                txtGripD2.Enabled = true;
                txtGripA.Enabled = true;
                btnSaveGripper.Enabled = true;
                txtGripD1.Text = SystemMode.GripDistance_Close.ToString();
                txtGripD2.Text = SystemMode.GripDistance_Open.ToString();
                txtGripA.Text = SystemMode.Grip_Thickness.ToString();
            }
            else 
            {
                txtVaCumD.Enabled = false;
                txtGripD1.Enabled = false;
                txtGripD2.Enabled = false;
                txtGripA.Enabled = false;
                btnSaveGripper.Enabled = false;
                btnSaveVacum.Enabled = false;
            }
        }

        private void FrSettingRobot_Load(object sender, EventArgs e)
        {
            refresh();
        }

        private void btnSaveVacum_Click(object sender, EventArgs e)
        {
            SystemMode.VacumPad_Diameter = Convert.ToInt32(txtVaCumD.Text);
            refresh();
        }

        private void btnSaveGripper_Click(object sender, EventArgs e)
        {
            SystemMode.GripDistance_Close = Convert.ToInt32(txtGripD1.Text);
            SystemMode.GripDistance_Open = Convert.ToInt32(txtGripD2.Text);
            SystemMode.Grip_Thickness = Convert.ToInt32(txtGripA.Text);
            refresh();
        }

        private void UpdateUI(string key, object value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(key, value)));
                return;
            }
            if (key == "DoworkMess")
            {
                lbSttModbus.Text = value.ToString();
            }
        }
 
        private void btnConnect_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                string selectedType = cmbConnectionType.SelectedItem.ToString();
                if (selectedType == "Espon")
                {
                    BackgroundWorkerService.Instance.ConnectionType = PlcConnectionType.Modbus;
                }
                else if (selectedType == "Mitsubishi")
                {
                    BackgroundWorkerService.Instance.ConnectionType = PlcConnectionType.ActUtl;
                }

                // Cập nhật địa chỉ IP nếu cần (dành cho Modbus)
                BackgroundWorkerService.Instance.UpdateData("IpAddress", txtRobotIPaddress.Text);
                BackgroundWorkerService.Instance.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (SystemMode.ProcessStep == 2)
                {
                    SystemMode.ProcessStep = 4;
                    FrMain.Instance.UIUpdatebtn();
                }
                else if (SystemMode.ProcessStep == 1)
                {
                    SystemMode.ProcessStep = 3;
                    FrMain.Instance.UIUpdatebtn();
                }
            }
            */
            
            try
            {
                BackgroundWorkerService.Instance.Start();
                InvokeService.SendData("IpAddress", txtRobotIPaddress.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (SystemMode.ProcessStep == 2)
                {
                    SystemMode.ProcessStep = 4;
                    FrMain.Instance.UIUpdatebtn();
                }
                else if (SystemMode.ProcessStep == 1)
                {
                    SystemMode.ProcessStep = 3;
                    FrMain.Instance.UIUpdatebtn();
                }
            } 
            
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            BackgroundWorkerService.Instance.Stop();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            
    
        }
    }
}
