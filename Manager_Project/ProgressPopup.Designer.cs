namespace VDF3_Solution3
{
    partial class ProgressPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.uiProgressIndicator1 = new Sunny.UI.UIProgressIndicator();
            this.timerStop = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // uiProgressIndicator1
            // 
            this.uiProgressIndicator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiProgressIndicator1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiProgressIndicator1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.uiProgressIndicator1.Location = new System.Drawing.Point(0, 0);
            this.uiProgressIndicator1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiProgressIndicator1.Name = "uiProgressIndicator1";
            this.uiProgressIndicator1.Size = new System.Drawing.Size(556, 397);
            this.uiProgressIndicator1.Style = Sunny.UI.UIStyle.Custom;
            this.uiProgressIndicator1.TabIndex = 0;
            this.uiProgressIndicator1.Text = "uiProgressIndicator1";
            this.uiProgressIndicator1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // timerStop
            // 
            this.timerStop.Interval = 1000;
            this.timerStop.Tick += new System.EventHandler(this.timerStop_Tick);
            // 
            // ProgressPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 397);
            this.ControlBox = false;
            this.Controls.Add(this.uiProgressIndicator1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressPopup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.ProgressPopup_Load);
            this.Leave += new System.EventHandler(this.ProgressPopup_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIProgressIndicator uiProgressIndicator1;
        private System.Windows.Forms.Timer timerStop;
    }
}