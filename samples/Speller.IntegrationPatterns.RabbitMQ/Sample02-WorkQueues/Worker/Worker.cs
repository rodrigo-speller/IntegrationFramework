using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Worker
{
    [Subscribe("task_queue")]
    public class Worker : IMessageHandler<RabbitMQDelivery>
    {
        public async Task Handle(RabbitMQDelivery message)
        {
            var body = message.AsString();

            Console.WriteLine(" [x] Received {0}", body);

            int dots = body.Split('.').Length - 1;
            Thread.Sleep(dots * 1000);

            Console.WriteLine(" [x] Done");

            await message.Acknowledge();
        }
    }
}
