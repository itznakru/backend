
/*======================================================================================================================================================= 
Class:DbContext
Назначение: 
    1. Предоставить доступ к коллекции из БД 
    
======================================================================================================================================================= */



namespace DbScanner.Tools
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    public enum DBCOLLECTIONNAME { tm, member, mktu }
    public enum DBNAME { Patents }

    public class DbContext
    {
        readonly MongoClient _client;
        readonly string _dbName;
        public DbContext(string cs,
                         string dbName)
        {
            _client = new MongoClient(cs);
            _dbName = dbName;
        }
        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return _client.GetDatabase(_dbName).GetCollection<BsonDocument>(collectionName);
        }
    }
}