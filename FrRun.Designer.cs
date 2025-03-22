namespace VDF3_Solution3
{
    partial class FrRun
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrRun));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnNewFile = new System.Windows.Forms.ToolStripButton();
            this.tsbtnOpenFile = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSaveFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnTrigger = new System.Windows.Forms.ToolStripButton();
            this.tsbtnContinuousTrigger = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnStart = new System.Windows.Forms.ToolStripButton();
            this.tsbtnStop = new System.Windows.Forms.ToolStripButton();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.PicRunRobot = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lwCameraRobot = new System.Windows.Forms.ListView();
            this.uiPanel2 = new Sunny.UI.UIPanel();
            this.pictemplateRunning = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicRunRobot)).BeginInit();
            this.panel1.SuspendLayout();
            this.uiPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictemplateRunning)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(5, 10, 10, 10);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnNewFile,
            this.tsbtnOpenFile,
            this.tsbtnSaveFile,
            this.toolStripSeparator2,
            this.tsbtnTrigger,
            this.tsbtnContinuousTrigger,
            this.toolStripSeparator1,
            this.tsbtnStart,
            this.tsbtnStop});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1383, 32);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnNewFile
            // 
            this.tsbtnNewFile.AutoSize = false;
            this.tsbtnNewFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnNewFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnNewFile.Image")));
            this.tsbtnNewFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnNewFile.Name = "tsbtnNewFile";
            this.tsbtnNewFile.Size = new System.Drawing.Size(40, 29);
            this.tsbtnNewFile.Text = "toolStripButton1";
            // 
            // tsbtnOpenFile
            // 
            this.tsbtnOpenFile.AutoSize = false;
            this.tsbtnOpenFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnOpenFile.Image")));
            this.tsbtnOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnOpenFile.Name = "tsbtnOpenFile";
            this.tsbtnOpenFile.Size = new System.Drawing.Size(40, 29);
            this.tsbtnOpenFile.Text = "toolStripButton2";
            // 
            // tsbtnSaveFile
            // 
            this.tsbtnSaveFile.AutoSize = false;
            this.tsbtnSaveFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSaveFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSaveFile.Image")));
            this.tsbtnSaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSaveFile.Name = "tsbtnSaveFile";
            this.tsbtnSaveFile.Size = new System.Drawing.Size(40, 29);
            this.tsbtnSaveFile.Text = "toolStripButton3";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbtnTrigger
            // 
            this.tsbtnTrigger.AutoSize = false;
            this.tsbtnTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnTrigger.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnTrigger.Image")));
            this.tsbtnTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnTrigger.Name = "tsbtnTrigger";
            this.tsbtnTrigger.Size = new System.Drawing.Size(40, 29);
            this.tsbtnTrigger.Text = "toolStripButton1";
            this.tsbtnTrigger.Click += new System.EventHandler(this.tsbtnTrigger_Click);
            // 
            // tsbtnContinuousTrigger
            // 
            this.tsbtnContinuousTrigger.AutoSize = false;
            this.tsbtnContinuousTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnContinuousTrigger.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnContinuousTrigger.Image")));
            this.tsbtnContinuousTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnContinuousTrigger.Name = "tsbtnContinuousTrigger";
            this.tsbtnContinuousTrigger.Size = new System.Drawing.Size(40, 29);
            this.tsbtnContinuousTrigger.Text = "toolStripButton2";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbtnStart
            // 
            this.tsbtnStart.AutoSize = false;
            this.tsbtnStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnStart.Image")));
            this.tsbtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStart.Name = "tsbtnStart";
            this.tsbtnStart.Size = new System.Drawing.Size(100, 29);
            this.tsbtnStart.Text = "System Start";
            this.tsbtnStart.Click += new System.EventHandler(this.tsbtnStart_Click);
            // 
            // tsbtnStop
            // 
            this.tsbtnStop.AutoSize = false;
            this.tsbtnStop.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnStop.Image")));
            this.tsbtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStop.Name = "tsbtnStop";
            this.tsbtnStop.Size = new System.Drawing.Size(100, 29);
            this.tsbtnStop.Text = "System Stop";
            this.tsbtnStop.Click += new System.EventHandler(this.tsbtnStop_Click);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel1.Controls.Add(this.PicRunRobot);
            this.uiPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiPanel1.Location = new System.Drawing.Point(7, 34);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Padding = new System.Windows.Forms.Padding(2);
            this.uiPanel1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiPanel1.Size = new System.Drawing.Size(884, 670);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Gray;
            this.uiPanel1.TabIndex = 17;
            this.uiPanel1.Text = "uiPanel1";
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // PicRunRobot
            // 
            this.PicRunRobot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PicRunRobot.Location = new System.Drawing.Point(2, 2);
            this.PicRunRobot.Name = "PicRunRobot";
            this.PicRunRobot.Size = new System.Drawing.Size(880, 666);
            this.PicRunRobot.TabIndex = 0;
            this.PicRunRobot.TabStop = false;
            this.PicRunRobot.Paint += new System.Windows.Forms.PaintEventHandler(this.PicRunRobot_Paint);
            this.PicRunRobot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.PicRunRobot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.PicRunRobot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lwCameraRobot);
            this.panel1.Controls.Add(this.uiPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(895, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(490, 676);
            this.panel1.TabIndex = 18;
            // 
            // lwCameraRobot
            // 
            this.lwCameraRobot.HideSelection = false;
            this.lwCameraRobot.Location = new System.Drawing.Point(2, 2);
            this.lwCameraRobot.Name = "lwCameraRobot";
            this.lwCameraRobot.Size = new System.Drawing.Size(488, 435);
            this.lwCameraRobot.TabIndex = 22;
            this.lwCameraRobot.UseCompatibleStateImageBehavior = false;
            // 
            // uiPanel2
            // 
            this.uiPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel2.Controls.Add(this.pictemplateRunning);
            this.uiPanel2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel2.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiPanel2.Location = new System.Drawing.Point(0, 441);
            this.uiPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Padding = new System.Windows.Forms.Padding(2);
            this.uiPanel2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiPanel2.Size = new System.Drawing.Size(490, 229);
            this.uiPanel2.Style = Sunny.UI.UIStyle.Gray;
            this.uiPanel2.TabIndex = 21;
            this.uiPanel2.Text = "uiPanel2";
            this.uiPanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // pictemplateRunning
            // 
            this.pictemplateRunning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictemplateRunning.Location = new System.Drawing.Point(2, 2);
            this.pictemplateRunning.Name = "pictemplateRunning";
            this.pictemplateRunning.Size = new System.Drawing.Size(486, 225);
            this.pictemplateRunning.TabIndex = 0;
            this.pictemplateRunning.TabStop = false;
            // 
            // FrRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1387, 712);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrRun";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicRunRobot)).EndInit();
            this.panel1.ResumeLayout(false);
            this.uiPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictemplateRunning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnNewFile;
        private System.Windows.Forms.ToolStripButton tsbtnOpenFile;
        private System.Windows.Forms.ToolStripButton tsbtnSaveFile;
        private System.Windows.Forms.ToolStripButton tsbtnTrigger;
        private System.Windows.Forms.ToolStripButton tsbtnContinuousTrigger;
        private System.Windows.Forms.ToolStripButton tsbtnStart;
        private System.Windows.Forms.ToolStripButton tsbtnStop;
        private Sunny.UI.UIPanel uiPanel1;
        private System.Windows.Forms.PictureBox PicRunRobot;
        private System.Windows.Forms.Panel panel1;
        private Sunny.UI.UIPanel uiPanel2;
        private System.Windows.Forms.PictureBox pictemplateRunning;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListView lwCameraRobot;
    }
}