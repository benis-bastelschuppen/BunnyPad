using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public float deadzone = 2500;

        protected float multiplier = 50.0f / short.MaxValue;

        protected Point leftThumb, rightThumb, dpad = new Point(0,0);
        protected float leftTrigger, rightTrigger;
        protected bool leftMouseDown, rightMouseDown = false;
        protected float mouseSpeed = 0.2f;

        protected Thread thread = null;
        protected int pollcount = 0;

        // WINDOWS SPECIFIC		
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
        private const int MOUSEEVENTF_WHEEL = 0x0800;
        // ENDOF WINDOWS SPECIFIC

        public XInputController(Frm_MJOY_Main frm)
        {
            mainForm = frm;
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
            this.connect();
            while(!connected)
            {
                this.connect();
                Thread.Sleep(250);
            }
            Update();
        }

        // the connect function itself.
        public void connect()
        {
            // this function may be started in a thread so the text functions need to invoke.
            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;

            if (connected)
            {
                string txt = "Controller specs:\n";
                string q = controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryType.ToString();
                if (q == "Disconnected")
                    q = "Wired or Unknown";
                txt += "Battery type: " + q + "\n";
                txt += "Battery level: " + controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel + "\n";
                mainForm.setLbl_connected(txt);
                Log.Line("Controller connected.");
                pollcount = 0;
            }
            else
            {
                mainForm.setLbl_connected("-- no controller connected or controller not found. --");
                if (pollcount == 0)
                    Log.Line("Polling for controller");
                else
                    Log.Append(".");
                pollcount++;
            }
        }

        // get the controller buttons and thumbs.
        protected void Update()
        {
            bool done = false;
            while(!done)
            {
                // check if there is a controller connected.
                if(!controller.IsConnected || !connected)
                {
                    done = true;
                    break;
                }

                // get the gamepad
                pad = controller.GetState().Gamepad;

                // get new values
                leftThumb.X = (pad.LeftThumbX < deadzone && pad.LeftThumbX > -deadzone) ? 0 : (int)((float)pad.LeftThumbX * multiplier);
                leftThumb.Y = (pad.LeftThumbY < deadzone && pad.LeftThumbY > -deadzone) ? 0 : (int)((float)pad.LeftThumbY * multiplier);
                rightThumb.X = (pad.RightThumbX < deadzone && pad.RightThumbX > -deadzone) ? 0 : (int)((float)pad.RightThumbX * multiplier);
                rightThumb.Y = (pad.RightThumbY < deadzone && pad.RightThumbY > -deadzone) ? 0 : (int)((float)pad.RightThumbY * multiplier);

                // triggers are used to speed the mouse up or down.
                leftTrigger = pad.LeftTrigger;
                rightTrigger = pad.RightTrigger;

                if (leftTrigger > 10 && rightTrigger <= 10)
                    mouseSpeed = 0.05f;

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

                // get digital pad values.
                int oldypad = dpad.Y;
                if(pad.Buttons == GamepadButtonFlags.DPadDown) dpad.Y = 1;
                if (pad.Buttons == GamepadButtonFlags.DPadUp) dpad.Y = -1;
                if (pad.Buttons != GamepadButtonFlags.DPadDown && pad.Buttons != GamepadButtonFlags.DPadUp) dpad.Y = 0;

                int oldxpad = dpad.X;
                if (pad.Buttons == GamepadButtonFlags.DPadRight) dpad.X = 1;
                if (pad.Buttons == GamepadButtonFlags.DPadLeft) dpad.X = -1;
                if (pad.Buttons != GamepadButtonFlags.DPadRight && pad.Buttons != GamepadButtonFlags.DPadLeft) dpad.X = 0;
                if (oldxpad != dpad.X || oldypad != dpad.Y)
                    Log.Line("DPad changed to: " + dpad.X+":"+dpad.Y);

                // simulate clicks.
                if(pad.Buttons == GamepadButtonFlags.A)
                {
                    if(!leftMouseDown)
                    {
                        Log.Line("Left mouse button down @ " + cur.X + ":" + cur.Y);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)cur.X, (uint)cur.Y, 0, 0);
                    }
                    leftMouseDown = true;
                }

                if (pad.Buttons != GamepadButtonFlags.A)
                {
                    if (leftMouseDown)
                    {
                        Log.Line("Left mouse button up @ " + cur.X + ":" + cur.Y);
                        mouse_event(MOUSEEVENTF_LEFTUP, (uint)cur.X, (uint)cur.Y, 0, 0);
                    }
                    leftMouseDown = false;
                }

                if (pad.Buttons == GamepadButtonFlags.B)
                { 
                    if (!rightMouseDown)
                    {
                        Log.Line("Right mouse button down @ "+cur.X+":"+cur.Y);
                        mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)cur.X, (uint)cur.Y, 0, 0);
                    }
                    rightMouseDown = true;
                }

                if (pad.Buttons != GamepadButtonFlags.B)
                {
                    if (rightMouseDown)
                    {
                        Log.Line("Right mouse button up @ " + cur.X + ":" + cur.Y);
                        mouse_event(MOUSEEVENTF_RIGHTUP, (uint)cur.X, (uint)cur.Y, 0, 0);
                    }
                    rightMouseDown = false;
                }

                Thread.Sleep(20);
            }
            connectThreadFunc();
        }
    }
}
