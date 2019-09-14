using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ReceiveLogsDirect
{
    public class Program
    {
        private static IHost BuildHost(string[] severities)
            => new HostBuilder()
                .ConfigureServices(new Startup(severities).ConfigureServices)
                .Build();

        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine(
                    "Usage: {0} [info] [warning] [error]",
                    Environment.GetCommandLineArgs()[0]
                );
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
                Environment.ExitCode = 1;
                return;
            }
            
            using (var host = BuildHost(args))
                Run(host)
                    .GetAwaiter()
                    .GetResult();
        }

        private static async Task Run(IHost host)
        {
            await host.StartAsync();

            Console.WriteLine(" [*] Waiting for messages.");
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            await host.StopAsync();
        }
    }
}
