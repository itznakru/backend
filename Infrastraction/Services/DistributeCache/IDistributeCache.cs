using Microsoft.Extensions.Caching.Distributed;

namespace ItZnak.Infrastruction.Services
{
    public interface IDistributeCache
    {
        string GetString(string key);
        byte[] Get(string key);
        void SetString(string key, string v, DistributedCacheEntryOptions opt=null);
        void Set(string key, byte[] v, DistributedCacheEntryOptions opt=null);
        void Remove(string key);
    }
}