using ClientWorker.ViewModel;
using ServiceStack.Text;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BroadcastToWorker();
        }

        public static void BroadcastToWorker()
        {
            var Client = new UdpClient();
            var RequestData = Encoding.ASCII.GetBytes("SomeRequestData");
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);

            Client.EnableBroadcast = true;
            Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            var ServerResponseData = Client.Receive(ref ServerEp);
            var ServerResponse = Encoding.UTF8.GetString(ServerResponseData);
            var printmensagem = JsonSerializer.DeserializeFromString<InformationSent>(ServerResponse);
            Console.WriteLine("Recived {0} from {1}", ServerResponse, ServerEp.Address.ToString());

            Client.Close();
        }

        
    }
}
