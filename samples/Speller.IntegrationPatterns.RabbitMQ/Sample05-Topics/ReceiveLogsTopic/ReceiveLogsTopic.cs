using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ;
using System;
using System.Threading.Tasks;

namespace ReceiveLogsTopic
{
    [AcknowledgeMode(AcknowledgeMode.Automatic)]
    public class ReceiveLogsTopic : IMessageHandler<RabbitMQDelivery>
    {
        public Task Handle(RabbitMQDelivery message)
        {
            var body = message.AsString();

            Console.WriteLine(" [x] Received '{0}':'{1}'", message.RoutingKey, body);

            return Task.CompletedTask;
        }
    }
}
