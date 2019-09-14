using Speller.IntegrationFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace NewTask
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
                Run(host, args)
                    .GetAwaiter()
                    .GetResult();
        }

        private static async Task Run(IHost host, string[] args)
        {
            await host.StartAsync();

            var channel = host.Services.GetService<IPublishSubscribeChannel>();

            var message = GetMessage(args);
            await channel.Publish(message);
            Console.WriteLine(" [x] Sent {0}", message);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            await host.StopAsync();
        }

        private static string GetMessage(string[] args)
            => (args.Length > 0)
                ? string.Join(" ", args)
                : "Hello World!";
    }
}
