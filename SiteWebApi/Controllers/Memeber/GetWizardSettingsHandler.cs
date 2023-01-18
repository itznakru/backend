
using ItZnak.Infrastruction.Web.Controllers;
using ItZnak.PatentsDtoLibrary.Services;
using ItZnak.PatentsDtoLibrary.Types.Members;
using ItZnak.SiteWebApi.Controllers.Base;
using MongoDB.Driver;
using DTO = ItZnak.PatentsDtoLibrary.Types;

namespace ItZnak.SiteWebApi.Controllers.Member
{
    /* Структура результата запроса  */
    public class GetWizardSettingsRM
    {
        public string key { get; set; }
        public string name { get; set; }
        public string assistantImage { get; set; }
        public string assistantName { get; set; }
        public string assistantPosition { get; set; } = "CEO & Head of Legal";
        public string homePage { get; set; } = "";

        public Dictionary<string, string> assistant { get; set; }
        public Array stages { get; set; }
    }

    /*======================================================================================================================================================= 
    Class: GetWizardSettingsHandler
    URL: api/member/getwizardsettings/?memberKey=<key>
    Implementation of the request for the wizard settings of the application for the specified system participant
    Example of the result.
    {"key":"demo","name":"demo","assistantImage":null,"assistantName":null,"assistantPosition":"CEO & Head of Legal","homePage":"app/wizard-plugin/test-site/","assistant":{"image":"","name":"Игорь Сидорофф","position":"ассистент"},"stages":[{"_id":0,"number":1,"helpText":"Комментарий по соданию профиля","name":"Создание профиля"},{"_id":1,"number":2,"helpText":"Комментарий по выбору клиента","name":"Заявитель"},{"_id":2,"number":3,"helpText":"Комментарий по загрузке знака","name":"Загрузка знака"},{"_id":3,"number":4,"helpText":"Комментарий по выбору МКТУ","name":"Выбор МКТУ"},{"_id":4,"number":5,"helpText":"Комментарий по выбору услуг ","name":"Выбор услуг"},{"_id":5,"number":6,"helpText":"Комментарий по итоговой странице ","name":"Итоговая страница"},{"_id":6,"number":7,"helpText":"Комментарий по странице закрывашке","name":"Закрывашка"}]}
    ======================================================================================================================================================= */

    public class GetWizardSettingsHandler : WebApiControllerHandler<string, GetWizardSettingsRM>
    {
        private readonly IMongoDbContextService _db;
        public GetWizardSettingsHandler(ISiteWebApiController context) : base(context)
        {
            _db = context.MongoDbContext;
        }
        /*      
            Please access the database using the memberKey key. 
            Convert the structure from the Types.Member database to the GetWizardSettingsRM structure.
        */
        public override async Task<GetWizardSettingsRM> HandleAsync(string memberKey)
        {
            int cnt = 0;
            /*convert WizardStageSetting to new anonymus type for JSON response  */
            Func<WizardStageSetting, object> toStages = (d) =>
            {
                cnt++;
                return new { d._id, number = cnt, helpText = d.HelpText, name = d.Name };
            };

            var projection = Builders<DTO.Members.Member>
               .Projection
               .Expression(pw => new GetWizardSettingsRM
               {
                   key = pw.Key,
                   name = pw.Name,
                   assistant = new Dictionary<string, string>(){
                                {"image",pw.Assistant.Image},
                                {"name",pw.Assistant.Name},
                                {"position",pw.Assistant.Position}
                           },
                   homePage = pw.Settings["WizardPage"],
                   stages = pw.WizardSettings
                                .Where(x => x.UseStage)
                                .Select(toStages)
                                .ToArray()
               });

            FindOptions<DTO.Members.Member, GetWizardSettingsRM> op = new FindOptions<DTO.Members.Member, GetWizardSettingsRM>() { Projection = projection };

            FilterDefinition<DTO.Members.Member> filter = new ExpressionFilterDefinition<DTO.Members.Member>(x =>x.Key==memberKey);

            return (await _db.Members.FindAsync(filter, op)).FirstOrDefault();
        }
    }
}