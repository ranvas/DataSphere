using DataSphere.Logic;
using GoogleSheet;
using GoogleSheet.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataSphereConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await BotProductionStart();

            Console.WriteLine("in the end");
            Console.ReadLine();

        }

        static async Task BotProductionStart()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var botToken = config.GetSection("botToken").Value;
            var bike = new GameProductionBike();
            await bike.Run(botToken);
        }
    }
}
