using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDF3_Solution3
{
    public partial class ProgressPopup : Form
    {
        public ProgressPopup()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent; // Hiển thị ở giữa form chính
        }

        private void ProgressPopup_Leave(object sender, EventArgs e)
        {

        }

        private void ProgressPopup_Load(object sender, EventArgs e)
        {
            timerStop.Start();
        }

        private void timerStop_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
