using System;
using System.ComponentModel;
using System.Windows.Forms;

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

            combo_Button.SelectedIndex = 0;
            // fill the other combo box with the actions
            updateFunctionCombo();
           // combo_Action.SelectedIndex = 0;

            LoadActualConfig();
            // show the config name.
            setActualConfigLbl();
        }

        // update the function combobox.
        protected byte actualFunctions = 0;
        protected void updateFunctionCombo()
        {
            byte actual = 1;
            switch(combo_Button.SelectedItem)
            {
                // if it is a stick, set actual to 2
                case EMJBUTTON.LeftThumbstick:
                case EMJBUTTON.RightThumbstick:
                    actual = 2;
                    break;
                default:
                    break;
            }

            // only reset if the button type changed.
            if (actual != actualFunctions)
            {
                combo_Action.Items.Clear();
                Array atvalues = Enum.GetValues(typeof(EMJFUNCTION));
                // check if it is a stick or button.
                foreach (EMJFUNCTION atval in atvalues)
                {
                    if((actual==1 && atval < EMJFUNCTION.__STICK_FUNCTIONS__) ||
                        (actual == 2 && atval > EMJFUNCTION.__STICK_FUNCTIONS__))
                            combo_Action.Items.Add(atval);
                }
            }
            actualFunctions = actual;
            combo_Action.SelectedIndex = 0;
        }

        // load the config into the UIs Listbox.
        private void LoadActualConfig()
        {
            // fill the listbox with the actual configuration.
            ListBox_Buttons.Items.Clear();
            foreach(MJButtonTranslation bt in Program.Input.Config.Items)
            {
                ListBox_Buttons.Items.Add(bt);
            }

            // set the checkboxes
            // invert y
            if (Program.Input.Config.invertYLeft < 0)
                chk_invertyleft.Checked = true;
            else
                chk_invertyleft.Checked = false;

            if (Program.Input.Config.invertYRight < 0)
                chk_invertyright.Checked = true;
            else
                chk_invertyright.Checked = false;

            // invert x
            if (Program.Input.Config.invertXLeft < 0)
                chk_invertxleft.Checked = true;
            else
                chk_invertxleft.Checked = false;

            if (Program.Input.Config.invertXRight < 0)
                chk_invertxright.Checked = true;
            else
                chk_invertxright.Checked = false;

            // exchange x for y
            chk_exchangexyleft.Checked = Program.Input.Config.exchangeXYLeft;
            chk_exchangexyright.Checked = Program.Input.Config.exchangeXYRight;

            // mouse speed slider
            hScrollBar1.Value = (int)(Program.Input.Config.baseMouseSpeed * 10);
            lbl_mousespeed.Text = hScrollBar1.Value.ToString();
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

        // another list item was selected.
        private void ListBox_Buttons_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 0.6.6: update the configuration items.
            MJButtonTranslation b = (MJButtonTranslation)ListBox_Buttons.SelectedItem;

            combo_Button.SelectedItem = b.button;
            combo_Action.SelectedItem = b.Function;
            if (b.FNindex > 0)
                chk_FN.Checked = true;
            else
                chk_FN.Checked = false;

            if(b.hitDelay>0)
            {
                chk_repeat.Checked = true;
                txt_repeattime.Text = b.hitDelay.ToString();
            }else{
                chk_repeat.Checked = false;
                txt_repeattime.Text = "0";
            }

            txt_keystroke.Text = b.keyStroke;

            enableButtons();
        }

        // another button was selected.
        private void combo_Button_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateFunctionCombo();
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

        // 0.6.9: create a button from the given values and return it.
        // was previously in the add-button function
        private MJButtonTranslation createButtonFromValues()
        {
            // empty button.
            MJButtonTranslation nobtn = new MJButtonTranslation(EMJBUTTON.None, EMJFUNCTION.SHOW_MENU);

            // Get the button.
            EMJBUTTON button = (EMJBUTTON)combo_Button.SelectedItem;
            // Get the action.
            EMJFUNCTION selaction = (EMJFUNCTION)combo_Action.SelectedItem;
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
                    return nobtn;
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
                    return nobtn;
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

            // return the new button.
            return btn;
        }

        // add the actual button config in the above field to the list.
        private void btn_AddNew_Click(object sender, EventArgs e)
        {
            MJButtonTranslation btn = createButtonFromValues();
            // if nothing went wrong, add it to the configuration.
            if(btn.button != EMJBUTTON.None)
                Program.Input.Config.addButton(btn);
            this.LoadActualConfig();
            this.enableButtons();
        }

        // update the selected item.
        private void btn_UpdateSelected_Click(object sender, EventArgs e)
        {
            MJButtonTranslation btn = createButtonFromValues();
            int si = ListBox_Buttons.SelectedIndex;
            // if nothing went wrong, add it to the configuration.
            if (btn.button != EMJBUTTON.None)
                Program.Input.Config.updateButton((MJButtonTranslation)ListBox_Buttons.SelectedItem, btn);
            this.LoadActualConfig();
            ListBox_Buttons.SelectedIndex = si;
            this.enableButtons();
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
            setActualConfigLbl();
        }

        private void dlg_saveConfig_FileOk(object sender, CancelEventArgs e)
        {
            string file = dlg_saveConfig.FileName;
            Log.Line("Saving config to: "+file);
            Program.Input.Config.SaveTo(file);
            setActualConfigLbl();
        }

        // set the text of the actual config label.
        private void setActualConfigLbl()
        {
            lbl_actualConfig.Text = "Actual config: "+Program.Input.Config.ConfigName;
        }

        // reset the config to the hardcoded defaults.
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons btns = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("This will overwrite the actual configuration.", "Really load default config?", btns);
            if(result == DialogResult.Yes)
            {
                Program.Input.Config.loadHardcodedDefaultConfig();
                lbl_actualConfig.Text = Program.Input.Config.ConfigName;
                this.LoadActualConfig();
                Log.Line("Default config loaded.");
            }
        }

        private void chk_invertyleft_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_invertyleft.Checked == true)
                Program.Input.Config.invertYLeft = -1;
            else
                Program.Input.Config.invertYLeft = 1;
        }

        private void chk_invertyright_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_invertyright.Checked == true)
                Program.Input.Config.invertYRight = -1;
            else
                Program.Input.Config.invertYRight = 1;

        }

        private void chk_invertxleft_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_invertxleft.Checked == true)
                Program.Input.Config.invertXLeft = -1;
            else
                Program.Input.Config.invertXLeft = 1;

        }

        private void chk_invertxright_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_invertxright.Checked == true)
                Program.Input.Config.invertXRight = -1;
            else
                Program.Input.Config.invertXRight = 1;
        }

        private void chk_exchangexyleft_CheckedChanged(object sender, EventArgs e)
        {
            Program.Input.Config.exchangeXYLeft = chk_exchangexyleft.Checked;
        }

        private void chk_exchangexyright_CheckedChanged(object sender, EventArgs e)
        {
            Program.Input.Config.exchangeXYRight = chk_exchangexyright.Checked;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            float v = hScrollBar1.Value;
            lbl_mousespeed.Text = hScrollBar1.Value.ToString();
            Program.Input.Config.baseMouseSpeed = hScrollBar1.Value * 0.1f;
        }
    }
}
