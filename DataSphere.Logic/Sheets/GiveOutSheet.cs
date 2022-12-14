using DataSphere.Logic.Models;
using GoogleSheet;
using GoogleSheet.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSphere.Logic.Sheets
{
    public class GiveOutSheet : SheetServiceBase<GiveOut>
    {
        public GiveOutSheet(ISheetAdapter adapter, ISheetCache cache) : base(adapter, cache)
        {
        }

        protected override string SpreadSheetId { get; set; } = "1fe9FEL7s2MuaSbvJN39eJMTzDRKhlo6852uTLyXt8sE";
        protected override GoogleSheetRange Range { get; set; } =
            new GoogleSheetRange
            {
                List = "giveOut",
                StartColumn = "A",
                StartRow = 1,
                EndRow = 10000,
                EndColumn = "D"
            };
    }
}
