using BookLibrary.Business.Services.Managers;
using System.ServiceModel;
using BookLibrary.Business.AppConfigs;
using BookLibrary.Business.Bootstrapper;

namespace BookLibrary.Hot.Wcf.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // If Unattended Process need Principal Permission Security!?
            // Then we should define it here

            
            // Build IoC container.
            BootContainer.Builder = Bootstrapper.LoadContainer;

            var bookHost = new ServiceHost(typeof(BookManager));
            var categoryHost = new ServiceHost(typeof(CategoryManager));

            StartHost(bookHost, nameof(BookManager));
            StartHost(categoryHost, nameof(CategoryManager));

            // Unattended process place here (Timer Start) ...

            System.Console.WriteLine("");
            System.Console.WriteLine("Press [Enter] to close app.");
            System.Console.ReadLine();
            System.Console.WriteLine("");

            // Timer Stop for Unattended process ...
            StopHost(bookHost, nameof(BookManager));
            StopHost(categoryHost, nameof(CategoryManager));
        }

        private static void StartHost(ServiceHost host, string serviceName)
        {
            host.Open();
            System.Console.WriteLine($"Service: '{serviceName}' is started.");
            foreach (var endpoint in host.Description.Endpoints)
            {
                System.Console.WriteLine($"\tAddress:\t{endpoint.Address}");
                System.Console.WriteLine($"\tBinding:\t{endpoint.Binding?.Name}");
                System.Console.WriteLine($"\tContract:\t{endpoint.Contract.ConfigurationName}");
            }

            System.Console.WriteLine("");
        }

        private static void StopHost(ServiceHost host, string serviceName)
        {
            host.Close();
            System.Console.WriteLine($"Service: '{serviceName}' is stop.");
        }

    }
}
