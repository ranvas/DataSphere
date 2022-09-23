using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using GoogleSheet.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet
{
    public abstract class SheetServiceBase<T> : ISheetService<T> where T : class, new()
    {
        ISheetAdapter _adapter;
        ISheetCache _cache;

        protected abstract string SpreadSheetId { get; set; }
        protected abstract GoogleSheetRange Range { get; set; }
        protected virtual TimeSpan DataLoadCacheTime { get; set; } = new TimeSpan(0, 0, 10);

        public SheetServiceBase(ISheetAdapter adapter, ISheetCache cache)
        {
            _adapter = adapter;
            _cache = cache;
        }

        public async Task<List<T>> GetAllItems()
        {
            return await GetOrCreateAllItems();
        }

        public async Task ClearAllItems()
        {
            var clearRange = new GoogleSheetRange { List = Range.List, StartRow = Range.StartRow+1, EndColumn = Range.EndColumn, StartColumn = Range.StartColumn, EndRow = Range.EndRow };
            await _adapter.ClearItems(SpreadSheetId, clearRange);
        }

        public async Task PostItem(T item)
        {
            var properties = await GetOrCreateProperties();
            var headers = await GetOrCreateHeaders();
            var data = new List<IList<object>> { Map(item, headers, properties) };
            AddToUpdateCache(data);
        }

        public async Task PostItems(List<T> items)
        {
            var properties = await GetOrCreateProperties();
            var headers = await GetOrCreateHeaders();
            var data = MapList(items, headers, properties);
            AddToUpdateCache(data);
        }

        #region private

        private async Task<List<T>> LoadAllItems()
        {
            var properties = await GetOrCreateProperties();
            var headers = await GetOrCreateHeaders();
            var data = _adapter.GetDataFromRange(SpreadSheetId, Range).Skip(1).ToList();
            return Map(data, headers, properties);
        }

        private List<object> Map(T item, List<string> headers, Dictionary<string, PropertyInfo> properties)
        {
            var items = new List<object>();
            if (headers.Count == 0)
                return items;
            for (int i = 0; i < headers.Count; i++)
            {
                if (!properties.TryGetValue(headers[i], out PropertyInfo? info))
                    items.Add(string.Empty);
                else
                    items.Add(info.GetValue(item) ?? string.Empty);
            }
            return items;
        }

        private List<IList<object>> MapList(List<T> items, List<string> headers, Dictionary<string, PropertyInfo> properties)
        {
            var result = new List<IList<object>>();
            foreach (var item in items)
            {
                result.Add(Map(item, headers, properties));
            }
            return result;
        }

        private List<T> Map(IList<IList<object>> data, List<string> headers, Dictionary<string, PropertyInfo> properties)
        {
            var items = new List<T>();
            if (headers.Count == 0)
                return items;
            foreach (var row in data)
            {
                var item = new T();
                for (int i = 0; i < row.Count; i++)
                {
                    if (headers.Count <= i)
                        continue;
                    if (!properties.TryGetValue(headers[i], out PropertyInfo? info))
                        continue;
                    info.SetValue(item, row[i].ToString());
                }
                items.Add(item);
            }
            return items;
        }

        private List<string> GetHeaders()
        {
            var headers = new List<string>();
            var data = _adapter.GetDataFromRange(SpreadSheetId, Range).FirstOrDefault();
            if (data == null)
                return headers;
            foreach (var item in data)
            {
                headers.Add(item?.ToString() ?? string.Empty);
            }
            return headers;
        }

        private Dictionary<string, PropertyInfo> GetProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dictionary = new Dictionary<string, PropertyInfo>();
            foreach (var p in properties)
            {
                var attribute = p.GetCustomAttributes<DisplayAttribute>().FirstOrDefault();
                if (attribute == null || string.IsNullOrEmpty(attribute.Name))
                    continue;
                if (p.PropertyType != typeof(string)) { continue; }
                if (!p.CanWrite || !p.CanRead) { continue; }
                var mget = p.GetGetMethod(false);
                var mset = p.GetSetMethod(false);
                if (mget == null) { continue; }
                if (mset == null) { continue; }
                if (!dictionary.TryAdd(attribute.Name, p))
                {
                    //TODO log it
                }
            }
            return dictionary;
        }

        #endregion

        #region cache

        private void AddToUpdateCache(List<IList<object>> items)
        {
            var key = typeof(T).ToString();
            foreach (var item in items)
            {
                _cache.AddToUpdateAsync(key, SpreadSheetId, Range, item, _adapter.Post);
            }
        }

        private async Task<Dictionary<string, PropertyInfo>> GetOrCreateProperties()
        {
            var id = $"property";
            var type = typeof(T);
            var key = $"{id}_{type}";
            return await _cache.GetOrCreateAsync(() => Task.FromResult(GetProperties(typeof(T))), key, true, new TimeSpan(1, 0, 0));
        }

        private async Task<List<string>> GetOrCreateHeaders()
        {
            var id = $"headers";
            var type = typeof(T);
            var key = $"{id}_{type}";
            return await _cache.GetOrCreateAsync(() => Task.FromResult(GetHeaders()), key, true, new TimeSpan(1, 0, 0));
        }

        private async Task<List<T>> GetOrCreateAllItems()
        {
            var id = $"allItems";
            var type = typeof(T);
            var key = $"{id}_{type}";
            return await _cache.GetOrCreateAsync(() => LoadAllItems(), key, true, DataLoadCacheTime);
        }



        #endregion

    }
}