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
    public partial class FormText : Form
    {
        public FormText()
        {
            InitializeComponent();
        }

        public FormText(string displayText)
        {
            InitializeComponent();
            richTextBox1.Text = displayText;
            InitRichTextBoxContextMenu(richTextBox1);
        }

        private static void InitRichTextBoxContextMenu(RichTextBox textBox)
        {
            //创建全选子菜单
            var selectAllMenuItem = new MenuItem("全选");
            selectAllMenuItem.Click += (sender, eventArgs) => textBox.SelectAll();

            //创建复制子菜单
            var copyMenuItem = new MenuItem("复制");
            copyMenuItem.Click += (sender, eventArgs) => textBox.Copy();

            //创建右键菜单并将子菜单加入到右键菜单中
            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(selectAllMenuItem);
            contextMenu.MenuItems.Add(copyMenuItem);
            textBox.ContextMenu = contextMenu;

        }
    }
}
