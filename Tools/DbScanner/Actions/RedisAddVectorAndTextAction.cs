using DbScanner.Process.Infrastruction;
using ItZnak.Infrastruction.Services;
using ItZnak.PatentsDtoLibrary.Services;
using MongoDB.Bson;
using System.Linq;
using PatentService.Types;
using MongoDB.Driver;

namespace DbScanner.Actions
{
    public class RedisAddVectorAndTextAction : IScanerAction
    {
        private readonly IConfigService _cnf;
        private readonly ILogService _log;
        private readonly IDistributeCache _cache;

        readonly IMongoDbContextService _dbContext;
        public RedisAddVectorAndTextAction(IConfigService cnf, ILogService log, IDistributeCache cache, IMongoDbContextService dbContext)
        {
            _log = log;
            _cnf = cnf;
            _cache = cache;
            _dbContext = dbContext;
        }

        public async Task Process(BsonDocument row)
        {
            string sql = "{'DocId':" + row["DocId"] + "}";
            var fltr = BsonDocument.Parse(sql);
            bool isExists = (await _dbContext.Tm.CountDocumentsAsync(fltr)) > 0;
            if (!isExists)
            {
                SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("SKIP KEY");
                return;
            }
            TradeMark tm = (await _dbContext.Tm.FindAsync<TradeMark>(fltr))
                           .ToList()
                           .FirstOrDefault();
            
            /* SET PHRASE AND VECTOR */
            _cache.SetString("tm_" + tm.DocId, tm.TMPhrase);
            if (tm.Vector != null)
                _cache.Set("v_" + tm.DocId, tm.Vector);

            SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("ADD KEY");
        }
    }
}