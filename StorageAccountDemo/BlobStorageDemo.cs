using System.Text;
using Azure.Storage.Blobs;
using StorageAccountDemo.Common;

namespace StorageAccountDemo;

public class BlobStorageDemo(StorageAccountConfiguration storageAccountConfiguration) : IDemo
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var blobServiceClient = new BlobServiceClient(storageAccountConfiguration.ConnectionString);
        await ListContainers(blobServiceClient, cancellationToken);

        var blobContainerClient = blobServiceClient.GetBlobContainerClient("demo");
        await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        await ListContainers(blobServiceClient, cancellationToken);

        await ListBlobsAsync(blobContainerClient, cancellationToken);

        await UploadBlobAsync(blobContainerClient, cancellationToken);

        await ListBlobsAsync(blobContainerClient, cancellationToken);
        
        await ReadBlobsDataAsync(blobContainerClient, cancellationToken);
    }

    private async Task ReadBlobsDataAsync(BlobContainerClient blobContainerClient, CancellationToken cancellationToken = default)
    {
        var blobs = blobContainerClient.GetBlobsAsync();
        await foreach(var blob in blobs)
        {
            Console.WriteLine($"Blob name: {blob.Name}");
            var blobClient = blobContainerClient.GetBlobClient(blob.Name);
            var response = await blobClient.DownloadAsync(cancellationToken);
            var data = await new StreamReader(response.Value.Content).ReadToEndAsync(cancellationToken);
            Console.WriteLine($"Blob content: {data}");
        }
    }

    private async Task ListBlobsAsync(BlobContainerClient blobContainerClient, CancellationToken cancellationToken = default)
    {
        var containerName = blobContainerClient.Name;
        Console.WriteLine($"Blobs in container '{containerName}':");
        var blobs = blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken);
        Console.WriteLine("Blobs:");
        await foreach (var blob in blobs)
        {
            Console.WriteLine(blob.Name);
        }
    }


    private async Task UploadBlobAsync(BlobContainerClient blobContainerClient, CancellationToken cancellationToken = default)
    {
        var blobClient = blobContainerClient.GetBlobClient("test.txt");
        var data = Encoding.UTF8.GetBytes("Hello, Azure!");
        var stream = new MemoryStream(data);
        await blobClient.UploadAsync(stream, true, cancellationToken);
        Console.WriteLine("Blob uploaded!");
    }


    private static async Task ListContainers(BlobServiceClient blobServiceClient,
        CancellationToken cancellationToken = default)
    {
        var list = blobServiceClient.GetBlobContainersAsync(cancellationToken: cancellationToken);
        Console.WriteLine("Containers:");
        await foreach (var container in list)
        {
            Console.WriteLine(container.Name);
        }
    }
}