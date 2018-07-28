using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Majestic_11
{
    public partial class actionoverlay : Form
    {
        // use this to set mouse transparency.
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        public actionoverlay()
        {
            InitializeComponent();
        }

        private void actionoverlay_Load(object sender, EventArgs e)
        {
            makeTransparentForMouse();
        }

        protected Image showImage;

        // make the form transparent for the mouse.
        private void makeTransparentForMouse()
        {
            // 0.7.10: Make the overlay form transparent for the mouse.
            const int GWL_EXSTYLE = -20;
            const int WS_EX_LAYERED = 0x00080000;
            const UInt32 WS_EX_TRANSPARENT = 0x00000020;
            UInt32 current = GetWindowLong(this.Handle, GWL_EXSTYLE);
            // This creates a new extended style for our window, making it transparent to the mouse.
            SetWindowLong(this.Handle, GWL_EXSTYLE, current|WS_EX_LAYERED|WS_EX_TRANSPARENT);
        }

        // show the image with the small characters.
        public void showSmallChars()
        {
            showImage = img_smallChars.Image;
        }
        // show the image with the big characters.
        public void showBigChars()
        {
            showImage = img_bigChars.Image;
        }

        // update the overlay drawing.
        public void updateView(int curx, int cury)
        {
            Color cursorColor = Color.FromArgb(255, Color.Green);
            Color cursorColor2 = Color.FromArgb(255, Color.Red);
            SolidBrush cursorBrush = new SolidBrush(cursorColor);
            SolidBrush cursorBrush2 = new SolidBrush(cursorColor2);

            Image gi = img_vkOverlay.Image;
            Graphics g = Graphics.FromImage(gi);
            g.Clear(new Color());
            // draw the cursor
            g.FillRectangle(cursorBrush, curx, cury, 50, 50);
            g.FillRectangle(cursorBrush2, curx - 1, 0, 2, this.Height);
            g.FillRectangle(cursorBrush2, curx + 49, 0, 2, this.Height);
            // draw the image
            g.DrawImageUnscaled(showImage, 0, 0);
            img_vkOverlay.Image = gi;
        }

        private void img_vkOverlay_Click(object sender, EventArgs e)
        {

        }
    }
}
