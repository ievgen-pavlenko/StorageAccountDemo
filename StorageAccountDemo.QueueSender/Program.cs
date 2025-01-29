using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StorageAccountDemo.Common;
using StorageAccountDemo.QueueSender;

var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices((hostContext, services) =>
{
    services.Configure<StorageAccountConfiguration>(
        hostContext.Configuration.GetSection("StorageAccountConfiguration"));

    services.AddSingleton<StorageAccountConfiguration>(sp =>
        sp.GetRequiredService<IOptions<StorageAccountConfiguration>>().Value);
    
    services.AddSingleton<QueueSender>();
});

var host = builder.Build();

var queueSender = host.Services.GetRequiredService<QueueSender>();
await queueSender.RunAsync();