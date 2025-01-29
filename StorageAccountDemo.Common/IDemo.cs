namespace StorageAccountDemo.Common;

public interface IDemo
{
    Task RunAsync(CancellationToken cancellationToken = default);
}