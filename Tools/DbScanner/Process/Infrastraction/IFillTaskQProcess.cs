/*The process for fill  queue of tasks of Process infrastruction entity. Always we  are running only one entity of IFillTaskQProcess */
namespace DbScanner.Process.Infrastruction
{
    public interface IFillTaskQProcess
    {
        void Run(CancellationToken ct);

        /* Main settings process can receive from setting object, but can receive by console input new settings. 
        The method will called before start method 'Run' */
        void Init();
    }
}