namespace VDF3_Solution3
{
    partial class FrPickTeaching
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrPickTeaching));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnLoadImg = new System.Windows.Forms.ToolStripButton();
            this.btnTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnRotateLeft = new System.Windows.Forms.ToolStripButton();
            this.btnRotateRight = new System.Windows.Forms.ToolStripButton();
            this.btnReset = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbtnFindROI = new System.Windows.Forms.ToolStripButton();
            this.lstResults = new System.Windows.Forms.ListView();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.tsbtnReadImg = new System.Windows.Forms.ToolStripButton();
            this.btnTraning = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(929, 570);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.picBox_Paint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseUp);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripMargin = new System.Windows.Forms.Padding(5, 10, 10, 10);
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadImg,
            this.tsbtnReadImg,
            this.btnTrigger,
            this.btnRotateLeft,
            this.btnRotateRight,
            this.btnReset,
            this.btnZoomOut,
            this.btnZoomIn,
            this.tsbtnFindROI});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip2.Location = new System.Drawing.Point(5, 5);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1251, 32);
            this.toolStrip2.TabIndex = 14;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnLoadImg
            // 
            this.btnLoadImg.AutoSize = false;
            this.btnLoadImg.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadImg.Image")));
            this.btnLoadImg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadImg.Name = "btnLoadImg";
            this.btnLoadImg.Size = new System.Drawing.Size(100, 29);
            this.btnLoadImg.Text = "Load Image";
            this.btnLoadImg.Click += new System.EventHandler(this.btnLoadImg_Click);
            // 
            // btnTrigger
            // 
            this.btnTrigger.AutoSize = false;
            this.btnTrigger.Image = ((System.Drawing.Image)(resources.GetObject("btnTrigger.Image")));
            this.btnTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTrigger.Name = "btnTrigger";
            this.btnTrigger.Size = new System.Drawing.Size(100, 29);
            this.btnTrigger.Text = "Trigger";
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnRotateLeft.AutoSize = false;
            this.btnRotateLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeft.Image")));
            this.btnRotateLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(40, 29);
            this.btnRotateLeft.Text = "toolStripButton6";
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnRotateRight.AutoSize = false;
            this.btnRotateRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateRight.Image")));
            this.btnRotateRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(40, 29);
            this.btnRotateRight.Text = "toolStripButton7";
            // 
            // btnReset
            // 
            this.btnReset.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnReset.AutoSize = false;
            this.btnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(40, 29);
            this.btnReset.Text = "toolStripButton3";
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnZoomOut.AutoSize = false;
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(40, 29);
            this.btnZoomOut.Text = "toolStripButton2";
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnZoomIn.AutoSize = false;
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(40, 29);
            this.btnZoomIn.Text = "toolStripButton1";
            // 
            // tsbtnFindROI
            // 
            this.tsbtnFindROI.AutoSize = false;
            this.tsbtnFindROI.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnFindROI.Image")));
            this.tsbtnFindROI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnFindROI.Name = "tsbtnFindROI";
            this.tsbtnFindROI.Size = new System.Drawing.Size(100, 29);
            this.tsbtnFindROI.Text = "Pick Tool";
            this.tsbtnFindROI.Click += new System.EventHandler(this.tsbtnFindROI_Click);
            // 
            // lstResults
            // 
            this.lstResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstResults.HideSelection = false;
            this.lstResults.Location = new System.Drawing.Point(951, 55);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(302, 470);
            this.lstResults.TabIndex = 16;
            this.lstResults.UseCompatibleStateImageBehavior = false;
            // 
            // uiPanel1
            // 
            this.uiPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel1.Controls.Add(this.pictureBox);
            this.uiPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiPanel1.Location = new System.Drawing.Point(9, 55);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiPanel1.Size = new System.Drawing.Size(935, 576);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Gray;
            this.uiPanel1.TabIndex = 17;
            this.uiPanel1.Text = "uiPanel1";
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // tsbtnReadImg
            // 
            this.tsbtnReadImg.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnReadImg.Image")));
            this.tsbtnReadImg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnReadImg.Name = "tsbtnReadImg";
            this.tsbtnReadImg.Size = new System.Drawing.Size(98, 29);
            this.tsbtnReadImg.Text = "Read Image";
            this.tsbtnReadImg.Click += new System.EventHandler(this.tsbtnReadImg_Click);
            // 
            // btnTraning
            // 
            this.btnTraning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTraning.IconChar = FontAwesome.Sharp.IconChar.Running;
            this.btnTraning.IconColor = System.Drawing.Color.Black;
            this.btnTraning.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnTraning.Location = new System.Drawing.Point(951, 531);
            this.btnTraning.Name = "btnTraning";
            this.btnTraning.Size = new System.Drawing.Size(302, 100);
            this.btnTraning.TabIndex = 25;
            this.btnTraning.Text = "Running";
            this.btnTraning.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTraning.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnTraning.UseVisualStyleBackColor = true;
            // 
            // FrPickTeaching
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 641);
            this.ControlBox = false;
            this.Controls.Add(this.btnTraning);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.toolStrip2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrPickTeaching";
            this.Padding = new System.Windows.Forms.Padding(5);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.uiPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnLoadImg;
        private System.Windows.Forms.ToolStripButton btnTrigger;
        private System.Windows.Forms.ToolStripButton btnRotateLeft;
        private System.Windows.Forms.ToolStripButton btnRotateRight;
        private System.Windows.Forms.ToolStripButton btnReset;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton tsbtnFindROI;
        private System.Windows.Forms.ListView lstResults;
        private Sunny.UI.UIPanel uiPanel1;
        private System.Windows.Forms.ToolStripButton tsbtnReadImg;
        private FontAwesome.Sharp.IconButton btnTraning;
    }
}