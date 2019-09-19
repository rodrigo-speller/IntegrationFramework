// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal sealed class RabbitMQBusService : IRabbitMQBusService, IHostedService
    {
        private readonly object sync = new object();

        private readonly RabbitMQBusServiceOptions options;
        private readonly LifecycleController controller;

        private IConnection connection;
        private IDictionary<object, IRabbitMQChannel> channels = new Dictionary<object, IRabbitMQChannel>();

        public RabbitMQBusService(RabbitMQBusServiceOptions options)
        {
            this.options = options;
            controller = options.Services.GetService<LifecycleController>();
        }
        
        public IRabbitMQChannel DefaultChannel
            => channels[options.DefaultChannelsTag];

        public IEnumerable<IRabbitMQChannel> Channels => channels.Values;

        public IRabbitMQChannel GetChannel(object tag) => channels[tag];

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var options = this.options;
            var controller = this.controller;
            
            controller.On(LifecycleState.PreInitialization, () => Task.Run(() => {
                connection = options.ConnectionFactory.CreateConnection();
            }));

            controller.On(LifecycleState.CreateChannels, async () => {
                await CreateChannels();
            });

            controller.On(LifecycleState.Shutdown, () => Task.Run(() => {
                connection.Close();
            }));

            await controller.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
            => await controller.Stop();

        private Task CreateChannels() => Task.Run(() =>
        {
            var options = this.options;
            var connection = this.connection;
            var controller = this.controller;

            foreach (var channelOptions in options.Channels)
            {
                var channelModel = connection.CreateModel();

                if (channelOptions.ChannelPrefetchCount.HasValue)
                    channelModel.BasicQos(0, channelOptions.ChannelPrefetchCount.Value, true);

                if (channelOptions.ConsumerPrefetchCount.HasValue)
                    channelModel.BasicQos(0, channelOptions.ConsumerPrefetchCount.Value, false);

                var channel = new RabbitMQChannel(channelOptions, channelModel);

                channels.Add(channel.Tag, channel);

                controller.On(LifecycleState.DeclareEntities, async () =>
                    await DeclareEntities(channel)
                );

                controller.On(LifecycleState.SubscribeQueues, async () =>
                    await SubscribeQueues(channel)
                );
            }
        });

        private async Task DeclareEntities(RabbitMQChannel channel)
        {
            var services = options.Services;
            var channelOptions = channel.Options;
            var channelModel = channel.Model;

            foreach (var exchangeOptions in channelOptions.ExchangesOptions)
            {
                channelModel.ExchangeDeclare(
                    exchangeOptions.Exchange,
                    exchangeOptions.Type,
                    exchangeOptions.Durable,
                    exchangeOptions.AutoDelete,
                    exchangeOptions.Arguments
                );
            }

            foreach (var queueOptions in channelOptions.QueuesOptions)
            {
                var queueDeclaration = channelModel.QueueDeclare(
                    queueOptions.Queue,
                    queueOptions.Durable,
                    queueOptions.Exclusive,
                    queueOptions.AutoDelete,
                    queueOptions.Arguments
                );

                var queue = new RabbitMQQueue(queueOptions, queueDeclaration.QueueName);

                channel.AddQueue(queue);

                services.GetContextAccessor()
                    .Set(channel, queue);

                foreach (var action in queueOptions.OnDeclaredActions)
                    await action(services);
            }
        }

        private async Task SubscribeQueues(RabbitMQChannel channel)
        {
            var services = options.Services;
            var channelOptions = channel.Options;

            services.GetContextAccessor()
                .Set(channel);

            foreach (var subscriberFactory in channelOptions.SubscribersFactories)
                await channel.Subscribe(subscriberFactory);
        }
    }
}
