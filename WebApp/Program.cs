using ClientWorker.ViewModel;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Iniciando Terminal");            
            var Client = new UdpClient();
            var ip = WorkerManager.ConectarAoWorker(Client);
            Console.WriteLine("Insira seu comando");            
            var comando = Console.ReadLine();             
            WorkerManager.EnviarComando(ip, comando);
        }

        

        
    }
}
