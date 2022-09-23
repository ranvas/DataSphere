using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Abstractions
{
    public interface ISheetCache
    {
        Task<TCache> GetOrCreateAsync<TCache>(Func<Task<TCache>> update, string id, bool sliding, TimeSpan span);
        void AddToUpdateAsync(string key, string sheetId, IGoogleSheetRange range,
            IList<object> item, Func<List<IList<object>>, string, IGoogleSheetRange, Task> updateFunc);
    }
}
