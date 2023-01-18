
using MongoDB.Driver;

using ItZnak.Infrastruction.Services;
using ItZnak.PatentsDtoLibrary.TypeEnums;
using ItZnak.PatentsDtoLibrary.Services.Enums;
using ItZnak.PatentsDtoLibrary.Types.Members;
using ItZnak.PatentsDtoLibrary.Types;
using MongoDB.Bson;
using PatentService.Types;

namespace ItZnak.PatentsDtoLibrary.Services
{
    public class MongoDbContextService : IMongoDbContextService
    {
        readonly IMongoDatabase _db;
        readonly MongoClient _client;
        public MongoDbContextService(string connectionString)
        {
            _client = new MongoClient(connectionString);
            _db = _client.GetDatabase(nameof(DB_NAME.Patents));
        }
        public MongoDbContextService(IConfigService conf)
        {
            _client = new MongoClient(conf.GetString("connectionString"));
            _db = _client.GetDatabase(nameof(DB_NAME.Patents));
        }
        private IMongoCollection<T> GetCollections<T>(DB_COLLECTION_NAME coll)
        {
            return _db.GetCollection<T>(coll.ToString());
        }
        public IMongoCollection<MktuClass> Mktu => GetCollections<MktuClass>(DB_COLLECTION_NAME.mktu);
        public IMongoCollection<Member> Members => GetCollections<Member>(DB_COLLECTION_NAME.members);
        public IMongoCollection<TradeMark> Tm => GetCollections<TradeMark>(DB_COLLECTION_NAME.tm);

        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return _client.GetDatabase(nameof(DB_NAME.Patents)).GetCollection<BsonDocument>(collectionName);
        }
    }
}
