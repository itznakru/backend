/*======================================================================================================================================================= 
"Class:     FillTaskQProcess
Purpose:
            Read BSON pages from the database.
            Write the obtained BSON to the ScannerQ queue.
            Interrupt the process depending on the ScannerQ.WaitHandler signals."
======================================================================================================================================================= */

using ItZnak.Infrastruction.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using DbScanner.Process.Infrastruction;
using DbScanner.Tools;

namespace DbScanner.Process
{
    public class TransferActionSettings
    {
        public string sourceConnection { get; set; }
        public string targetConnetion { get; set; }
        public int startDocId { get; set; }
        public int dbPageSize { get; set; }
        public string collectionName { get; set; }
        public static string SettingFieldName { get { return "transferAction"; } }
    }
    public class TransferFillTaskQProcess : IFillTaskQProcess
    {
        readonly object _lock = new();
        /* different sql conditions for different collections  */
        readonly Dictionary<DBCOLLECTIONNAME, Func<int, int, BsonDocument>> _collectionFilter;

        /*  collecton name for reading*/
        readonly DBCOLLECTIONNAME _collName = DBCOLLECTIONNAME.tm;

        /*helper for MongoDb */
        readonly DbContext _dbContext;

        /* configuration service */
        readonly IConfigService _cnf;

        /* log service */
        readonly ILogService _log;
        int _startIndx = 0;
        private static TransferActionSettings s_settings = null;

        /* Инициировать значения из файла конфигурации, инициировать карту соответсвия (_collectionFilter) коллекция - шаблон SQL  */
        public TransferFillTaskQProcess(IConfigService cnf, ILogService log)
        {
            _cnf = cnf;
            _log = log;

            /* read settings */
            s_settings ??= _cnf.GetObject<TransferActionSettings>(TransferActionSettings.SettingFieldName);

            /* read collecton name */
            _collName = (DBCOLLECTIONNAME)Enum.Parse(typeof(DBCOLLECTIONNAME), s_settings.collectionName);

            /* read first docId */
            _startIndx = s_settings.startDocId;

            /* init database layer */
            _dbContext = new DbContext(s_settings.sourceConnection, nameof(DBNAME.Patents));

            /* FILL SQL LIST Dictionary<DBCOLLECTIONNAME,Func<int,int,BsonDocument>> */
            #region  
            _collectionFilter = new Dictionary<DBCOLLECTIONNAME, Func<int, int, BsonDocument>>(){
                                    {DBCOLLECTIONNAME.member, (from,to)=>{
                                                        string s="";
                                                        List<string> d=new();
                                                        for(int i=from;i<to;i++){
                                                                d.Add($"'{i}'");
                                                        }
                                                        s=String.Join(",",d);
                                                        return BsonDocument.Parse("{'_id':{'$in':["+s+"]}}");
                                                        }},
                                    {DBCOLLECTIONNAME.mktu, (from,to)=>{
                                                        string s="";
                                                        List<string> d=new();
                                                        for(int i=from;i<to;i++){
                                                                d.Add($"{i}");
                                                        }
                                                        s=String.Join(",",d);
                                                        return BsonDocument.Parse("{'Code':{'$in':["+s+"]}}");
                                                        }},
                                    {DBCOLLECTIONNAME.tm, (from,to)=>{
                                                        string s="";
                                                        List<string> d=new();
                                                        for(int i=from;i<to;i++){
                                                                d.Add($"{i}");
                                                        }
                                                        s=String.Join(",",d);
                                                        string sql="{'DocId':{'$in':["+s+"]}}";
                                                        return BsonDocument.Parse(sql);
                                                        }}
                            };
            #endregion
        }

        /* Read the database chunck dependent by state of semaphore */
        public void Run(CancellationToken ct)
        {
            try
            {
                do
                {
                    SingletonProcessInfrastraction.Itstance.WaitProcessThread();
                    Next(s_settings.dbPageSize);
                    SingletonProcessInfrastraction.Itstance.ResetProcessThread();
                } while (!ct.IsCancellationRequested);
            }
            catch (Exception e)
            {
                _log.Exception(e.ToString());
                SingletonProcessInfrastraction.Itstance.CTS.Cancel();
            }
            _log.Info("FillTaskQProcess:DONE");
        }
        /* Read next db chunk  in TaskQ */
        private void Next(int size)
        {
            lock (_lock)
            {
                BsonDocument d = _collectionFilter[_collName](_startIndx, _startIndx + size);

                /* get next db page */
                List<BsonDocument> rs = _dbContext.GetCollection(_collName.ToString()).Find(d).ToList();

                /* add records from db to TaskQ */
                rs.ForEach(
                        itm => SingletonProcessInfrastraction.Itstance.Enque(itm)
                        );

                /* increase variable _startIndex */
                _startIndx += size;
                SingletonProcessInfrastraction.Itstance.UpdateState("LAST_DOCID", _startIndx.ToString());
                SingletonProcessInfrastraction.Itstance.UpdateState("LAST_READER_TIME", DateTime.Now.ToString());
            }
        }
        /* Confirm setiings */
        public void Init()
        {
            Console.WriteLine("CONFIRM SETTINGS");
            Console.WriteLine("MODE:TRANSFER");
            Console.WriteLine("FROM:" + s_settings.sourceConnection);
            Console.WriteLine("TO:" + s_settings.sourceConnection);
            Console.WriteLine("START DOCID:" + s_settings.startDocId);
            Console.WriteLine("PAGE SIZE:" + s_settings.dbPageSize);
            Console.WriteLine("Y/NO");
            var line = Console.ReadLine();
            if (!line.Equals("Y", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException();
        }
    }
}