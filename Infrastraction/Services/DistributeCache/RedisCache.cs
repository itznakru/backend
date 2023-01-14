using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace ItZnak.Infrastruction.Services
{
    public class RedisCacheConfig
    {
        public string redisUrl { get; set; }
        public string instanceName { get; set; }
        public int absoluteExpirationMin { get; set; } = 1440;
        public int slidingExpirationMin { get; set; } = 1440;
        public static string rootName { get { return "redis"; } }
    }

    public class RedisCache : IDistributeCache
    {
        private readonly RedisCacheConfig _configRedis;
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        public RedisCache(IServiceCollection services, RedisCacheConfig cnfg)
        {
            _configRedis=cnfg;    
            services.AddDistributedRedisCache(opt =>
            {
                opt.Configuration = _configRedis.redisUrl;
                opt.InstanceName = _configRedis.instanceName;
              //  opt.ConfigurationOptions= new StackExchange.Redis.ConfigurationOptions(){SyncTimeout=15000};
            });
            _cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_configRedis.absoluteExpirationMin),
                SlidingExpiration = TimeSpan.FromMinutes(_configRedis.slidingExpirationMin)
            };

             IServiceProvider serviceProvider = services.BuildServiceProvider();
            _cache = serviceProvider.GetRequiredService<IDistributedCache>();
        }
        public byte[] Get(string key)
        {
            throw new NotImplementedException();
        }

        public string GetString(string key)
        {
            return _cache.GetString(key);
        }

        public void Set(string key, byte[] v, DistributedCacheEntryOptions opt=null)
        {
            _cache.Set(key, v, opt??_cacheOptions);
        }

        public void SetString(string key, string v,DistributedCacheEntryOptions opt=null)
        {
            _cache.SetString(key, v, opt??_cacheOptions);
        }
    }
}