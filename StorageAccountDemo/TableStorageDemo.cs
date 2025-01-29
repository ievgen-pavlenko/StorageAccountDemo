using Azure;
using Azure.Data.Tables;
using StorageAccountDemo.Common;

namespace StorageAccountDemo;

public class TableStorageDemo(StorageAccountConfiguration storageAccountConfiguration) : IDemo
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var tableServiceClient = new TableServiceClient(storageAccountConfiguration.ConnectionString);
        var tableClient = tableServiceClient.GetTableClient("demo");
        await tableClient.CreateIfNotExistsAsync(cancellationToken);

        await InsertEntityAsync(tableClient, cancellationToken);
        await InsertPersonDtoAsync(tableClient, cancellationToken);
        await InsertPedDtoAsync(tableClient, cancellationToken);
        await QueryAllEntitiesAsync(tableClient, cancellationToken);
        await QueryAllPeopleAsync(tableClient, cancellationToken);
    }

    private async Task QueryAllPeopleAsync(TableClient tableClient, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Querying all People entities...");
        var entities = tableClient.QueryAsync<PersonDto>(x => x.PartitionKey == "Person");
        await foreach (var entity in entities)
        {
            Console.WriteLine($"Entity: {entity.Name} - {entity.Age}");
        }

        Console.WriteLine("Querying all People entities done!");
    }

    private async Task QueryAllEntitiesAsync(TableClient tableClient, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Querying all entities...");
        var entities = tableClient.QueryAsync<TableEntity>();
        await foreach (var entity in entities)
        {
            Console.WriteLine($"Entity: {entity["Name"]} - {entity["Age"]}");
        }

        Console.WriteLine("Querying all entities done!");
    }

    private async Task InsertPedDtoAsync(TableClient tableClient, CancellationToken cancellationToken = default)
    {
        var dto = new PetDto
        {
            Name = "Fluffy",
            Age = 3
        };

        await tableClient.AddEntityAsync(dto, cancellationToken);
        Console.WriteLine("Dto inserted!");
    }

    private async Task InsertPersonDtoAsync(TableClient tableClient, CancellationToken cancellationToken = default)
    {
        var dto = new PersonDto
        {
            Name = "Jane Doe",
            Age = 25
        };

        await tableClient.AddEntityAsync(dto, cancellationToken);
        Console.WriteLine("Dto inserted!");
    }

    private async Task QueryEntitiesAsync(TableClient tableClient, CancellationToken cancellationToken = default)
    {
        var entities = tableClient.QueryAsync<PersonDto>();
        await foreach (var entity in entities)
        {
            Console.WriteLine($"Entity: {entity.Name} - {entity.Age}");
        }
    }

    private async Task InsertEntityAsync(TableClient tableClient, CancellationToken cancellationToken = default)
    {
        var entity = new TableEntity("Person", Guid.NewGuid().ToString())
        {
            { "Name", "John Doe" },
            { "Age", 33 }
        };

        await tableClient.AddEntityAsync(entity, cancellationToken);
        Console.WriteLine("Entity inserted!");
    }
}

internal class PersonDto : ITableEntity
{
    public string PartitionKey { get; set; } = "Person";
    public string RowKey { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

internal class PetDto : ITableEntity
{
    public string PartitionKey { get; set; } = "Pet";
    public string RowKey { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}