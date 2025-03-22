namespace VDF3_Solution3
{
    partial class FrProject
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pinThisItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.txtProjectPath = new Sunny.UI.UITextBox();
            this.txtProjectName = new Sunny.UI.UITextBox();
            this.btnOk = new Sunny.UI.UISymbolButton();
            this.btnBrower = new Sunny.UI.UISymbolButton();
            this.lbHelloPrjName = new Sunny.UI.UILabel();
            this.lbHelloTitle = new Sunny.UI.UILabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.copyPathToolStripMenuItem,
            this.pinThisItemToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(141, 70);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeFromListToolStripMenuItem_Click);
            // 
            // copyPathToolStripMenuItem
            // 
            this.copyPathToolStripMenuItem.Name = "copyPathToolStripMenuItem";
            this.copyPathToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.copyPathToolStripMenuItem.Text = "Copy path";
            this.copyPathToolStripMenuItem.Click += new System.EventHandler(this.copyPathToolStripMenuItem_Click);
            // 
            // pinThisItemToolStripMenuItem
            // 
            this.pinThisItemToolStripMenuItem.Name = "pinThisItemToolStripMenuItem";
            this.pinThisItemToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.pinThisItemToolStripMenuItem.Text = "Pin this item";
            this.pinThisItemToolStripMenuItem.Click += new System.EventHandler(this.pinToRecentListToolStripMenuItem_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(33, 215);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(596, 394);
            this.listView1.TabIndex = 20;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // txtProjectPath
            // 
            this.txtProjectPath.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.txtProjectPath.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.txtProjectPath.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.txtProjectPath.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.txtProjectPath.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.txtProjectPath.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.txtProjectPath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProjectPath.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtProjectPath.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.txtProjectPath.Location = new System.Drawing.Point(211, 110);
            this.txtProjectPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProjectPath.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtProjectPath.Name = "txtProjectPath";
            this.txtProjectPath.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.txtProjectPath.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.txtProjectPath.ShowText = false;
            this.txtProjectPath.Size = new System.Drawing.Size(345, 30);
            this.txtProjectPath.Style = Sunny.UI.UIStyle.Gray;
            this.txtProjectPath.TabIndex = 18;
            this.txtProjectPath.Text = "C:/Desktop/";
            this.txtProjectPath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtProjectPath.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // txtProjectName
            // 
            this.txtProjectName.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.txtProjectName.ButtonFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.txtProjectName.ButtonFillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.txtProjectName.ButtonRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.txtProjectName.ButtonRectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.txtProjectName.ButtonRectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.txtProjectName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProjectName.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtProjectName.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.txtProjectName.Location = new System.Drawing.Point(211, 68);
            this.txtProjectName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProjectName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.txtProjectName.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.txtProjectName.ShowText = false;
            this.txtProjectName.Size = new System.Drawing.Size(345, 30);
            this.txtProjectName.Style = Sunny.UI.UIStyle.Gray;
            this.txtProjectName.TabIndex = 19;
            this.txtProjectName.Text = "Project";
            this.txtProjectName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtProjectName.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // btnOk
            // 
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnOk.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnOk.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnOk.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnOk.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(421, 157);
            this.btnOk.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOk.Name = "btnOk";
            this.btnOk.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnOk.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnOk.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnOk.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnOk.Size = new System.Drawing.Size(133, 39);
            this.btnOk.Style = Sunny.UI.UIStyle.Gray;
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "OK";
            this.btnOk.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // btnBrower
            // 
            this.btnBrower.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrower.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnBrower.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnBrower.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnBrower.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnBrower.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnBrower.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrower.Location = new System.Drawing.Point(57, 106);
            this.btnBrower.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnBrower.Name = "btnBrower";
            this.btnBrower.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.btnBrower.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(163)))), ((int)(((byte)(163)))));
            this.btnBrower.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnBrower.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.btnBrower.Size = new System.Drawing.Size(122, 39);
            this.btnBrower.Style = Sunny.UI.UIStyle.Gray;
            this.btnBrower.Symbol = 61563;
            this.btnBrower.TabIndex = 17;
            this.btnBrower.Text = "Browser";
            this.btnBrower.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // lbHelloPrjName
            // 
            this.lbHelloPrjName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHelloPrjName.Location = new System.Drawing.Point(57, 68);
            this.lbHelloPrjName.Name = "lbHelloPrjName";
            this.lbHelloPrjName.Size = new System.Drawing.Size(131, 30);
            this.lbHelloPrjName.TabIndex = 14;
            this.lbHelloPrjName.Text = "Project Name: ";
            this.lbHelloPrjName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbHelloPrjName.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // lbHelloTitle
            // 
            this.lbHelloTitle.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHelloTitle.Location = new System.Drawing.Point(206, 20);
            this.lbHelloTitle.Name = "lbHelloTitle";
            this.lbHelloTitle.Size = new System.Drawing.Size(219, 30);
            this.lbHelloTitle.TabIndex = 15;
            this.lbHelloTitle.Text = "Create New Project";
            this.lbHelloTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbHelloTitle.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.lbHelloTitle);
            this.panel1.Controls.Add(this.lbHelloPrjName);
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.btnBrower);
            this.panel1.Controls.Add(this.txtProjectPath);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.txtProjectName);
            this.panel1.Location = new System.Drawing.Point(584, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 627);
            this.panel1.TabIndex = 21;
            // 
            // FrProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 652);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrProject";
            this.Activated += new System.EventHandler(this.FrProject_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrProject_FormClosing);
            this.Load += new System.EventHandler(this.FrProject_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pinThisItemToolStripMenuItem;
        public System.Windows.Forms.ListView listView1;
        private Sunny.UI.UITextBox txtProjectPath;
        private Sunny.UI.UITextBox txtProjectName;
        private Sunny.UI.UISymbolButton btnOk;
        private Sunny.UI.UISymbolButton btnBrower;
        private Sunny.UI.UILabel lbHelloPrjName;
        private Sunny.UI.UILabel lbHelloTitle;
        private System.Windows.Forms.Panel panel1;
    }
}