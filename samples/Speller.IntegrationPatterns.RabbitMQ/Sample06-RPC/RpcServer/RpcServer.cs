using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ;
using Speller.IntegrationFramework.RabbitMQ.Content;
using System;
using System.Threading.Tasks;

namespace RpcServer
{
    [Subscribe("rpc_queue")]
    public class RpcServer : IMessageHandler<RabbitMQDelivery>
    {
        public RpcServer(IRabbitMQChannel channel)
        {
            Channel = channel;
        }

        public IRabbitMQChannel Channel { get; }

        public async Task Handle(RabbitMQDelivery message)
        {
            var body = message.AsString();
            var n = int.Parse(body);

            Console.WriteLine(" [.] fib({0})", body);

            IMessageContent response;
            try
            {
                n = fib(n);
                response = new StringContent(n.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(" [.] " + e.Message);
                response = new StringContent("");
            }

            response.Properties.CorrelationId = message.CorrelationId;

            await Channel.Publish(response, routingKey: message.ReplyTo);
            await message.Acknowledge();
        }

        ///
        /// Assumes only valid positive integer input.
        /// Don't expect this one to work for big numbers, and it's
        /// probably the slowest recursive implementation possible.
        ///

        private static int fib(int n)
        {
            if (n == 0 || n == 1)
                return n;

            return fib(n - 1) + fib(n - 2);
        }
    }
}
