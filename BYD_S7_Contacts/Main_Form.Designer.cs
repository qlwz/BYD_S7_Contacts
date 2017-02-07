namespace BYD_S7_Contacts
{
    partial class Main_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Form));
            this.txt_说明 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_CSVFile = new System.Windows.Forms.TextBox();
            this.btn_OpenFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_BluetoothName = new System.Windows.Forms.TextBox();
            this.btn_Generate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbo_NameMerger = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txt_说明
            // 
            this.txt_说明.Location = new System.Drawing.Point(6, 134);
            this.txt_说明.Multiline = true;
            this.txt_说明.Name = "txt_说明";
            this.txt_说明.ReadOnly = true;
            this.txt_说明.Size = new System.Drawing.Size(362, 320);
            this.txt_说明.TabIndex = 888;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "文件路径：";
            // 
            // txt_CSVFile
            // 
            this.txt_CSVFile.Location = new System.Drawing.Point(75, 5);
            this.txt_CSVFile.Name = "txt_CSVFile";
            this.txt_CSVFile.ReadOnly = true;
            this.txt_CSVFile.Size = new System.Drawing.Size(178, 21);
            this.txt_CSVFile.TabIndex = 1;
            // 
            // btn_OpenFile
            // 
            this.btn_OpenFile.Location = new System.Drawing.Point(270, 4);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenFile.TabIndex = 2;
            this.btn_OpenFile.Text = "打开";
            this.btn_OpenFile.UseVisualStyleBackColor = true;
            this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "蓝牙名称：";
            // 
            // txt_BluetoothName
            // 
            this.txt_BluetoothName.Location = new System.Drawing.Point(75, 32);
            this.txt_BluetoothName.Name = "txt_BluetoothName";
            this.txt_BluetoothName.Size = new System.Drawing.Size(178, 21);
            this.txt_BluetoothName.TabIndex = 3;
            this.txt_BluetoothName.Text = "罗桂文的 iPhone";
            // 
            // btn_Generate
            // 
            this.btn_Generate.Location = new System.Drawing.Point(143, 93);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(75, 23);
            this.btn_Generate.TabIndex = 5;
            this.btn_Generate.Text = "生成";
            this.btn_Generate.UseVisualStyleBackColor = true;
            this.btn_Generate.Click += new System.EventHandler(this.btn_Generate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "姓名组合：";
            // 
            // cbo_NameMerger
            // 
            this.cbo_NameMerger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_NameMerger.FormattingEnabled = true;
            this.cbo_NameMerger.Items.AddRange(new object[] {
            "姓氏+名字",
            "名字+姓氏",
            "仅姓氏",
            "仅名字"});
            this.cbo_NameMerger.Location = new System.Drawing.Point(77, 61);
            this.cbo_NameMerger.Name = "cbo_NameMerger";
            this.cbo_NameMerger.Size = new System.Drawing.Size(176, 20);
            this.cbo_NameMerger.TabIndex = 4;
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 462);
            this.Controls.Add(this.cbo_NameMerger);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_Generate);
            this.Controls.Add(this.txt_BluetoothName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_OpenFile);
            this.Controls.Add(this.txt_CSVFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_说明);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BYD S7 通讯录同步工具";
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_说明;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_CSVFile;
        private System.Windows.Forms.Button btn_OpenFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_BluetoothName;
        private System.Windows.Forms.Button btn_Generate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbo_NameMerger;
    }
}

