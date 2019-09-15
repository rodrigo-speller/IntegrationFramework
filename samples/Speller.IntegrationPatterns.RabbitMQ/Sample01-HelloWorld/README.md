# RabbitMQ "Hello World" Sample Projects

In this sample, we have a message publisher (sender) - ` Send.csproj` - and a message consumer (receiver) - `Receive.csproj`.

The publisher will connect to RabbitMQ, send a single message, then exit.

As for the consumer, it listening for messages from RabbitMQ. So unlike the publisher which publishes a single message, we'll keep the consumer running continuously to listen for messages and print them out.

This sample is based on [RabbitMQ tutorial - "Hello World"][rabbitmqtutorial1]. Visit the original RabbitMQ tutorial for detailed concepts.

## Prerequisites

This sample assumes that:

* RabbitMQ is installed and running on localhost on standard port (5672). In case you use a different host, port or credentials, connections settings would require adjusting.

* You understands [Generic Host][msdocs:generic-host] and [Hosted Service][msdocs:hosted-service] concepts, it's great.

## Sending

The most important thing in this example is the `Startup.ConfigureServices` method. When it's called, it will register and configure the *RabbitMQ Bus Service* in the services container.

*The RabbitMQ Bus Service is registered as a [Hosted Service][msdocs:hosted-service].*

While configuring the bus service, we defines the connection configuration and the default channel configuration - in this sample we'll uses only the default channel.

In the channel configuration, we declares the `hello` queue, and routes all the `string` typed messages from this channel to this queue.

```csharp
internal static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
        => services
            .AddRabbitMQBusService(
                service => service
                    .ConnectionString("amqp://localhost")
                    .DefaultChannel(
                        channel => channel
                            .DeclareQueue("hello", exclusive: false)
                            .MapRoute<string>("hello")
                    )
            );
}
```

We send the message through the default channel. To resolve the default channel, only get it from the services provider.

Our message type is `string`, it will be automatically routed as configured for this channel.

```csharp
var channel = services.GetService<IPublishSubscribeChannel>();

var message = "Hello World!";
await channel.Publish(message);
```

To run the sender application, in the *`samples\Speller.IntegrationPatterns.RabbitMQ\Sample01-HelloWorld`* directory, call:

```bash
dotnet run Send
```

## Receiving

The receiver's `Startup.ConfigureServices` is similiar to the sender's, but, as we'll not send any message, we don't need to register any routes. Instead, let's register the subscriber to the `hello` queue.

```csharp
internal static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
        => services
            .AddRabbitMQBusService(
                service => service
                    .ConnectionString("amqp://localhost")
                    .DefaultChannel(
                        channel => channel
                            .DeclareQueue("hello", exclusive: false)
                            .Subscribe<Receive>()
                    )
            );
}
```

In the Integration Framework for RabbitMQ, a subscriber may be defined deriving from `IMessageHandler<RabbitMQDelivery>` interface.

The Integration Framework also provides some attributes that can be used to configure message subscribers, this makes setup easier and readable. At this point, we uses the `SubscribeAttribute` to subscribe the `hello` queue, and `AcknowledgeModeAttribute` to auto-acknowledge the deliveries.

```csharp
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
```

To run the receiver application, in the *`samples\Speller.IntegrationPatterns.RabbitMQ\Sample01-HelloWorld`* directory, call:

```bash
dotnet run Receive
```

# Integration Patterns Reference

* [Point-to-Point Channel]
* [Document Message]

<!-- RabbitMQ Tutorials Links -->
[rabbitmqtutorial1]: https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html

<!-- Microsoft Documentation Links -->
[msdocs:generic-host]:      https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.2
[msdocs:hosted-service]:    https://docs.microsoft.com/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2

<!-- Enterprise Integration Patterns Links-->
[Enterprise Integration Patterns]:  https://www.enterpriseintegrationpatterns.com/
[Document Message]:                 https://www.enterpriseintegrationpatterns.com/patterns/messaging/DocumentMessage.html
[Point-to-Point Channel]:           https://www.enterpriseintegrationpatterns.com/patterns/messaging/PointToPointChannel.html