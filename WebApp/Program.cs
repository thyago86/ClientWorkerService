using System;
using System.Net;

namespace WebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
        }

        public void PrintBroadcastAddress()
        {
            // Get the IP Broadcast address and convert it to string.
            string ipAddressString = IPAddress.Broadcast.ToString();
            Console.WriteLine("Broadcast IP address: {0}", ipAddressString);
        }
    }
}
