namespace GenshinAchievement.Forms
{
    partial class FormUploadTips
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(401, 103);
            this.label2.TabIndex = 6;
            this.label2.Text = "一键识别成就后，\r\n打开软件所在目录的 UserData 文件夹，\r\n里面的 天地万象_img_section / \r\n心跳的记忆_img_section 文件夹" +
    "保存有所有成就的图片。";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(151, 155);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(125, 18);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "cocogoat.work";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "然后游览器打开 ";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Location = new System.Drawing.Point(12, 115);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(262, 29);
            this.btnOpenFolder.TabIndex = 7;
            this.btnOpenFolder.Text = "打开成就图片文件夹";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(396, 55);
            this.label3.TabIndex = 8;
            this.label3.Text = "点击右上角的“识别”按钮，使用椰羊的导入第三方扫描器的图片结果的功能导入所有成就图片";
            // 
            // FormUploadTips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 250);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = global::GenshinAchievement.Properties.Resources.kokomi;
            this.Name = "FormUploadTips";
            this.Text = "上传至椰羊识别";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Label label3;
    }
}