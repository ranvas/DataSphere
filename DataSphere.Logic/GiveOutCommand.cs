using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DataSphere.Logic
{
    public class GiveOutCommand : DataSphereCommand
    {
        public override string? Command { get; set; } = "/giveout";

        public override async Task Execute(string? param, Update executionContext, DataSphereBot service)
        {
            await service.GiveOut();
            await service.SendTextMessage(GetChatId(executionContext), "рассылка успешно завершена");
        }
    }
}
