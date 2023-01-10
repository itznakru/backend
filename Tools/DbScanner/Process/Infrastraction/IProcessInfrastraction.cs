using MongoDB.Bson;

namespace DbScanner.Process.Infrastruction
{
    /* Infrastructure to support the interaction process. Contains: Queue of tasks, shared semaphore and dictionary for registration log. */
    public interface IProcessInfrastraction : IProcessInfrastractionQ, IProcessInfrastractionState, IProcessInfrastractionSemaphore { }
    public interface IProcessInfrastractionQ
    {
        void Enque(BsonDocument d);
        BsonDocument Deque();
        CancellationTokenSource CTS { get; }
    }

    public interface IProcessInfrastractionState
    {
        void UpdateState(string key, string value);
        string GetStateValue(string key);
        void IncAndUpdateStateValue(string key);
        void ShutDown();
        string GetStateAsString();
    }
    public interface IProcessInfrastractionSemaphore
    {
        void WaitProcessThread();
        void ResetProcessThread();
    }

}