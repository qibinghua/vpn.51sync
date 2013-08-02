namespace VPN51SYNC
{
    partial class TinyVpnUI
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TinyVpnUI));
            this.UIConnect = new System.Windows.Forms.Button();
            this.UIServerList = new System.Windows.Forms.ComboBox();
            this.UIStatusStrip = new System.Windows.Forms.StatusStrip();
            this.UIMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.UIDisconnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.UIMessageText = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.UIClearMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.UIPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.UIUsername = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UIStatusStrip.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIConnect
            // 
            this.UIConnect.Location = new System.Drawing.Point(218, 42);
            this.UIConnect.Name = "UIConnect";
            this.UIConnect.Size = new System.Drawing.Size(75, 23);
            this.UIConnect.TabIndex = 0;
            this.UIConnect.Text = "连接";
            this.UIConnect.UseVisualStyleBackColor = true;
            this.UIConnect.Click += new System.EventHandler(this.UIConnect_Click);
            // 
            // UIServerList
            // 
            this.UIServerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UIServerList.FormattingEnabled = true;
            this.UIServerList.Location = new System.Drawing.Point(66, 43);
            this.UIServerList.Name = "UIServerList";
            this.UIServerList.Size = new System.Drawing.Size(144, 20);
            this.UIServerList.TabIndex = 1;
            // 
            // UIStatusStrip
            // 
            this.UIStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UIMessage});
            this.UIStatusStrip.Location = new System.Drawing.Point(0, 347);
            this.UIStatusStrip.Name = "UIStatusStrip";
            this.UIStatusStrip.Size = new System.Drawing.Size(431, 22);
            this.UIStatusStrip.TabIndex = 2;
            this.UIStatusStrip.Text = "statusStrip1";
            // 
            // UIMessage
            // 
            this.UIMessage.Name = "UIMessage";
            this.UIMessage.Size = new System.Drawing.Size(57, 17);
            this.UIMessage.Text = "VPN就绪";
            this.UIMessage.Click += new System.EventHandler(this.UIMessage_Click);
            // 
            // UIDisconnect
            // 
            this.UIDisconnect.Enabled = false;
            this.UIDisconnect.Location = new System.Drawing.Point(300, 42);
            this.UIDisconnect.Name = "UIDisconnect";
            this.UIDisconnect.Size = new System.Drawing.Size(120, 23);
            this.UIDisconnect.TabIndex = 0;
            this.UIDisconnect.Text = "断开";
            this.UIDisconnect.UseVisualStyleBackColor = true;
            this.UIDisconnect.Click += new System.EventHandler(this.UIDisconnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择VPN：";
            // 
            // UIMessageText
            // 
            this.UIMessageText.AcceptsReturn = true;
            this.UIMessageText.ContextMenuStrip = this.contextMenuStrip1;
            this.UIMessageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIMessageText.Location = new System.Drawing.Point(0, 74);
            this.UIMessageText.Multiline = true;
            this.UIMessageText.Name = "UIMessageText";
            this.UIMessageText.ReadOnly = true;
            this.UIMessageText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UIMessageText.Size = new System.Drawing.Size(431, 273);
            this.UIMessageText.TabIndex = 4;
            this.UIMessageText.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UIClearMessages});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // UIClearMessages
            // 
            this.UIClearMessages.Name = "UIClearMessages";
            this.UIClearMessages.Size = new System.Drawing.Size(124, 22);
            this.UIClearMessages.Text = "清空日志";
            this.UIClearMessages.Click += new System.EventHandler(this.UIClearMessages_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.UIPassword);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.UIUsername);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.UIConnect);
            this.panel1.Controls.Add(this.UIDisconnect);
            this.panel1.Controls.Add(this.UIServerList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 74);
            this.panel1.TabIndex = 5;
            // 
            // UIPassword
            // 
            this.UIPassword.Location = new System.Drawing.Point(275, 12);
            this.UIPassword.MaxLength = 255;
            this.UIPassword.Name = "UIPassword";
            this.UIPassword.Size = new System.Drawing.Size(144, 21);
            this.UIPassword.TabIndex = 7;
            this.UIPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "密  码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "用户名：";
            // 
            // UIUsername
            // 
            this.UIUsername.Location = new System.Drawing.Point(66, 13);
            this.UIUsername.MaxLength = 255;
            this.UIUsername.Name = "UIUsername";
            this.UIUsername.Size = new System.Drawing.Size(144, 21);
            this.UIUsername.TabIndex = 4;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip2;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "TinyVpn2 Pro";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(101, 26);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // TinyVpnUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 369);
            this.Controls.Add(this.UIMessageText);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.UIStatusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TinyVpnUI";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VPN.51SYNC.COM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TinyVpnUI_FormClosing);
            this.Load += new System.EventHandler(this.TinyVpnUI_Load);
            this.UIStatusStrip.ResumeLayout(false);
            this.UIStatusStrip.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UIConnect;
        private System.Windows.Forms.ComboBox UIServerList;
        private System.Windows.Forms.StatusStrip UIStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel UIMessage;
        private System.Windows.Forms.Button UIDisconnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UIMessageText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem UIClearMessages;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.TextBox UIUsername;
        private System.Windows.Forms.TextBox UIPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}

