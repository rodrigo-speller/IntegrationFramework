# .Net Speller Integration Framework

The ***Integration Framework*** ia an initiative to implement a modular framework of integration patterns, as proposed by *Gregor Hohpe* and *Bobby Woolf* in the *[Enterprise Integration Patterns](https://www.enterpriseintegrationpatterns.com/)* book.

The concern of this project is to maintain a collaborative development of a complete framework that abstracts and simplifies the concrete concepts of key application integration technologies through the definitions and the vocabulary of the *Enterprise Integration Patterns* book.

In an microservice architeture, we need to integrate service communication with one or more integration buses. These buses can be deployed in a variety of different technologies, as well as services. We are working on the challenge of developing a framework that does not limit the use of the target technologies. We uses only the available features in the target technology, avoiding hacks that can result in a coupled component. That way, we can work on a side-by-side architeture with no side effects. For example, in RabbitMQ modules, we don't limit the messaging topology.

# Get the Integration Framework
The quickest way to get the latest release of the Integration Framework is to add it to your project using NuGet:

|Module|Package name|Latest version|
|------|------------|--------------|
|RabbitMQ Publish/Subscribe|Speller.IntegrationFramework.RabbitMQ|[![NuGet](https://img.shields.io/nuget/v/Speller.IntegrationFramework.RabbitMQ.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Speller.IntegrationFramework.RabbitMQ/)|
|RabbitMQ Request/Reply|Speller.IntegrationFramework.RabbitMQ.RequestReply|[![NuGet](https://img.shields.io/nuget/v/Speller.IntegrationFramework.RabbitMQ.RequestReply.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Speller.IntegrationFramework.RabbitMQ.RequestReply/)|

# Samples

## Simple RabbitMQ "Hello World" sample code

In the sender application, we need only resolve the channel, and publish the message to `hello-queue`:

```csharp
public static async Task SayHelloWorld()
{
    services.GetService<IRabbitMQChannel>();

    var message = "Hello World!";
    
    await channel.Publish("hello-queue", message);
}
```

In the receiver application, we declaree a message handler to subscribe the message queue `hello-queue`:

```csharp
[Subscribe("hello-queue")]
[AcknowledgeMode(AcknowledgeMode.Automatic)]
public class Receiver : IMessageHandler<RabbitMQDelivery>
{
    public Task Handle(RabbitMQDelivery message)
    {
        var content = message.AsString();

        Console.WriteLine($"Received: {content}");

        return Task.CompletedTask;
    }
}
```

To see a complete Sender/Receiver sample, go to the *["Hello World" RabbitMQ sample code](samples/Speller.IntegrationPatterns.RabbitMQ/Sample01-HelloWorld)*

## RabbitMQ samples

|Sample|Description|Pattern|
|-|-|-|
|[Hello World](samples/Speller.IntegrationPatterns.RabbitMQ/Sample01-HelloWorld)|Sending and receiving text messages through a queue.|[Point-to-Point Channel](https://www.enterpriseintegrationpatterns.com/patterns/messaging/PointToPointChannel.html)|
|[Work queues](samples/Speller.IntegrationPatterns.RabbitMQ/Sample02-WorkQueues)|Distributing to workers.|[Competing Consumers](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html)|
|[Publish/Subscribe](samples/Speller.IntegrationPatterns.RabbitMQ/Sample03-PublishSubscribe)|Sending messages to many consumers at once.|[Publish-Subscribe Channel](https://www.enterpriseintegrationpatterns.com/patterns/messaging/PublishSubscribeChannel.html)
|[Routing](samples/Speller.IntegrationPatterns.RabbitMQ/Sample04-Routing)|Receiving messages selectively.|[Event Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/EventMessage.html), [Message Router](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageRouter.html)|
|[Topics](samples/Speller.IntegrationPatterns.RabbitMQ/Sample05-Topics)|Receiving messages based on a pattern (topics).|[Event Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/EventMessage.html), [Message Router](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageRouter.html)|
|[Request/Reply](samples/Speller.IntegrationPatterns.RabbitMQ/Sample06-RPC)|RPC/RPI communication.|[Remote Procedure Invocation](https://www.enterpriseintegrationpatterns.com/patterns/messaging/EncapsulatedSynchronousIntegration.html), [Request-Reply](https://www.enterpriseintegrationpatterns.com/patterns/messaging/RequestReply.html), [Command Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CommandMessage.html), [Document Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/DocumentMessage.html), [Correlation Identifier](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CorrelationIdentifier.html), [Return Address](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ReturnAddress.html)|

# Bug Reports
If you find any bug, please report them using the GitHub issue tracker.

# License
This software is licensed under the Apache License, Version 2.0 (see LICENSE.txt).

# Copyright
Copyright (c) Rodrigo Speller. All rights reserved.
