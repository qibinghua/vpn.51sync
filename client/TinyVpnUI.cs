using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VPN51SYNC;
using System.Threading;
using DotRas;
using System.Collections.ObjectModel;

namespace VPN51SYNC
{
    public partial class TinyVpnUI : Form
    {
        private bool shouldClose;
        private Thread StartThread;
        private Thread StopThread;
        private static TinyVpnClient client;
        public TinyVpnUI()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            StartThread = new Thread(new ParameterizedThreadStart(StartVpn));
            StopThread = new Thread(new ThreadStart(StopConnection));
        }
        private void TinyVpnUI_Load(object sender, EventArgs e)
        {
            this.panel1.Enabled = false;
            client = new TinyVpnClient();
            client.ClientException += client_ClientException;
            client.ClientMassage += client_ClientMassage;
            client.ConnectionStatusChanged += client_ConnectionStatusChanged;
            client.ClientDialCompleted += client_ClientDialCompleted;
            client.ClientInitializeCompleted += client_ClientInitializeCompleted;
            new Thread(delegate()
            {
                client.Initialize();
            }).Start();
        }

        void client_ClientInitializeCompleted()
        {
            this.Invoke(new Action(() =>
            {
                if (client.flagConnected)
                {
                    UIDisconnect.Enabled = true;
                    UIDisconnect.Text = "断开" + client.lastServerUsed;
                }
                else
                {
                    UIDisconnect.Text = "断开";
                    UIDisconnect.Enabled = false;
                }
                UIServerList.DataSource = new VpnServerList().VpnServers;
                UIServerList.DisplayMember = "UIName";
                UIServerList.ValueMember = "Server";
                this.panel1.Enabled = true;
            }));
        }

        void client_ClientDialCompleted(object sender, DialCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                AppendText("操作已取消");
            }
            else if (e.TimedOut)
            {
                AppendText("操作超时");
            }
            else if (e.Error != null)
            {
                if (client.macAdd != client.macAddress)
                {
                    AppendText("绑定到本机当前MAC的试用许可已过期。");
                    AppendText("将本机MAC地址修改为" + client.macAddress.Replace(":", "").ToUpper() + "以绕过版本许可限制。");
                }
                else
                {
                    AppendText("ERROR: " + e.Error.Message);
                }
            }
            else if (e.Connected)
            {
                AppendText("[" + DateTime.Now.ToShortTimeString() + "] " + "VPN已连接，更新IP...");
                if (client.updateIpaddress())
                {
                    AppendText("[" + DateTime.Now.ToShortTimeString() + "] " + "当前IP地址：" + client.publicIpAddress);
                }
                else
                    AppendText("[" + DateTime.Now.ToShortTimeString() + "] " + "更新IP失败，请访问http://ip138.com查看");
                this.Invoke(new Action(() =>
                {
                    UIDisconnect.Text = "断开" + client.lastServerUsed;
                    UIConnect.Enabled = true;
                    UIDisconnect.Enabled = true;
                }));
            }
            if (!e.Connected)
            {
                this.Invoke(new Action(() =>
                {
                    UIConnect.Enabled = true;
                    UIDisconnect.Text = "断开";
                    UIDisconnect.Enabled = false;
                }));
            }
            Application.DoEvents();
        }

        private void client_ConnectionStatusChanged(DotRas.RasConnectionState connectionState)
        {
            this.Invoke(new Action(() =>
            {
                UIConnect.Enabled = false;
                UIDisconnect.Enabled = false;
                UIMessage.Text = connectionState.ToString();
            }));
            Application.DoEvents();
        }

        private void client_ClientException(Exception ex)
        {
            this.Invoke(new Action(() =>
            {
                UIConnect.Enabled = true;
                UIDisconnect.Enabled = false;

            }));
            AppendText(ex.Message);
            Application.DoEvents();
        }
        void AppendText(string message)
        {
            UIMessage.Tag = message;
            this.Invoke(new Action(() =>
            {
                UIMessageText.AppendText(message + "\r\n");
                UIMessageText.ScrollToCaret();
            }));
            message = message.Replace(Environment.NewLine, "");
            if (message.Length > 50)
            {
                message = message.Substring(0, 50) + "...";
            }
            this.Invoke(new Action(() => { UIMessage.Text = message; }));

        }
        private void client_ClientMassage(string message)
        {
            AppendText(message);
            Application.DoEvents();
        }

        private void UIMessage_Click(object sender, EventArgs e)
        {
            MessageBox.Show(UIMessage.Tag as string);
            Application.DoEvents();
        }

        private void UIConnect_Click(object sender, EventArgs e)
        {
            object selectedServer = UIServerList.SelectedValue;
            if (UIServerList.SelectedIndex != -1)
            {
                this.UIConnect.Enabled = false;
                if (StartThread.ThreadState == ThreadState.Stopped)
                {
                    StartThread = new Thread(new ParameterizedThreadStart(StartVpn));
                    StartThread.Start(selectedServer);
                }
                else if (StartThread.ThreadState == ThreadState.Unstarted )
                {
                    StartThread.Start(selectedServer);
                }
                else
                {
                    DialogResult d = MessageBox.Show("连接正在进行，是否终止操作？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (d == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            StartThread.Abort();
                        }
                        catch
                        {

                        }
                        finally
                        {
                            this.UIConnect.Enabled = true;
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("选择一个服务器以继续...");
            }

        }

        void StartVpn(object selectedServer)
        {
            KeyValuePair<string, string> server = (KeyValuePair<string, string>)selectedServer;
            client.userName = UIUsername.Text;
            client.passWord = UIPassword.Text;
            client.StartVpn(server.Key, server.Value);
        }
        private void UIDisconnect_Click(object sender, EventArgs e)
        {
            this.UIDisconnect.Enabled = false;
            if (StopThread.ThreadState == ThreadState.Unstarted)
            {
                StopThread.Start();
            }
            else if (StopThread.ThreadState == ThreadState.Stopped)
            {
                StopThread = new Thread(new ThreadStart(StopConnection));
                StopThread.Start();
            }
            else
            {
                MessageBox.Show("操作正在进行，请稍后再试...");
                this.UIDisconnect.Enabled = true;
                return;
            }
        }
        private void StopConnection()
        {
            client.StopConnection();
        }

        private void TinyVpnUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            if (!shouldClose)
            {
                e.Cancel = true;
            }
            else
            {
                client.ClientException -= client_ClientException;
                client.ClientMassage -= client_ClientMassage;
                client.ConnectionStatusChanged -= client_ConnectionStatusChanged;
                client.ClientDialCompleted -= client_ClientDialCompleted;
                client.ClientInitializeCompleted -= client_ClientInitializeCompleted;
            }
        }

        private void UIClearMessages_Click(object sender, EventArgs e)
        {
            this.UIMessageText.ResetText();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shouldClose = true;
            this.Close();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.Activate();
            this.WindowState = FormWindowState.Normal;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
