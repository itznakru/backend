/*======================================================================================================================================================= 
Class:TranserAction 

Назначение: Алгоритм построчного копирования коллекции с проверкой задвойки по полю DocId 
======================================================================================================================================================= */
using DbScanner.Tools;

using ItZnak.Infrastruction.Services;
using MongoDB.Bson;
using DbScanner.Process;
using DbScanner.Process.Infrastruction;
using ItZnak.PatentsDtoLibrary.Services;

namespace DbScanner.Actions
{
    public class TranserAction : IScanerAction
    {
        readonly string _collName = "";
        readonly MongoDbContextService _dbContext;
        readonly IConfigService _cnf;

        private static TransferActionSettings s_settings;
        public TranserAction(IConfigService cnf)
        {
            _cnf = cnf;
             s_settings ??= _cnf.GetObject<TransferActionSettings>(TransferActionSettings.SettingFieldName);
            _dbContext = new MongoDbContextService(s_settings.targetConnetion);
            _collName = s_settings.collectionName;
        }


        /* Проверить наличие записи в целевой БД -если нет добавить. Если есть задвойки - убрать.  */
        public async Task Process(BsonDocument row)
        {
            string sql = "{'DocId':" + row["DocId"] + "}";
            var fltr = BsonDocument.Parse(sql);
            var tmColl = _dbContext.GetCollection(_collName);
            long cnt = await tmColl.CountDocumentsAsync(fltr);
            if (cnt == 1)
            {
                SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("UPD ROWS");
                return;
            }
            if (cnt > 1)
            {
                Console.Beep();
                await tmColl.DeleteManyAsync(fltr);
            }
            await tmColl.InsertOneAsync(row);
            SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("ADD ROWS");
        }
    }
}