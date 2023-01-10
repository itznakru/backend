
using MongoDB.Driver;
using ItZnak.PatentsDtoLibrary.Types.Members;
using ItZnak.PatentsDtoLibrary.Types;
using MongoDB.Bson;
using PatentService.Types;

namespace ItZnak.PatentsDtoLibrary.Services
{
    public interface IMongoDbContextService
    {
        IMongoCollection<MktuClass> Mktu { get; }
        IMongoCollection<Member> Members { get; }
        IMongoCollection<TradeMark> Tm { get; }
        IMongoCollection<BsonDocument> GetCollection(string collectionName);
      
    }
}