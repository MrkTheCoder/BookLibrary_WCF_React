using System;
using System.ServiceModel.Web;
using BookLibrary.Business.Services.Managers;

namespace BookLibrary.Host.Wcf.ConsoleHost.RelayAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            // If Unattended Process need Principal Permission Security!?
            // Then we should define it here

            var hostWeb = new WebServiceHost(typeof(BookManager));

            StartHost(hostWeb, nameof(BookManager));

            // Unattended process place here (Timer Start) ...

            Console.WriteLine("");
            Console.WriteLine("Press [Enter] to close app.");
            Console.ReadLine();
            Console.WriteLine("");


            // Timer Stop for Unattended process ...
            StopHost(hostWeb, nameof(BookManager));
        }

        private static void StartHost(WebServiceHost host, string serviceName)
        {
            host.Open();
            Console.WriteLine($"Service: '{serviceName}' is started.");
            foreach (var endpoint in host.Description.Endpoints)
            {
                Console.WriteLine($"\tAddress:\t{endpoint.Address}");
                Console.WriteLine($"\tBinding:\t{endpoint.Binding?.Name}");
                Console.WriteLine($"\tContract:\t{endpoint.Contract.ConfigurationName}");
            }

            Console.WriteLine("");
        }

        private static void StopHost(WebServiceHost host, string serviceName)
        {
            host.Close();
            Console.WriteLine($"Web Service: '{serviceName}' is stop.");
        }

    }
}
