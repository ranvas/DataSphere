using GoogleSheet.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Cache
{
    public class UpdateCacheService
    {
        public UpdateCacheService(string key, 
            string sheetId, 
            IGoogleSheetRange range, 
            List<IList<object>> items, 
            Func<List<IList<object>>, string, IGoogleSheetRange, Task> updateFunc)
        {
            Key = key;
            Items = new();
            AddNewItems(items);
            Range = range;
            UpdateFunc = updateFunc;
            SheetId = sheetId;
        }

        public void AddNewItems(List<IList<object>> items)
        {
            foreach (var item in items)
            {
                AddNewItem(item);
            }
        }

        public void AddNewItem(IList<object> item)
        {
            Items.Enqueue(item);
        }

        public void RunUpdate()
        {
            var list = Items.ToList();
            UpdateFunc.Invoke(list, SheetId, Range);
        }

        public IGoogleSheetRange Range { get; set; }
        public string SheetId { get; set; }
        public string Key { get; set; }
        public ConcurrentQueue<IList<object>> Items { get; set; }
        public Func<List<IList<object>>, string, IGoogleSheetRange, Task> UpdateFunc { get; set; }
    }
}
