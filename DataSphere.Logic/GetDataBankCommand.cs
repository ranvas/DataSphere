using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot;

namespace DataSphere.Logic
{
    public class GetDataBankCommand : DataSphereCommand
    {
        public override string? Command { get; set; } = "/s";

        public override async Task Execute(string? param, Update executionContext, DataSphereBot service)
        {
            var chatId = GetChatId(executionContext);
            var result = await service.GetDataBank(param ?? string.Empty, chatId);
            await service.LogUser(chatId, executionContext?.Message?.Chat?.Username ?? "unknown user");
            await service.LogAction(chatId, Command ?? string.Empty, param ?? "пусто", result);
        }
    }
}
