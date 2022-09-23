using DataSphere.Logic.Models;
using DataSphere.Logic.Sheets;
using GoogleSheet.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSphere.Logic
{
    public class DataSphereSheetManager
    {
        ISheetAdapter _adapter;
        ISheetCache _cache;

        public DataSphereSheetManager(ISheetAdapter adapter, ISheetCache cache)
        {
            _adapter = adapter;
            _cache = cache;
            DataBank = new DataBankSheet(_adapter, _cache);
            Users = new UserSheet(_adapter, _cache);
            Logs = new LogSheet(_adapter, _cache);
            GiveOuts = new GiveOutSheet(_adapter, _cache);
        }
        public ISheetService<DataBank> DataBank { get; set; }
        public ISheetService<User> Users { get; set; }
        public ISheetService<Log> Logs { get; set; }
        public ISheetService<GiveOut> GiveOuts { get; set; }
    }
}
