using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ;
using System;
using System.Threading.Tasks;

namespace Receive
{
    [Subscribe("hello")]
    [AcknowledgeMode(AcknowledgeMode.Automatic)]
    public class Receive : IMessageHandler<RabbitMQDelivery>
    {
        public Task Handle(RabbitMQDelivery message)
        {
            var body = message.AsString();

            Console.WriteLine(" [x] Received {0}", body);

            return Task.CompletedTask;
        }
    }
}
