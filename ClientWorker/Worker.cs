using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ClientWorker.Getters;
using ClientWorker.ViewModel;
using ServiceStack.Text;
using System.Text;
using ClientWorker.Helper;

namespace ClientWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
                
        private InformationSent _informationSent;
        private string _mensagem;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _informationSent = InformationGetter.RetornaInformacoes();
            _mensagem = JsonSerializer.SerializeToString(_informationSent);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("Servi�o foi parado.");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
                var Server = new UdpClient(8888);
                var ResponseData = Encoding.UTF8.GetBytes(_mensagem);

                var ClientEp = new IPEndPoint(IPAddress.Any, 0);
                var ClientRequestData = Server.Receive(ref ClientEp);
                var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);
                if(!string.IsNullOrEmpty(ClientRequest))
                {
                    //executar powershell
                    PowerShellManager.PowerShellExecuter(ClientRequest);
                    _logger.LogInformation(ClientRequest);
                    var resposta = "comando executado";                    
                    ResponseData = Encoding.ASCII.GetBytes(resposta);
                }
                Server.Send(ResponseData, ResponseData.Length, ClientEp);
                
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, stoppingToken);
            }
        }



    }
}
