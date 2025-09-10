using MortalCombatXData;
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
            NetTcpBinding tcp = new NetTcpBinding();

            host.AddServiceEndpoint(typeof(GamingLobbyServerInterface), tcp, "net.tcp://0.0.0.0:8200/GamingLobbyServer");

            // Open host
            host.Open();
            Console.WriteLine("Successful. GamingLobbyServer is now online.");
            Console.ReadLine();

            // Close host when finished
            host.Close();
        }
    }
}
