using GoogleSheet.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleSheet.Cache
{
    public class SheetCache : ISheetCache
    {
        IMemoryCache _cache;
        ConcurrentDictionary<string, UpdateCacheService> _cacheServices = new();


        public SheetCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<TCache> GetOrCreateAsync<TCache>(Func<Task<TCache>> update, string key, bool sliding, TimeSpan span)
        {
            TCache cacheEntry;

            cacheEntry = await _cache.GetOrCreateAsync(key, entry =>
            {
                if (sliding)
                    entry.SlidingExpiration = span;
                else
                    entry.AbsoluteExpiration = DateTimeOffset.Now.Add(span);
                return update();
            });
            return cacheEntry;
        }

        public void AddToUpdateAsync(string key, string sheetId, IGoogleSheetRange range,
            IList<object> item, Func<List<IList<object>>, string, IGoogleSheetRange, Task> updateFunc)
        {
            if (_cacheServices.ContainsKey(key))
                _cacheServices[key].AddNewItem(item);
            else
            {
                var service = new UpdateCacheService(key, sheetId, range, new List<IList<object>>() { item }, updateFunc);
                _cacheServices.TryAdd(key, service);
            }
        }

        public async Task RunUpdate()
        {
            if (_cacheServices.Count == 0)
                await Task.CompletedTask;
            foreach (var item in _cacheServices)
            {
                _cacheServices.Remove(item.Key, out UpdateCacheService? cachedItem);
                cachedItem?.RunUpdate();
            }
        }

    }
}