/*======================================================================================================================================================= 
Class:ScanerProcess
Назначение: 
    1.Выбирать из очереди BSON документов следующий
    2.Обрабатывать выбранный документ используя алгоритм предоставленный реализацией IScanerAction
======================================================================================================================================================= */

using MongoDB.Bson;
using ItZnak.Infrastruction.Services;

namespace DbScanner.Process.Infrastruction
{
    public class TaskScanerProcess : ITaskScanerProcess
    {
        /* задержка в мск если очередь задая пуста */
        const int DEALY_IF_Q_EMPTY = 1000;
        /* команда обработчик строки "полезная работа"  */
        readonly IScanerAction _actionHandler;

        /* сервис конфигурации */
        private readonly IConfigService _cnf;
        /* сервис логирования */
        readonly ILogService _log;
        public TaskScanerProcess(IScanerAction actionHandler, IConfigService cnf, ILogService log)
        {
            _actionHandler = actionHandler;
            _cnf = cnf;
            _log = log;
        }

        /* 
            В цикле выбираем документ из очереди и запускаем его на обработку в  _actionHandler
            Если очередь пуста, замираем на DEALY_IF_Q_EMPTY
            Цикл до запроса IsCancellationRequested
            В случае ошибки обработчика - вызвать прекращение всех процессов  ScannerQ.CTS.Cancel();
        */
        public async Task Run(CancellationToken ct)
        {
            try
            {
                do
                {
                    BsonDocument d = SingletonProcessInfrastraction.Itstance.Deque();
                    if (d != null)
                    {
                        await _actionHandler.Process(d);
                        continue;
                    }
                    await Task.Delay(DEALY_IF_Q_EMPTY, ct);
                } while (!ct.IsCancellationRequested);
            }
            catch (Exception e)
            {
                _log.Exception(e.ToString());
                SingletonProcessInfrastraction.Itstance.ResetProcessThread();
                SingletonProcessInfrastraction.Itstance.CTS.Cancel();
            }
            finally
            {
                _log.Info("IScanerProcess:DONE");
            }
        }
    }
}