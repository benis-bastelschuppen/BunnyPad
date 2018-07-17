namespace Majestic_11
{
    partial class Frm_ButtonConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_ButtonConfig));
            this.ListBox_Buttons = new System.Windows.Forms.ListBox();
            this.btn_removeButton = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.combo_Button = new System.Windows.Forms.ComboBox();
            this.combo_Action = new System.Windows.Forms.ComboBox();
            this.grp_newBtn = new System.Windows.Forms.GroupBox();
            this.btn_AddNew = new System.Windows.Forms.Button();
            this.chk_FN = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_UpdateSelected = new System.Windows.Forms.Button();
            this.txt_keystroke = new System.Windows.Forms.TextBox();
            this.chk_repeat = new System.Windows.Forms.CheckBox();
            this.txt_repeattime = new System.Windows.Forms.TextBox();
            this.lbl_ms = new System.Windows.Forms.Label();
            this.lbl_keys = new System.Windows.Forms.Label();
            this.panel_keystuff = new System.Windows.Forms.Panel();
            this.grp_newBtn.SuspendLayout();
            this.panel_keystuff.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListBox_Buttons
            // 
            this.ListBox_Buttons.FormattingEnabled = true;
            this.ListBox_Buttons.Location = new System.Drawing.Point(9, 95);
            this.ListBox_Buttons.Margin = new System.Windows.Forms.Padding(2);
            this.ListBox_Buttons.Name = "ListBox_Buttons";
            this.ListBox_Buttons.Size = new System.Drawing.Size(380, 212);
            this.ListBox_Buttons.TabIndex = 0;
            this.ListBox_Buttons.SelectedIndexChanged += new System.EventHandler(this.ListBox_Buttons_SelectedIndexChanged);
            // 
            // btn_removeButton
            // 
            this.btn_removeButton.Enabled = false;
            this.btn_removeButton.Location = new System.Drawing.Point(394, 131);
            this.btn_removeButton.Margin = new System.Windows.Forms.Padding(2);
            this.btn_removeButton.Name = "btn_removeButton";
            this.btn_removeButton.Size = new System.Drawing.Size(82, 30);
            this.btn_removeButton.TabIndex = 1;
            this.btn_removeButton.Text = "Remove";
            this.btn_removeButton.UseVisualStyleBackColor = true;
            this.btn_removeButton.Click += new System.EventHandler(this.btn_removeButton_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(394, 277);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(2);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(82, 30);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // combo_Button
            // 
            this.combo_Button.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Button.Location = new System.Drawing.Point(52, 18);
            this.combo_Button.Margin = new System.Windows.Forms.Padding(2);
            this.combo_Button.Name = "combo_Button";
            this.combo_Button.Size = new System.Drawing.Size(142, 21);
            this.combo_Button.TabIndex = 3;
            this.combo_Button.SelectedIndexChanged += new System.EventHandler(this.combo_Button_SelectedIndexChanged);
            // 
            // combo_Action
            // 
            this.combo_Action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Action.Location = new System.Drawing.Point(221, 18);
            this.combo_Action.Margin = new System.Windows.Forms.Padding(2);
            this.combo_Action.Name = "combo_Action";
            this.combo_Action.Size = new System.Drawing.Size(159, 21);
            this.combo_Action.TabIndex = 4;
            this.combo_Action.SelectedIndexChanged += new System.EventHandler(this.combo_Action_SelectedIndexChanged);
            // 
            // grp_newBtn
            // 
            this.grp_newBtn.Controls.Add(this.btn_AddNew);
            this.grp_newBtn.Controls.Add(this.combo_Button);
            this.grp_newBtn.Controls.Add(this.combo_Action);
            this.grp_newBtn.Controls.Add(this.chk_FN);
            this.grp_newBtn.Controls.Add(this.label1);
            this.grp_newBtn.Location = new System.Drawing.Point(9, 10);
            this.grp_newBtn.Margin = new System.Windows.Forms.Padding(2);
            this.grp_newBtn.Name = "grp_newBtn";
            this.grp_newBtn.Padding = new System.Windows.Forms.Padding(2);
            this.grp_newBtn.Size = new System.Drawing.Size(467, 81);
            this.grp_newBtn.TabIndex = 5;
            this.grp_newBtn.TabStop = false;
            this.grp_newBtn.Text = "Edit or Add Button";
            // 
            // btn_AddNew
            // 
            this.btn_AddNew.Enabled = false;
            this.btn_AddNew.Location = new System.Drawing.Point(385, 18);
            this.btn_AddNew.Margin = new System.Windows.Forms.Padding(2);
            this.btn_AddNew.Name = "btn_AddNew";
            this.btn_AddNew.Size = new System.Drawing.Size(82, 50);
            this.btn_AddNew.TabIndex = 7;
            this.btn_AddNew.Text = "Add";
            this.btn_AddNew.UseVisualStyleBackColor = true;
            // 
            // chk_FN
            // 
            this.chk_FN.AutoSize = true;
            this.chk_FN.Location = new System.Drawing.Point(7, 20);
            this.chk_FN.Margin = new System.Windows.Forms.Padding(2);
            this.chk_FN.Name = "chk_FN";
            this.chk_FN.Size = new System.Drawing.Size(49, 17);
            this.chk_FN.TabIndex = 8;
            this.chk_FN.Text = "FN +";
            this.chk_FN.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "==>";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_UpdateSelected
            // 
            this.btn_UpdateSelected.Enabled = false;
            this.btn_UpdateSelected.Location = new System.Drawing.Point(394, 95);
            this.btn_UpdateSelected.Margin = new System.Windows.Forms.Padding(2);
            this.btn_UpdateSelected.Name = "btn_UpdateSelected";
            this.btn_UpdateSelected.Size = new System.Drawing.Size(82, 30);
            this.btn_UpdateSelected.TabIndex = 6;
            this.btn_UpdateSelected.Text = "<- Update ^";
            this.btn_UpdateSelected.UseVisualStyleBackColor = true;
            // 
            // txt_keystroke
            // 
            this.txt_keystroke.Location = new System.Drawing.Point(216, 6);
            this.txt_keystroke.Margin = new System.Windows.Forms.Padding(2);
            this.txt_keystroke.Name = "txt_keystroke";
            this.txt_keystroke.Size = new System.Drawing.Size(159, 20);
            this.txt_keystroke.TabIndex = 6;
            // 
            // chk_repeat
            // 
            this.chk_repeat.AutoSize = true;
            this.chk_repeat.Location = new System.Drawing.Point(2, 8);
            this.chk_repeat.Margin = new System.Windows.Forms.Padding(2);
            this.chk_repeat.Name = "chk_repeat";
            this.chk_repeat.Size = new System.Drawing.Size(85, 17);
            this.chk_repeat.TabIndex = 7;
            this.chk_repeat.Text = "Repeat after";
            this.chk_repeat.UseVisualStyleBackColor = true;
            this.chk_repeat.CheckedChanged += new System.EventHandler(this.chk_repeat_CheckedChanged);
            // 
            // txt_repeattime
            // 
            this.txt_repeattime.Enabled = false;
            this.txt_repeattime.Location = new System.Drawing.Point(83, 6);
            this.txt_repeattime.Margin = new System.Windows.Forms.Padding(2);
            this.txt_repeattime.Name = "txt_repeattime";
            this.txt_repeattime.Size = new System.Drawing.Size(31, 20);
            this.txt_repeattime.TabIndex = 8;
            this.txt_repeattime.Text = "250";
            this.txt_repeattime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbl_ms
            // 
            this.lbl_ms.AutoSize = true;
            this.lbl_ms.Location = new System.Drawing.Point(114, 9);
            this.lbl_ms.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_ms.Name = "lbl_ms";
            this.lbl_ms.Size = new System.Drawing.Size(20, 13);
            this.lbl_ms.TabIndex = 9;
            this.lbl_ms.Text = "ms";
            // 
            // lbl_keys
            // 
            this.lbl_keys.AutoSize = true;
            this.lbl_keys.Location = new System.Drawing.Point(186, 9);
            this.lbl_keys.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_keys.Name = "lbl_keys";
            this.lbl_keys.Size = new System.Drawing.Size(33, 13);
            this.lbl_keys.TabIndex = 10;
            this.lbl_keys.Text = "Keys:";
            this.lbl_keys.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel_keystuff
            // 
            this.panel_keystuff.Controls.Add(this.txt_keystroke);
            this.panel_keystuff.Controls.Add(this.lbl_keys);
            this.panel_keystuff.Controls.Add(this.txt_repeattime);
            this.panel_keystuff.Controls.Add(this.chk_repeat);
            this.panel_keystuff.Controls.Add(this.lbl_ms);
            this.panel_keystuff.Enabled = false;
            this.panel_keystuff.Location = new System.Drawing.Point(14, 52);
            this.panel_keystuff.Margin = new System.Windows.Forms.Padding(2);
            this.panel_keystuff.Name = "panel_keystuff";
            this.panel_keystuff.Size = new System.Drawing.Size(375, 35);
            this.panel_keystuff.TabIndex = 7;
            // 
            // Frm_ButtonConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(483, 314);
            this.Controls.Add(this.panel_keystuff);
            this.Controls.Add(this.btn_UpdateSelected);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_removeButton);
            this.Controls.Add(this.ListBox_Buttons);
            this.Controls.Add(this.grp_newBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Frm_ButtonConfig";
            this.Text = "Configuration Manager";
            this.Load += new System.EventHandler(this.Frm_ButtonConfig_Load);
            this.grp_newBtn.ResumeLayout(false);
            this.grp_newBtn.PerformLayout();
            this.panel_keystuff.ResumeLayout(false);
            this.panel_keystuff.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListBox_Buttons;
        private System.Windows.Forms.Button btn_removeButton;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.ComboBox combo_Button;
        private System.Windows.Forms.ComboBox combo_Action;
        private System.Windows.Forms.GroupBox grp_newBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_UpdateSelected;
        private System.Windows.Forms.Button btn_AddNew;
        private System.Windows.Forms.TextBox txt_keystroke;
        private System.Windows.Forms.CheckBox chk_repeat;
        private System.Windows.Forms.Label lbl_ms;
        private System.Windows.Forms.TextBox txt_repeattime;
        private System.Windows.Forms.Label lbl_keys;
        private System.Windows.Forms.Panel panel_keystuff;
        private System.Windows.Forms.CheckBox chk_FN;
    }
}