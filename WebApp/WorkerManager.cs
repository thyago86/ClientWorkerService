﻿using ClientWorker.ViewModel;
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
        public static void ConectarAoWorker(UdpClient client)
        {

            var RequestData = Encoding.UTF8.GetBytes("");
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);

            client.EnableBroadcast = true;
            client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            var ServerResponseData = client.Receive(ref ServerEp);
            var ServerResponse = Encoding.UTF8.GetString(ServerResponseData);

            Console.WriteLine("Worker Service do ip {1} respondeu: {0}", ServerResponse, ServerEp.Address.ToString());
        }



        public static void EnviarComando(UdpClient client, byte[] comando)
        {
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);
            client.EnableBroadcast = true;
            client.Send(comando, comando.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
            var ServerResponseData = client.Receive(ref ServerEp);
            var ServerResponse = Encoding.UTF8.GetString(ServerResponseData);
            var printmensagem = JsonSerializer.DeserializeFromString<InformationSent>(ServerResponse);
            Console.WriteLine("{0}", printmensagem);
            client.Close();
        }
    }
}
