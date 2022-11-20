using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Majestic_11
{
    public partial class Frm_About : Form
    {
        protected int movedirection = -1;
        protected bool done = false;
        public Rectangle pos;
        protected Rectangle clientRectangle;
        protected Graphics g;

        public Frm_About()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            done = true;
            this.Hide();
        }

        private void Frm_About_Load(object sender, EventArgs e)
        {
            clientRectangle = RectangleToScreen(this.ClientRectangle);
            //titleHeight = clientRectangle.Top - this.Top;
            pos = new Rectangle(0, 30,604,905);
            g = aboutImage.CreateGraphics();
            //aboutImage.Location = pos;
            //bgLoop();
        }

        public void Start()
        {
            bgLoop();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        private async void bgLoop()
        {
            pos.Y = 30;
            done = false;
            while (!done)
            {
                if (this.Visible)
                {
                    pos.Y = pos.Y + movedirection;
                    g.DrawImage(aboutImage.BackgroundImage, pos);

                    if (pos.Y + aboutImage.BackgroundImage.Height <= this.Height-bottompanel.Height*3)
                        movedirection = 1;

                    if (pos.Y >= 30)
                        movedirection = -1;

                }
                await Task.Delay(25);
            }
        }

        // open a browser window with the source code url.
        private void url_sourceLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/benis-bastelschuppen/BunnyPad");
        }
    }
}
