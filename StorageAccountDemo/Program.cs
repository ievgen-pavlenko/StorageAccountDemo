using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StorageAccountDemo;
using StorageAccountDemo.Common;

var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices((hostContext, services) =>
{
    services.Configure<StorageAccountConfiguration>(
        hostContext.Configuration.GetSection("StorageAccountConfiguration"));

    services.AddSingleton<StorageAccountConfiguration>(sp =>
        sp.GetRequiredService<IOptions<StorageAccountConfiguration>>().Value);

    services.AddSingleton<BlobStorageDemo>();
    services.AddSingleton<TableStorageDemo>();
    services.AddSingleton<QueueStorageDemo>();
});

var host = builder.Build();

var blobStorageDemo = host.Services.GetRequiredService<BlobStorageDemo>();
await blobStorageDemo.RunAsync();

var tableStorageDemo = host.Services.GetRequiredService<TableStorageDemo>();
await tableStorageDemo.RunAsync();

var queueStorageDemo = host.Services.GetRequiredService<QueueStorageDemo>();
await queueStorageDemo.RunAsync();


