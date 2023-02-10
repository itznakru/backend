using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Web.Controllers;
using ItZnak.PatentsDtoLibrary.TypeEnums;
using Newtonsoft.Json;
using SiteWebApi.Controllers.Wizard;

namespace SiteWebApi.Controllers.Wizard
{
    public class CheckValueTmIn
    {
        [JsonProperty("tmType")]
        public TmType TmType { get; set; }
        [JsonProperty("tmValue")]
        public string TmValue { get; set; }
        [JsonProperty("tmText")]
        public string TmText { get; set; }
        [JsonProperty("memberKey")]
        public string MemberKey { get; set; }
    }
}

public class CheckValueTmHandler : WebApiControllerHandler<CheckValueTmIn, bool>
{
    public CheckValueTmHandler(IMWebApiController context) : base(context)
    {
    }
    public override bool Handle(CheckValueTmIn p)
    {
        return true;
    }
}
