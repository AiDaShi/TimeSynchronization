namespace TimeSynchronizationClient
{
    partial class TimeClient
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
            this.SendMsgNew = new System.Windows.Forms.TextBox();
            this.ListedButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ProtContent = new System.Windows.Forms.TextBox();
            this.IPContent = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GetMsgList = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SendMsgNew
            // 
            this.SendMsgNew.Enabled = false;
            this.SendMsgNew.Location = new System.Drawing.Point(7, 265);
            this.SendMsgNew.Margin = new System.Windows.Forms.Padding(2);
            this.SendMsgNew.Name = "SendMsgNew";
            this.SendMsgNew.Size = new System.Drawing.Size(401, 20);
            this.SendMsgNew.TabIndex = 19;
            // 
            // ListedButton
            // 
            this.ListedButton.Location = new System.Drawing.Point(424, 16);
            this.ListedButton.Margin = new System.Windows.Forms.Padding(2);
            this.ListedButton.Name = "ListedButton";
            this.ListedButton.Size = new System.Drawing.Size(116, 37);
            this.ListedButton.TabIndex = 18;
            this.ListedButton.Text = "Connection Server";
            this.ListedButton.UseVisualStyleBackColor = true;
            this.ListedButton.Click += new System.EventHandler(this.ListedButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ProtContent);
            this.groupBox1.Controls.Add(this.IPContent);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(400, 81);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // ProtContent
            // 
            this.ProtContent.Location = new System.Drawing.Point(107, 50);
            this.ProtContent.Margin = new System.Windows.Forms.Padding(2);
            this.ProtContent.Name = "ProtContent";
            this.ProtContent.Size = new System.Drawing.Size(188, 20);
            this.ProtContent.TabIndex = 1;
            this.ProtContent.Text = "4850";
            // 
            // IPContent
            // 
            this.IPContent.Location = new System.Drawing.Point(107, 23);
            this.IPContent.Margin = new System.Windows.Forms.Padding(2);
            this.IPContent.Name = "IPContent";
            this.IPContent.Size = new System.Drawing.Size(188, 20);
            this.IPContent.TabIndex = 1;
            this.IPContent.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Prot：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP：";
            // 
            // GetMsgList
            // 
            this.GetMsgList.FormattingEnabled = true;
            this.GetMsgList.Location = new System.Drawing.Point(7, 88);
            this.GetMsgList.Margin = new System.Windows.Forms.Padding(2);
            this.GetMsgList.Name = "GetMsgList";
            this.GetMsgList.Size = new System.Drawing.Size(401, 160);
            this.GetMsgList.TabIndex = 15;
            // 
            // TimeClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 257);
            this.Controls.Add(this.SendMsgNew);
            this.Controls.Add(this.ListedButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GetMsgList);
            this.Name = "TimeClient";
            this.Load += new System.EventHandler(this.TimeClient_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox SendMsgNew;
        public System.Windows.Forms.Button ListedButton;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox ProtContent;
        public System.Windows.Forms.TextBox IPContent;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListBox GetMsgList;
    }
}