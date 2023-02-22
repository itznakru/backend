using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastraction.Services.MemoryCache
{
    public class MemoryCache<T> : IMemoryCache<T>
    {
        readonly Dictionary<string, ConcurrentDictionary<string, T>> _partitions;

        public MemoryCache()
        {
            _partitions = new Dictionary<string, ConcurrentDictionary<string, T>>();
        }
        public int Count
        {
            get
            {
                int rslt = 0;
                foreach (var partionKey in _partitions.Keys)
                {
                    rslt = +_partitions[partionKey].Keys.Count;
                }
                return rslt;
            }
        }

        public T Get(string partion, string key)
        {
            throw new NotImplementedException();
        }

        public bool IsExists(string partion, string key)
        {
            if (_partitions.ContainsKey(partion))
            {
                return _partitions[partion].ContainsKey(key);
            }
            return false;
        }

        public ICollection<string> PartionKeys(string partion)
        {
             if (_partitions.ContainsKey(partion))
                    return _partitions[partion].Keys;

            return new List<string>();
        }

        public void Remove(string partion, string key)
        {
            if (_partitions.ContainsKey(partion))
            {
                if (_partitions[partion].ContainsKey(key))
                    _partitions[partion].TryRemove(key, out T tmp);
            }
        }

        public void Set(string partion, string key, T value)
        {
            if (!_partitions.ContainsKey(partion))
                _partitions.Add(partion, new ConcurrentDictionary<string, T>());

            _partitions[partion].TryAdd(key, value);
        }
    }
    // public class MemoryCache<T> : IMemoryCache<T>
    // {
    //     private readonly ConcurrentDictionary<string, T> _cache;

    //     public MemoryCache()
    //     {
    //         _cache = new ConcurrentDictionary<string, T>();
    //     }

    //     public MemoryCache(ConcurrentDictionary<string, T> cache)
    //     {
    //         _cache = cache;
    //     }

    //     public int Count
    //     {
    //         get { _ = _cache.TryGetNonEnumeratedCount(out int rslt); return rslt; }
    //     }
    //     public bool IsReadOnly => false;

    //     public ICollection<string> Keys
    //     {
    //         get
    //         {
    //             return _cache.Keys;
    //         }
    //     }

    //     public void Add(T item)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public void Clear()
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public bool Contains(T item)
    //     {
    //         return _cache.Values.Contains<T>(item);
    //     }

    //     public void CopyTo(T[] array, int arrayIndex)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public IEnumerator<T> GetEnumerator()
    //     {
    //         return _cache.Values.GetEnumerator() as IEnumerator<T>;
    //     }

    //     public T Get(string key)
    //     {
    //         _cache.TryGetValue(key, out T value);
    //         return value;
    //     }

    //     public void Remove(string key)
    //     {
    //         _cache.Remove(key, out T vw);
    //     }

    //     public bool Remove(T item)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public void Set(string key, T value)
    //     {
    //         _cache.TryAdd(key, value);
    //     }

    //     IEnumerator IEnumerable.GetEnumerator()
    //     {
    //         return _cache.Keys.GetEnumerator();
    //     }

    //     public bool IsExists(string key)
    //     {
    //         return _cache.ContainsKey(key);
    //     }
    // }
}