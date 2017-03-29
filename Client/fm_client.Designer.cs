namespace Client
{
    partial class fm_client
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
            if(ClientToServerChat!=null)
            ClientToServerChat.Close();
            if (CLientToServerFile != null)
                CLientToServerFile.Close();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.tb_rec = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_send = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.tb_send = new System.Windows.Forms.TextBox();
            this.btn_sendFile = new System.Windows.Forms.Button();
            this.btn_recFile = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tb_rec);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 178);
            this.panel1.TabIndex = 3;
            // 
            // tb_rec
            // 
            this.tb_rec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_rec.Location = new System.Drawing.Point(0, 0);
            this.tb_rec.Multiline = true;
            this.tb_rec.Name = "tb_rec";
            this.tb_rec.Size = new System.Drawing.Size(574, 178);
            this.tb_rec.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_send);
            this.panel2.Controls.Add(this.btn_clear);
            this.panel2.Controls.Add(this.tb_send);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 210);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(574, 189);
            this.panel2.TabIndex = 4;
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(487, 160);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 2;
            this.btn_send.Text = "发送";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(397, 160);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(75, 23);
            this.btn_clear.TabIndex = 1;
            this.btn_clear.Text = "清空";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // tb_send
            // 
            this.tb_send.Dock = System.Windows.Forms.DockStyle.Top;
            this.tb_send.Location = new System.Drawing.Point(0, 0);
            this.tb_send.Multiline = true;
            this.tb_send.Name = "tb_send";
            this.tb_send.Size = new System.Drawing.Size(574, 154);
            this.tb_send.TabIndex = 0;
            // 
            // btn_sendFile
            // 
            this.btn_sendFile.Location = new System.Drawing.Point(397, 181);
            this.btn_sendFile.Name = "btn_sendFile";
            this.btn_sendFile.Size = new System.Drawing.Size(75, 23);
            this.btn_sendFile.TabIndex = 5;
            this.btn_sendFile.Text = " 发送文件";
            this.btn_sendFile.UseVisualStyleBackColor = true;
          
            // 
            // btn_recFile
            // 
            this.btn_recFile.Location = new System.Drawing.Point(487, 181);
            this.btn_recFile.Name = "btn_recFile";
            this.btn_recFile.Size = new System.Drawing.Size(75, 23);
            this.btn_recFile.TabIndex = 6;
            this.btn_recFile.Text = "接收文件";
            this.btn_recFile.UseVisualStyleBackColor = true;
            // 
            // fm_client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 399);
            this.Controls.Add(this.btn_recFile);
            this.Controls.Add(this.btn_sendFile);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fm_client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "客户端";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tb_rec;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.TextBox tb_send;
        private System.Windows.Forms.Button btn_sendFile;
        private System.Windows.Forms.Button btn_recFile;
    }
}