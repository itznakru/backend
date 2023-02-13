using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;

namespace Infrastraction.Services.MemoryCache
{
    public interface IMemoryCache<T>:ICollection<T>
    {
         void  Set(string key, T value );
         T  Get(string key);

         void Remove(string key);
         bool IsExists(string key);
    }
}