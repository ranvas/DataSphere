using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot;

namespace DataSphere.Logic
{
    public abstract class DataSphereCommand : BotCommandBase
    {
        public abstract Task Execute(string? param, Update executionContext, DataSphereBot service);

        public override async Task Execute(string? param, Update executionContext, BotAdapterBase service)
        {
            if (service is DataSphereBot)
                await Execute(param, executionContext, (DataSphereBot)service);
        }

        public string GetChatId(Update executionContext)
        {
            return (executionContext.Message?.Chat?.Id ?? 0).ToString();
        }
    }
}
