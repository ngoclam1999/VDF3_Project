using FontAwesome.Sharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDF3_Solution3;

namespace VDF3_Solution3
{
    public partial class FrMain : Form
    {
        public static FrMain Instance { get; private set; }
        public Label lbTitleform { get; private set; } 

        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;
        FrSettingRobot _ConfigRobot;
        FrMain _Main;
        FrHome _Home;
        FrSetting _Setting;
        FrProject _Project;
        FrVision _Vision;
        FrRun _Run;
        FrRobot _Robot;
        public FrMain()
        {
            InitializeComponent();
            mdiProp();
            Instance = this;
            lbTitleform = lblTitleChildForm;
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            pnlMenu.Controls.Add(leftBorderBtn);
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            
        }
        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.White;
        }
        //Structs
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(35, 134, 54);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }
        //Methods
        public void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(10, 13, 18);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.Font = new Font(currentBtn.Font.FontFamily, 12, FontStyle.Bold);
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Current Child Form Icon
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = color;
            }
        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(13, 17, 23);
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.Font = new Font(currentBtn.Font.FontFamily, 12, FontStyle.Bold);
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }
       
        private void _Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Main = null;
        }
        private void FrMain_Load(object sender, EventArgs e)
        {
            lblTitleChildForm.Text = "Hone";
            IconButton btn = this.Controls.OfType<IconButton>().FirstOrDefault();
            if (btn != null)
            {
                ActivateButton(sender, RGBColors.color1);
            }
            if (_Home == null)
            {
                _Home = new FrHome();
                _Home.MdiParent = this;
                _Home.Dock = DockStyle.Fill;
                _Home.FormClosed += _Main_FormClosed;
                _Home.Show();
            }
            else
            {
                _Home.Activate();
            }

            UIUpdatebtn();
        }

        private void btnProject_Click(object sender, EventArgs e)
        {
            lblTitleChildForm.Text = "Project";
            ActivateButton(sender, RGBColors.color1);
            if (_Project == null)
            {
                _Project = new FrProject();
                _Project.MdiParent = this;
                _Project.Dock = DockStyle.Fill;
                _Project.FormClosed += _Main_FormClosed;
                _Project.Show();
            }
            else
            {
                _Project.Activate();
            }
        }
        

        private void btnSystem_Click(object sender, EventArgs e)
        {
            if (SystemMode.ProcessStep >= 1)
            {
                lblTitleChildForm.Text = "Setting/Camera";
                ActivateButton(sender, RGBColors.color1);
                if (_Setting == null)
                {
                    _Setting = new FrSetting();
                    _Setting.MdiParent = this;
                    _Setting.Dock = DockStyle.Fill;
                    _Setting.FormClosed += _Main_FormClosed;
                    _Setting.Show();
                }
                else
                {
                    _Setting.Activate();
                }
            }
            else
            {
                MessageBox.Show("Please select the project Before config parameters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);   
                return;
            }
        }

        private void btnVision_Click(object sender, EventArgs e)
        {
            if (SystemMode.ProcessStep == 1)//(SystemMode.ProcessStep == 2 || SystemMode.ProcessStep == 4)
            {
                lblTitleChildForm.Text = "Vision/Starting Up";
                ActivateButton(sender, RGBColors.color1);
                if (_Vision == null)
                {
                    _Vision = new FrVision();
                    _Vision.MdiParent = this;
                    _Vision.Dock = DockStyle.Fill;
                    _Vision.FormClosed += _Main_FormClosed;
                    _Vision.Show();
                }
                else
                {
                    _Vision.Activate();
                }
            }
            else
            {
                MessageBox.Show("Please connect camera Before config vision parameters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 
        }

        private void btnRobot_Click(object sender, EventArgs e)
        {
            if (SystemMode.ProcessStep == 1)//(SystemMode.ProcessStep >= 3)
            {
                lblTitleChildForm.Text = "Robot";
                ActivateButton(sender, RGBColors.color1);
                if (_Robot == null)
                {
                    _Robot = new FrRobot();
                    _Robot.MdiParent = this;
                    _Robot.Dock = DockStyle.Fill;
                    _Robot.FormClosed += _Main_FormClosed;
                    _Robot.Show();
                }
                else
                {
                    _Robot.Activate();
                }
            }
            else
            {
                MessageBox.Show("Please connect robot modbus TCP/IP Before teaching robot", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (SystemMode.ProcessStep == 1)//(SystemMode.ProcessStep >= 4)
            {
                lblTitleChildForm.Text = "Run";
                ActivateButton(sender, RGBColors.color1);
                if (_Run == null)
                {
                    _Run = new FrRun();
                    _Run.MdiParent = this;
                    _Run.Dock = DockStyle.Fill;
                    _Run.FormClosed += _Main_FormClosed;
                    _Run.Show();
                }
                else
                {
                    _Run.Activate();
                }
            }
            else
            {
                MessageBox.Show("Please complete all the device before running", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            lblTitleChildForm.Text = "Home";
            ActivateButton(sender, RGBColors.color1);
            if (_Home == null)
            {
                _Home = new FrHome();
                _Home.MdiParent = this;
                _Home.Dock = DockStyle.Fill;
                _Home.FormClosed += _Main_FormClosed;
                _Home.Show();
            }
            else
            {
                _Home.Activate();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                   "Are you sure you want to exit?",
                   "Exit the application",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if (FrSetting.hikCamera != null)
                    FrSetting.hikCamera.Stop();
                if(BackgroundWorkerService.Instance != null)
                    BackgroundWorkerService.Instance.Stop();

                Application.Exit(); // Đóng ứng dụng nếu người dùng chọn Yes
            }
        }
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        //Remove transparent border in maximized state
        private void FormMainMenu_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        public void UIUpdatebtn()
        {
            if (SystemMode.ProcessStep == 0) // mới load form
            {
                btnSystem.ForeColor = SystemColors.AppWorkspace;
                btnSystem.IconColor = SystemColors.AppWorkspace;

                btnVision.ForeColor = SystemColors.AppWorkspace;
                btnVision.IconColor = SystemColors.AppWorkspace;

                btnRobot.ForeColor = SystemColors.AppWorkspace;
                btnRobot.IconColor = SystemColors.AppWorkspace;

                btnRun.ForeColor = SystemColors.AppWorkspace;
                btnRun.IconColor = SystemColors.AppWorkspace;
            }
            else if (SystemMode.ProcessStep == 1) // chọn projet load thông số lên
            {
                btnSystem.ForeColor = SystemColors.ControlLightLight;
                btnSystem.IconColor = Color.WhiteSmoke;

                btnVision.ForeColor = SystemColors.AppWorkspace;
                btnVision.IconColor = SystemColors.AppWorkspace;

                btnRobot.ForeColor = SystemColors.AppWorkspace;
                btnRobot.IconColor = SystemColors.AppWorkspace;

                btnRun.ForeColor = SystemColors.AppWorkspace;
                btnRun.IconColor = SystemColors.AppWorkspace;
            }
            else if (SystemMode.ProcessStep == 2) // cấu hình camera
            {
                btnSystem.ForeColor = SystemColors.ControlLightLight;
                btnSystem.IconColor = Color.WhiteSmoke;

                btnVision.ForeColor = SystemColors.ControlLightLight;
                btnVision.IconColor = Color.WhiteSmoke;

                btnRobot.ForeColor = SystemColors.AppWorkspace;
                btnRobot.IconColor = SystemColors.AppWorkspace;

                btnRun.ForeColor = SystemColors.AppWorkspace;
                btnRun.IconColor = SystemColors.AppWorkspace;
            }
            else if (SystemMode.ProcessStep == 3) // cấu hình robot
            {
                btnSystem.ForeColor = SystemColors.ControlLightLight;
                btnSystem.IconColor = Color.WhiteSmoke;

                btnVision.ForeColor = SystemColors.AppWorkspace;
                btnVision.IconColor = SystemColors.AppWorkspace;

                btnRobot.ForeColor = SystemColors.ControlLightLight;
                btnRobot.IconColor = Color.WhiteSmoke;

                btnRun.ForeColor = SystemColors.AppWorkspace;
                btnRun.IconColor = SystemColors.AppWorkspace;
            }    
            else if (SystemMode.ProcessStep == 4) // hoàn thành cấu hình camera và robot
            {
                btnSystem.ForeColor = SystemColors.ControlLightLight;
                btnSystem.IconColor = Color.WhiteSmoke;

                btnVision.ForeColor = SystemColors.ControlLightLight;
                btnVision.IconColor = Color.WhiteSmoke;

                btnRobot.ForeColor = SystemColors.ControlLightLight;
                btnRobot.IconColor = Color.WhiteSmoke;

                btnRun.ForeColor = SystemColors.ControlLightLight;
                btnRun.IconColor = Color.WhiteSmoke;
            }    
        }
    }
}
