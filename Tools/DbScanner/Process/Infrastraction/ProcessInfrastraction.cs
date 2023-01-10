
/*======================================================================================================================================================= 
Class:ScannerQ
Назначение: 
    1. Контейнер (очередь) содержащий BSON документы считанные из БД потоком IFillTaskQProcess 
    2. Контейнер для метрик текущей деятельности потоков IFillTaskQProcess и IScanerProcess
    3. Набор потокобезопасных методов для сичтывания и изменения значений метрик.
    4. Запуск останов процесса IFillTaskQProcess в зависимости от состояния очереди   
======================================================================================================================================================= */

using System.Collections.Concurrent;
using MongoDB.Bson;

namespace DbScanner.Process.Infrastruction
{
    public class ProcessInfrastraction : IProcessInfrastraction
    {
        const int MIN_Q_LEN = 100;
        const int MAX_Q_LEN = 10000;
        private readonly ConcurrentQueue<BsonDocument> _taskQ;
        private readonly ManualResetEvent _waitHandler;
        private readonly Dictionary<string, string> _state;
        public CancellationTokenSource CTS { get; }

        public ProcessInfrastraction()
        {
            _taskQ = new ConcurrentQueue<BsonDocument>();
            _waitHandler = new ManualResetEvent(true);
            _state = new Dictionary<string, string>();
            CTS = new CancellationTokenSource();
        }

        public BsonDocument Deque()
        {
            if (_taskQ.Count < MIN_Q_LEN)
                _waitHandler.Set();

            if (_taskQ.TryDequeue(out BsonDocument d))
                return d;

            return null;
        }

        public void Enque(BsonDocument d)
        {
            _taskQ.Enqueue(d);
            UpdateState("TASK_Q_SIZE", _taskQ.Count.ToString());
            if (_taskQ.Count > MAX_Q_LEN)
                _waitHandler.Reset();
        }

        public string GetStateAsString()
        {
            var v = _state.Keys.Select(key => key + "=" + _state[key]).ToArray();
            return _state.Count == 0 ? "..." : string.Join(" | ", v);
        }

        public string GetStateValue(string key)
        {
            return _state.ContainsKey(key) ? _state[key] : "";
        }

        public void IncAndUpdateStateValue(string key)
        {
            if (_state.ContainsKey(key))
            {
                _state[key] = (int.Parse(_state[key]) + 1).ToString();
            }
            else
            {
                _state.Add(key, "0");
            }
        }

        public void ResetProcessThread()
        {
            _waitHandler.Reset();
        }

        public void UpdateState(string key, string value)
        {
            if (_state.ContainsKey(key))
                _state[key] = value;
            else
                _state.Add(key, value);
        }

        public void WaitProcessThread()
        {
            _waitHandler.WaitOne();
        }
    }

}