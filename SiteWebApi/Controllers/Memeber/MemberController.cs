
using ItZnak.Infrastruction.Services;
using ItZnak.PatentsDtoLibrary.Services;
using ItZnak.SiteWebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
/*======================================================================================================================================================= 
Class: MemberController
URL: api/member/getwizardsettings/?memberKey=<key>
Implementation of operations related to the Member business entity.
Member describes a company-participant of the SAAS service http://itznak.ru
======================================================================================================================================================= */


namespace ItZnak.SiteWebApi.Controllers.Member
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : SiteWebApiController
    {
        public MemberController(IConfigService conf, ILogService logger, IMongoDbContextService dbMongoContext) : base(conf, logger, dbMongoContext)
        {
        }
        /* вернуть натройки Wizard-a для участника системы заданного параметром memberKey */
        [HttpGet]
        [Route("getwizardsettings")]
        public async Task<IActionResult> GetWizardSettingsAsync( string memberKey)
        {
            var memberSettings=await new GetWizardSettingsHandler(this)
                                                .HandleAsync(memberKey);

            return  this.Ok(memberSettings);
        }
    }
}