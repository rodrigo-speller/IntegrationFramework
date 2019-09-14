using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ;
using System;
using System.Threading.Tasks;

namespace ReceiveLogs
{
    [AcknowledgeMode(AcknowledgeMode.Automatic)]
    [ExchangeBinding("logs")]
    public class ReceiveLogs : IMessageHandler<RabbitMQDelivery>
    {
        public Task Handle(RabbitMQDelivery message)
        {
            var body = message.AsString();

            Console.WriteLine(" [x] {0}", body);

            return Task.CompletedTask;
        }
    }
}
