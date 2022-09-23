using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot;

namespace DataSphere.Logic
{
    public class GetUserKnowledgeCommand : DataSphereCommand
    {
        public override string? Command { get; set; } = "/getuserknowledge";

        public override async Task Execute(string? param, Update executionContext, DataSphereBot service)
        {
            var knowledge = await service.GetUserKnowledge(param ?? "");
            var chatId = GetChatId(executionContext);
            if (knowledge.Count == 0)
            {
                await service.SendTextMessage(chatId, $"Пользователь не знает ничего или пользователь не найден");
            }
            else
            {
                await service.SendTextMessage(chatId, $"Пользователь знает ключевые слова: {string.Join("; ",knowledge)}");
            }
        }

    }
}
