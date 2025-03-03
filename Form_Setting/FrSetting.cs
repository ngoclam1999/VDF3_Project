using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDF3_Solution3.Properties;

namespace VDF3_Solution3
{
    public partial class FrSetting : Form
    {
        FrSettingRobot _ConfigRobot;
        public FrSetting()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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
    }
}
