using System;
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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.LB_Console = new System.Windows.Forms.ListBox();
            this.lbl_connected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LB_Console
            // 
            this.LB_Console.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LB_Console.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Console.ForeColor = System.Drawing.Color.Lime;
            this.LB_Console.FormattingEnabled = true;
            this.LB_Console.ItemHeight = 16;
            this.LB_Console.Location = new System.Drawing.Point(12, 162);
            this.LB_Console.Name = "LB_Console";
            this.LB_Console.Size = new System.Drawing.Size(549, 276);
            this.LB_Console.TabIndex = 0;
            // 
            // lbl_connected
            // 
            this.lbl_connected.AutoSize = true;
            this.lbl_connected.Location = new System.Drawing.Point(12, 9);
            this.lbl_connected.Name = "lbl_connected";
            this.lbl_connected.Size = new System.Drawing.Size(175, 17);
            this.lbl_connected.TabIndex = 1;
            this.lbl_connected.Text = "- no controller connected -";
            // 
            // Frm_MJOY_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 450);
            this.Controls.Add(this.lbl_connected);
            this.Controls.Add(this.LB_Console);
            this.MaximizeBox = false;
            this.Name = "Frm_MJOY_Main";
            this.Text = "The joy of a mouse.";
            this.Load += new System.EventHandler(this.Frm_MJOY_Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox LB_Console;
        private System.Windows.Forms.Label lbl_connected;

        public void setLbl_connected(string text)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate { lbl_connected.Text = text; });
            }catch (Exception ex) { }
        }
    }
}

