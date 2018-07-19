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
        }

        // add the actual button config in the above field to the list.
        private void btn_AddNew_Click(object sender, EventArgs e)
        {
            // we need a string here because it could be something 
            // other than gamepadbuttonflags.
            EMJBUTTON selbtn = (EMJBUTTON)combo_Button.SelectedItem;
            EMJFUNCTION selaction = (EMJFUNCTION) combo_Action.SelectedItem;
            string mykeys = txt_keystroke.Text;
            byte fnidx = 0;
            int hitdelay = 0;

            // get the new hit delay.
            if (chk_repeat.Checked == true)
                hitdelay = Int32.Parse(txt_repeattime.Text);

            // set fn index.
            if (chk_FN.Checked == true)
                fnidx = 1;

            // get the desired button.
            EMJBUTTON button = selbtn; // EMJBUTTON.None;
            // check if the button works.
            switch(selaction)
            { 
                case EMJFUNCTION.KEYBOARD_COMBINATION:
                    if (mykeys == "")
                    {
                        MessageBox.Show("You need to set a key combination.", "Cannot create button:");
                        return;
                    }

                    // check if the keyboard combination works.
                    try
                    {
                        // create the button for testing.
                        MJButtonTranslation testbtn = new MJButtonTranslation(button, mykeys, fnidx);
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
                    break;
                case EMJFUNCTION.LEFT_MOUSE_BUTTON:
                    mykeys = "@leftmouse@";
                    break;
                case EMJFUNCTION.RIGHT_MOUSE_BUTTON:
                    mykeys = "@rightmouse@";
                    break;
                case EMJFUNCTION.MIDDLE_MOUSE_BUTTON:
                    mykeys = "@middlemouse@";
                    break;
                case EMJFUNCTION.VOLUME_UP:
                    mykeys = "@volumeup@";
                    break;
                case EMJFUNCTION.VOLUME_DOWN:
                    mykeys = "@volumedown@";
                    break;
                case EMJFUNCTION.MUTE_VOLUME:
                    mykeys = "@mutevolume@";
                    break;
                case EMJFUNCTION.FN_MODIFICATOR:
                    mykeys = "@FN@";
                    break;
                case EMJFUNCTION.SHOW_MENU:
                    mykeys = "@mainmenu@";
                    break;
                default:
                    MessageBox.Show("Function not known!");
                    Log.Line("Function not known! "+selaction.ToString());
                    break;
            }

            // create the button
            MJButtonTranslation btn = new MJButtonTranslation(button, mykeys, fnidx);
            btn.hitDelay = (uint)hitdelay;

            // TODO: remove this two selactions!
            // set functions and stuff.
            switch (selaction)
            {
                case EMJFUNCTION.FN_MODIFICATOR:
                    btn.onButtonDown = Program.Input.Config.FN1Down;
                    btn.hitDelay = 1;
                    btn.ActionText = "FN MODIFICATOR";
                    break;
                case EMJFUNCTION.SHOW_MENU:
                    btn.onButtonDown = Program.SwitchMainFormVisibility;
                    btn.ActionText = "MAIN MENU";
                    break;
                case EMJFUNCTION.SLOWER_MOUSE:
                    btn.onButtonDown = Program.Input.Config.mouseSlower;
                    btn.onButtonUp = Program.Input.Config.mouseSlower_release;
                    btn.ActionText = "Slower Mouse";
                    break;
                case EMJFUNCTION.FASTER_MOUSE:
                    btn.onButtonDown = Program.Input.Config.mouseFaster;
                    btn.onButtonUp = Program.Input.Config.mouseFaster_release;
                    btn.ActionText = "Faster Mouse";
                    break;

                default:
                    break;
            }

            // if nothing went wrong, add it to the configuration.
            Program.Input.Config.addButton(btn);
            this.LoadActualConfig();
        }

        private void lbl_keys_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.microsoft.com/dotnet/api/system.windows.forms.sendkeys");
        }

        private void lbl_keyinfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lbl_keys_LinkClicked(sender, e);
        }
    }
}
