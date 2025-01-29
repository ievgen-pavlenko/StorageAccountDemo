using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using StorageAccountDemo.Common;

namespace StorageAccountDemo;

public class QueueStorageDemo(StorageAccountConfiguration storageAccountConfiguration) : IDemo
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var queueServiceClient = new QueueServiceClient(storageAccountConfiguration.ConnectionString);
        await ListQueuesAsync(queueServiceClient, cancellationToken);

        var queueClient = queueServiceClient.GetQueueClient("demo");
        await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        await ListQueuesAsync(queueServiceClient, cancellationToken);

        await EnqueueMessageAsync(queueClient, cancellationToken);

        await ListMessagesAsync(queueClient, cancellationToken);
        await DequeueMessageAsync(queueClient, cancellationToken);

        await ListMessagesAsync(queueClient, cancellationToken);
    }

    private static async Task DequeueMessageAsync(QueueClient queueClient, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Dequeueing message...");
        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(32, cancellationToken: cancellationToken);

        messages = messages ?? [];
        if (messages.Length == 0)
        {
            Console.WriteLine("No messages to dequeue!");
            return;
        }

        foreach (var message in messages)
        {
            Console.WriteLine($"Message: {message.MessageText}");
            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, cancellationToken);
            Console.WriteLine("Message dequeued!");
        }
    }

    private static async Task ListMessagesAsync(QueueClient queueClient, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Messages:");
        PeekedMessage[] messages = await queueClient.PeekMessagesAsync(32, cancellationToken);
        messages = messages ?? [];
        Console.WriteLine($"Messages count: {messages.Length}");
        foreach (var message in messages)
        {
            Console.WriteLine($"Message: {message.MessageText}");
        }
    }

    private static async Task EnqueueMessageAsync(QueueClient queueClient, CancellationToken cancellationToken = default)
    {
        await queueClient.SendMessageAsync("Hello, Azure!", cancellationToken);
        Console.WriteLine("Message enqueued!");
    }

    private static async Task ListQueuesAsync(QueueServiceClient queueServiceClient,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Queues:");
        await foreach (var queue in queueServiceClient.GetQueuesAsync(cancellationToken: cancellationToken))
        {
            Console.WriteLine(queue.Name);
        }
    }
}