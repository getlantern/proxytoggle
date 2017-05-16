using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ProxyToggle
{

    class Program
    {

        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;

        private const string userRoot = "HKEY_CURRENT_USER";
        private const string subkey = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
        private const string keyName = userRoot + "\\" + subkey;


        static void setProxy(string proxyhost, bool proxyEnabled)
        {

            Registry.SetValue(keyName, "ProxyServer", proxyhost);
            Registry.SetValue(keyName, "ProxyOverride", "<local>");
            Registry.SetValue(keyName, "ProxyEnable", proxyEnabled ? 1 : 0);

            // These lines implement the Interface in the beginning of program 
            // They cause the OS to refresh the settings, causing the IP to realy update
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Usage: on/off host port or show");
            }
            else if (args[0] == "off")
            {
                setProxy("", false);
            }
            else if (args[0] == "on")
            {
                setProxy(args[1]+":"+args[2], true);
            }
            else if (args[0] == "show")
            {
                string proxyServer = (string)Registry.GetValue(keyName, "ProxyServer", "");
                Console.WriteLine(proxyServer);
            }
        }
    }
}




