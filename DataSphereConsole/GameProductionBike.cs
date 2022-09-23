using DataSphere.Logic;
using GoogleSheet.Cache;
using GoogleSheet;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSphereConsole
{
    public class GameProductionBike
    {
        DataSphereBot dataSphereBot;
        public async Task Run(string botToken)
        {
            var cache = new SheetCache(new MemoryCache(new MemoryCacheOptions()));
            await RunBot(cache, botToken);
            await RunGoogle(cache);
        }

        private Task RunBot(SheetCache cache, string botToken)
        {

            dataSphereBot = new DataSphereBot(botToken, true, new DataSphereSheetManager(new SheetAdapter(), cache));
            dataSphereBot.TryAddCommand(new GetDataBankCommand());
            dataSphereBot.TryAddCommand(new GetUserKnowledgeCommand());
            dataSphereBot.TryAddCommand(new CompareUserKnowledgeCommand());
            dataSphereBot.TryAddCommand(new GiveOutCommand());
            dataSphereBot.TryAddCommand(new GetTagsCommand());
            Console.WriteLine("read started");
            return Task.CompletedTask;
        }

        private async Task RunGoogle(SheetCache cache)
        {
            Console.WriteLine("update started");
            while (true)
            {
                await cache.RunUpdate();
                await Task.Delay(10000);
            }
        }

    }
}
