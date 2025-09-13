using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GamingLobbyServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Attempting to host the Gaming Lobby Server service...");

            ServiceHost host = new ServiceHost(typeof(GamingLobbyServer));
            ServiceHost hostDuplex = new ServiceHost(typeof(GamingLobbyServerDuplex));
            NetTcpBinding tcp = new NetTcpBinding();

            host.AddServiceEndpoint(typeof(GamingLobbyServerInterface), tcp, "net.tcp://0.0.0.0:8200/GamingLobbyService");
            hostDuplex.AddServiceEndpoint(typeof(GamingLobbyServerDuplexInterface), tcp, "net.tcp://0.0.0.0:8201/GamingLobbyDuplexService");

            // Open host
            host.Open();
            hostDuplex.Open();
            Console.WriteLine("Successful. Business Tier (GamingLobbyServer) is now online.");
            Console.ReadLine();

            // Close host when finished
            host.Close();
            hostDuplex.Close();
        }
    }
}
