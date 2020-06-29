using ClientWorker.ViewModel;
using Microsoft.Win32;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientWorker.Getters
{
    public static class InformationGetter
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        //fonte: https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
        public static string GetNetVersion()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    return $"Versão do .NET Framework: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}";
                }
                else
                {
                    return ".NET Framework Version 4.5 ou anterior não detectada.";
                }
            }

            // Checking the version using >= enables forward compatibility.
            string CheckFor45PlusVersion(int releaseKey)
            {
                if (releaseKey >= 528040)
                    return "4.8 ou posterior";
                if (releaseKey >= 461808)
                    return "4.7.2";
                if (releaseKey >= 461308)
                    return "4.7.1";
                if (releaseKey >= 460798)
                    return "4.7";
                if (releaseKey >= 394802)
                    return "4.6.2";
                if (releaseKey >= 394254)
                    return "4.6.1";
                if (releaseKey >= 393295)
                    return "4.6";
                if (releaseKey >= 379893)
                    return "4.5.2";
                if (releaseKey >= 378675)
                    return "4.5.1";
                if (releaseKey >= 378389)
                    return "4.5";
                // This code should never execute. A non-null release key should mean
                // that 4.5 or later is installed.
                return "No 4.5 ou mais antiga detectada";
            }
        }

        public static string VersaoWindows()
        {
            string versao = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                ManagementObjectCollection information = searcher.Get();
                if (information != null)
                {
                    foreach (ManagementObject obj in information)
                    {
                        versao = obj["Caption"].ToString() + " - " + obj["OSArchitecture"].ToString();
                    }
                }
                versao = versao.Replace("NT 5.1.2600", "XP");
                versao = versao.Replace("NT 5.2.3790", "Server 2003");
                return versao;
            }
        }

        public static string NomeAntivirus()
        {
            ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectCollection data = wmiData.Get();
            string retorno = "";
            foreach (ManagementObject management in data)
            {
                retorno = management["displayName"].ToString();
            }
            return retorno;
        }

        public static List<HDDInformation> InformacaoDrivers()
        {
            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            HDDInformation information = new HDDInformation();
            List<HDDInformation> hDDInformation = new List<HDDInformation>();
            foreach (var item in driveInfo)
            {
                information.Name = item.Name;
                information.DriveFormat = item.DriveFormat;
                information.DriveType = item.DriveType;
                information.VolumeLabel = item.VolumeLabel;
                information.AvailableFreeSpace = item.AvailableFreeSpace;
                information.TotalSize = item.TotalSize;
                hDDInformation.Add(information);
            }
            return hDDInformation;
        }

        public static string InformacaoFirewall()
        {

            try
            {

                Type tpNetFirewall = Type.GetTypeFromProgID
                   ("HNetCfg.FwMgr", false);

                INetFwMgr mgrInstance = (INetFwMgr)Activator
                   .CreateInstance(tpNetFirewall);

                bool blnEnabled = mgrInstance.LocalPolicy
                   .CurrentProfile.FirewallEnabled;

                mgrInstance = null;

                tpNetFirewall = null;

                if (blnEnabled)
                    return "Firewall ativo";
                return "Firewall inativo";

            }
            catch (Exception e)
            {

                return "";

            }
        }
    }
}
