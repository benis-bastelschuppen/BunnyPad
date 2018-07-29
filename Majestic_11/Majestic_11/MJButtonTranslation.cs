/*
 * BunnyPad 
 * Work title: Majestic 11
 * a.k.a. JoyMouse
 * [The Joy Of A Mouse]
 * 
 * by Benedict "Oki Wan Ben0bi" Jäggi
 * (Joymouse) ~2002
 * Copyright 2018 Ben0bi Enterprises
 * https://github.com/ben0bi/BunnyPad
 * 
 * LICENSE:
 * Use of this source code and/or the executables is free in all terms for private use,
 * "private" hereby translated to "ONE natural person",
 * by adding the above credentials and this license text to your end-product.
 * Giving away, copying, and putting this product online in a LAN or WAN, altering the code,
 * derive new products and doing the same for or with them, is free for private use,
 * festivals, parties, events (especially eSport-events), hospitals, social institutions,
 * and schools where the oldest school clients (scholars) are less-equal than 18 years old 
 * (USA: 21 years). Every other commercial use is forbidden.
 * It is forbidden to sell this product or parts of it. Commercial use is not allowed in 
 * all terms except the ones declared above.
 */

 using SharpDX.XInput;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Majestic_11
{
    // What can we do with a button?
    public enum EMJFUNCTION
    {
        SHOW_MENU = 291,
        SWITCH_VIRTUAL_KEYBOARD = 821,
        KEYBOARD_COMBINATION = 1000,
        LEFT_MOUSE_BUTTON = 2000,
        RIGHT_MOUSE_BUTTON,
        MIDDLE_MOUSE_BUTTON,
        VOLUME_UP = 3000,
        VOLUME_DOWN,
        MUTE_VOLUME,
        FN_MODIFICATOR = 4000,
        SLOWER_MOVEMENT = 5000,
        FASTER_MOVEMENT,
        __STICK_FUNCTIONS__ = 10000,
        MOUSE_MOVEMENT = 11000,
        ARROW_KEYS,
        WASD_KEYS,
        MOUSE_WHEEL
    }

    // a stick or button.
    public enum EMJBUTTON
    {
        None = GamepadButtonFlags.None,
        DPadUp = GamepadButtonFlags.DPadUp,
        DPadDown = GamepadButtonFlags.DPadDown,
        DPadLeft = GamepadButtonFlags.DPadLeft,
        DPadRight = GamepadButtonFlags.DPadRight,
        Start = GamepadButtonFlags.Start,
        Back = GamepadButtonFlags.Back,
        LeftThumb = GamepadButtonFlags.LeftThumb,
        RightThumb = GamepadButtonFlags.RightThumb,
        LeftShoulder = GamepadButtonFlags.LeftShoulder,
        RightShoulder = GamepadButtonFlags.RightShoulder,
        // Special buttons are < 0
        LeftTrigger = -1,
        RightTrigger = -2,
        // main buttons.
        A = GamepadButtonFlags.A,
        B = GamepadButtonFlags.B,
        X = GamepadButtonFlags.X,
        Y = GamepadButtonFlags.Y,
        // The thumbsticks.
        LeftThumbstick=-10,
        RightThumbstick=-11
    }

    // a button and its associated key config.
    public class MJButtonTranslation
    {
        // WINDOWS SPECIFIC
        // import keyboard event only for setting the volume.
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        // import mouse_event from user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);
        
        //Mouse actions for the windows API.
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_WHEEL = 0x0800;
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        // ENDOF WINDOWS SPECIFIC

        public string keyStroke; // key combination to hit when the button is pressed.
        public EMJBUTTON button; // the associated gamepad button.
        protected EMJFUNCTION function; // the associated function.
        public EMJFUNCTION Function => function; // you cannot set the function directly.

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

        // 0.5.21 New constructor.
        public MJButtonTranslation(EMJBUTTON btn, EMJFUNCTION func, byte FN = 0, string keychars="")
        {
            this.keyStroke = keychars;
            this.button = btn;
            this.FNindex = FN;
            this.assignFunction(func);
        }

        // 0.6.0: save the button.
        public bool serialize(TextWriter bw)
        {
            try
            {
                // write all values to the file.
                bw.WriteLine((int)this.button);
                bw.WriteLine(this.keyStroke);
                bw.WriteLine(this.FNindex);
                bw.WriteLine((int)this.Function);
                bw.WriteLine(this.hitDelay);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 0.6.0: Another new constructor for loading a button.
        public MJButtonTranslation(TextReader br)
        {
            // read all the values from the file.
            string t=br.ReadLine();
            this.button = (EMJBUTTON)int.Parse(t);

            this.keyStroke = br.ReadLine();

            t = br.ReadLine();
            this.FNindex = byte.Parse(t);

            t = br.ReadLine();
            EMJFUNCTION func = (EMJFUNCTION)int.Parse(t);

            t = br.ReadLine();
            this.hitDelay = uint.Parse(t);

            this.assignFunction(func);
        }

        // assign some of the functions (internal ones)
        public void assignFunction(EMJFUNCTION func)
        {
            this.function = func;

            // define default delegates.
            onButtonDown = new voidDelegate(voidFunc);
            onButtonUp = new voidDelegate(voidFunc);
            actionText = "!NOTHING!";

            switch(this.function)
            {
                case EMJFUNCTION.KEYBOARD_COMBINATION:
                    onButtonDown = new voidDelegate(hitKey);
                    actionText = "Keyboard: " + keyStroke;
                    break;
                case EMJFUNCTION.LEFT_MOUSE_BUTTON:
                    onButtonDown = new voidDelegate(leftMouseDown);
                    onButtonUp = new voidDelegate(leftMouseUp);
                    actionText = "Left Mouse Button";
                    break;
                case EMJFUNCTION.RIGHT_MOUSE_BUTTON:
                    onButtonDown = new voidDelegate(rightMouseDown);
                    onButtonUp = new voidDelegate(rightMouseUp);
                    actionText = "Right Mouse Button";
                    break;
                case EMJFUNCTION.MIDDLE_MOUSE_BUTTON:
                    onButtonDown = new voidDelegate(middleMouseDown);
                    onButtonUp = new voidDelegate(middleMouseUp);
                    actionText = "Middle Mouse Button";
                    break;
                case EMJFUNCTION.VOLUME_UP:
                    onButtonDown = new voidDelegate(volumeUp);
                    actionText = "Volume UP";
                    break;
                case EMJFUNCTION.VOLUME_DOWN:
                    onButtonDown = new voidDelegate(volumeDown);
                    actionText = "Volume DOWN";
                    break;
                case EMJFUNCTION.MUTE_VOLUME:
                    onButtonDown = new voidDelegate(volumeMute);
                    actionText = "MUTE Volume";
                    break;
                // The stick functions do not need to be assigned.
                // They are executed otherwise.
                case EMJFUNCTION.ARROW_KEYS:
                    actionText = "Arrow Keys";
                    break;
                case EMJFUNCTION.WASD_KEYS:
                    actionText = "W A S D Keys";
                    break;
                case EMJFUNCTION.MOUSE_MOVEMENT:
                    actionText = "Mouse Movement";
                    break;
                case EMJFUNCTION.MOUSE_WHEEL:
                    actionText = "Mouse Wheel";
                    break;
                default:
                    break;
            }
        }

        // simulate a mouse click on the cursor position.
        protected void mouseevt(uint func) => mouse_event(func, (int)Cursor.Position.X, (int)Cursor.Position.Y, 0, 0);

        // some functions for the delegates.

        // a function which does nothing and returns nothing.
        public static void voidFunc() { }

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

        // update the button
        public void Update(Gamepad pad, byte FNflag)
        {
            // return if the virtual keyboard is on.
            if(Program.Input.Config.IsVirtualKeyboardOn && this.Function!=EMJFUNCTION.SWITCH_VIRTUAL_KEYBOARD && this.Function!=EMJFUNCTION.SHOW_MENU)
            {
                // The VK will be updated in the configs Update
                // function, not in the MJButtonTranslation one.
                return;
            }

            // check if the button is down or not and call the delegates if needed.
            GamepadButtonFlags fl = GamepadButtonFlags.None;
            bool isdown = false;
            // first check if it is a button, then check if it is down.
            // each value >= 0 is copied from GamepadButtonFlags.
            // own values (sticks and triggers) are <0.
            if (this.button >= 0)
            {
                fl = (GamepadButtonFlags)this.button;
                if ((FNflag == FNindex) && ((pad.Buttons & fl) == fl))
                    isdown = true;
            }else{
                //it's a special button, we need to do something other..
                byte trigger = 0;
                isdown = false;
                switch(this.button)
                {
                    case EMJBUTTON.RightTrigger:
                        trigger = pad.RightTrigger;
                        break;
                    case EMJBUTTON.LeftTrigger:
                        trigger = pad.LeftTrigger;
                        break;
                    case EMJBUTTON.LeftThumbstick:
                    case EMJBUTTON.RightThumbstick:
                        // it's a stick function, lets do
                        // some stick magic.
                        this.updateSticks(pad);
                        // we don't need the stuff below then.
                        return;
                    default:
                        isdown = false;
                        break;
                }
                if (trigger >= 10)
                    isdown = true;
            }

            // ok, the button is down.
            if (isdown)
            {
                if (!buttonDown)
                    this.onButtonDown();
                buttonDown = true;
            } else { // ..or not.
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

        // update the sticks and do the function for them.
        protected void updateSticks(Gamepad pad)
        {
            // TODO: remove stuff from program.input

            float stickx = 0;
            float sticky = 0;
            // get the raw values.
            switch (this.button)
            {
                case EMJBUTTON.LeftThumbstick:
                    stickx = pad.LeftThumbX;
                    sticky = pad.LeftThumbY;
                    break;
                case EMJBUTTON.RightThumbstick:
                    stickx = pad.RightThumbX;
                    sticky = pad.RightThumbY;
                    break;
                default:
                    break;
            }

            // implement deadzone
            float dzone = Program.Input.deadzone;
            if (stickx < dzone && stickx > -dzone)
                stickx = 0;
            if (sticky < dzone && sticky > -dzone)
                sticky = 0;

            // normalize the values
            stickx *= Program.Input.StickMultiplier;
            sticky *= Program.Input.StickMultiplier;

            int curx = Cursor.Position.X;
            int cury = Cursor.Position.Y;

            // simulate mouse movement or button hits.
            switch (this.Function)
            {
                case EMJFUNCTION.MOUSE_MOVEMENT:
                    // move mouse
                    curx = (int)(stickx * Program.Input.MouseSpeed);
                    cury = -(int)(sticky * Program.Input.MouseSpeed);
                    mouse_event(MOUSEEVENTF_MOVE, curx, cury, 0, 0);
                    break;
                case EMJFUNCTION.MOUSE_WHEEL:
                    // turn mouse wheel
                    if(sticky != 0)
                        mouse_event(MOUSEEVENTF_WHEEL, curx, cury, (uint)(sticky*Program.Input.MouseSpeed), 0);
                    break;
                case EMJFUNCTION.ARROW_KEYS:
                case EMJFUNCTION.WASD_KEYS:
                    // simulate keypresses.
                    string updown = "";
                    string leftright = "";
                    if(this.Function == EMJFUNCTION.ARROW_KEYS)
                    {
                        if (stickx < 0) leftright = "{LEFT}";
                        if (stickx > 0) leftright = "{RIGHT}";
                        if (sticky > 0) updown = "{UP}";
                        if (sticky < 0) updown = "{DOWN}";
                    }else{
                        if (stickx < 0) leftright = "a";
                        if (stickx > 0) leftright = "d";
                        if (sticky > 0) updown = "w";
                        if (sticky < 0) updown = "s";
                    }
                    if (updown != "")
                        SendKeys.SendWait(updown);
                    if (leftright!="")
                        SendKeys.SendWait(leftright);
                    break;
                // TODO: add the other functions.
                default:
                    break;
            }
        }
    }

//----------------------------------------------------------------

    // a configuration.
    public class MJConfig
    {
        // the gamepad buttons.
        protected List<MJButtonTranslation> buttons;
        public List<MJButtonTranslation> Items => buttons;

        // 0.5.12: new flags for the mouse speed.
        protected bool mousespeed_slower = false;
        protected bool mousespeed_faster = false;
        public bool MouseSpeed_Slower => mousespeed_slower;
        public bool MouseSpeed_Faster => mousespeed_faster;
        public void mouseSlower() { mousespeed_slower = true; }
        public void mouseSlower_release() { mousespeed_slower = false; }
        public void mouseFaster() { mousespeed_faster = true; }
        public void mouseFaster_release() { mousespeed_faster = false; }

        // the actual FN state.
        public byte FNflag = 0;

        // use such functions for setting the FN flag.
        public void FN1Down() { FNflag = 1; }

        // 0.7.2: Virtual keyboard stuff.
        protected bool showVirtualKeyboard = false;
        public bool IsVirtualKeyboardOn => showVirtualKeyboard;
        public void switchVirtualKeyboard()
        {
            showVirtualKeyboard = !showVirtualKeyboard;
            Program.SwitchVKVisibility(showVirtualKeyboard);
        }

        // Time to wait until the key will be pressed each frame in ms.
        // Default is 500ms, a frame is 20ms fixed.
        // Keystroke delay of 0 means that the button only will be pressed once.
        public uint DefaultKeyStrokeDelay = 500;

        protected string configName = "[unknown]";
        public string ConfigName => configName;

        // Constructor.
        public MJConfig()
        {
            buttons = new List<MJButtonTranslation>();
            createVKArray(); // 0.7.12
        }

        // <0.7.9 This function updates the virtual keyboard functionality.
        // Move the cursor with left or right stick.
        // Use left trigger to directly hit the key under the cursor.
        // Use A B X Y Leftshoulder for hitting the key assigned
        // to the given button under the cursor.
        // use right trigger for shift key.
        int vkPosX = 0;
        int vkPosY = 0;
        static int vkMaxX = 10;
        static int vkMaxY = 5;
        int lastVKCursorKey = -1;
        int VKtimer = 0;
        bool VKfirsttime = true;
        bool vkshiftkey = false;
        public void UpdateVirtualKeyboard(Gamepad pad)
        {
            // Get the shift flag.
            vkshiftkey = (pad.RightTrigger > 10 || pad.LeftTrigger > 10) ? true : false;

            // get the actual cursor moving parameter.
            // 0.7.11: also use thumbsticks
            float dzone = Program.Input.deadzone;
            int moveCursor = -1;
            if ((pad.Buttons & GamepadButtonFlags.DPadUp) == GamepadButtonFlags.DPadUp ||
                pad.LeftThumbY > dzone || pad.RightThumbY > dzone)
                moveCursor = 1; // up
            if ((pad.Buttons & GamepadButtonFlags.DPadDown) == GamepadButtonFlags.DPadDown ||
                pad.LeftThumbY < -dzone || pad.RightThumbY < -dzone)
                moveCursor = 2; // down
            if ((pad.Buttons & GamepadButtonFlags.DPadLeft) == GamepadButtonFlags.DPadLeft ||
                pad.LeftThumbX < -dzone || pad.RightThumbX < -dzone)
                moveCursor = 3; // left
            if ((pad.Buttons & GamepadButtonFlags.DPadRight) == GamepadButtonFlags.DPadRight ||
                pad.LeftThumbX > dzone || pad.RightThumbX > dzone)
                    moveCursor = 4; // right

            // set the y cursor position directly 
            // with the shortcut keys. And then press.
            if ((pad.Buttons & GamepadButtonFlags.RightShoulder) == GamepadButtonFlags.RightShoulder)
                moveCursor = 9;
            if ((pad.Buttons & GamepadButtonFlags.A) == GamepadButtonFlags.A)
                moveCursor = 8;
            if ((pad.Buttons & GamepadButtonFlags.B) == GamepadButtonFlags.B)
                moveCursor = 7;
            if ((pad.Buttons & GamepadButtonFlags.X) == GamepadButtonFlags.X)
                moveCursor = 6;
            if ((pad.Buttons & GamepadButtonFlags.Y) == GamepadButtonFlags.Y)
                moveCursor = 5;

            // or hit the key under the cursor directly with LS.
            if ((pad.Buttons & GamepadButtonFlags.LeftShoulder) == GamepadButtonFlags.LeftShoulder)
                moveCursor = 10;

            // update the cursor position, but just once.
            if (moveCursor!=lastVKCursorKey ||
                (VKfirsttime==false && VKtimer > 60) ||
                (VKfirsttime == true && VKtimer > 250))
            {
                switch(moveCursor)
                {
                    case 1: // up
                        vkPosY -= 1;
                        if (vkPosY < 0)
                            vkPosY = vkMaxY - 1;
                        break;
                    case 2: // down
                        vkPosY += 1;
                        if (vkPosY >= vkMaxY)
                            vkPosY = 0;
                        break;
                    case 3: // left
                        vkPosX -= 1;
                        if (vkPosX < 0)
                            vkPosX = vkMaxX - 1;
                        break;
                    case 4: // right
                        vkPosX += 1;
                        if (vkPosX >= vkMaxX)
                            vkPosX = 0;
                        break;
                    // special shortcut cursor 
                    // positions with a direct keyhit.
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        vkPosY = moveCursor - 5;
                        hitCursor();
                        break;
                    case 10:
                        hitCursor();
                        break;
                    default:
                        break;
                }
                // reset timer.
                VKtimer = 0;
                // reset the first timer only
                // if another key is pressed.
                if (moveCursor == lastVKCursorKey)
                    VKfirsttime = false;
                else
                    VKfirsttime = true;
            }
            lastVKCursorKey = moveCursor;
            VKtimer += 20;
            Program.UpdateVKForm(vkshiftkey, vkPosX, vkPosY);
        }

        // v0.7.12 hit the virtual keyboard key.        
        protected void hitCursor()
        {
            string hit = vkArray[vkPosX, vkPosY, (vkshiftkey == true ? 1 : 0)];
            Log.Line("VK: "+hit);
            SendKeys.SendWait(hit);
        }


        // this function updates all the stuff.
        public void Update(Gamepad pad)
        {
            // first, just check for FN flags.
            // This is the bugfix which made 0.4.x to 0.5.x
            FNflag = 0;
            foreach (MJButtonTranslation btn in buttons)
            {
                if (btn.Function == EMJFUNCTION.FN_MODIFICATOR)
                    btn.Update(pad, FNflag);
            }
            // then update the buttons.
            foreach (MJButtonTranslation btn in buttons)
            {
                btn.Update(pad, FNflag);
            }

            // update the virtual keyboard if the keyboard is on.
            if (Program.Input.Config.IsVirtualKeyboardOn)
            {
                this.UpdateVirtualKeyboard(pad);
            }
        }

        // TODO: Check if button already exists.
        // add a button to the config. the button must exist.
        public MJButtonTranslation addButton(MJButtonTranslation bt)
        {
            this.buttons.Add(bt);
            return bt;
        }

        // update a specific button in the list with a new one.
        public MJButtonTranslation updateButton(MJButtonTranslation target, MJButtonTranslation newone)
        {
            int i = 0;
            foreach(MJButtonTranslation bt in this.buttons)
            {
                if(bt == target)
                {
                    this.buttons.Insert(i, newone);
                    this.buttons.Remove(bt);
                    return newone;
                }
                i++;
            }
            return newone;
        }

        // add a new button to the config. you can alter it afterwards.
        public MJButtonTranslation addButton(EMJBUTTON btn, EMJFUNCTION func, byte FNidx = 0, string keys ="")
        {
            MJButtonTranslation bt = new MJButtonTranslation(btn, func, FNidx, keys);
            this.buttons.Add(bt);
            return bt;
        }

        // remove a button from the config.
        public bool removeButton(int index)
        {
            MJButtonTranslation btn = this.buttons[index];
            Log.Line("Remove button from config: " + btn.keyStroke.ToLower());
            if (btn.keyStroke.ToLower() == "@mainmenu@")
            {
                // Check for other main menu.
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

        // remove all buttons from the config.
        public void clearButtons()
        {
            this.buttons.Clear();
        }

        // assign a "not button translation conform" function to a button.
        public MJButtonTranslation assignExternalFunction(MJButtonTranslation bt)
        {
            // check if the button has an external function and
            // assign the functions for it.
            switch(bt.Function)
            {
                case EMJFUNCTION.SHOW_MENU:
                    bt.onButtonDown = Program.SwitchMainFormVisibility;
                    bt.onButtonUp = MJButtonTranslation.voidFunc;
                    bt.ActionText = "MENU BUTTON";
                    break;
                case EMJFUNCTION.FN_MODIFICATOR:
                    bt.hitDelay = 1;
                    bt.onButtonUp = MJButtonTranslation.voidFunc;
                    bt.onButtonDown = this.FN1Down;
                    bt.ActionText = "FN Modificator";
                    break;
                case EMJFUNCTION.SLOWER_MOVEMENT:
                    bt.onButtonDown = this.mouseSlower;
                    bt.onButtonUp = this.mouseSlower_release;
                    bt.ActionText = "Slower movement";
                    break;
                case EMJFUNCTION.FASTER_MOVEMENT:
                    bt.onButtonDown = this.mouseFaster;
                    bt.onButtonUp = this.mouseFaster_release;
                    bt.ActionText = "Faster movement";
                    break;
                case EMJFUNCTION.SWITCH_VIRTUAL_KEYBOARD:
                    // 0.7.2: switch virtual keyboard.
                    bt.onButtonDown = this.switchVirtualKeyboard;
                    bt.onButtonUp = MJButtonTranslation.voidFunc;
                    bt.ActionText = "Switch Virtual Keyboard";
                    break;
                default:
                    break;
            }
            return bt;
        }

        // load the developers test config. ;)
        public void loadHardcodedDefaultConfig()
        {
            this.clearButtons();
            MJButtonTranslation b; // you can change the button config after creation with b.

            // set the configuration name.
            this.configName = "[Hardcoded defaults]";
            Properties.Settings.Default.StartupConfig = "!default!";
            Properties.Settings.Default.Save();

            // the main menu button.
            b = this.addButton(EMJBUTTON.Start, EMJFUNCTION.SHOW_MENU);
            assignExternalFunction(b);

            // the switch virtual keyboard button.
            b = this.addButton(EMJBUTTON.LeftThumb, EMJFUNCTION.SWITCH_VIRTUAL_KEYBOARD);
            assignExternalFunction(b);

            // left stick for mouse movement.
            b = this.addButton(EMJBUTTON.LeftThumbstick, EMJFUNCTION.MOUSE_MOVEMENT);

            // right stick for mouse wheel.
            b = this.addButton(EMJBUTTON.RightThumbstick, EMJFUNCTION.MOUSE_WHEEL);

            // FN_1 button = need this keystroke!
            b = this.addButton(EMJBUTTON.LeftShoulder, EMJFUNCTION.FN_MODIFICATOR);
            assignExternalFunction(b);

            // mouse buttons => need this keystroke!
            b = this.addButton(EMJBUTTON.A, EMJFUNCTION.LEFT_MOUSE_BUTTON);
            b = this.addButton(EMJBUTTON.B, EMJFUNCTION.RIGHT_MOUSE_BUTTON);
            b = this.addButton(EMJBUTTON.RightThumb, EMJFUNCTION.MIDDLE_MOUSE_BUTTON);

            // mouse slower and faster.
            b = this.addButton(EMJBUTTON.LeftTrigger, EMJFUNCTION.SLOWER_MOVEMENT);
            assignExternalFunction(b);

            b = this.addButton(EMJBUTTON.RightTrigger, EMJFUNCTION.FASTER_MOVEMENT);
            assignExternalFunction(b);

            // dpad buttons
            b = this.addButton(EMJBUTTON.DPadUp, EMJFUNCTION.KEYBOARD_COMBINATION, 0, "{UP}");
            b.hitDelay = this.DefaultKeyStrokeDelay;
            b = this.addButton(EMJBUTTON.DPadDown, EMJFUNCTION.KEYBOARD_COMBINATION,0,"{DOWN}");
            b.hitDelay = this.DefaultKeyStrokeDelay;
            b = this.addButton(EMJBUTTON.DPadLeft, EMJFUNCTION.KEYBOARD_COMBINATION, 0, "{LEFT}");
            b.hitDelay = this.DefaultKeyStrokeDelay;
            b = this.addButton(EMJBUTTON.DPadRight, EMJFUNCTION.KEYBOARD_COMBINATION, 0, "{RIGHT}");
            b.hitDelay = this.DefaultKeyStrokeDelay;

            // backspace key
            b = this.addButton(EMJBUTTON.X, EMJFUNCTION.KEYBOARD_COMBINATION, 0, "{BACKSPACE}");
            b.hitDelay = this.DefaultKeyStrokeDelay;

            // esc key - only once
            b = this.addButton(EMJBUTTON.Back, EMJFUNCTION.KEYBOARD_COMBINATION, 0, "{ESC}");
            // enter key
            b = this.addButton(EMJBUTTON.Y, EMJFUNCTION.KEYBOARD_COMBINATION, 0, "{ENTER}");
            b.hitDelay = this.DefaultKeyStrokeDelay;

            // TABulator key
            b = this.addButton(EMJBUTTON.RightShoulder, EMJFUNCTION.KEYBOARD_COMBINATION, 0, "{TAB}");
            b.hitDelay = this.DefaultKeyStrokeDelay;

            // ctrl-c with FN_1
            b = this.addButton(EMJBUTTON.A, EMJFUNCTION.KEYBOARD_COMBINATION, 1, "^c");
            // ctrl-v with FN_1
            b = this.addButton(EMJBUTTON.B, EMJFUNCTION.KEYBOARD_COMBINATION, 1, "^v");
            // ctrl-z with FN_1
            b = this.addButton(EMJBUTTON.X, EMJFUNCTION.KEYBOARD_COMBINATION, 1, "^z");
            // ctrl-y with FN_1
            b = this.addButton(EMJBUTTON.Y, EMJFUNCTION.KEYBOARD_COMBINATION, 1, "^y");

            // volume UP with FN_1 => need this keystroke!
            b = this.addButton(EMJBUTTON.DPadUp, EMJFUNCTION.VOLUME_UP, 1);
            b.hitDelay = this.DefaultKeyStrokeDelay;
            // volume DOWN with FN_1 => need this keystroke!
            b = this.addButton(EMJBUTTON.DPadDown, EMJFUNCTION.VOLUME_DOWN,1);
            b.hitDelay = this.DefaultKeyStrokeDelay;
            // MUTE volume with FN_1 => need this keystroke!
            b = this.addButton(EMJBUTTON.DPadLeft, EMJFUNCTION.MUTE_VOLUME,1);
        }

        // we use this version to determine if the fileloader works.
        // config files have this number as first line.
        protected byte configFileDeterminator = 129;
        // the version number is the second line.
        protected byte configFileVersion = 3;
        public bool SaveTo(string filename)
        {
            try
            {
                Log.Line("Opening " + filename + " for saving the configuration..");
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                StreamWriter bw = new StreamWriter(fs);

                Log.Append("done.");

                // set the configname to the filename.
                this.configName = Path.GetFileName(filename);
                Properties.Settings.Default.StartupConfig = filename;
                Properties.Settings.Default.Save();

                // TODO: set as default config.

                // write determinator and version
                bw.WriteLine(configFileDeterminator);
                bw.WriteLine(configFileVersion);

                // write count of mjbuttons.
                Log.Line("Writing " + this.buttons.Count + " buttons..");
                bw.WriteLine(this.buttons.Count);
                foreach(MJButtonTranslation btn in this.buttons)
                {
                    btn.serialize(bw);
                }

                bw.Close();
                Log.Line("Saving success.");
                return true;
            }
            catch
            {
                Log.Line("ERROR: Could not create file " + filename+"! Is it write protected?");
                MessageBox.Show("Could not create or open " + filename + ". Is it write protected?", "I/O Error");
                return false;
            }
        }

        // load a configuration.
        public bool LoadFrom(string filename)
        {
            try
            {
                Log.Line("Opening " + filename + " for loading the configuration..");
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader br = new StreamReader(fs);

                // read determinator and version
                string t= br.ReadLine();
                byte cd = byte.Parse(t);
                if(cd == configFileDeterminator)
                {
                    // determinator is ok, now read version.
                    t = br.ReadLine();
                    cd = byte.Parse(t);
                    if (cd==configFileVersion)
                    {
                        // OK File and File Version are ok.
                        // clear the buttons..
                        this.clearButtons();

                        // load count of buttons.
                        t = br.ReadLine();
                        int count = int.Parse(t);
                        // .. and load the new ones.
                        Log.Line("Loading " + count + " buttons..");
                        for (int c = 0;c < count; c++)
                        {
                            // create a button and load it.
                            MJButtonTranslation b = new MJButtonTranslation(br);
                            assignExternalFunction(b);  // maybe assign an external function.
                            this.addButton(b);
                        }

                        // set the new config name.
                        this.configName = Path.GetFileName(filename);
                        Properties.Settings.Default.StartupConfig = filename;
                        Properties.Settings.Default.Save();

                        Log.Line("Loading success.");
                    }
                    else
                    {
                        Log.Line("Wrong file version: " + (int)cd + " [actual: " + (int)configFileVersion+"]");
                        MessageBox.Show("Wrong file version, sorry.", "I/O Error");
                    }
                }
                else
                {
                    Log.Line("Unknown filetype.");
                    MessageBox.Show("Invalid file.", "I/O Error");
                }

                Log.Line("DONE");
                br.Close();
                return true;
            }
            catch
            {
                Log.Line("ERROR: Could not open file " + filename + "! Is it read protected?");
                MessageBox.Show("Could not open " + filename + ". Loading defaults.", "I/O Error");
                this.loadHardcodedDefaultConfig();
                return false;
            }
        }

        // creaate the virtual key arrays.
        string[,,] vkArray;
        public void createVKArray()
        {
            // x,y,z where z is the shift key.
            // x and y are the layout on the VK image.
            string[,,] ar = new string[vkMaxX,vkMaxY,2];
            ar[0, 0, 0] = "a";
            ar[0, 1, 0] = "b";
            ar[0, 2, 0] = "c";
            ar[0, 3, 0] = "d";

            ar[0, 0, 1] = "A";
            ar[0, 1, 1] = "B";
            ar[0, 2, 1] = "C";
            ar[0, 3, 1] = "D";

            ar[1, 0, 0] = "e";
            ar[1, 1, 0] = "f";
            ar[1, 2, 0] = "g";
            ar[1, 3, 0] = "h";

            ar[1, 0, 1] = "E";
            ar[1, 1, 1] = "F";
            ar[1, 2, 1] = "G";
            ar[1, 3, 1] = "H";

            ar[2, 0, 0] = "i";
            ar[2, 1, 0] = "j";
            ar[2, 2, 0] = "k";
            ar[2, 3, 0] = "l";

            ar[2, 0, 1] = "I";
            ar[2, 1, 1] = "J";
            ar[2, 2, 1] = "K";
            ar[2, 3, 1] = "L";

            ar[3, 0, 0] = "m";
            ar[3, 1, 0] = "n";
            ar[3, 2, 0] = "o";
            ar[3, 3, 0] = "p";

            ar[3, 0, 1] = "M";
            ar[3, 1, 1] = "N";
            ar[3, 2, 1] = "O";
            ar[3, 3, 1] = "P";

            ar[4, 0, 0] = "q";
            ar[4, 1, 0] = "r";
            ar[4, 2, 0] = "s";
            ar[4, 3, 0] = "t";

            ar[4, 0, 1] = "Q";
            ar[4, 1, 1] = "R";
            ar[4, 2, 1] = "S";
            ar[4, 3, 1] = "T";

            ar[5, 0, 0] = "u";
            ar[5, 1, 0] = "v";
            ar[5, 2, 0] = "w";
            ar[5, 3, 0] = "x";

            ar[5, 0, 1] = "U";
            ar[5, 1, 1] = "V";
            ar[5, 2, 1] = "W";
            ar[5, 3, 1] = "X";

            ar[6, 0, 0] = "y";
            ar[6, 1, 0] = "z";
            ar[6, 2, 0] = ",";
            ar[6, 3, 0] = ".";

            ar[6, 0, 1] = "Y";
            ar[6, 1, 1] = "Z";
            ar[6, 2, 1] = ";";
            ar[6, 3, 1] = ":";

            ar[7, 0, 0] = "7";
            ar[7, 1, 0] = "4";
            ar[7, 2, 0] = "1";
            ar[7, 3, 0] = "@";

            ar[7, 0, 1] = "{+}";
            ar[7, 1, 1] = "=";
            ar[7, 2, 1] = "&";
            ar[7, 3, 1] = "{^}";

            ar[8, 0, 0] = "8";
            ar[8, 1, 0] = "5";
            ar[8, 2, 0] = "2";
            ar[8, 3, 0] = "0";

            ar[8, 0, 1] = "*";
            ar[8, 1, 1] = "#";
            ar[8, 2, 1] = "{(}";
            ar[8, 3, 1] = "{)}";

            ar[9, 0, 0] = "9";
            ar[9, 1, 0] = "6";
            ar[9, 2, 0] = "3";
            ar[9, 3, 0] = "/";

            ar[9, 0, 1] = "-";
            ar[9, 1, 1] = "_";
            ar[9, 2, 1] = "[";
            ar[9, 3, 1] = "]";

            // 0.7.13: Special chars for RS button.
            ar[0, 4, 0] = "{TAB}";
            ar[1, 4, 0] = "{ENTER}";
            ar[2, 4, 0] = " ";
            ar[3, 4, 0] = "{LEFT}";
            ar[4, 4, 0] = "{RIGHT}";
            ar[5, 4, 0] = "{UP}";
            ar[6, 4, 0] = "{DOWN}";
            ar[7, 4, 0] = "?";
            ar[8, 4, 0] = "!";
            ar[9, 4, 0] = "{BACKSPACE}";

            ar[0, 4, 1] = "{TAB}";
            ar[1, 4, 1] = "{ENTER}";
            ar[2, 4, 1] = " ";
            ar[3, 4, 1] = "{LEFT}";
            ar[4, 4, 1] = "{RIGHT}";
            ar[5, 4, 1] = "{UP}";
            ar[6, 4, 1] = "{DOWN}";
            ar[7, 4, 1] = "ä";
            ar[8, 4, 1] = "ö";
            ar[9, 4, 1] = "ü";

            vkArray = ar;
        }
    }
}
