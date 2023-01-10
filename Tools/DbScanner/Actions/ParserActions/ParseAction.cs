using DbScanner.Process.Infrastruction;
using ItZnak.Infrastruction.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using PatentService.Types;
using DbScanner.Actions.ParserActions;
using ItZnak.PatentsDtoLibrary.Services;
using DbScanner.Exceptions;

namespace DbScanner.Actions
{
    public class ParseAction : IScanerAction
    {
        readonly IConfigService _cnfg;
        readonly ISiteParser _parser;
        readonly ILogService _log;
        readonly IMongoDbContextService _dbContext;
        public ParseAction(ILogService log, IConfigService cnfg, IMongoDbContextService dbContext)
        {
             _log=log;
            _cnfg = cnfg;
            _dbContext = dbContext;
            _parser = new ZnakovedSiteParser();
        }

        public async Task Process(BsonDocument row)
        {
            try
            {
                int docId = int.Parse(row["DocId"].ToString());
                if (!CheckExists(docId))
                {
                    TradeMark tm = await _parser.ProcessAsync(docId, new List<Tuple<string, string>>());
                    await _dbContext.Tm.InsertOneAsync(tm);
                    SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("SUCEESS PARSE ACTION");
                    Console.WriteLine(docId+":"+ tm.TMPhrase);
                }
                else
                {
                    SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("SKIP PARSE ACTION");
                }
            }
            catch (ParseException ex)
            {
                  SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("ERROR PARSE ACTION");
                 _log.Exception(ex);
            }
        }
        private bool CheckExists(int docId)
        {
            return _dbContext.Tm.CountDocuments(d => d.DocId == docId) > 0;
        }
    }
}