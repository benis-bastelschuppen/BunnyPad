using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Majestic_11
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        /// 

        static Frm_MJOY_Main mainform;
        static Frm_About aboutform;
        static Frm_ButtonConfig configform;
        // the control poller and its get-property
        public static XInputController controlpoller;
        public static XInputController Input => controlpoller;

        public static bool Running = false;

        static bool mainFormVisibility = true;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainform = new Frm_MJOY_Main();

            // the action overlay is the overlay which shows the virtual keyboard.

            //actionoverlay overlay = new actionoverlay();

            // set actionoverlay position
            /*foreach (var scrn in Screen.AllScreens)
            {
                if (scrn.Bounds.Contains(overlay.Location))
                {
                    overlay.Location = new Point(scrn.Bounds.Right - overlay.Width, scrn.Bounds.Top);
                    break;
                }
            }*/

            // set the overlay forms properties.
            /*overlay.BackColor = System.Drawing.Color.LightGreen;
            overlay.TransparencyKey = System.Drawing.Color.LightGreen;
            overlay.TopMost = true;
            */

            // this is the main poller for the controls.
            controlpoller = new XInputController(mainform);

            // create an about form
            aboutform = new Frm_About();
            aboutform.Text = aboutform.Text + " v" + Application.ProductVersion;
            aboutform.Hide();

            // create a config form.
            configform = new Frm_ButtonConfig();
            configform.Hide();

            Log.Line("Startup config 2: " + Properties.Settings.Default.StartupConfig);

            Running = true;

            Application.Run(mainform);
        }
        
        // show or hide the main form.
        public static void SwitchMainFormVisibility()
        {
            mainFormVisibility = !mainFormVisibility;
            if (mainform.WindowState == FormWindowState.Minimized)
                mainFormVisibility = true;

            if (mainFormVisibility)
                ShowMainForm();
            else
                HideMainForm();
        }

        public static void HideMainForm()
        {
            mainform.Invoke((MethodInvoker)delegate
            {
                mainFormVisibility = false;
                if (Program.controlpoller.IsConnected)
                    mainform.Hide();
                else
                    mainform.WindowState = FormWindowState.Minimized;
            });
        }

        public static void ShowMainForm()
        {
            mainform.Invoke((MethodInvoker)delegate {
                mainFormVisibility = true;
                mainform.Show();
                mainform.WindowState = FormWindowState.Normal;
                mainform.BringToFront();
                mainform.Activate(); 
            });
        }

        public static void ShowAboutForm()
        {
            if (aboutform.IsDisposed)
                aboutform = new Frm_About();
            aboutform.Start();
        }

        public static void ShowButtonConfigForm()
        {
            if (configform.IsDisposed)
                configform = new Frm_ButtonConfig();
            configform.Start();
        }

        // update all the labels on the main form.
        public static void UpdateLabels()
        {
            // BUGFIX_1: This gets called in another thread and it could create an exception when closing the
            // program. That is why we ask for the Running flag first. 
            if (!Program.Running)
                return;
            if (controlpoller.ConnectText != mainform.ConnectText)
                mainform.setLbl_connected(controlpoller.ConnectText);
        }
    }
}
