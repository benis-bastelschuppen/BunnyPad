﻿using System;
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
    public partial class Frm_MJOY_Main : Form
    {
        public Frm_MJOY_Main()
        {
            InitializeComponent();
        }

        private void Frm_MJOY_Main_Load(object sender, EventArgs e)
        {
            showMainImage();
        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
            Program.Running = false;
            Application.Exit();
        }

        private void btn_minimize_Click(object sender, EventArgs e)
        {
            Program.HideMainForm(false);
        }

        private void btn_about_Click(object sender, EventArgs e)
        {
            Program.ShowAboutForm();
        }

        private void btn_Config_Click(object sender, EventArgs e)
        {
            Program.ShowButtonConfigForm();
        }

        // show the main or vk image.
        public void showMainImage(bool vk = false)
        {
            if (!vk)
                img_MAIN.BackgroundImage = img_defaultConfig.Image;
            else
                img_MAIN.BackgroundImage = img_VKConfig.Image;
        }
    }
}
