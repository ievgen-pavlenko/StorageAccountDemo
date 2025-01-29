using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using StorageAccountDemo.Common;

namespace StorageAccountDemo.QueueListener;

public class QueueListener(StorageAccountConfiguration storageAccountConfiguration) : IDemo
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var queueServiceClient = new QueueServiceClient(storageAccountConfiguration.ConnectionString);
        var queueClient = queueServiceClient.GetQueueClient("demo");
        await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        while (true)
        {
            Console.WriteLine("Waiting for messages...");
            QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(
                maxMessages: 1, cancellationToken: cancellationToken
            );

            if (messages.Length > 0)
            {
                var message = messages[0];
                Console.WriteLine($"Message: {message.MessageText}");
                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, cancellationToken);
                Console.WriteLine("Message deleted!");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }
}