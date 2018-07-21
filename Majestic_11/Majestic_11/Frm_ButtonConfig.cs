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
            Array btvalues = Enum.GetValues(typeof(EMJBUTTON));
            foreach (EMJBUTTON btval in btvalues)
            {
                combo_Button.Items.Add(btval);
            }

            // fill the other combo box with the actions
            combo_Action.Items.Clear();
            Array atvalues = Enum.GetValues(typeof(EMJFUNCTION));
            foreach (EMJFUNCTION atval in atvalues)
            {
                combo_Action.Items.Add(atval);
            }

/*          combo_Action.Items.Clear();
            combo_Action.Items.Add("Show Menu");
            combo_Action.Items.Add("Keyboard Combination");
            combo_Action.Items.Add("Left Mouse Button");
            combo_Action.Items.Add("Right Mouse Button");
            combo_Action.Items.Add("Middle Mouse Button");
            combo_Action.Items.Add("Volume UP");
            combo_Action.Items.Add("Volume DOWN");
            combo_Action.Items.Add("MUTE Volume");
            combo_Action.Items.Add("FN Modificator");
*/

            LoadActualConfig();
            combo_Action.SelectedIndex = 0;
            combo_Button.SelectedIndex = 0;
        }

        // load the config into the UI.
        private void LoadActualConfig()
        {
            // fill the listbox with the actual configuration.
            ListBox_Buttons.Items.Clear();
            foreach(MJButtonTranslation bt in Program.Input.Config.Items)
            {
                ListBox_Buttons.Items.Add(bt);
            }
        }

        // call this after creating the window.
        public void Start()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        // configuration is ok.
        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        // another action selected, show or hide some stuff.
        private void combo_Action_SelectedIndexChanged(object sender, EventArgs e)
        {
            // show key stuff if the keyboard action is selected.
            // keyboard is the second entry, zero based.
            EMJFUNCTION itm = EMJFUNCTION.SHOW_MENU;
            if (combo_Action.SelectedIndex >= 0)
                itm = (EMJFUNCTION) combo_Action.Items[combo_Action.SelectedIndex];

            // show repeating flag and keyboard keys.
            if(itm==EMJFUNCTION.KEYBOARD_COMBINATION || itm==EMJFUNCTION.VOLUME_UP || itm==EMJFUNCTION.VOLUME_DOWN)
            {
                panel_keystuff.Enabled = true;
                txt_repeattime.Text = ""+Program.Input.Config.DefaultKeyStrokeDelay;
                if (itm == EMJFUNCTION.KEYBOARD_COMBINATION)
                {
                    txt_keystroke.Enabled = true;
                    lbl_keys.Enabled = true;
                }
                else
                {
                    txt_keystroke.Enabled = false;
                    lbl_keys.Enabled = false;
                }
            } else {
                // do not repeat at all costs :)
                chk_repeat.Checked = false;
                txt_repeattime.Text = "0";
                panel_keystuff.Enabled = false;
            }

            // maybe disable the FN checkbox when the FN button would need FN for itself.
            // FN is the last entry.
            if(itm == EMJFUNCTION.FN_MODIFICATOR)
            {
                chk_FN.Checked = false;
                chk_FN.Enabled = false;
            } else {
                chk_FN.Enabled = true;
            }
        }

        // repeating the keystroke or not?
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
            enableButtons();
        }

        private void combo_Button_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableButtons();
        }

        // enable or disable the remove, add and update button.
        private void enableButtons()
        {
            bool valid = true;
            // if the button is none, valid is false.
            if (combo_Button.Items[combo_Button.SelectedIndex].ToString() == "None")
                valid = false;

            // button is ok, enable the add button.
            if (valid)
                btn_AddNew.Enabled = true;
            else
                btn_AddNew.Enabled = false;

            // check if a listbox item is selected and enable some things.
            if (ListBox_Buttons.SelectedIndex >= 0)
            {
                btn_removeButton.Enabled = true;
                if(valid)
                    btn_UpdateSelected.Enabled = true;
                else
                    btn_UpdateSelected.Enabled = false;
            }
            else
            {
                btn_UpdateSelected.Enabled = false;
                btn_removeButton.Enabled = false;
            }
        }

        // remove a button from the config.
        private void btn_removeButton_Click(object sender, EventArgs e)
        {
            int idx = ListBox_Buttons.SelectedIndex;
            if (idx >= 0)
            {
                // only reload if the remove was successfull.
                if(Program.Input.Config.removeButton(idx))
                    LoadActualConfig();
            }
            enableButtons();
        }

        // add the actual button config in the above field to the list.
        private void btn_AddNew_Click(object sender, EventArgs e)
        {
            // Get the button.
            EMJBUTTON button = (EMJBUTTON)combo_Button.SelectedItem;
            // Get the action.
            EMJFUNCTION selaction = (EMJFUNCTION) combo_Action.SelectedItem;
            // keystroke, hitdelay and FN index.
            string mykeys = txt_keystroke.Text;
            byte fnidx = 0;
            int hitdelay = 0;

            // get the new hit delay.
            if (chk_repeat.Checked == true)
                hitdelay = Int32.Parse(txt_repeattime.Text);

            // get fn index.
            if (chk_FN.Checked == true)
                fnidx = 1;

            // get the desired button.
            // check if the button works.
            if (selaction == EMJFUNCTION.KEYBOARD_COMBINATION)
            {
                // it's a keyboard combination, it needs some keys.
                if (mykeys == "")
                {
                    MessageBox.Show("You need to set a key combination.", "Cannot create button:");
                    return;
                }

                // check if the keyboard combination works.
                try
                {
                    // create the button for testing.
                    MJButtonTranslation testbtn = new MJButtonTranslation(button, EMJFUNCTION.KEYBOARD_COMBINATION, fnidx, mykeys);
                    // select the test box so nothing bad can happen.
                    txt_Test.Focus();
                    testbtn.hitKey();
                    btn_AddNew.Focus();
                }
                catch
                {
                    MessageBox.Show("The key combination is invalid! (break)", "Cannot create button.");
                    return;
                }
            }

            // create the button
            MJButtonTranslation btn = new MJButtonTranslation(button, selaction, fnidx, mykeys);
            btn.hitDelay = (uint)hitdelay;

            // "external" settings.
            // set functions and stuff.
            switch (selaction)
            {
                case EMJFUNCTION.FN_MODIFICATOR:
                case EMJFUNCTION.SHOW_MENU:
                case EMJFUNCTION.SLOWER_MOVEMENT:
                case EMJFUNCTION.FASTER_MOVEMENT:
                    Program.Input.Config.assignExternalFunction(btn);
                    break;
                default:
                    break;
            }

            // if nothing went wrong, add it to the configuration.
            Program.Input.Config.addButton(btn);
            this.LoadActualConfig();
            enableButtons();
        }

        // show the doc for sendkeys function on msdn.
        private void lbl_keys_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.microsoft.com/dotnet/api/system.windows.forms.sendkeys");
        }

        private void lbl_keyinfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lbl_keys_LinkClicked(sender, e);
        }

        private void btn_saveConfig_Click(object sender, EventArgs e)
        {
            dlg_saveConfig.ShowDialog();
        }

        private void btn_loadConfig_Click(object sender, EventArgs e)
        {
            dlg_loadConfig.ShowDialog();
        }

        private void dlg_loadConfig_FileOk(object sender, CancelEventArgs e)
        {
            string file = dlg_loadConfig.FileName;
            Log.Line("Loading config from: " + file);
            Program.Input.Config.LoadFrom(file);
            this.LoadActualConfig();
        }

        private void dlg_saveConfig_FileOk(object sender, CancelEventArgs e)
        {
            string file = dlg_saveConfig.FileName;
            Log.Line("Saving config to: "+file);
            Program.Input.Config.SaveTo(file);
        }

        // reset the config to the hardcoded defaults.
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons btns = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("This will overwrite the actual configuration.", "Really load default config?", btns);
            if(result == DialogResult.Yes)
            {
                Program.Input.Config.loadHardcodedDefaultConfig();
                this.LoadActualConfig();
                Log.Line("Default config loaded.");
            }
        }
    }
}
