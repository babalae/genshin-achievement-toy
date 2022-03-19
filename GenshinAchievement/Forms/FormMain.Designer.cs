
namespace GenshinAchievement
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblYSStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboEdition = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnExport3 = new System.Windows.Forms.Button();
            this.btnExport2 = new System.Windows.Forms.Button();
            this.btnExport1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnUploadCocogoat = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbConsole
            // 
            this.rtbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rtbConsole.Location = new System.Drawing.Point(12, 424);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.Size = new System.Drawing.Size(298, 180);
            this.rtbConsole.TabIndex = 0;
            this.rtbConsole.Text = "";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(22, 29);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(253, 30);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "一键识别成就";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "原神状态：";
            // 
            // lblYSStatus
            // 
            this.lblYSStatus.AutoSize = true;
            this.lblYSStatus.ForeColor = System.Drawing.Color.Red;
            this.lblYSStatus.Location = new System.Drawing.Point(123, 33);
            this.lblYSStatus.Name = "lblYSStatus";
            this.lblYSStatus.Size = new System.Drawing.Size(62, 18);
            this.lblYSStatus.TabIndex = 8;
            this.lblYSStatus.Text = "未启动";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboEdition);
            this.groupBox1.Controls.Add(this.lblYSStatus);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 91);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "第一步：启动原神并打开成就页面";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "特辑：";
            // 
            // cboEdition
            // 
            this.cboEdition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEdition.FormattingEnabled = true;
            this.cboEdition.Location = new System.Drawing.Point(87, 58);
            this.cboEdition.Name = "cboEdition";
            this.cboEdition.Size = new System.Drawing.Size(173, 26);
            this.cboEdition.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Location = new System.Drawing.Point(12, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 89);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "第二步：滚动截图、保存并识别";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(22, 65);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(253, 15);
            this.progressBar1.TabIndex = 4;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnExport3);
            this.groupBox4.Controls.Add(this.btnExport2);
            this.groupBox4.Controls.Add(this.btnExport1);
            this.groupBox4.Location = new System.Drawing.Point(12, 243);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(298, 138);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "第三步：导出结果";
            // 
            // btnExport3
            // 
            this.btnExport3.Location = new System.Drawing.Point(22, 27);
            this.btnExport3.Name = "btnExport3";
            this.btnExport3.Size = new System.Drawing.Size(253, 30);
            this.btnExport3.TabIndex = 2;
            this.btnExport3.Text = "导出到 cocogoat.work";
            this.btnExport3.UseVisualStyleBackColor = true;
            this.btnExport3.Click += new System.EventHandler(this.btnExport3_Click);
            // 
            // btnExport2
            // 
            this.btnExport2.Location = new System.Drawing.Point(22, 99);
            this.btnExport2.Name = "btnExport2";
            this.btnExport2.Size = new System.Drawing.Size(253, 30);
            this.btnExport2.TabIndex = 1;
            this.btnExport2.Text = "导出到 seelie.me";
            this.btnExport2.UseVisualStyleBackColor = true;
            this.btnExport2.Click += new System.EventHandler(this.btnExport2_Click);
            // 
            // btnExport1
            // 
            this.btnExport1.Location = new System.Drawing.Point(22, 63);
            this.btnExport1.Name = "btnExport1";
            this.btnExport1.Size = new System.Drawing.Size(253, 30);
            this.btnExport1.TabIndex = 0;
            this.btnExport1.Text = "导出到 paimon.moe";
            this.btnExport1.UseVisualStyleBackColor = true;
            this.btnExport1.Click += new System.EventHandler(this.btnExport1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(31, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 18);
            this.label3.TabIndex = 13;
            this.label3.Text = "开源地址与使用说明：";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(206, 9);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(80, 18);
            this.linkLabel1.TabIndex = 14;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "软件主页";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(367, 177);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 30);
            this.button1.TabIndex = 15;
            this.button1.Text = "OcrTest";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 387);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(130, 30);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "清空记录";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnUploadCocogoat
            // 
            this.btnUploadCocogoat.ForeColor = System.Drawing.Color.BlueViolet;
            this.btnUploadCocogoat.Location = new System.Drawing.Point(148, 387);
            this.btnUploadCocogoat.Name = "btnUploadCocogoat";
            this.btnUploadCocogoat.Size = new System.Drawing.Size(162, 30);
            this.btnUploadCocogoat.TabIndex = 16;
            this.btnUploadCocogoat.Text = "上传至椰羊识别";
            this.btnUploadCocogoat.UseVisualStyleBackColor = true;
            this.btnUploadCocogoat.Click += new System.EventHandler(this.btnUploadCocogoat_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 616);
            this.Controls.Add(this.btnUploadCocogoat);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtbConsole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::GenshinAchievement.Properties.Resources.kokomi;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "原神成就导出";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblYSStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboEdition;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnExport2;
        private System.Windows.Forms.Button btnExport1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnExport3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnUploadCocogoat;
    }
}

