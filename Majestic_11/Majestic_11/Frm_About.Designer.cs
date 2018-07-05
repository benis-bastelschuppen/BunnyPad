namespace Majestic_11
{
    partial class Frm_About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_About));
            this.btn_OK = new System.Windows.Forms.Button();
            this.aboutImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.aboutImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(525, 500);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(65, 29);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // aboutImage
            // 
            this.aboutImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("aboutImage.BackgroundImage")));
            this.aboutImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.aboutImage.Location = new System.Drawing.Point(-1, -44);
            this.aboutImage.Name = "aboutImage";
            this.aboutImage.Size = new System.Drawing.Size(605, 905);
            this.aboutImage.TabIndex = 2;
            this.aboutImage.TabStop = false;
            // 
            // Frm_About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 541);
            this.ControlBox = false;
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.aboutImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Frm_About";
            this.Text = "About [The Joy Of A Mouse]";
            this.Load += new System.EventHandler(this.Frm_About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.aboutImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.PictureBox aboutImage;
    }
}