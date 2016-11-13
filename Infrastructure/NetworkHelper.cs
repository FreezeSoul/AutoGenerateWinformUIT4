using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class NetworkHelper
    {
        public static string GetLocalIp()
        {
            string ip = string.Empty;
            var hostname = System.Net.Dns.GetHostName();
            var localhost = System.Net.Dns.GetHostEntry(hostname);
            ip = localhost.AddressList[0].ToString();
            return ip;
        }
    }
}
