using ItZnak.Infrastruction.Services;
using ItZnak.PatentsDtoLibrary.Services;
using ItZnak.SiteWebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace SiteWebApi.Controllers.Wizard
{

    [Route("api/[controller]")]
    [ApiController]
    public class WizardController : SiteWebApiController
    {
        public WizardController(IConfigService conf, ILogService logger, IMongoDbContextService dbMongoContext) : base(conf, logger, dbMongoContext)
        {
        }
        /* */
        [HttpPost]
        [Route("checkvaluetm")]
        public IActionResult CheckValueTm([FromBody] CheckValueTmIn p)
        {
            return this.Ok(p);
        }
    }

}
