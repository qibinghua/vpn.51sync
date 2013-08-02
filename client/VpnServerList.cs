using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VPN51SYNC
{
    public class VpnServerList
    {
        public List<VpnServer> FreeVpnServers
        {
            get
            {
                if (_vpnServers == null)
                {
                    _vpnServers = new List<VpnServer>();
                    _vpnServers.Add(new VpnServer("随机国家或地区", "TINY-VPN-FREE", "ip-free.vpnoneclick.com"));
                }
                return _vpnServers;
            }
        }
        public static List<VpnServer> _vpnServers;
        public List<VpnServer> VpnServers
        {
            get
            {
                if (_vpnServers == null)
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("http://vpn.51sync.com/servers.php?"+DateTime.Now.ToLongTimeString());
                    StreamReader reader = new StreamReader(stream);
                    string serverString = reader.ReadToEnd();
                    JObject ja = (JObject)JsonConvert.DeserializeObject(serverString);
                    _vpnServers = new List<VpnServer>();
                    foreach (JObject server in ja["list"])
                    {
                        //string desc = server["desc"].ToString();
                        _vpnServers.Add(new VpnServer(server["desc"].ToString(), server["name"].ToString(), server["domain"].ToString()));
                    }
                    
                    
                    /*
                    _vpnServers.Add(new VpnServer("随机国家或地区", "TINY-VPN-RND", "ip-rnd.vpnoneclick.com"));//随机国家或地区
                    _vpnServers.Add(new VpnServer("澳大利亚", "TINY-VPN-AU", "ip-au.vpnoneclick.com"));//澳大利亚
                    _vpnServers.Add(new VpnServer("加拿大", "TINY-VPN-CA", "ip-ca.vpnoneclick.com"));//加拿大
                    _vpnServers.Add(new VpnServer("埃及", "TINY-VPN-EG", "ip-eg.vpnoneclick.com"));//埃及
                    _vpnServers.Add(new VpnServer("法国", "TINY-VPN-FR", "ip-fr.vpnoneclick.com"));//法国
                    _vpnServers.Add(new VpnServer("德国", "TINY-VPN-DE", "ip-de.vpnoneclick.com"));//德国
                    _vpnServers.Add(new VpnServer("香港", "TINY-VPN-HK", "ip-hk.vpnoneclick.com"));//香港
                    _vpnServers.Add(new VpnServer("印度", "TINY-VPN-IN", "ip-in.vpnoneclick.com"));//印度
                    _vpnServers.Add(new VpnServer("意大利", "TINY-VPN-IT", "ip-it.vpnoneclick.com"));//意大利
                    _vpnServers.Add(new VpnServer("荷兰", "TINY-VPN-NL", "ip-nl.vpnoneclick.com"));//荷兰
                    _vpnServers.Add(new VpnServer("俄罗斯", "TINY-VPN-RU", "ip-ru.vpnoneclick.com"));//俄罗斯
                    _vpnServers.Add(new VpnServer("新加坡", "TINY-VPN-SG", "ip-sg.vpnoneclick.com"));//新加坡
                    _vpnServers.Add(new VpnServer("西班牙", "TINY-VPN-SP", "ip-sp.vpnoneclick.com"));//西班牙
                    _vpnServers.Add(new VpnServer("瑞典", "TINY-VPN-SE", "ip-se.vpnoneclick.com"));//瑞典
                    _vpnServers.Add(new VpnServer("瑞士", "TINY-VPN-CH", "ip-ch.vpnoneclick.com"));//瑞士
                    _vpnServers.Add(new VpnServer("土耳其", "TINY-VPN-TK", "ip-tk.vpnoneclick.com"));//土耳其
                    _vpnServers.Add(new VpnServer("英国", "TINY-VPN-UK", "ip-uk.vpnoneclick.com"));//英国
                    _vpnServers.Add(new VpnServer("美国", "TINY-VPN-US", "ip-us.vpnoneclick.com"));//美国
                    */
                }
                return _vpnServers;
            }
        }
    }
    public class VpnServer
    {

        public string UIName { get; set; }
        public KeyValuePair<string, string> Server { get; set; }
        public VpnServer(string uiName, string displayName, string domain)
        {
            UIName = uiName;
            Server = new KeyValuePair<string, string>(displayName, domain);
        }
    }
}
