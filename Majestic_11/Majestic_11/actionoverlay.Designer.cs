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
            this.lbl_overlay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_overlay
            // 
            this.lbl_overlay.AutoSize = true;
            this.lbl_overlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_overlay.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_overlay.Location = new System.Drawing.Point(2, -1);
            this.lbl_overlay.Name = "lbl_overlay";
            this.lbl_overlay.Size = new System.Drawing.Size(71, 69);
            this.lbl_overlay.TabIndex = 0;
            this.lbl_overlay.Text = "A";
            this.lbl_overlay.Click += new System.EventHandler(this.lbl_overlay_Click);
            // 
            // actionoverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(74, 69);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_overlay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "actionoverlay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "actionoverlay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_overlay;
    }
}