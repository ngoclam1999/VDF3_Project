using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDF3_Solution3
{
    
    public partial class FrPickTeaching : Form
    {

        public FrPickTeaching()
        {
            InitializeComponent();
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            
        }
        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
  
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            
        }
        private void tsbtnFindROI_Click(object sender, EventArgs e)
        {
            
        }

        private void tsbtnReadImg_Click(object sender, EventArgs e)
        {

        }
    }
}
