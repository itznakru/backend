using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;

namespace Infrastraction.Services.MemoryCache
{
    public interface IMemoryCache<T>
    {
         void  Set(string partion, string key, T value );
         T  Get(string partion, string key);

         void Remove(string partion, string key);
         bool IsExists(string partion, string key);

         ICollection<string> PartionKeys (string partion);

         int Count {get;}
    }
}