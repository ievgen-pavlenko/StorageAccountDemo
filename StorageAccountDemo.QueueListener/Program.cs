using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StorageAccountDemo.Common;
using StorageAccountDemo.QueueListener;

var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices((hostContext, services) =>
{
    services.Configure<StorageAccountConfiguration>(
        hostContext.Configuration.GetSection("StorageAccountConfiguration"));

    services.AddSingleton<StorageAccountConfiguration>(sp =>
        sp.GetRequiredService<IOptions<StorageAccountConfiguration>>().Value);
    
    services.AddSingleton<QueueListener>();
});

var host = builder.Build();

var queueListener = host.Services.GetRequiredService<QueueListener>();
await queueListener.RunAsync();