using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Majestic_11
{
    // a button and its associated key config.
    public class MJButtonTranslation
    {
        // import keyboard event only for setting the volume.
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        // import mouse_event from user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        
        //Mouse actions for the windows API.
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_WHEEL = 0x0800;

        // create the point structure for the windows getcursorpos function.
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        // import getcursorpos from user32.dll
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        // return the cursor position as POINT.
        public static POINT GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        // import setcursorpos from user32.dll
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        // ENDOF WINDOWS SPECIFIC

        public string keyStroke; // key combination to hit when the button is pressed.
        public GamepadButtonFlags button; // the associated gamepad button.

        // FNindex is the FN flag. There can be FN-buttons from 1  to 10.
        // if FNindex is 0, it will be used as primary button without holding an FN button.
        public byte FNindex = 0;

        // if this is 0, it will wait with the next hit until the button is up.
        // else it will wait the given millisecs and then hit the button every frame.
        public uint hitDelay = 0;
        public uint hitDelayCount = 0;

        // is the button actually down or not?
        protected bool buttonDown = false;
        public bool isButtonDown => buttonDown;

        // use this delegates for special functions. Default is sending a keystroke on buttonDown.
        public delegate void voidDelegate();

        // this are the "events" of a controller button.
        public voidDelegate onButtonDown;
        public voidDelegate onButtonUp;

        protected string actionText; // which action is associated to this button in human readable form.
        public string ActionText { get { return actionText; } set { actionText = value; } }

        // returns a string which describes the buttons config.
        public override string ToString()
        {
            string f = "";
            if (FNindex > 0)
                f = "FN + ";
            string rep = "";
            if (hitDelay > 0)
                rep = "[Repeating after " + hitDelay + "ms] ";
            return f+button.ToString() + " => " +rep+ actionText;
        }

        // keychars is the key combination which will be simulated when pressing that button.
        // the following keychars-strings are special cases:
        // "@leftmouse@" will simulate a left mouse click.
        // "@rightmouse@" will simulate a right mouse click.
        // "@middlemouse@" will simulate a click on the wheel of the mouse.
        // "@volumeup@" will raise the system volume.
        // "@volumedown@" will lower the system volume.
        // "@volumemute@" will mute the system volume.
        // All other combinations will be simulated over the keyboard, except you 
        // rewrite the delegate functions.
        public MJButtonTranslation(GamepadButtonFlags btn, string keychars, byte FN = 0)
        {
            this.keyStroke = keychars;
            this.button = btn;
            this.FNindex = FN;

            // define default delegates.
            onButtonDown = new voidDelegate(hitKey);
            onButtonUp = new voidDelegate(voidFunc);
            actionText = "Keyboard: "+keyStroke;

            // check if it is a mouse button, then use another delegate.
            if (keychars.ToLower() == "@leftmouse@")
            {
                onButtonDown = new voidDelegate(leftMouseDown);
                onButtonUp = new voidDelegate(leftMouseUp);
                actionText = "Left Mouse Button";
            }
            if (keychars.ToLower() == "@rightmouse@")
            {
                onButtonDown = new voidDelegate(rightMouseDown);
                onButtonUp = new voidDelegate(rightMouseUp);
                actionText = "Right Mouse Button";
            }
            if (keychars.ToLower() == "@middlemouse@")
            {
                onButtonDown = new voidDelegate(middleMouseDown);
                onButtonUp = new voidDelegate(middleMouseUp);
                actionText = "Middle Mouse Button";
            }

            // or maybe we want to set another volume?
            if(keychars.ToLower() == "@volumeup@")
            {
                onButtonDown = new voidDelegate(volumeUp);
                actionText = "Volume UP";
            }
            if (keychars.ToLower() == "@volumedown@")
            {
                onButtonDown = new voidDelegate(volumeDown);
                actionText = "Volume DOWN";
            }
            if (keychars.ToLower() == "@volumemute@")
            {
                onButtonDown = new voidDelegate(volumeMute);
                actionText = "MUTE Volume";
            }
        }

        // a function which does nothing and returns nothing.
        public void voidFunc() { }

        // simulate a key hit.
        public void hitKey()
        {
            // send a key combination to the system, usually at text cursor position.
            SendKeys.SendWait(this.keyStroke);
        }

        // do a click
        public void leftMouseDown() => mouseevt(MOUSEEVENTF_LEFTDOWN);
        public void rightMouseDown() => mouseevt(MOUSEEVENTF_RIGHTDOWN);
        public void middleMouseDown() => mouseevt(MOUSEEVENTF_MIDDLEDOWN);
        public void leftMouseUp() => mouseevt(MOUSEEVENTF_LEFTUP);
        public void rightMouseUp() => mouseevt(MOUSEEVENTF_RIGHTUP);
        public void middleMouseUp() => mouseevt(MOUSEEVENTF_MIDDLEUP);

        // adjust the system volume.
        public void volumeUp() => keybd_event((byte)Keys.VolumeUp, 0, 0, 0);
        public void volumeDown() => keybd_event((byte)Keys.VolumeDown, 0, 0, 0);
        public void volumeMute() => keybd_event((byte)Keys.VolumeMute, 0, 0, 0);

        // simulate a mouse click on the cursor position.
        protected void mouseevt(uint func)
        {
            POINT p = GetCursorPosition();
            mouse_event(func, (uint)p.X, (uint)p.Y, 0, 0);
        }

        public void Update(Gamepad pad, byte FNflag)
        {
            // check if the button is down or not and call the delegates if needed.
            if ((FNflag == FNindex) && ((pad.Buttons & this.button) == this.button))
            {
                if (!buttonDown)
                    this.onButtonDown();
                buttonDown = true;
            } else {
                if (buttonDown)
                    this.onButtonUp();
                hitDelayCount = 0;
                buttonDown = false;
            }

            // maybe hit the keys more than once.
            if (buttonDown == true && hitDelay >= 1)
            {
                if (hitDelayCount >= hitDelay)
                    this.onButtonDown();
                else
                    hitDelayCount += 20; // add 20 ms.
            }
        }
    }

    // a configuration.
    public class MJConfig
    {
        // the gamepad buttons.
        protected List<MJButtonTranslation> buttons;
        public List<MJButtonTranslation> Items => buttons;

        // the actual FN state.
        public byte FNflag = 0;

        // use such functions for setting the FN flag.
        public void FN1Down() { FNflag = 1; }
        //public void FNUp() { FNflag = 0; }

        // Time to wait until the key will be pressed each frame in ms.
        // Default is 500ms, a frame is 20ms fixed.
        // Keystroke delay of 0 means that the button only will be pressed once.
        public uint DefaultKeyStrokeDelay = 500;

        public MJConfig() { buttons = new List<MJButtonTranslation>(); }
        public void Update(Gamepad pad)
        {
            // first, just check for FN flags.
            // This is the bugfix which made 0.4.x to 0.5.x
            FNflag = 0;
            foreach (MJButtonTranslation btn in buttons)
            {
                if(btn.keyStroke=="@FN@")
                    btn.Update(pad, FNflag);
            }
            // then update the buttons.
            foreach (MJButtonTranslation btn in buttons)
            {
                btn.Update(pad, FNflag);
            }
        }

        public MJButtonTranslation addButton(MJButtonTranslation bt)
        {
            this.buttons.Add(bt);
            return bt;
        }

        public MJButtonTranslation addButton(GamepadButtonFlags btn, string keys, byte FNidx = 0)
        {
            MJButtonTranslation bt = new MJButtonTranslation(btn, keys, FNidx);
            this.buttons.Add(bt);
            return bt;
        }

        public bool removeButton(int index)
        {
            MJButtonTranslation btn = this.buttons[index];
            Log.Line("Remove button from config: " + btn.keyStroke.ToLower());
            if (btn.keyStroke.ToLower() == "@mainmenu@")
            {
                // TODO: check for other main menu.
                uint count = 0;
                foreach (MJButtonTranslation b in buttons)
                {
                    if (b.keyStroke.ToLower() == "@mainmenu@")
                        count++;
                }
                if (count > 1)
                {
                    this.buttons.RemoveAt(index);
                    return true;
                } else {
                    MessageBox.Show("You need at least one MAIN MENU button.", "Deletion not allowed!");
                    return false;
                }
            }
            else { this.buttons.RemoveAt(index); return true; }
        }

        public void clearButtons()
        {
            this.buttons.Clear();
        }

        public void loadHardcodedDefaultConfig()
        {
            this.clearButtons();
            MJButtonTranslation b; // you can change the button config after creation with b.

            // the main menu button => needs this keystroke!
            b = this.addButton(GamepadButtonFlags.Start, "@mainmenu@");
            b.onButtonDown = Program.SwitchMainFormVisibility;
            b.ActionText = "MENU BUTTON";

            // FN_1 button = need this keystroke!
            b = this.addButton(GamepadButtonFlags.LeftShoulder, "@FN@");
            b.hitDelay = 1;                   // smallest hitdelay possible (it's 20).
            b.onButtonDown = this.FN1Down;    // set FN to "true", ever.
                                              // b.onButtonUp = this.FNUp;         // set FN to "false", once. Will be overwritten by other FN's
            b.ActionText = "FN Modificator";  // TODO: remove that.  

            // mouse buttons => need this keystroke!
            b = this.addButton(GamepadButtonFlags.A, "@leftmouse@");
            b = this.addButton(GamepadButtonFlags.B, "@rightmouse@");
            b = this.addButton(GamepadButtonFlags.RightThumb, "@middlemouse@");

            // dpad buttons
            b = this.addButton(GamepadButtonFlags.DPadUp, "{UP}");
            b.hitDelay = this.DefaultKeyStrokeDelay;
            b = this.addButton(GamepadButtonFlags.DPadDown, "{DOWN}");
            b.hitDelay = this.DefaultKeyStrokeDelay;
            b = this.addButton(GamepadButtonFlags.DPadLeft, "{LEFT}");
            b.hitDelay = this.DefaultKeyStrokeDelay;
            b = this.addButton(GamepadButtonFlags.DPadRight, "{RIGHT}");
            b.hitDelay = this.DefaultKeyStrokeDelay;

            // backspace key
            b = this.addButton(GamepadButtonFlags.X, "{BACKSPACE}");
            b.hitDelay = this.DefaultKeyStrokeDelay;

            // esc key - only once
            b = this.addButton(GamepadButtonFlags.Back, "{ESC}");
            // enter key
            b = this.addButton(GamepadButtonFlags.Y, "{ENTER}");
            b.hitDelay = this.DefaultKeyStrokeDelay;

            // TABulator key
            b = this.addButton(GamepadButtonFlags.RightShoulder, "{TAB}");
            b.hitDelay = this.DefaultKeyStrokeDelay;

            // ctrl-c with FN_1
            b = this.addButton(GamepadButtonFlags.A, "^c", 1);
            // ctrl-v with FN_1
            b = this.addButton(GamepadButtonFlags.B, "^v", 1);
            // ctrl-z with FN_1
            b = this.addButton(GamepadButtonFlags.X, "^z", 1);
            // ctrl-y with FN_1
            b = this.addButton(GamepadButtonFlags.Y, "^y", 1);

            // volume UP with FN_1 => need this keystroke!
            b = this.addButton(GamepadButtonFlags.DPadUp, "@volumeup@", 1);
            b.hitDelay = this.DefaultKeyStrokeDelay;
            // volume DOWN with FN_1 => need this keystroke!
            b = this.addButton(GamepadButtonFlags.DPadDown, "@volumedown@",1);
            b.hitDelay = this.DefaultKeyStrokeDelay;
            // MUTE volume with FN_1 => need this keystroke!
            b = this.addButton(GamepadButtonFlags.DPadLeft, "@volumemute@",1);
        }
    }
}
