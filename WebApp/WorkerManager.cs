using ClientWorker.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServiceStack.Text;

namespace WebApp
{
    public class WorkerManager
    {
        
        public static string ConectarAoWorker(UdpClient client)
        {
            var RequestData = Encoding.UTF8.GetBytes("");
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);

            client.EnableBroadcast = true;
            client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            var ServerResponseData = client.Receive(ref ServerEp);
            
            var ServerResponse = Encoding.UTF8.GetString(ServerResponseData);

            Console.WriteLine("Worker Service do ip {1} respondeu: {0}", ServerResponse, ServerEp.Address.ToString());
            client.Close();
            return ServerEp.Address.ToString();
        }
        

        public static void EnviarComando(string clientIP, string comando)
        {
            try
            {
                if(string.IsNullOrEmpty(clientIP))
                    return;
                TcpClient client = new TcpClient(clientIP, 8887);
                Byte[] data = Encoding.ASCII.GetBytes(comando);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", comando);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
                //var ServerEp = new IPEndPoint(IPAddress.Any, 0);
                //UdpClient client = new UdpClient();

                //var cmd = Encoding.UTF8.GetBytes(comando);
                //client.Send(cmd, cmd.Length, new IPEndPoint(IPAddress.Parse(clientIP), 8888));
                //var ServerResponseData = client.Receive(ref ServerEp);
                //var ServerResponse = Encoding.UTF8.GetString(ServerResponseData);

                //Console.WriteLine(ServerResponse);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
