using Azure.Storage.Queues;
using StorageAccountDemo.Common;

namespace StorageAccountDemo.QueueSender;

public class QueueSender(StorageAccountConfiguration storageAccountConfiguration) : IDemo
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var queueServiceClient = new QueueServiceClient(storageAccountConfiguration.ConnectionString);
        var queueClient = queueServiceClient.GetQueueClient("demo");
        await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        while (true)
        {
            Console.Write("Press 'q' to quit, 's' to send a message ");
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.KeyChar)
            {
                case 'q':
                    return;
                case 's':
                    await SendMessageAsync(queueClient, cancellationToken);
                    break;
            }
        }
    }

    private static async Task SendMessageAsync(QueueClient queueClient, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Enter message:");
        var message = Console.ReadLine();
        await queueClient.SendMessageAsync(message, cancellationToken: cancellationToken);
        Console.WriteLine("Message sent!");
    }
}