/*
 * Majestic 11
 * a.k.a. JoyMouse
 * [The Joy Of A Mouse]
 * 
 * by Benedict "Oki Wan Ben0bi" Jäggi
 * (Joymouse) ~2002
 * Copyright 2018 Ben0bi Enterprises
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

using System;

// Use NuGet to install SharpDX and SharpDX.XInput
using SharpDX.XInput;

using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Majestic_11
{
    public class XInputController
    {
        protected Frm_MJOY_Main mainForm;

        protected Gamepad pad;
        protected Controller controller;
        protected bool connected = false;
        public bool IsConnected { get { return connected;} }
        public float deadzone = 5000;

        protected float multiplier = 50.0f / short.MaxValue;
         
        protected Point leftThumb, rightThumb = new Point(0,0);
        protected byte dpad, olddpad;
        protected int arrowWaitTime = 0; // wait time counter for the arrow keys.
        protected float leftTrigger, rightTrigger;
        protected bool showMenuDown = false; // REMOVE
        protected float mouseSpeed = 0.2f;

        protected Thread thread = null;
        protected int pollcount = 0;

        // text for the connection.
        protected string m_connectingText = "-=+ connection status not known yet +=-";
        public string ConnectText => m_connectingText;

        // some definitions which can be configured.
        GamepadButtonFlags ctrl_ShowMenu = GamepadButtonFlags.Start;

        // NEW 0.4.x:
        protected MJConfig config = new MJConfig();
        public MJConfig Config => config;

        // ENDOF NEW

        // WINDOWS SPECIFIC
        // OLD REMOVE
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
// ENDOF REMOVE

        public XInputController(Frm_MJOY_Main frm)
        {
            config = new MJConfig();

            // create the default config. 
            config.loadHardcodedDefaultConfig();

            this.mainForm = frm;
            this.connectThread();
        }

        // start function for the connecting thread.
        public void connectThread()
        {
            thread = new Thread(connectThreadFunc);
            thread.IsBackground = true;   // it will abort when the application exits.
            Log.Line("Controller thread started.");
            thread.Start();
        }

        // thread for selecting a controller. It tries to get one until it has one, then it starts the update thread.
        protected void connectThreadFunc()
        {
            char c = '*';
            byte q = 1;
            this.connect(c);
            while(!connected)
            {
                switch(q)
                {
                    case 1: c = '+';break;
                    case 2: c = 'o';break;
                    case 3: c = 'O'; break;
                    case 4: c = 'o'; break;
                    case 5: c = '+'; break;
                    case 6: c = 'x'; break;
                    case 7: c = 'X'; break;
                    case 8: c = 'x'; q = 0; break;
                    default:
                        q = 0;
                        break;
                }
                this.connect(c);
                q++;
                Thread.Sleep(250);
            }
            UpdateLoop();
        }

        // the connect function itself.
        public void connect(char loadingchar)
        {
            // this function may be started in a thread so the text functions need to invoke.
            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;

            if (connected)
            {
                string txt = "+++ CONNECTED +++\n";
                string q = controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryType.ToString();
                if (q == "Disconnected")
                    q = "Wired or Unknown";
                txt += "Bat. Type: " + q + " | ";
                txt += "Bat. Lvl.: " + controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel + "\n";
                m_connectingText = txt;
                // this will be done in the update function.
                //mainForm.setLbl_connected(m_connectingText);
                Log.Line("Controller connected.");
                pollcount = 0;
            }else{
                m_connectingText = "!.. Waiting for connection ..!";
                mainForm.setLbl_connected(""+loadingchar+" ..Waiting for connection.. "+loadingchar);
                if (pollcount == 0)
                    Log.Line("Polling for controller");
                else
                    Log.Append(".");
                pollcount++;
            }
        }

        // TODO: Remove
        // returns true if the button is down, else false.
        protected bool isButtonDown(GamepadButtonFlags button, Gamepad gpad)
        {
            if ((gpad.Buttons & button) == button)
                return true;
            return false;
        }
        // ENDOF REMOVE

        // get the controller buttons and thumbs.
        protected void UpdateLoop()
        {
            bool done = false;
            while(!done)
            {
                Program.UpdateLabels();

                // check if there is a controller connected.
                if(!controller.IsConnected || !connected)
                {
                    done = true;
                    break;
                }

                // get the gamepad
                try
                {
                    pad = controller.GetState().Gamepad;
                    config.Update(pad); // NEW 0.4.x

                    // OLD

                    // get new values
                    leftThumb.X = (pad.LeftThumbX < deadzone && pad.LeftThumbX > -deadzone) ? 0 : (int)((float)pad.LeftThumbX * multiplier);
                    leftThumb.Y = (pad.LeftThumbY < deadzone && pad.LeftThumbY > -deadzone) ? 0 : (int)((float)pad.LeftThumbY * multiplier);
                    rightThumb.X = (pad.RightThumbX < deadzone && pad.RightThumbX > -deadzone) ? 0 : (int)((float)pad.RightThumbX * multiplier);
                    rightThumb.Y = (pad.RightThumbY < deadzone && pad.RightThumbY > -deadzone) ? 0 : (int)((float)pad.RightThumbY * multiplier);

                    // triggers are used to speed the mouse up or down.
                    leftTrigger = pad.LeftTrigger;
                    rightTrigger = pad.RightTrigger;
                
                    if (leftTrigger > 10 && rightTrigger <= 10)
                        mouseSpeed = 0.1f;

                    if (rightTrigger > 10 && leftTrigger <= 10)
                        mouseSpeed = 1.0f;

                    if (rightTrigger <= 10 && leftTrigger <= 10)
                        mouseSpeed = 0.5f;

                    // set new mouse values
                    POINT cur = GetCursorPosition();
                    cur.X = (int)(cur.X + leftThumb.X * mouseSpeed);
                    cur.Y = (int)(cur.Y - leftThumb.Y * mouseSpeed);
                    SetCursorPos(cur.X, cur.Y);

                    // maybe use the scroll wheel.
                    if (rightThumb.Y != 0)
                        mouse_event(MOUSEEVENTF_WHEEL, (uint)cur.X, (uint)cur.Y, (uint)(rightThumb.Y*mouseSpeed), 0);

                    // show menu button
                    if (isButtonDown(ctrl_ShowMenu, pad))// && !fnDown)
                    {
                        if (!showMenuDown)
                        {
                            Log.Line("Show menu pressed.");
                            Program.SwitchMainFormVisibility();
                        }
                        showMenuDown = true;
                    }else{ showMenuDown = false; }

                } catch (Exception ex)
                {
                    continue;
                    //Log.Line("EXCEPTION: Gamepad not found.");
                }

                Thread.Sleep(20);
                if(dpad!=0) arrowWaitTime += 20; // add those millisecs to the wait time
            }

            // *10 ooops, we got thrown out of the update loop...

            // if the connection was lost, we try to get a new one.
            // first show the main form if it is hidden.
            Program.ShowMainForm();
            // then try to connect again and again and again..
            connectThreadFunc();
            // ..if there is a connection, the connect func will create a new update-loop. goto *10.
        }
    }
}
