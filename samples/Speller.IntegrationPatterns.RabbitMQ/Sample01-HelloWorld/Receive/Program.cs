using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Receive
{
    public class Program
    {
        private static IHost BuildHost()
            => new HostBuilder()
                .ConfigureServices(Startup.ConfigureServices)
                .Build();

        public static void Main(string[] args)
        {
            using (var host = BuildHost())
                Run(host)
                    .GetAwaiter()
                    .GetResult();
        }

        private static async Task Run(IHost host)
        {
            await host.StartAsync();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            await host.StopAsync();
        }
    }
}
