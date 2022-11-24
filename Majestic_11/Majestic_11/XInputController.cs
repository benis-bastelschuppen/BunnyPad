/*
 * BunnyPad 
 * Work title: Majestic 11
 * a.k.a. JoyMouse
 * [The Joy Of A Mouse]
 * 
 * Version 0.8.1
 * 
 * by Benedict "Oki Wan Ben0bi" Jäggi
 * (Joymouse) ~2002
 * Copyright 2018, 2022 Ben0bi Enterprises / Kaiserliche Spiele Manufaktur / Benedict Jaeggi
 * https://github.com/benis-bastelschuppen/BunnyPad
 * 
 */


using System;

// Use NuGet to install SharpDX and SharpDX.XInput
using SharpDX.XInput; // shit for using XInput (XBox 360 / ONE Controller)

// new v2.0
using SharpDX.DirectInput; // shit for using DirectInput (standard pc controller)
// does not work with my controllers.
// endof new

using System.Threading;
using System.Windows.Forms;

namespace Majestic_11
{
    public class XInputController
    {
        protected Frm_MJOY_Main mainForm;

// new 0.8.1
        protected DirectInput directInput;
        Joystick joystick=null;
        Joystick joystick2 = null;
// endof new

        protected Gamepad pad;
        protected Controller controller;
        protected bool Xconnected = false;

        // 0.8.1 DirectInput
        protected Gamepad padj;
        protected bool DIconnected = false;

        public bool IsConnected { get { return Xconnected || DIconnected;} }

        // stick deadzone.
        public float deadzone = 5000.0f;

        protected float multiplier = 50000.0f / (float)short.MaxValue;
        public float StickMultiplier => multiplier;

        protected Thread thread = null;
        protected int pollcount = 0;

        // text for the connection.
        protected string m_connectingText = "-=+ connection status not known yet +=-";
        public string ConnectText => m_connectingText;

        protected float mouseSpeed = 0.0005f;
        public float MouseSpeed => mouseSpeed;

        // NEW 0.4.x:
        protected MJConfig config = new MJConfig();
        public MJConfig Config => config;

        public XInputController(Frm_MJOY_Main frm)
        {
            config = new MJConfig();

            //new version 2.0
            directInput = new DirectInput();
            //endof new

            string loadconfigfile = Properties.Settings.Default.StartupConfig;
            Log.Line("Startup config 1: " + Properties.Settings.Default.StartupConfig);
            
            // maybe load a config, maybe create the default one.
            if (loadconfigfile[0] != '!')
                config.LoadFrom(loadconfigfile);
            else
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
            while(!Xconnected && !DIconnected)
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
            // old, XINPUT    
            // this function may be started in a thread so the text functions need to invoke.
           controller = new Controller(UserIndex.One);
           Xconnected = controller.IsConnected;

            if (Xconnected)
            {
                string txt = "+++ CONNECTED (XBox) +++\n";
                string q = controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryType.ToString();
                if (q == "Disconnected")
                    q = "Wired or Unknown";
                txt += "Bat. Type: " + q + " | ";
                txt += "Bat. Lvl.: " + controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel + "\n";
                m_connectingText = txt;
                // maybe we need to show the cursor 
                // when the computer comes back from hibernation.
                Cursor.Show();
                Log.Line("Controller connected.");
                pollcount = 0;
            }
            else
            {
                if (pollcount == 0)
                    Log.Line("Polling for controller");
                else
                    Log.Append(".");
                pollcount++;

                // 8.0.1 new, directinput
                // Find a Gamepad Guid
                Guid joystickGuid = Guid.Empty;
                if (joystickGuid == Guid.Empty)
                {
                    foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Gamepad,
                            DeviceEnumerationFlags.AllDevices))
                        joystickGuid = deviceInstance.InstanceGuid;

                    // No Gamepad Guid found, find a Joystick GUID (PS2 controller...)
                    if (joystickGuid == Guid.Empty)
                    {
                        foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Joystick,
                                DeviceEnumerationFlags.AllDevices))
                        {
                            joystickGuid = deviceInstance.InstanceGuid;
                        }
                    }
                }

                // there is a joystick or gamepad, show that in the text.
                if (joystickGuid != Guid.Empty)
                {
                    m_connectingText="+++ CONNECTED (DirectInput) +++";
                    DIconnected = true;
                    pollcount = 0;

                    joystick = new Joystick(directInput, joystickGuid);
                    joystick.Acquire();

                }else{
                    m_connectingText = "!.. Waiting for connection ..!";
                    mainForm.setLbl_connected("" + loadingchar + " ..Waiting for connection.. " + loadingchar);
                }
              }
        }

        // get the controller buttons and thumbs.
        protected void UpdateLoop()
        {
            bool done = false;
            while(!done)
            {
                Program.UpdateLabels();

                // check if there is a controller connected.
                if ((Xconnected && !controller.IsConnected) || (!Xconnected && !DIconnected)) //!controller.IsConnected || !connected)
                {
                    done = true;
                    break;
                }

                // get the gamepad
                try
                {
                    if (Xconnected)
                    {
                        pad = controller.GetState().Gamepad;
                        config.XUpdate(pad); // NEW 0.4.x
                    }

                    // new 0.8.x > .0
                    if (DIconnected)
                    {
                        joystick.Poll();
                        JoystickState state = joystick.GetCurrentState();
                        config.DIUpdate(state);
                    }

                    // 0.5.12: update mouse speed
                    if (!config.MouseSpeed_Slower && !config.MouseSpeed_Faster)
                        mouseSpeed = 0.0005f;
                    if (config.MouseSpeed_Slower && !config.MouseSpeed_Faster)
                        mouseSpeed = 0.0001f;
                    if (!config.MouseSpeed_Slower && config.MouseSpeed_Faster)
                        mouseSpeed = 0.001f;

                } catch (Exception ex) {
                    // sometimes the gamepad will not be found, like when you
                    // get the computer back from hibernation or something like this.
                    // to not crash the program then, we just continue here.

                    // This also happens when a DirectInput controller is disconnected so
                    // we set DIconnected to false to wait for another connecting thread.
                    DIconnected = false;
                    continue;
                }

                // wait some millisecs.
                Thread.Sleep(20);
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
