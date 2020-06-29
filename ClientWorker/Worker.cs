using System;
using System.Collections.Generic;
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
using ClientWorker.Getters;

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
                string versaoNet ="Vers�o do .NET: " + InformationGetter.GetNetVersion();
                string versaoWindows ="Vers�o do Windows: " + InformationGetter.VersaoWindows();
                string antivirus ="Nome do Antiv�rus: " + InformationGetter.NomeAntivirus();
                string firewall = InformationGetter.InformacaoFirewall();
                bool conectado = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                if (conectado)
                {
                    ip ="Endere�o de IP: "+ InformationGetter.GetLocalIPAddress();
                }

                //ao iniciar, aplica��o deve se registrar ao servi�o web e informar a ela essas informa��es

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        

    }
}
