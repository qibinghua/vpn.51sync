using DotRas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Collections.ObjectModel;

namespace VPN51SYNC
{
    public class TinyVpnClient
    {
        public static int generateNewMACCount = 1;
        public string macAddress = "";
        public delegate void ClientInitializeCompletedHandler();
        public delegate void ClientMsgHandler(string msg);
        public delegate void ClientExceptionHandler(Exception ex);
        public delegate void ConnectionStatusChangedHandler(RasConnectionState connectionState);
        public delegate void ClientDialCompletedHandler(object sender, DialCompletedEventArgs e);
        public event ConnectionStatusChangedHandler ConnectionStatusChanged;
        public event ClientInitializeCompletedHandler ClientInitializeCompleted;
        public event ClientExceptionHandler ClientException;
        public event ClientMsgHandler ClientMassage;
        public event ClientDialCompletedHandler ClientDialCompleted;
        public RasHandle connectionHandle;
        public RasConnectionStatus connectionstatus;
        public RasConnection connectionVar;

        public bool disconnected;
        public string expiringDate;
        public bool flagConnected { get; private set; }
        private bool initialized;
        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }
        public int ipaddressCounter;
        public string kryptotelurl;
        public string kryptotelurlbackup;
        public string lastServerIpUsed;
        public string lastServerUsed;
        public string macAdd;
        public string passWord;
        public string publicIpAddress;
        public string subscriptiontype;
        private const int SW_SHOWNORMAL = 1;
        public string userName;
        public string vpnoneclickurl;
        public RasConnectionWatcher watcher;

        public TinyVpnClient()
        {
            this.macAdd = "";
            this.userName = "test";
            this.passWord = "12345678";
            this.expiringDate = "";
            this.subscriptiontype = "";
            this.flagConnected = false;
            this.watcher = new RasConnectionWatcher();
            this.disconnected = true;
            this.ipaddressCounter = 0;
            this.initialized = false;
            this.publicIpAddress = "";
            this.kryptotelurl = "http://vpn.51sync.com";
            this.kryptotelurlbackup = "http://vpn.51sync.com";
            this.vpnoneclickurl = "http://vpn.51sync.com";

        }
        public void Initialize()
        {
            OnClientMessageEvent("VPN初始化开始...");
            string initProcessMessage = "";
            this.AllUsersPhoneBook = new RasPhoneBook();
            this.Dialer = new RasDialer();
            if (!checkdnspoisoning("vpn.51sync.com"))
            {
                //kryptotelurlbackup = "http://200.63.44.24";
                //kryptotelurl = "http://216.185.105.35";
                initProcessMessage += "认证网络失败";
            }
            else
                initProcessMessage += "认证网络正常";
            updateIpaddress();
            if (!checkdnspoisoning("vpn.51sync.com"))
            {
                //vpnoneclickurl = "http://216.185.105.35/vpnoneclick";
                initProcessMessage += "；VPN网络不正常";
            }
            else
            {
                initProcessMessage += "；VPN网络正常";
            }
            /**
            this.macAdd = macAddress = this.GetMACAddress();
            this.GetUserNamePassword(macAddress);
            while (this.subscriptiontype != "ELITE" || DateTime.Parse(this.expiringDate).Date < DateTime.Now.Date)
            {
                macAddress = GenNewMac(macAddress);
                OnClientMessageEvent("认证信息修正+" + generateNewMACCount);
                generateNewMACCount++;
                this.GetUserNamePassword(macAddress);
            }
            if (this.macAdd != macAddress)
            {
                OnClientMessageEvent("绑定到本机当前MAC的试用许可已过期。");
                OnClientMessageEvent("将本机MAC地址修改为"+macAddress.Replace(":","").ToUpper()+"以绕过版本许可限制。");
            }
            */
            this.Initialized = true;
            OnClientMessageEvent(initProcessMessage);
            RasConnection conn = GetActiveConnection();
            if (conn != null)
            {
                OnClientMessageEvent("当前活动VPN连接:" + conn.EntryName + "");
                this.connectionVar = conn;
                this.connectionstatus = connectionVar.GetConnectionStatus();
                this.connectionHandle = connectionVar.Handle;
                this.flagConnected = true;
                this.lastServerUsed = conn.EntryName;
                this.Dialer.EntryName = conn.EntryName;
                this.Dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
                this.Dialer.Credentials = new NetworkCredential(this.userName, this.passWord);
            }
            OnInitializeCompletedEvent();
            OnClientMessageEvent("VPN初始化完成，" + ((this.publicIpAddress.Trim().Length == 0) ? ("外网IP检测失败，请访问http://ip138.com查看") : "当前连接IP：" + this.publicIpAddress + "，请点击连接"));

        }
        private RasConnection GetActiveConnection()
        {
            foreach (RasConnection conn in RasConnection.GetActiveConnections())
            {
                if (conn != null)
                {
                    return conn;
                }
            }
            return null;
        }
        private bool checkdnspoisoning(string namep)
        {
            try
            {
                string hostNameOrAddress =  namep;
                IPHostEntry hostEntry = Dns.GetHostEntry(hostNameOrAddress);
                IPAddress[] addressList = hostEntry.AddressList;
                string str = addressList[0].ToString();
                if (str.Equals("182.237.0.18"))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        protected virtual void OnConnectionStatusChangedEvent(RasConnectionState state)
        {
            if (ConnectionStatusChanged != null)
            {
                ConnectionStatusChanged(state);
            }
        }
        protected virtual void OnClientMessageEvent(string msg)
        {
            if (ClientException != null)
            {
                ClientMassage("[" + DateTime.Now.ToShortTimeString() + "] " + msg);
            }
        }
        protected virtual void OnInitializeCompletedEvent()
        {
            if (ClientException != null)
            {
                ClientInitializeCompleted();
            }
        }
        public void OnClientExceptionEvent(Exception ex)
        {
            this.flagConnected = false;
            if (ClientException != null)
            {
                Exception exception = new Exception("[" + DateTime.Now.ToShortTimeString() + "] " + ex.Message);
                ClientException(exception);
            }
        }
        public void OnClientDialCompleted(object sender, DialCompletedEventArgs e)
        {
            if (ClientDialCompleted != null)
            {
                ClientDialCompleted(sender, e);
            }
        }
        private RasDialer _Dialer;
        private void Dialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.flagConnected = false;
            }
            else if (e.TimedOut)
            {
                this.flagConnected = false;
            }
            else if (e.Error != null)
            {
                this.flagConnected = false;
            }
            else if (e.Connected)
            {
                this.flagConnected = true;
            }
            if (!e.Connected)
            {
                this.flagConnected = false;
            }
            OnClientDialCompleted(sender, e);
        }
        private void Dialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            OnConnectionStatusChangedEvent(e.State);
        }
        internal virtual RasDialer Dialer
        {
            get
            {
                return this._Dialer;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler<DialCompletedEventArgs> handler = new EventHandler<DialCompletedEventArgs>(this.Dialer_DialCompleted);
                EventHandler<StateChangedEventArgs> handler2 = new EventHandler<StateChangedEventArgs>(this.Dialer_StateChanged);
                if (this._Dialer != null)
                {
                    this._Dialer.DialCompleted -= handler;
                    this._Dialer.StateChanged -= handler2;
                }
                this._Dialer = value;
                if (this._Dialer != null)
                {
                    this._Dialer.DialCompleted += handler;
                    this._Dialer.StateChanged += handler2;
                }
            }
        }
        public void StartVpn(string serverDesc, string serverIp)
        {
            //OnClientMessageEvent("username:"+this.userName+",password:"+this.passWord);
            if (this.Dialer.IsBusy)
            {
                OnClientMessageEvent("拨号器忙，请稍后再试。");
            }
            else
            {
                if (this.flagConnected)
                {
                    RasConnection conn = GetActiveConnection();
                    if (conn != null)
                    {
                        this.DisconnectVpn(conn.EntryName);
                    }
                    this.checkConnectionStatus();
                    this.updateIpaddress();
                }
                addVpnConfiguration(serverDesc, serverIp);
                
                this.DialVpn(serverDesc);
            }
            this.lastServerIpUsed = serverIp;
            this.lastServerUsed = serverDesc;
        }
        private void DialVpn(string serverDesc)
        {
            OnClientMessageEvent("正在连接" + serverDesc + "...");
            this.Dialer.EntryName = serverDesc;
            this.Dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
            try
            {
                this.Dialer.Credentials = new NetworkCredential(this.userName, this.passWord);
                this.connectionHandle = this.Dialer.DialAsync();
            }
            catch (Exception ex)
            {
                OnClientExceptionEvent(ex);
                return;
            }
        }
        public void StopConnection()
        {
            if (this.Dialer.IsBusy)
            {
                OnClientMessageEvent("拨号器忙，请稍后再试...");
            }
            else
            {
                this.DisconnectVpn(this.lastServerUsed);
                this.checkConnectionStatus();
                this.updateIpaddress();
            }
        }
        public void checkConnectionStatus()
        {
            try
            {
                if (connectionVar != null)
                {
                    this.connectionstatus = this.connectionVar.GetConnectionStatus();
                    OnConnectionStatusChangedEvent(this.connectionstatus.ConnectionState);
                }
                else
                {
                    OnConnectionStatusChangedEvent(RasConnectionState.Disconnected);
                    OnClientDialCompleted(this, new DialCompletedEventArgs(null, null, false, false, false, null));
                    this.flagConnected = false;
                }
            }
            catch (Exception exception1)
            {
                OnClientExceptionEvent(exception1);
            }
        }
        /// <summary>
        /// 创建或更新一个VPN连接(指定VPN名称，及IP)
        /// </summary>
        public void CreateOrUpdateVPN(string updateVPNname, string updateVPNip)
        {
            RasDialer dialer = new RasDialer();
            RasPhoneBook allUsersPhoneBook = new RasPhoneBook();
            allUsersPhoneBook.Open();
            // 如果已经该名称的VPN已经存在，则更新这个VPN服务器地址
            if (allUsersPhoneBook.Entries.Contains(updateVPNname))
            {
                allUsersPhoneBook.Entries[updateVPNname].PhoneNumber = updateVPNip;
                // 不管当前VPN是否连接，服务器地址的更新总能成功，如果正在连接，则需要VPN重启后才能起作用
                allUsersPhoneBook.Entries[updateVPNname].Update();
            }
            // 创建一个新VPN
            else
            {
                //RasEntry entry = RasEntry.CreateVpnEntry(updateVPNname, updateVPNip, RasVpnStrategy.PptpFirst, RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn));
                //allUsersPhoneBook.Entries.Add(entry);
                RasEntry entryL2TP = RasEntry.CreateVpnEntry(updateVPNname, updateVPNip, RasVpnStrategy.L2tpOnly, RasDevice.GetDeviceByName("(L2TP)", RasDeviceType.Vpn));
                
                allUsersPhoneBook.Entries.Add(entryL2TP);
                dialer.EntryName = updateVPNname;
                dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
            }
        }
        public void addVpnConfiguration(string serverdescription, string servername)
        {
            this.AllUsersPhoneBook = new RasPhoneBook();
            this.AllUsersPhoneBook.Open(true);
            string serverAddress = this.getipaddressbyname(servername);
            RasEntryOptions options = new RasEntryOptions();
            options.PreviewDomain = false;
            options.UsePreSharedKey = true;
            options.ShowDialingProgress = true;
            options.RemoteDefaultGateway = true;
            options.DoNotNegotiateMultilink = true;
            options.UseLogOnCredentials = true;
            //RasDevice deviceByName = RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn);
            RasDevice deviceByName = RasDevice.GetDeviceByName("(L2TP)", RasDeviceType.Vpn);
            RasEntry item;
            if (this.AllUsersPhoneBook.Entries.Contains(serverdescription))
            {
                //'entry' must have the PhoneNumber, DeviceType, DeviceName, FramingProtocol, and EntryType properties set as a minimum.
                item = this.AllUsersPhoneBook.Entries[serverdescription];
                item.PhoneNumber = serverAddress;
                item.Device = deviceByName;
                item.Options = options;
                
                item.UpdateCredentials(RasPreSharedKey.Client, "51sync");
                item.Update();
            }
            else
            {
                item = RasEntry.CreateVpnEntry(serverdescription, serverAddress, RasVpnStrategy.L2tpFirst, deviceByName);
                item.Options = options;
                this.AllUsersPhoneBook.Entries.Add(item);
                item.UpdateCredentials(RasPreSharedKey.Client, "51sync");
            }

        }
        public string getipaddressbyname(string name)
        {
            WebClient client = new WebClient();
            string str2 = "";
            string address = this.kryptotelurl + "/myip.php?name=" + name;
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            try
            {
                Stream stream = client.OpenRead(address);
                StreamReader reader = new StreamReader(stream);
                str2 = reader.ReadToEnd();
                stream.Close();
                reader.Close();
            }
            catch (Exception exception1)
            {
                OnClientExceptionEvent(new Exception("获取主认证服务器IP地址失败", exception1));
                str2 = name;
            }
            if (str2 == name)
            {
                string str4 = this.kryptotelurlbackup + "/myip.php?name=" + name;
                try
                {
                    Stream stream2 = client.OpenRead(str4);
                    StreamReader reader2 = new StreamReader(stream2);
                    str2 = reader2.ReadToEnd();
                    stream2.Close();
                    reader2.Close();
                }
                catch (Exception exception3)
                {
                    OnClientExceptionEvent(new Exception("获取备份认证服务器IP地址失败", exception3));
                }
            }
            return str2;
        }
        private void DisconnectVpn(string serverDesc)
        {
            OnClientMessageEvent("正在断开" + serverDesc + "...");
            this.Dialer.DialAsyncCancel();
            RasConnection activeConnectionByHandle = RasConnection.GetActiveConnectionByHandle(this.connectionHandle);
            if (activeConnectionByHandle != null)
            {
                activeConnectionByHandle.HangUp();
                Process process = new Process();
                process.StartInfo.FileName = Environment.SystemDirectory + @"\\rasdial.exe";
                process.StartInfo.Arguments = serverDesc + " /d";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }
            OnClientMessageEvent(serverDesc + "已断开");
        }
        private RasPhoneBook _AllUsersPhoneBook;
        public virtual RasPhoneBook AllUsersPhoneBook
        {
            get
            {
                return this._AllUsersPhoneBook;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._AllUsersPhoneBook = value;
            }
        }



        public void GetUserNamePassword(string macAddress)
        {
            try
            {
                OnClientMessageEvent("获取认证信息...");
                WebClient client = new WebClient();
                string input = "Wal29p.xy32zx8792.a8";
                string str = GetMd5Hash(MD5.Create(), input);
                string address = this.kryptotelurl + "/vpnoneclick_addfreetrial_windows2.php?macaddress=" + macAddress + "&pwd=" + str;
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                Stream stream = client.OpenRead(address);
                StreamReader reader = new StreamReader(stream);
                string str2 = reader.ReadToEnd();
                stream.Close();
                reader.Close();
                if (str2.Length >= 0x1f)
                {
                    string[] msg = str2.Split('#');
                    this.userName = msg[1];
                    this.passWord = msg[2];
                    this.expiringDate = msg[3];
                    this.subscriptiontype = msg[4];
                    OnClientMessageEvent("获取认证信息完成");
                }
                else
                {
                    OnClientExceptionEvent(new Exception("获取认证信息失败"));
                }
            }
            catch (Exception ex)
            {
                OnClientExceptionEvent(new Exception("获取认证信息失败", ex));
            }
        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] buffer = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            int num2 = buffer.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }
        public string GetMACAddress()
        {
            ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
            string macAddress = string.Empty;
            if (macAddress.Equals(""))
            {
                foreach (ManagementObject obj2 in instances)
                {
                    if (macAddress.Equals(string.Empty))
                    {
                        if (Convert.ToBoolean(((obj2["IPEnabled"]))))
                        {
                            macAddress = obj2["MacAddress"].ToString();
                            obj2.Dispose();
                            break;
                        }
                        obj2.Dispose();
                    }
                }
                if (macAddress.Equals(""))
                {
                    string str3 = this.generate_macaddress();
                    macAddress = str3;
                }
            }
            return macAddress;
        }
        public string GenNewMac(string macAddress)
        {
            string result = "";
            string[] mac = macAddress.Split(':');
            for (int i = 0; i < mac.Length; i++)
            {
                short item = short.Parse(mac[i], System.Globalization.NumberStyles.HexNumber);
                short itemNext = short.Parse(mac[i + 1 == mac.Length ? 0 : i + 1], System.Globalization.NumberStyles.HexNumber);
                if (item == 0)
                    item &= itemNext;
                else
                    item ^= itemNext;// (short)(itemNext >> 1);
                mac[i] = item.ToString("x8").Substring(6, 2);
            }
            for (int i = 0; i < mac.Length; i++)
            {
                if (i == mac.Length - 1)
                {
                    result += mac[i];
                    continue;
                }
                result += mac[i];
                result += ":";
            }
            return result;
        }
        public string generate_macaddress()
        {
            Random random = new Random();
            char[] chArray = "ABCDEFGHIJKLMNOPQRSTUVZXW0123456789ABCDFE".ToCharArray();
            string str2 = "AUTOGEN-";
            int num2 = 1;
            do
            {
                int index = random.Next(0, 0x21);
                str2 = str2 + Convert.ToString(chArray[index]);
                num2++;
            }
            while (num2 <= 20);
            return str2;
        }


        public bool updateIpaddress()
        {
            WebClient client = new WebClient();
            string str = "";
            string address = this.kryptotelurl + "/myip.php";
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            try
            {
                Stream stream = client.OpenRead(address);
                StreamReader reader = new StreamReader(stream);
                str = reader.ReadToEnd();
                stream.Close();
                reader.Close();
            }
            catch //(Exception exception1)
            {
                //OnClientExceptionEvent(new Exception("连接主认证服务器失败.", exception1));
            }
            if (str == "")
            {
                address = this.kryptotelurlbackup + "/myip.php";
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                try
                {
                    Stream stream2 = client.OpenRead(address);
                    StreamReader reader2 = new StreamReader(stream2);
                    str = reader2.ReadToEnd();
                    stream2.Close();
                    reader2.Close();
                }
                catch// (Exception exception3)
                {
                    //OnClientExceptionEvent(new Exception("连接备份认证服务器失败.", exception3));
                }
            }
            if (str.Length >= 7)
            {
                this.publicIpAddress = str;
                return true;
            }
            else
            {
                this.publicIpAddress = "";
                return false;
            }
        }
    }
}
