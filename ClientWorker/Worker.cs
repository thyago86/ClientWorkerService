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
using System.Net.Http;

namespace ClientWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("Servi�o foi parado.");
            return base.StopAsync(cancellationToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string nomeMaquina ="Nome da m�quina: " + Environment.MachineName;
                string ip = "N�o conectado";
                string versaoNet =InformationGetter.GetNetVersion();
                string versaoWindows ="Vers�o do Windows: " + InformationGetter.VersaoWindows();
                string antivirus ="Nome do Antiv�rus: " + InformationGetter.NomeAntivirus();
                string firewall = InformationGetter.InformacaoFirewall();
                bool conectado = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                if (conectado)
                {
                    ip ="Endere�o de IP: "+ InformationGetter.GetLocalIPAddress();
                }

                //ao iniciar, aplica��o deve se registrar ao servi�o web e informar a ela essas informa��es
                //o service que busca o 

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                await Task.Delay(60*1000, stoppingToken);
            }
        }

        

    }
}
