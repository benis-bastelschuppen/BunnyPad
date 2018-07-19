using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Majestic_11
{
    partial class Frm_MJOY_Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        //private byte vsUnblockErrorCount = 0;
        protected override void Dispose(bool disposing)
        {
            /* *Bug overrun - The bug is probably fixed, see BUGFIX_1 in Program.cs
             * Sometimes there is some unknown CreateHandle() wich intercepts this dispose when closing
            the app. This try-catch is a workaround. It just waits some millisecs and tries again then.
            To not block the app from closing if ALL went wrong, we count up to max 20, that is 1 second.
            FOUND IT: Labels were updated from another thread and...so...on.
            if it does not work either, uncomment this stuff below. */

            // try
            //{
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            //}catch(Exception ex)
            //{
                /* maybe (in 0.000000000000001% of all executions, never happened to me) it
                 may loop while ever blocking with something. We leave this recursive catch after
                 20 times. */
              //  vsUnblockErrorCount++;
              //  if (vsUnblockErrorCount > 20)
              //      return;
              //  Task.Delay(50); // wait 50 ms and try again
              //  this.Dispose(disposing);
            //}
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_MJOY_Main));
            this.lbl_connected = new System.Windows.Forms.Label();
            this.btn_quit = new System.Windows.Forms.Button();
            this.btn_minimize = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_about = new System.Windows.Forms.Button();
            this.btn_Config = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_connected
            // 
            this.lbl_connected.AutoSize = true;
            this.lbl_connected.Location = new System.Drawing.Point(11, 310);
            this.lbl_connected.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_connected.Name = "lbl_connected";
            this.lbl_connected.Size = new System.Drawing.Size(131, 13);
            this.lbl_connected.TabIndex = 1;
            this.lbl_connected.Text = "- no controller connected -";
            // 
            // btn_quit
            // 
            this.btn_quit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btn_quit.ForeColor = System.Drawing.Color.White;
            this.btn_quit.Location = new System.Drawing.Point(532, 272);
            this.btn_quit.Margin = new System.Windows.Forms.Padding(2);
            this.btn_quit.Name = "btn_quit";
            this.btn_quit.Size = new System.Drawing.Size(71, 27);
            this.btn_quit.TabIndex = 3;
            this.btn_quit.Text = "X Quit X";
            this.btn_quit.UseVisualStyleBackColor = false;
            this.btn_quit.Click += new System.EventHandler(this.btn_quit_Click);
            // 
            // btn_minimize
            // 
            this.btn_minimize.Location = new System.Drawing.Point(532, 303);
            this.btn_minimize.Margin = new System.Windows.Forms.Padding(2);
            this.btn_minimize.Name = "btn_minimize";
            this.btn_minimize.Size = new System.Drawing.Size(71, 27);
            this.btn_minimize.TabIndex = 1;
            this.btn_minimize.Text = "Hide Me";
            this.btn_minimize.UseVisualStyleBackColor = true;
            this.btn_minimize.Click += new System.EventHandler(this.btn_minimize_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(2, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(611, 288);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // btn_about
            // 
            this.btn_about.Location = new System.Drawing.Point(457, 303);
            this.btn_about.Margin = new System.Windows.Forms.Padding(2);
            this.btn_about.Name = "btn_about";
            this.btn_about.Size = new System.Drawing.Size(71, 27);
            this.btn_about.TabIndex = 2;
            this.btn_about.Text = "About";
            this.btn_about.UseVisualStyleBackColor = true;
            this.btn_about.Click += new System.EventHandler(this.btn_about_Click);
            // 
            // btn_Config
            // 
            this.btn_Config.Location = new System.Drawing.Point(382, 303);
            this.btn_Config.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Config.Name = "btn_Config";
            this.btn_Config.Size = new System.Drawing.Size(71, 27);
            this.btn_Config.TabIndex = 5;
            this.btn_Config.Text = "Config";
            this.btn_Config.UseVisualStyleBackColor = true;
            this.btn_Config.Click += new System.EventHandler(this.btn_Config_Click);
            // 
            // Frm_MJOY_Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(614, 341);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_connected);
            this.Controls.Add(this.btn_Config);
            this.Controls.Add(this.btn_about);
            this.Controls.Add(this.btn_minimize);
            this.Controls.Add(this.btn_quit);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_MJOY_Main";
            this.Text = "BunnyPad";
            this.Load += new System.EventHandler(this.Frm_MJOY_Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_connected;

        public void setLbl_connected(string text)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate { lbl_connected.Text = text; });
            }catch (Exception ex) { }
        }

        public string ConnectText => lbl_connected.Text;

        private Button btn_quit;
        private Button btn_minimize;
        private PictureBox pictureBox1;
        private Button btn_about;
        private Button btn_Config;
    }
}

