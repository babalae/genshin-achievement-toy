using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GenshinAchievement.Forms
{
    public partial class FormUploadTips : Form
    {
        public FormUploadTips()
        {
            InitializeComponent();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData"));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://cocogoat.work/achievement");
        }
    }
}
