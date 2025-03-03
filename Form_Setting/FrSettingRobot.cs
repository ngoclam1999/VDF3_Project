using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDF3_Solution3
{
    public partial class FrSettingRobot : Form
    {
        FrSetting _ConfigCamera;
        public FrSettingRobot()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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
    }
}
