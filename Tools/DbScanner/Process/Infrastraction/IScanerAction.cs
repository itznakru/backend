/*======================================================================================================================================================= 
Interface:IScanerAction
    Итерфейс класса-обработчика BSON документа. 


======================================================================================================================================================= */
using MongoDB.Bson;
namespace DbScanner.Process.Infrastruction
{
    public interface IScanerAction
    {
        Task Process(BsonDocument row);
    }
}