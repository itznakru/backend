
using ItZnak.Infrastruction.Web.Controllers;
using ItZnak.PatentsDtoLibrary.Services;

/* Интерфейс контроллера конечной точки приложения  GameteoWebApi обогащающий стандартный контроллер сервисом доступа 
к БД с DTO обьектами приложения и справочнком наименований валют    */
namespace ItZnak.SiteWebApi.Controllers.Base
{
    public interface ISiteWebApiController : IMWebApiController
    {
         IMongoDbContextService MongoDbContext { get; }
    }
}