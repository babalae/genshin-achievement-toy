using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinAchievement
{
    public partial class FormPreviewCaptureArea : Form
    {
        public FormPreviewCaptureArea()
        {
            InitializeComponent();
        }

        public FormPreviewCaptureArea(Bitmap bitmap)
        {
            InitializeComponent();
            pictureBox1.Image = bitmap;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
