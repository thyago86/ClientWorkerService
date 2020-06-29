using System;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Common;
using Microsoft.Win32;
using NetFwTypeLib;

namespace ClientWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string nomeMaquina ="Nome da m�quina: " + Environment.MachineName;
                string ip = "N�o conectado";
                string versaoNet ="Vers�o do .NET: " + GetNetVersion();
                string versaoWindows ="Vers�o do Windows: " + VersaoWindows();
                string antivirus ="Nome do Antiv�rus: " + NomeAntivirus();
                string firewall = GetFirewallActivated();
                bool conectado = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                if (conectado)
                {
                    ip = GetLocalIPAddress();
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private static string GetLocalIPAddress()
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



        private static string GetNetVersion()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    return $"Vers�o do .NET Framework: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}";
                }
                else
                {
                    return ".NET Framework Version 4.5 ou anterior n�o detectada.";
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

        private string VersaoWindows()
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

        private string NomeAntivirus()
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

        private static void EspacoLivre()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }
        }

        private static string GetFirewallActivated()
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

                if(blnEnabled)
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
