using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DataSphere.Logic
{
    public class CompareUserKnowledgeCommand : DataSphereCommand
    {
        public override string? Command { get; set; } = "/compareuserknowledge";

        public override async Task Execute(string? param, Update executionContext, DataSphereBot service)
        {
            var users = param?.Split(',');
            var chatId = GetChatId(executionContext);
            if (users == null || users.Length != 2)
            {
                await service.SendTextMessage(chatId, $"неверный ввод");
                return;
            }
            var user1Know = await service.GetUserKnowledge(users[0].Trim());
            var user2Know = await service.GetUserKnowledge(users[1].Trim());
            var result = new List<string>();
            foreach (var word in user2Know)
            {
                if (!user1Know.Contains(word))
                {
                    result.Add(word);
                }
            }
            await service.SendTextMessage(chatId, $"Пользователь {users[0]} может узнать у пользователя {users[1]} ключевые слова: {string.Join("; ", result)}");
        }
    }
}
