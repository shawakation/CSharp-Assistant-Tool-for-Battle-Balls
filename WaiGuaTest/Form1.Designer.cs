namespace WaiGuaTest
{
    public partial class theMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(theMainForm));
            this.theXYLabel = new System.Windows.Forms.Label();
            this.theCrosshair = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.theCrosshair)).BeginInit();
            this.SuspendLayout();
            // 
            // theXYLabel
            // 
            this.theXYLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.theXYLabel.AllowDrop = true;
            this.theXYLabel.AutoEllipsis = true;
            this.theXYLabel.AutoSize = true;
            this.theXYLabel.BackColor = System.Drawing.Color.Black;
            this.theXYLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.theXYLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.theXYLabel.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.theXYLabel.ForeColor = System.Drawing.Color.White;
            this.theXYLabel.Location = new System.Drawing.Point(0, 0);
            this.theXYLabel.Name = "theXYLabel";
            this.theXYLabel.Size = new System.Drawing.Size(46, 16);
            this.theXYLabel.TabIndex = 0;
            this.theXYLabel.Text = "X:0  Y:0";
            this.theXYLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // theCrosshair
            // 
            this.theCrosshair.BackColor = System.Drawing.Color.Transparent;
            this.theCrosshair.BackgroundImage = global::WaiGuaTest.Properties.Resources.crosshair;
            this.theCrosshair.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.theCrosshair.Location = new System.Drawing.Point(0, 0);
            this.theCrosshair.Name = "theCrosshair";
            this.theCrosshair.Size = new System.Drawing.Size(60, 60);
            this.theCrosshair.TabIndex = 1;
            this.theCrosshair.TabStop = false;
            this.theCrosshair.Visible = false;
            // 
            // theMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            this.ClientSize = new System.Drawing.Size(405, 348);
            this.Controls.Add(this.theXYLabel);
            this.Controls.Add(this.theCrosshair);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MaximizeBox = false;
            this.Name = "theMainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wai Gua Test";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.theMainForm_FormClosing);
            this.Load += new System.EventHandler(this.theMainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.theCrosshair)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label theXYLabel;
        public System.Windows.Forms.PictureBox theCrosshair;
    }
}