// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ.RequestReply;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ResquestReplyExtensions
    {
        public static RabbitMQChannelOptionsBuilder AddRequestReply(this RabbitMQChannelOptionsBuilder builder)
            => builder.AddRequestReply(AcknowledgeModeAttribute.DefaultMode, ExceptionModeAttribute.DefaultMode);

        public static RabbitMQChannelOptionsBuilder AddRequestReply(this RabbitMQChannelOptionsBuilder builder, AcknowledgeMode acknowledgeMode)
            => builder.AddRequestReply(acknowledgeMode, ExceptionModeAttribute.DefaultMode);

        public static RabbitMQChannelOptionsBuilder AddRequestReply(this RabbitMQChannelOptionsBuilder builder, AcknowledgeMode acknowledgeMode, ExceptionMode exceptionMode)
        {
            RequestReplyController controller = null;

            builder
                .DeclareQueue(queueBuilder => queueBuilder
                    .OnDeclare(async queue => controller = new RequestReplyController(queue.Name))
                    .Subscribe(acknowledgeMode, exceptionMode, delivery => controller.OnDelivery(delivery))
                )
                .Map<RequestReplyModel>(model => {
                    var context = controller.Request(model.SourceContent);

                    model.Context = context;

                    return context.RequestContent;
                });

            return builder;
        }
    }
}
