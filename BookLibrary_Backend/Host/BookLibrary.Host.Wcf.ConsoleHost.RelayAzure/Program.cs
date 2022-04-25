using System;
using System.ServiceModel.Web;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Bootstrapper;
using BookLibrary.Business.Services.Managers;
using BookLibrary.DataAccess.SQLite;

namespace BookLibrary.Host.Wcf.ConsoleHost.RelayAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Database if not exists or if it is old version.
            CreateInitialDatabase.Initialize();
            // Build IoC container.
            BootContainer.Builder = Bootstrapper.LoadContainer;

            // If Unattended Process need Principal Permission Security!?
            // Then we should define it here

            var bookHost = new WebServiceHost(typeof(BookManager));
            var categoryHost = new WebServiceHost(typeof(CategoryManager));
            var borrowerHost = new WebServiceHost(typeof(BorrowerManager));

            StartHost(bookHost, nameof(BookManager));
            StartHost(categoryHost, nameof(CategoryManager));
            StartHost(borrowerHost, nameof(BorrowerManager));

            // Unattended process place here (Timer Start) ...

            Console.WriteLine("");
            Console.WriteLine("Press [Enter] to close app.");
            Console.ReadLine();
            Console.WriteLine("");


            // Timer Stop for Unattended process ...
            StopHost(bookHost, nameof(BookManager));
            StopHost(categoryHost, nameof(CategoryManager));
            StopHost(borrowerHost, nameof(BorrowerManager));
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
