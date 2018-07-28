namespace Majestic_11
{
    partial class actionoverlay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(actionoverlay));
            this.img_vkOverlay = new System.Windows.Forms.PictureBox();
            this.img_smallChars = new System.Windows.Forms.PictureBox();
            this.img_bigChars = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.img_vkOverlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_smallChars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_bigChars)).BeginInit();
            this.SuspendLayout();
            // 
            // img_vkOverlay
            // 
            this.img_vkOverlay.BackColor = System.Drawing.Color.Transparent;
            this.img_vkOverlay.Image = ((System.Drawing.Image)(resources.GetObject("img_vkOverlay.Image")));
            this.img_vkOverlay.Location = new System.Drawing.Point(-1, 0);
            this.img_vkOverlay.Name = "img_vkOverlay";
            this.img_vkOverlay.Size = new System.Drawing.Size(553, 239);
            this.img_vkOverlay.TabIndex = 1;
            this.img_vkOverlay.TabStop = false;
            this.img_vkOverlay.Click += new System.EventHandler(this.img_vkOverlay_Click);
            // 
            // img_smallChars
            // 
            this.img_smallChars.Image = ((System.Drawing.Image)(resources.GetObject("img_smallChars.Image")));
            this.img_smallChars.Location = new System.Drawing.Point(-1, 2);
            this.img_smallChars.Name = "img_smallChars";
            this.img_smallChars.Size = new System.Drawing.Size(63, 53);
            this.img_smallChars.TabIndex = 2;
            this.img_smallChars.TabStop = false;
            this.img_smallChars.Visible = false;
            // 
            // img_bigChars
            // 
            this.img_bigChars.Image = ((System.Drawing.Image)(resources.GetObject("img_bigChars.Image")));
            this.img_bigChars.Location = new System.Drawing.Point(68, 2);
            this.img_bigChars.Name = "img_bigChars";
            this.img_bigChars.Size = new System.Drawing.Size(58, 53);
            this.img_bigChars.TabIndex = 3;
            this.img_bigChars.TabStop = false;
            this.img_bigChars.Visible = false;
            // 
            // actionoverlay
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(554, 245);
            this.ControlBox = false;
            this.Controls.Add(this.img_bigChars);
            this.Controls.Add(this.img_smallChars);
            this.Controls.Add(this.img_vkOverlay);
            this.DoubleBuffered = true;
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "actionoverlay";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "actionoverlay";
            this.Load += new System.EventHandler(this.actionoverlay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.img_vkOverlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_smallChars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.img_bigChars)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox img_vkOverlay;
        private System.Windows.Forms.PictureBox img_smallChars;
        private System.Windows.Forms.PictureBox img_bigChars;
    }
}