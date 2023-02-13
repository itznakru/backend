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
        private readonly ConcurrentDictionary<string, T> _cache;

        public MemoryCache()
        {
            _cache = new ConcurrentDictionary<string, T>();
        }

        public MemoryCache(ConcurrentDictionary<string, T> cache)
        {
            _cache = cache;
        }

        public int Count
        {
            get { _ = _cache.TryGetNonEnumeratedCount(out int rslt); return rslt; }
        }
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return _cache.Values.Contains<T>(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _cache.Values.GetEnumerator() as IEnumerator<T>;
        }

        public T Get(string key)
        {
            _cache.TryGetValue(key, out T value);
            return value;
        }

        public void Remove(string key)
        {
            _cache.Remove(key, out T vw);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, T value)
        {
            _cache.TryAdd(key, value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cache.Values.GetEnumerator();
        }

        public bool IsExists(string key)
        {
            return _cache.ContainsKey(key);
        }
    }
}