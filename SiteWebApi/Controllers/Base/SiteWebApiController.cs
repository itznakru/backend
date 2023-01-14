using Microsoft.AspNetCore.Mvc;
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Controllers;
using ItZnak.PatentsDtoLibrary.Services;

namespace ItZnak.SiteWebApi.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteWebApiController : WebApiController, ISiteWebApiController
    {
        readonly private IMongoDbContextService _mongoDbContext;
        public SiteWebApiController(IConfigService conf, ILogService logger, IMongoDbContextService dbMongoContext) : base(conf, logger)
        {
            _mongoDbContext = dbMongoContext;
        }

        public IMongoDbContextService MongoDbContext => _mongoDbContext;
    }
}