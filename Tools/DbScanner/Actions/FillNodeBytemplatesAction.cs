using DbScanner.Process.Infrastruction;
using ItZnak.Infrastruction.Services;
using ItZnak.PatentsDtoLibrary.Services;
using MongoDB.Bson;
using System.Linq;
using PatentService.Types;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace DbScanner.Actions
{
    public class FillNodeBytemplatesAction : IScanerAction
    {
        private readonly IConfigService _cnf;
        private readonly ILogService _log;


        readonly IMongoDbContextService _dbContext;
        public FillNodeBytemplatesAction(IConfigService cnf, ILogService log, IMongoDbContextService dbContext)
        {
            _log = log;
            _cnf = cnf;
            _dbContext = dbContext;
        }
        static async Task AddTemplate(TradeMark tm)
        {
            if(tm.Vector==null)
                return;

            using var client = new HttpClient();
           // Console.WriteLine("CHECK LENGTH:"+tm.Vector.Length);
            string json= JsonConvert.SerializeObject(new
                    {
                        internalkey = tm.DocId.ToString(),
                        image=tm.Image,
                        template = Convert.ToBase64String(tm.Vector),
                        imagetype = tm.TextImageInfo
                    });

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://20.160.63.109:8888/tm/core/addtemplate"),
                Content = new StringContent(
                    json,
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JsonConvert.DeserializeObject(content));
            }
            else
            {
                Console.WriteLine("An error occurred: " + response.StatusCode);
            }
        }
        public async Task Process(BsonDocument row)
        {
            string sql = "{'DocId':" + row["DocId"] + "}";
            var fltr = BsonDocument.Parse(sql);
            bool isExists = (await _dbContext.Tm.CountDocumentsAsync(fltr)) > 0;
            if (!isExists)
            {
                SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("SKIP KEY");
                return;
            }
            TradeMark tm = (await _dbContext.Tm.FindAsync<TradeMark>(fltr))
                           .ToList()
                           .FirstOrDefault();
            
            //Console.WriteLine(tm.DocId);
            // /* SET PHRASE AND VECTOR */
            // _cache.SetString("tm_" + tm.DocId, tm.TMPhrase);
             if (tm.Vector != null)
                    await AddTemplate(tm);

            SingletonProcessInfrastraction.Itstance.IncAndUpdateStateValue("ADD KEY");
        }
    }
}