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
using System.Windows.Forms;

namespace Majestic_11
{
    public class XInputController
    {
        protected Frm_MJOY_Main mainForm;

        protected Gamepad pad;
        protected Controller controller;
        protected bool connected = false;
        public bool IsConnected { get { return connected;} }
        public float deadzone = 2500;

        protected float multiplier = 50.0f / short.MaxValue;

        protected Point leftThumb, rightThumb = new Point(0,0);
        protected byte dpad, olddpad = 0;
        protected int arrowWaitTime = 0; // wait time counter for the arrow keys.
        protected float leftTrigger, rightTrigger;
        protected bool leftMouseDown, rightMouseDown, middleMouseDown, enterKeyDown, escapeKeyDown,
            ctrlZDown, ctrlYDown, ctrlCDown, ctrlVDown, backspaceDown, fnDown, showMenuDown, tabulatorKeyDown = false;
        protected float mouseSpeed = 0.2f;

        protected Thread thread = null;
        protected int pollcount = 0;

        // text for the connection.
        protected string m_connectingText = "-=+ connection status not known yet +=-";
        public string ConnectText => m_connectingText;

        // some definitions which can be configured.
        GamepadButtonFlags ctrl_ButtonEscapeKey = GamepadButtonFlags.Back;
        GamepadButtonFlags ctrl_ShowMenu = GamepadButtonFlags.Start;

        GamepadButtonFlags ctrl_ButtonLeftMouse = GamepadButtonFlags.A;
        GamepadButtonFlags ctrl_ButtonRightMouse = GamepadButtonFlags.B;
        GamepadButtonFlags ctrl_ButtonEnterKey = GamepadButtonFlags.Y;
        GamepadButtonFlags ctrl_ButtonBackspaceKey = GamepadButtonFlags.X;
        GamepadButtonFlags ctrl_ButtonTabulatorKey = GamepadButtonFlags.RightShoulder;
        GamepadButtonFlags ctrl_ButtonMiddleMouse = GamepadButtonFlags.RightThumb;

        GamepadButtonFlags ctrl_ButtonFN = GamepadButtonFlags.LeftShoulder;

        // FN keys
        GamepadButtonFlags ctrl_ButtonCtrlCKey = GamepadButtonFlags.A;
        GamepadButtonFlags ctrl_ButtonCtrlVKey = GamepadButtonFlags.B;
        GamepadButtonFlags ctrl_ButtonCtrlZKey = GamepadButtonFlags.X;
        GamepadButtonFlags ctrl_ButtonCtrlYKey = GamepadButtonFlags.Y;

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
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
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
            Update();
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

        // returns true if the button is down, else false.
        protected bool isButtonDown(GamepadButtonFlags button, Gamepad gpad)
        {
            if ((gpad.Buttons & button) == button)
                return true;
            return false;
        }

        // get the controller buttons and thumbs.
        protected void Update()
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

                    // get digital pad keys.
                    olddpad = dpad;
                    switch(pad.Buttons)
                    {
                        case GamepadButtonFlags.DPadDown:dpad = 1; break;
                        case GamepadButtonFlags.DPadUp: dpad = 2; break;
                        case GamepadButtonFlags.DPadRight: dpad = 3; break;
                        case GamepadButtonFlags.DPadLeft: dpad = 4; break;
                        default: dpad = 0; break;
                    }

                    bool sendarrow = false;
                    // send a key on begin of keypress..
                    if(dpad!=olddpad)
                    {
                        arrowWaitTime = 0;
                        sendarrow = true;
                    }

                    // and after some time after the key went down.
                    if (dpad == olddpad && arrowWaitTime > 500)
                        sendarrow = true;

                    // finally send the arrow keys.
                    if(sendarrow)
                    {
                        if (dpad == 1) SendKeys.SendWait("{DOWN}");
                        if (dpad == 2) SendKeys.SendWait("{UP}");
                        if (dpad == 3) SendKeys.SendWait("{RIGHT}");
                        if (dpad == 4) SendKeys.SendWait("{LEFT}");
                    }

                    // check if FN button is down and set FN flag.
                    if ((pad.Buttons & ctrl_ButtonFN) == ctrl_ButtonFN)
                    {
                        fnDown = true;
                    }else{
                        fnDown = false;
                    }

                    // simulate clicks.
                    // left click
                    if (isButtonDown(ctrl_ButtonLeftMouse, pad) && !fnDown)
                    {
                        if(!leftMouseDown)
                        {
                            Log.Line("Left mouse button down @ " + cur.X + ":" + cur.Y);
                            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)cur.X, (uint)cur.Y, 0, 0);
                        }
                        leftMouseDown = true;
                    }else{
                        if (leftMouseDown)
                        {
                            Log.Line("Left mouse button up @ " + cur.X + ":" + cur.Y);
                            mouse_event(MOUSEEVENTF_LEFTUP, (uint)cur.X, (uint)cur.Y, 0, 0);
                        }
                        leftMouseDown = false;
                    }

                    // right click
                    if (isButtonDown(ctrl_ButtonRightMouse, pad) && !fnDown)
                    { 
                        if (!rightMouseDown)
                        {
                            Log.Line("Right mouse button down @ "+cur.X+":"+cur.Y);
                            mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)cur.X, (uint)cur.Y, 0, 0);
                        }
                        rightMouseDown = true;
                    }else{
                        if (rightMouseDown)
                        {
                            Log.Line("Right mouse button up @ " + cur.X + ":" + cur.Y);
                            mouse_event(MOUSEEVENTF_RIGHTUP, (uint)cur.X, (uint)cur.Y, 0, 0);
                        }
                        rightMouseDown = false;
                    }

                    // middle click
                    if (isButtonDown(ctrl_ButtonMiddleMouse, pad))
                    {
                        if (!middleMouseDown)
                        {
                            Log.Line("Middle mouse button down @ " + cur.X + ":" + cur.Y);
                            mouse_event(MOUSEEVENTF_MIDDLEDOWN, (uint)cur.X, (uint)cur.Y, 0, 0);
                        }
                        middleMouseDown = true;
                    }else{
                        if (middleMouseDown)
                        {
                            Log.Line("Middle mouse button up @ " + cur.X + ":" + cur.Y);
                            mouse_event(MOUSEEVENTF_MIDDLEUP, (uint)cur.X, (uint)cur.Y, 0, 0);
                        }
                        middleMouseDown = false;
                    }

                    // enter key button
                    if (isButtonDown(ctrl_ButtonEnterKey, pad) && !fnDown)
                    {
                        if (!enterKeyDown)
                        {
                            Log.Line("Enter key pressed.");
                            SendKeys.SendWait("{ENTER}");
                        }
                        enterKeyDown = true;
                    }else{ enterKeyDown = false; }

                    // tabulator key button
                    if (isButtonDown(ctrl_ButtonTabulatorKey, pad) && !fnDown)
                    {
                        if (!tabulatorKeyDown)
                        {
                            Log.Line("TAB key pressed.");
                            SendKeys.SendWait("{TAB}");
                        }
                        tabulatorKeyDown = true;
                    } else { tabulatorKeyDown = false; }

                    // esc button
                    if (isButtonDown(ctrl_ButtonEscapeKey, pad))// && !fnDown)
                    {
                        if (!escapeKeyDown)
                        {
                            Log.Line("ESC pressed.");
                            SendKeys.SendWait("{ESC}");
                        }
                        escapeKeyDown = true;
                    }else{ escapeKeyDown = false; }

                    // backspace button
                    if (isButtonDown(ctrl_ButtonBackspaceKey, pad) && !fnDown)
                    {
                        if (!backspaceDown)
                        {
                            Log.Line("Backspace pressed.");
                            SendKeys.SendWait("{BACKSPACE}");
                        }
                        backspaceDown = true;
                    }else{ backspaceDown = false; }

                    // ctrl-c button (with FN)
                    if (isButtonDown(ctrl_ButtonCtrlCKey, pad) && fnDown)
                    {
                        if (!ctrlCDown)
                        {
                            Log.Line("CTRL-C pressed.");
                            SendKeys.SendWait("^c");
                        }
                        ctrlCDown = true;
                    }else{ ctrlCDown = false; }

                    // ctrl-v button (with FN)
                    if (isButtonDown(ctrl_ButtonCtrlVKey, pad) && fnDown)
                    {
                        if (!ctrlVDown)
                        {
                            Log.Line("CTRL-V pressed.");
                            SendKeys.SendWait("^v");
                        }
                        ctrlVDown = true;
                    }else{ ctrlVDown = false; }

                    // ctrl-z button (with FN)
                    if (isButtonDown(ctrl_ButtonCtrlZKey, pad) && fnDown)
                    {
                        if (!ctrlZDown)
                        {
                            Log.Line("CTRL-Z pressed.");
                            SendKeys.SendWait("^z");
                        }
                        ctrlZDown = true;
                    }else{ ctrlZDown = false; }

                    // ctrl-y button (with FN)
                    if (isButtonDown(ctrl_ButtonCtrlYKey, pad) && fnDown)
                    {
                        if (!ctrlYDown)
                        {
                            Log.Line("CTRL-Y pressed.");
                            SendKeys.SendWait("^y");
                        }
                        ctrlYDown = true;
                    }else{ ctrlYDown = false; }

                    // show menu button
                    if (isButtonDown(ctrl_ShowMenu, pad))// && !fnDown)
                    {
                        if (!showMenuDown)
                        {
                            Log.Line("Show menu pressed.");
                            Program.ShowMainForm();
                        }
                        showMenuDown = true;
                    }else{ showMenuDown = false; }
                }catch (Exception ex)
                {
                    continue;
                    //Log.Line("EXCEPTION: Gamepad not found.");
                }

                Thread.Sleep(20);
                if(dpad!=0) arrowWaitTime += 20; // add those millisecs to the wait time
            }

            // if the connection was lost, we try to get a new one.
            // first show the main form if it is hidden.
            Program.ShowMainForm();
            connectThreadFunc();
        }
    }
}
