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

namespace ClientWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        
        private UdpClient UdpClient = new UdpClient(8888);
        private IPEndPoint endPoint;
        private InformationSent informationSent;
        private string mensagem;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {            
            informationSent = InformationGetter.RetornaInformacoes();
            mensagem = JsonSerializer.SerializeToString(informationSent);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("Serviço foi parado.");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {


                //ao iniciar, aplicação deve se registrar ao serviço web e informar a ela essas informações

                //o service que busca o web app e manda informações do PC que está instalado

                informationSent = InformationGetter.RetornaInformacoes();

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                await Task.Delay(60*1000, stoppingToken);
            }
        }

        

    }
}
