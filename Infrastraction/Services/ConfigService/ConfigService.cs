
using Microsoft.Extensions.Configuration;

/* реализация  IConfigService*/
namespace ItZnak.Infrastruction.Services
{
    public class ConfigService : IConfigService
    {
        readonly IConfigurationRoot _root;
        public ConfigService()
        {
            _root=new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
        }
        public int GetInt(string field)
        {
            return int.Parse(_root[field]??"0");
        }

         public bool GetBool (string field){
              return bool.Parse(_root[field]);
         }

        public T GetObject<T>(string filed)
        {
            T dto= Activator.CreateInstance<T>();
            _root.GetSection(filed).Bind(dto);
            return dto;
        }

        public string GetString(string field)
        {
            return _root[field]??"";
        }
    }
}