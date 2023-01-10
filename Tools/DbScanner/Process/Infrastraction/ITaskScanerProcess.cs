
namespace DbScanner.Process.Infrastruction
{
    public interface ITaskScanerProcess
    {
        Task Run(CancellationToken ct);
    }
}