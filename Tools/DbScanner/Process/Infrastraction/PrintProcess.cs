/*======================================================================================================================================================= 
Class:PrintProcess
Purpose: Monitor progress. The process print to console state of IProcessInfrastraction entity.         
  
======================================================================================================================================================= */

using ItZnak.Infrastruction.Services;

namespace DbScanner.Process.Infrastruction
{
    public class PrintProcess : IPrintProcess
    {
        const int DELAY_TASK_MS = 10000;
        readonly ILogService _log;
        public PrintProcess(ILogService log)
        {
            _log = log;
        }
        /* print state to log until request of stop flow */
        public void Run(CancellationToken ct)
        {
            do
            {
                Console.Clear();
                _log.Info(SingletonProcessInfrastraction.Itstance.GetStateAsString());
                Task.Delay(DELAY_TASK_MS).Wait();
            } while (!ct.IsCancellationRequested);

            _log.Info("PrintProcess:DONE");
        }
    }
}