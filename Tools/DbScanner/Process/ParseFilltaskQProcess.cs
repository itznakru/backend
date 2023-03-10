using DbScanner.Process.Infrastruction;
using ItZnak.Infrastruction.Services;
using MongoDB.Bson;

namespace DbScanner.Process
{

    /* DTO class for prasing appsettings.json */
    public class ParseActionSettings
    {
        public string paserClass { get; set; }
        public int startDocId { get; set; }
        public int finishDocId { get; set; }
        public int dbPageSize { get; set; }
        public string connectionString { get; set; }
        public static string SettingFieldName { get { return "parserAction"; } }
    }

    /*Purpose: To fill task queue for action site parsing*/
    public class ParseFillTaskQProcess : IFillTaskQProcess
    {
        private static ParseActionSettings s_settings = null;
        private readonly ILogService _log;
        private int _startIndx;
        public ParseFillTaskQProcess(IConfigService cnf, ILogService log)
        {
            /* read settings */
            s_settings ??= cnf.GetObject<ParseActionSettings>(ParseActionSettings.SettingFieldName);
            s_settings.connectionString = cnf.GetString("connectionString");
            _log = log;
            _startIndx = s_settings.startDocId;
        }

        public void Init()
        {
            Console.WriteLine("CONFIRM SETTINGS");
            Console.WriteLine("MODE:PARSING");
            Console.WriteLine("DB:" + s_settings.connectionString);
            Console.WriteLine("CLASS:" + s_settings.paserClass);
            Console.WriteLine("PAGE SIZE:" + s_settings.dbPageSize);
            Console.Write("Enter START DOCID :");
            string docId = Console.ReadLine().Replace("Enter START DOCID:", "");
            _startIndx = int.Parse(docId);
            Console.Write("Enter FINISH DOCID:");
            docId = Console.ReadLine().Replace("Enter FINISH DOCID:", "");
            s_settings.finishDocId = int.Parse(docId);
            Console.WriteLine("Y/NO");
            var line = Console.ReadLine();
            if (!line.Equals("Y", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException();
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
            SingletonProcessInfrastraction.Itstance.UpdateState("LAST_READER_TIME", DateTime.Now.ToString());
        }
    }
}