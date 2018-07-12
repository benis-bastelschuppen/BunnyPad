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
        public static XInputController controlpoller;
        public static bool Running = false;

        static bool mainFormVisibility = true;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainform = new Frm_MJOY_Main();

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

            controlpoller = new XInputController(mainform);
            Running = true;
            aboutform = new Frm_About();
            aboutform.Text = aboutform.Text + " v" + Application.ProductVersion;
            aboutform.Hide();
            Application.Run(mainform);
        }

        public static void SwitchMainFormVisibility()
        {
            mainFormVisibility = !mainFormVisibility;
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
            aboutform.Start();
        }

        // update all the labels on the main form.
        public static void UpdateLabels()
        {
            if (controlpoller.ConnectText != mainform.ConnectText)
                mainform.setLbl_connected(controlpoller.ConnectText);
        }
    }
}
