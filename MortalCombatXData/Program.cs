using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MortalCombatXData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Attempting to host the data tier service...");

            ServiceHost host = new ServiceHost(typeof(DataServer));
            NetTcpBinding tcp = new NetTcpBinding();

            host.AddServiceEndpoint(typeof(DataServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataService");

            // Open host
            host.Open();
            Console.WriteLine("Successful. Data tier is now online.");
            Console.ReadLine();

            // Close host when finished
            host.Close();
        }
    }
}
