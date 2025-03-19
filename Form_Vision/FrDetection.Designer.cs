using System;
using System.Threading.Tasks;

namespace VDF3_Solution3
{
    partial class FrDetection
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
        private async Task InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrDetection));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnLoadImg = new System.Windows.Forms.ToolStripButton();
            this.btnTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnRotateLeft = new System.Windows.Forms.ToolStripButton();
            this.btnRotateRight = new System.Windows.Forms.ToolStripButton();
            this.btnReset = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSaveImage = new System.Windows.Forms.ToolStripButton();
            this.tsbtnBoundTool = new System.Windows.Forms.ToolStripButton();
            this.tsbtnEditBoudingBox = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRotateBouding = new System.Windows.Forms.ToolStripButton();
            this.btnManualRotateRight = new System.Windows.Forms.ToolStripButton();
            this.txtRotateDegrees = new System.Windows.Forms.ToolStripTextBox();
            this.btnManualRotateLeft = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new FontAwesome.Sharp.IconButton();
            this.uiPanel4 = new Sunny.UI.UIPanel();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrevious = new Sunny.UI.UISymbolButton();
            this.btnFirst = new Sunny.UI.UISymbolButton();
            this.btnLast = new Sunny.UI.UISymbolButton();
            this.btnNext = new Sunny.UI.UISymbolButton();
            this.uiListBox1 = new Sunny.UI.UIListBox();
            this.pnlContaner = new Sunny.UI.UIPanel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.picTemplate = new System.Windows.Forms.PictureBox();
            this.btnPickTemplate = new FontAwesome.Sharp.IconButton();
            this.btnTraining = new FontAwesome.Sharp.IconButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip2.SuspendLayout();
            this.uiPanel4.SuspendLayout();
            this.pnlContaner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripMargin = new System.Windows.Forms.Padding(5, 10, 10, 10);
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadImg,
            this.btnTrigger,
            this.btnRotateLeft,
            this.btnRotateRight,
            this.btnReset,
            this.btnZoomOut,
            this.btnZoomIn,
            this.toolStripSeparator4,
            this.tsbtnSaveImage,
            this.toolStripSeparator2,
            this.tsbtnBoundTool,
            this.tsbtnEditBoudingBox,
            this.toolStripSeparator1,
            this.tsbtnRotateBouding,
            this.btnManualRotateRight,
            this.txtRotateDegrees,
            this.btnManualRotateLeft});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip2.Location = new System.Drawing.Point(5, 5);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1251, 32);
            this.toolStrip2.TabIndex = 13;
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
            this.btnTrigger.Click += new System.EventHandler(this.btnTrigger_Click);
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
            this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
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
            this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click);
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
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
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
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
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
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // tsbtnSaveImage
            // 
            this.tsbtnSaveImage.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSaveImage.Image")));
            this.tsbtnSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSaveImage.Name = "tsbtnSaveImage";
            this.tsbtnSaveImage.Size = new System.Drawing.Size(96, 29);
            this.tsbtnSaveImage.Text = "Save Image";
            this.tsbtnSaveImage.Click += new System.EventHandler(this.tsbtnSaveImage_Click);
            // 
            // tsbtnBoundTool
            // 
            this.tsbtnBoundTool.AutoSize = false;
            this.tsbtnBoundTool.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnBoundTool.Image")));
            this.tsbtnBoundTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnBoundTool.Name = "tsbtnBoundTool";
            this.tsbtnBoundTool.Size = new System.Drawing.Size(100, 29);
            this.tsbtnBoundTool.Text = "Bound Tool";
            this.tsbtnBoundTool.Click += new System.EventHandler(this.tsbtnBoundTool_Click);
            // 
            // tsbtnEditBoudingBox
            // 
            this.tsbtnEditBoudingBox.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnEditBoudingBox.Image")));
            this.tsbtnEditBoudingBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnEditBoudingBox.Name = "tsbtnEditBoudingBox";
            this.tsbtnEditBoudingBox.Size = new System.Drawing.Size(104, 29);
            this.tsbtnEditBoudingBox.Text = "Edit Bouding";
            this.tsbtnEditBoudingBox.Click += new System.EventHandler(this.tsbtnEditBoudingBox_Click);
            // 
            // tsbtnRotateBouding
            // 
            this.tsbtnRotateBouding.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnRotateBouding.Image")));
            this.tsbtnRotateBouding.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRotateBouding.Name = "tsbtnRotateBouding";
            this.tsbtnRotateBouding.Size = new System.Drawing.Size(118, 29);
            this.tsbtnRotateBouding.Text = "Rotate Bouding";
            this.tsbtnRotateBouding.Click += new System.EventHandler(this.tsbtnRotateBouding_Click);
            // 
            // btnManualRotateRight
            // 
            this.btnManualRotateRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnManualRotateRight.Image = ((System.Drawing.Image)(resources.GetObject("btnManualRotateRight.Image")));
            this.btnManualRotateRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManualRotateRight.Name = "btnManualRotateRight";
            this.btnManualRotateRight.Size = new System.Drawing.Size(29, 29);
            this.btnManualRotateRight.Text = "toolStripButton2";
            this.btnManualRotateRight.Click += new System.EventHandler(this.btnManualRotateRight_Click);
            // 
            // txtRotateDegrees
            // 
            this.txtRotateDegrees.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRotateDegrees.Name = "txtRotateDegrees";
            this.txtRotateDegrees.Size = new System.Drawing.Size(50, 32);
            // 
            // btnManualRotateLeft
            // 
            this.btnManualRotateLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnManualRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnManualRotateLeft.Image")));
            this.btnManualRotateLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManualRotateLeft.Name = "btnManualRotateLeft";
            this.btnManualRotateLeft.Size = new System.Drawing.Size(29, 29);
            this.btnManualRotateLeft.Text = "toolStripButton1";
            this.btnManualRotateLeft.Click += new System.EventHandler(this.btnManualRotateLeft_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.IconChar = FontAwesome.Sharp.IconChar.Trash;
            this.btnDelete.IconColor = System.Drawing.Color.Black;
            this.btnDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnDelete.Location = new System.Drawing.Point(1166, 463);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 70);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // uiPanel4
            // 
            this.uiPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel4.Controls.Add(this.flowPanel);
            this.uiPanel4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel4.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.uiPanel4.Location = new System.Drawing.Point(222, 545);
            this.uiPanel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel4.Name = "uiPanel4";
            this.uiPanel4.Radius = 1;
            this.uiPanel4.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiPanel4.Size = new System.Drawing.Size(814, 130);
            this.uiPanel4.Style = Sunny.UI.UIStyle.Custom;
            this.uiPanel4.TabIndex = 23;
            this.uiPanel4.Text = null;
            this.uiPanel4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel4.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // flowPanel
            // 
            this.flowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPanel.AutoScroll = true;
            this.flowPanel.Location = new System.Drawing.Point(3, 5);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(808, 120);
            this.flowPanel.TabIndex = 13;
            this.flowPanel.WrapContents = false;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevious.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnPrevious.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnPrevious.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnPrevious.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnPrevious.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnPrevious.Location = new System.Drawing.Point(116, 550);
            this.btnPrevious.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnPrevious.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnPrevious.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnPrevious.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnPrevious.Size = new System.Drawing.Size(100, 120);
            this.btnPrevious.Style = Sunny.UI.UIStyle.Custom;
            this.btnPrevious.Symbol = 61514;
            this.btnPrevious.SymbolSize = 40;
            this.btnPrevious.TabIndex = 19;
            this.btnPrevious.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnPrevious.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFirst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFirst.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnFirst.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnFirst.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnFirst.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnFirst.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnFirst.Location = new System.Drawing.Point(10, 550);
            this.btnFirst.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnFirst.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnFirst.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnFirst.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnFirst.Size = new System.Drawing.Size(100, 120);
            this.btnFirst.Style = Sunny.UI.UIStyle.Custom;
            this.btnFirst.Symbol = 61513;
            this.btnFirst.SymbolSize = 40;
            this.btnFirst.TabIndex = 20;
            this.btnFirst.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnFirst.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnLast
            // 
            this.btnLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLast.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnLast.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnLast.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnLast.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnLast.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnLast.Location = new System.Drawing.Point(1148, 550);
            this.btnLast.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLast.Name = "btnLast";
            this.btnLast.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnLast.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnLast.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnLast.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnLast.Size = new System.Drawing.Size(100, 120);
            this.btnLast.Style = Sunny.UI.UIStyle.Custom;
            this.btnLast.Symbol = 61520;
            this.btnLast.SymbolSize = 40;
            this.btnLast.TabIndex = 21;
            this.btnLast.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnLast.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnNext.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnNext.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnNext.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnNext.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnNext.Location = new System.Drawing.Point(1042, 550);
            this.btnNext.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnNext.Name = "btnNext";
            this.btnNext.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnNext.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnNext.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnNext.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnNext.Size = new System.Drawing.Size(100, 120);
            this.btnNext.Style = Sunny.UI.UIStyle.Custom;
            this.btnNext.Symbol = 61518;
            this.btnNext.SymbolSize = 40;
            this.btnNext.TabIndex = 22;
            this.btnNext.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnNext.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // uiListBox1
            // 
            this.uiListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiListBox1.FillColor = System.Drawing.Color.White;
            this.uiListBox1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiListBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.uiListBox1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.uiListBox1.ItemSelectBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiListBox1.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiListBox1.Location = new System.Drawing.Point(996, 47);
            this.uiListBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiListBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiListBox1.Name = "uiListBox1";
            this.uiListBox1.Padding = new System.Windows.Forms.Padding(2);
            this.uiListBox1.Radius = 1;
            this.uiListBox1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiListBox1.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiListBox1.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiListBox1.ShowText = false;
            this.uiListBox1.Size = new System.Drawing.Size(254, 153);
            this.uiListBox1.Style = Sunny.UI.UIStyle.Custom;
            this.uiListBox1.TabIndex = 18;
            this.uiListBox1.Text = "uiListBox1";
            this.uiListBox1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.uiListBox1.SelectedIndexChanged += new System.EventHandler(this.uiListBox1_SelectedIndexChanged);
            // 
            // pnlContaner
            // 
            this.pnlContaner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContaner.Controls.Add(this.pictureBox);
            this.pnlContaner.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlContaner.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlContaner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlContaner.Location = new System.Drawing.Point(10, 47);
            this.pnlContaner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlContaner.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlContaner.Name = "pnlContaner";
            this.pnlContaner.Radius = 1;
            this.pnlContaner.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.pnlContaner.Size = new System.Drawing.Size(978, 486);
            this.pnlContaner.Style = Sunny.UI.UIStyle.Custom;
            this.pnlContaner.TabIndex = 17;
            this.pnlContaner.Text = null;
            this.pnlContaner.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.pnlContaner.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(972, 480);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPanel1.Controls.Add(this.picTemplate);
            this.uiPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiPanel1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiPanel1.Location = new System.Drawing.Point(996, 210);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.uiPanel1.Size = new System.Drawing.Size(254, 246);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Gray;
            this.uiPanel1.TabIndex = 26;
            this.uiPanel1.Text = "uiPanel1";
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiPanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // picTemplate
            // 
            this.picTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picTemplate.Location = new System.Drawing.Point(3, 4);
            this.picTemplate.Name = "picTemplate";
            this.picTemplate.Size = new System.Drawing.Size(248, 239);
            this.picTemplate.TabIndex = 0;
            this.picTemplate.TabStop = false;
            // 
            // btnPickTemplate
            // 
            this.btnPickTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickTemplate.IconChar = FontAwesome.Sharp.IconChar.TicketSimple;
            this.btnPickTemplate.IconColor = System.Drawing.Color.Black;
            this.btnPickTemplate.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPickTemplate.Location = new System.Drawing.Point(996, 463);
            this.btnPickTemplate.Name = "btnPickTemplate";
            this.btnPickTemplate.Size = new System.Drawing.Size(85, 70);
            this.btnPickTemplate.TabIndex = 24;
            this.btnPickTemplate.Text = "Template";
            this.btnPickTemplate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPickTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPickTemplate.UseVisualStyleBackColor = true;
            this.btnPickTemplate.Click += new System.EventHandler(this.btnPickTemplate_Click);
            // 
            // btnTraining
            // 
            this.btnTraining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTraining.IconChar = FontAwesome.Sharp.IconChar.BattleNet;
            this.btnTraining.IconColor = System.Drawing.Color.Black;
            this.btnTraining.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnTraining.Location = new System.Drawing.Point(1081, 463);
            this.btnTraining.Name = "btnTraining";
            this.btnTraining.Size = new System.Drawing.Size(85, 70);
            this.btnTraining.TabIndex = 24;
            this.btnTraining.Text = "Training";
            this.btnTraining.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTraining.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnTraining.UseVisualStyleBackColor = true;
            this.btnTraining.Click += new System.EventHandler(this.btnTraining_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 32);
            // 
            // FrDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 682);
            this.ControlBox = false;
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.btnTraining);
            this.Controls.Add(this.btnPickTemplate);
            this.Controls.Add(this.uiListBox1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.uiPanel4);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.pnlContaner);
            this.Controls.Add(this.toolStrip2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrDetection";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.uiPanel4.ResumeLayout(false);
            this.pnlContaner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTemplate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnLoadImg;
        private System.Windows.Forms.ToolStripButton btnTrigger;
        private System.Windows.Forms.ToolStripButton btnRotateLeft;
        private System.Windows.Forms.ToolStripButton btnRotateRight;
        private System.Windows.Forms.ToolStripButton btnReset;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton tsbtnBoundTool;
        private FontAwesome.Sharp.IconButton btnDelete;
        private Sunny.UI.UIPanel uiPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private Sunny.UI.UISymbolButton btnPrevious;
        private Sunny.UI.UISymbolButton btnFirst;
        private Sunny.UI.UISymbolButton btnLast;
        private Sunny.UI.UISymbolButton btnNext;
        private Sunny.UI.UIListBox uiListBox1;
        private Sunny.UI.UIPanel pnlContaner;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripButton tsbtnEditBoudingBox;
        private Sunny.UI.UIPanel uiPanel1;
        private System.Windows.Forms.PictureBox picTemplate;
        private FontAwesome.Sharp.IconButton btnPickTemplate;
        private System.Windows.Forms.ToolStripButton tsbtnRotateBouding;
        private FontAwesome.Sharp.IconButton btnTraining;
        private System.Windows.Forms.ToolStripTextBox txtRotateDegrees;
        private System.Windows.Forms.ToolStripButton btnManualRotateLeft;
        private System.Windows.Forms.ToolStripButton btnManualRotateRight;
        private System.Windows.Forms.ToolStripButton tsbtnSaveImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

        public EventHandler btnTraning_Click { get; private set; }
    }
}