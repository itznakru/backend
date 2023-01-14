using DbScanner.Process.Infrastruction;
using ItZnak.Infrastruction.Services;
using MongoDB.Bson;

namespace DbScanner.Process
{
    public class RedisFillSettings
    {
        public int finishDocId { get; set; }
        public int startDocId { get; set; }
        public string connectionString { get; set; }
        public int dbPageSize { get; set; }
        public static string SettingFieldName { get { return "redis"; } }

    }

    /*Purpose: To fill task queue for action fill redis cahe (company names| image vectors)*/
    public class RedisFillTaskQProcess : IFillTaskQProcess
    {
        private readonly RedisFillSettings s_settings = null;
        private int _startIndx;
        private readonly ILogService _log;
        public RedisFillTaskQProcess(IConfigService cnf, ILogService log)
        {
            _log = log;
            _startIndx = 100;
            s_settings ??= cnf.GetObject<RedisFillSettings>(RedisFillSettings.SettingFieldName);
            s_settings.connectionString ??= cnf.GetString("connectionString");
            _startIndx = s_settings.startDocId;
        }

        public void Init()
        {
            const string PROMT_START = "Enter START DOCID:";
            const string PROMT_END = "Enter FINISH DOCID:";
            Console.WriteLine("CONFIRM SETTINGS");
            Console.WriteLine("MODE:FILL REDIS CACHE");
            Console.WriteLine("DB:" + s_settings.connectionString);
            Console.WriteLine("PAGE SIZE:" + s_settings.dbPageSize);
            Console.Write(PROMT_START);
            string docId = Console.ReadLine().Replace(PROMT_START, "");
            _startIndx = int.Parse(docId);
            Console.Write(PROMT_END);
            docId = Console.ReadLine().Replace(PROMT_END, "");
            s_settings.finishDocId = int.Parse(docId);
        }


        public void Run(CancellationToken ct)
        {
            try
            {
                do
                {
                    SingletonProcessInfrastraction.Itstance.WaitProcessThread();
                    Next(s_settings.dbPageSize);
                    SingletonProcessInfrastraction.Itstance.ResetProcessThread();
                } while (!ct.IsCancellationRequested && _startIndx < s_settings.finishDocId);

                /* if we should stop by the reason end  operation - wait finish of actions*/
                if(!ct.IsCancellationRequested)
                    SingletonProcessInfrastraction.Itstance.ShutDown();

            }
            catch (Exception e)
            {
                _log.Exception(e.ToString());
                SingletonProcessInfrastraction.Itstance.CTS.Cancel();
            }
            _log.Info("FillTaskQProcess:DONE");
        }



        private void Next(int size)
        {
            int docId = _startIndx + 1;
            do
            {
                SingletonProcessInfrastraction
                .Itstance
                .Enque(BsonDocument.Parse("{DocId:" + docId + "}"));
                docId++;
            } while (docId < _startIndx + size);
            _startIndx += size;

            SingletonProcessInfrastraction.Itstance.UpdateState("LAST_DOCID", _startIndx.ToString());
            SingletonProcessInfrastraction.Itstance.UpdateState("LAST_ADD_Q_TIME", DateTime.Now.ToString());
        }
    }
}