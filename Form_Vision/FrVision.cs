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
    public partial class FrVision : Form
    {
        FrStartingUp _Startup;
        FrDetection _Detection;
        FrPickTeaching _PickTeaching;
        FrResultEvaluation _ResultEvaluation;
        FrFinish _Finish;
        public FrVision()
        {
            InitializeComponent();
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void btnStartingUp_Click(object sender, EventArgs e)
        {
            FrMain.Instance.lbTitleform.Text = "Vision/Starting Up";
            if (_Startup == null)
            {
                _Startup = new FrStartingUp();
                _Startup.TopLevel = false;
                _Startup.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền
                _Startup.Dock = DockStyle.Fill;
                this.Controls.Add(_Startup); // Thêm vào Form2
                _Startup.BringToFront();
                _Startup.FormClosed += StartingUp_FormClosed;
                _Startup.Show();
            }
            else
            {
                _Startup.Activate();
                this.Controls.Add(_Startup);
                _Startup.BringToFront();
            }
        }
        private void StartingUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Startup = null;
        }

        private void btnDetection_Click(object sender, EventArgs e)
        {
            FrMain.Instance.lbTitleform.Text = "Vision/Detection";
            if (_Detection == null)
            {
                _Detection = new FrDetection();
                _Detection.TopLevel = false;
                _Detection.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền
                _Detection.Dock = DockStyle.Fill;
                this.Controls.Add(_Detection); // Thêm vào Form2
                _Detection.BringToFront();
                _Detection.FormClosed += Detection_FormClosed;
                _Detection.Show();
            }
            else
            {
                _Detection.Activate();
                this.Controls.Add(_Detection);
                _Detection.BringToFront();
            }
        }

        private void Detection_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Startup = null;
        }

        private void btnPickTeaching_Click(object sender, EventArgs e)
        {
            FrMain.Instance.lbTitleform.Text = "Vision/Pick Teaching";
            if (_PickTeaching == null)
            {
                _PickTeaching = new FrPickTeaching();
                _PickTeaching.TopLevel = false;
                _PickTeaching.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền
                _PickTeaching.Dock = DockStyle.Fill;
                this.Controls.Add(_PickTeaching); // Thêm vào Form2
                _PickTeaching.BringToFront();
                _PickTeaching.FormClosed += _PickTeaching_FormClosed;
                _PickTeaching.Show();
            }
            else
            {
                _PickTeaching.Activate();
                this.Controls.Add(_PickTeaching);
                _PickTeaching.BringToFront();
            }
        }

        private void _PickTeaching_FormClosed(object sender, FormClosedEventArgs e)
        {
            _PickTeaching = null;
        }

        private void btnResultEvalueation_Click(object sender, EventArgs e)
        {
            FrMain.Instance.lbTitleform.Text = "Vision/Evalueation";
            if (_ResultEvaluation == null)
            {
                _ResultEvaluation = new FrResultEvaluation();
                _ResultEvaluation.TopLevel = false;
                _ResultEvaluation.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền
                _ResultEvaluation.Dock = DockStyle.Fill;
                this.Controls.Add(_ResultEvaluation); // Thêm vào Form2
                _ResultEvaluation.BringToFront();
                _ResultEvaluation.FormClosed += _ResultEvaluation_FormClosed;
                _ResultEvaluation.Show();
            }
            else
            {
                _ResultEvaluation.Activate();
                this.Controls.Add(_ResultEvaluation);
                _ResultEvaluation.BringToFront();
            }
        }

        private void _ResultEvaluation_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ResultEvaluation = null;
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            FrMain.Instance.lbTitleform.Text = "Vision/Finish";
            if (_Finish == null)
            {
                _Finish = new FrFinish();
                _Finish.TopLevel = false;
                _Finish.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền
                _Finish.Dock = DockStyle.Fill;
                this.Controls.Add(_Finish); // Thêm vào Form2
                _Finish.BringToFront();
                _Finish.FormClosed += _Finish_FormClosed;
                _Finish.Show();
            }
            else
            {
                _Finish.Activate();
                this.Controls.Add(_Finish);
                _Finish.BringToFront();
            }
        }

        private void _Finish_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Finish = null;
        }

        private void FrVision_Activated(object sender, EventArgs e)
        {
            btnStartingUp_Click(null, null);
        }
    }
}
