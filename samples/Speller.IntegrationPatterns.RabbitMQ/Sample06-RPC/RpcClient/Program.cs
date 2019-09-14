using Speller.IntegrationFramework.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace RpcClient
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

            var channel = host.Services.GetService<IRabbitMQChannel>();

            Console.WriteLine(" [x] Requesting fib(30)");
            
            var response = channel.Request("30", routingKey: "rpc_queue")
                .GetAwaiter()
                .GetResult()
                .AsString();

            Console.WriteLine(" [.] Got '{0}'", response);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            await host.StopAsync();
        }
    }
}
