using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using SharpDX.XInput;

namespace Majestic_11
{
    public partial class Frm_ButtonConfig : Form
    {
        public Frm_ButtonConfig()
        {
            InitializeComponent();
        }

        private void Frm_ButtonConfig_Load(object sender, EventArgs e)
        {
            // fill the combo box with the buttons.
            combo_Button.Items.Clear();
            Array btvalues = Enum.GetValues(typeof(GamepadButtonFlags));
            foreach (GamepadButtonFlags btval in btvalues)
            {
                combo_Button.Items.Add(btval);
            }

            // fill the other combo box with the actions
            combo_Action.Items.Clear();
            combo_Action.Items.Add("Show Menu");
            combo_Action.Items.Add("Keyboard Combination");
            combo_Action.Items.Add("Left Mouse Button");
            combo_Action.Items.Add("Right Mouse Button");
            combo_Action.Items.Add("Middle Mouse Button");
            combo_Action.Items.Add("FN Modificator");

            LoadActualConfig();
            combo_Action.SelectedIndex = 0;
            combo_Button.SelectedIndex = 0;
        }

        private void LoadActualConfig()
        {
            // fill the listbox with the actual configuration.
            ListBox_Buttons.Items.Clear();
            foreach(MJButtonTranslation bt in Program.Input.Config.Items)
            {
                ListBox_Buttons.Items.Add(bt);
            }
        }

        public void Start()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void combo_Action_SelectedIndexChanged(object sender, EventArgs e)
        {
            // show key stuff if the keyboard action is selected.
            // keyboard is the second entry, zero based.
            if(combo_Action.SelectedIndex==1)
            {
                panel_keystuff.Enabled = true;
                txt_repeattime.Text = ""+Program.Input.Config.DefaultKeyStrokeDelay;
            } else {
                // do not repeat at all costs :)
                chk_repeat.Checked = false;
                txt_repeattime.Text = "0";
                panel_keystuff.Enabled = false;
            }

            // maybe disable the FN checkbox when the FN button would need FN for itself.
            // FN is the last entry.
            if(combo_Action.SelectedIndex == combo_Action.Items.Count-1)
            {
                chk_FN.Checked = false;
                chk_FN.Enabled = false;
            } else {
                chk_FN.Enabled = true;
            }
        }

        private void chk_repeat_CheckedChanged(object sender, EventArgs e)
        {
            // enable the wait-for-repeat-time textbox.
            if (chk_repeat.Checked)
            {
                txt_repeattime.Enabled = true;
            } else {
                txt_repeattime.Enabled = false;
            }
        }

        private void ListBox_Buttons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ListBox_Buttons.SelectedIndex >=0 )
            {
                checkForAddOrUpdate();
                btn_removeButton.Enabled = true;
            } else {
                btn_UpdateSelected.Enabled = false ;
                btn_removeButton.Enabled = false;
            }
        }

        private void combo_Button_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkForAddOrUpdate();
        }

        // check if all the values are right and the button can be added or updated.
        private bool checkForAddOrUpdate()
        {
            bool valid = true;
            if (combo_Button.Items[combo_Button.SelectedIndex].ToString() == "None")
                valid = false;

            if (valid)
            {
                btn_AddNew.Enabled = true;
                if (ListBox_Buttons.SelectedIndex >= 0)
                    btn_UpdateSelected.Enabled = true;
            } else {
                btn_AddNew.Enabled = false;
                btn_UpdateSelected.Enabled = false;
            }
            return valid;
        }
    }
}
