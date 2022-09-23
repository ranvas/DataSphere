using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DataSphere.Logic
{
    public class GetTagsCommand : DataSphereCommand
    {
        public override string? Command { get; set; } = "/gettags";

        public override async Task Execute(string? param, Update executionContext, DataSphereBot service)
        {
            if (string.IsNullOrEmpty(param))
            {
                await service.SendTextMessage(GetChatId(executionContext), "необходимо указать тег для поиска");
                return;
            }
            var list = await service.GetTags(param);
            await service.SendTextMessage(GetChatId(executionContext), $"тег #{param} встречается в текстах с ключами: {string.Join("; ", list)}");
        }
    }
}
