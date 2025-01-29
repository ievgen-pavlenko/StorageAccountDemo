# Storage Account Demo

This solution demonstrates how to interact with Azure Storage services, including Queue Storage and Table Storage, using C#. The solution includes examples of listing, creating, and managing queues and tables, as well as inserting and querying entities.

## Prerequisites

- .NET 6.0 SDK or later
- Azure Storage account
- Visual Studio or JetBrains Rider

## Configuration

1. Open the `appsettings.json` file located in the `StorageAccountDemo.Common` directory.
2. Update the `ConnectionString` property under `StorageAccountConfiguration` with your Azure Storage account connection string.

```json
{
  "StorageAccountConfiguration": {
    "ConnectionString": "your_connection_string_here"
  }
}
```

## Projects

### StorageAccountDemo

This project contains the main demo classes for interacting with Azure Queue Storage and Table Storage.

#### QueueStorageDemo

This class demonstrates how to:

- List queues
- Create a queue if it does not exist
- Enqueue messages
- Peek messages
- Dequeue messages

#### TableStorageDemo

This class demonstrates how to:

- Create a table if it does not exist
- Insert entities
- Query entities

## Running the Demos

1. Ensure the `StorageAccountConfiguration` is properly set in the `appsettings.json` file.
2. Build the solution.
3. Run the desired demo class (e.g., `QueueStorageDemo` or `TableStorageDemo`) from your IDE.

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.
