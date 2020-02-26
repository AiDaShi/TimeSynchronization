namespace TimeSynchronization.TimeForms
{
    partial class TimeMainForm
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
            this.StartServerBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SettingBtn = new System.Windows.Forms.Button();
            this.LogList = new System.Windows.Forms.ListBox();
            this.ChildName = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartServerBtn
            // 
            this.StartServerBtn.BackColor = System.Drawing.Color.Red;
            this.StartServerBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StartServerBtn.Location = new System.Drawing.Point(16, 13);
            this.StartServerBtn.Name = "StartServerBtn";
            this.StartServerBtn.Size = new System.Drawing.Size(93, 204);
            this.StartServerBtn.TabIndex = 0;
            this.StartServerBtn.Text = "StartServer";
            this.StartServerBtn.UseVisualStyleBackColor = false;
            this.StartServerBtn.Click += new System.EventHandler(this.StartServerBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StartServerBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 223);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // SettingBtn
            // 
            this.SettingBtn.Location = new System.Drawing.Point(154, 12);
            this.SettingBtn.Name = "SettingBtn";
            this.SettingBtn.Size = new System.Drawing.Size(255, 23);
            this.SettingBtn.TabIndex = 2;
            this.SettingBtn.Text = "Setting Server";
            this.SettingBtn.UseVisualStyleBackColor = true;
            this.SettingBtn.Click += new System.EventHandler(this.SettingBtn_Click);
            // 
            // LogList
            // 
            this.LogList.FormattingEnabled = true;
            this.LogList.Location = new System.Drawing.Point(154, 97);
            this.LogList.Name = "LogList";
            this.LogList.Size = new System.Drawing.Size(255, 134);
            this.LogList.TabIndex = 3;
            // 
            // ChildName
            // 
            this.ChildName.Location = new System.Drawing.Point(154, 41);
            this.ChildName.Name = "ChildName";
            this.ChildName.Size = new System.Drawing.Size(255, 23);
            this.ChildName.TabIndex = 4;
            this.ChildName.Text = "Child List";
            this.ChildName.UseVisualStyleBackColor = true;
            this.ChildName.Click += new System.EventHandler(this.ChildName_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(255, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Setting Time";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TimeMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 247);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ChildName);
            this.Controls.Add(this.LogList);
            this.Controls.Add(this.SettingBtn);
            this.Controls.Add(this.groupBox1);
            this.Name = "TimeMainForm";
            this.Load += new System.EventHandler(this.TimeMainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartServerBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SettingBtn;
        public System.Windows.Forms.ListBox LogList;
        private System.Windows.Forms.Button ChildName;
        private System.Windows.Forms.Button button1;
    }
}