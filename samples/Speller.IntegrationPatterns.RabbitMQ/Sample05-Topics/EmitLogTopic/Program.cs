using Speller.IntegrationFramework.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmitLogTopic
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

            var channel = host.Services.GetService<IRabbitMQChannel>();

            var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
            var message = (args.Length > 1)
                ? string.Join(" ", args.Skip(1).ToArray())
                : "Hello World!";

            await channel.Publish(message, routingKey: routingKey);
            Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            await host.StopAsync();
        }
    }
}
