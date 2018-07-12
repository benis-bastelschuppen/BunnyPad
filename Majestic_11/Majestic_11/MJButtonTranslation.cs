using SharpDX.XInput;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Majestic_11
{
    // a button and it's associated key config.
    public class MJButtonTranslation
    {
        // import mouse_event from user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_WHEEL = 0x0800;
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

        public voidDelegate onButtonDown;
        public voidDelegate onButtonUp;

        public MJButtonTranslation(GamepadButtonFlags btn, string keychars, byte FN = 0)
        {
            this.keyStroke = keychars;
            this.button = btn;
            this.FNindex = FN;
            // define default delegates.
            onButtonDown = new voidDelegate(hitKey);
            onButtonUp = new voidDelegate(voidFunc);
        }

        // a function which does nothing and returns nothing.
        public void voidFunc() { }

        // simulate a key hit.
        public void hitKey()
        {
            // send a key combination to the system, usually at text cursor position.
            SendKeys.SendWait(this.keyStroke);
        }

        public void Update(Gamepad pad, byte FNflag)
        {
            // check if the button is down or not and call the delegates if needed.
            if ((FNflag == FNindex) && ((pad.Buttons & this.button) == this.button))
            {
                if (!buttonDown)
                    this.onButtonDown();
                buttonDown = true;
            } else
            {
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
        // the actual FN state.
        protected byte FNflag = 0;
        // Time to wait until the key will be pressed each frame in ms.
        // Default is 500ms, a frame is 20ms fixed.
        // Keystroke delay of 0 means that the button only will be pressed once.
        public uint DefaultKeyStrokeDelay = 500;

        public MJConfig() { buttons = new List<MJButtonTranslation>(); }
        public void Update(Gamepad pad)
        {
            foreach (MJButtonTranslation btn in buttons)
            {
                btn.Update(pad, FNflag);
            }
        }

        public MJButtonTranslation addButton(MJButtonTranslation bt)
        {
            buttons.Add(bt);
            return bt;
        }

        public MJButtonTranslation addButton(GamepadButtonFlags btn, string keys, byte FNidx = 0)
        {
            MJButtonTranslation bt = new MJButtonTranslation(btn, keys, FNidx);
            buttons.Add(bt);
            return bt;
        }

        public void clearButtons()
        {
            buttons.Clear();
        }
    }
}
