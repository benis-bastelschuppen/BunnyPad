using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Majestic_11
{
    public partial class actionoverlay : Form
    {
        public actionoverlay()
        {
            InitializeComponent();
        }

        private void btn_menu_Click(object sender, EventArgs e)
        {
            Program.ShowMainForm();
        }

        private void lbl_overlay_Click(object sender, EventArgs e)
        {

        }
    }
}
