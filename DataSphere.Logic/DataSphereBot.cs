using DataSphere.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TgBot;

namespace DataSphere.Logic
{
    public class DataSphereBot : BotServiceBase
    {
        DataSphereSheetManager _manager { get; set; }

        public DataSphereBot(string token, bool webHooks, DataSphereSheetManager manager) : base(token, webHooks)
        {
            _manager = manager;
        }

        public async Task<List<string>> GetTags(string tag)
        {
            var list = await _manager.DataBank.GetAllItems();
            var tags = list.Where(t => t.DataContext != null && t.DataContext.Contains($"#{tag}"));
            return tags.Select(t => t.DataKeyWord ?? "").ToList();
        }

        public async Task GiveOut()
        {
            var distributionList = await _manager.GiveOuts.GetAllItems();
            var users = await _manager.Users.GetAllItems();
            await _manager.GiveOuts.ClearAllItems();
            foreach (var item in distributionList)
            {
                if (item.State == "новый")
                {
                    var error = await SendInformation(item, users);
                    if (!string.IsNullOrEmpty(error))
                    {
                        item.State = "проблемы";
                        item.Error = error;
                    }
                    else
                    {
                        item.State = "успешно";
                    }
                    item.DateTime = GetTime();
                }
            }
            await _manager.GiveOuts.PostItems(distributionList);
        }

        private async Task<string> SendInformation(GiveOut giveOut, List<User> users)
        {
            try
            {
                User? user;
                if (!string.IsNullOrEmpty(giveOut.TgUser))
                {
                    var userToFind = giveOut.TgUser.ToLower();
                    user = users.FirstOrDefault(u => u.TgUser == userToFind);

                }
                else if (!string.IsNullOrEmpty(giveOut.CharName))
                {
                    var userToFind = giveOut.CharName.ToLower();
                    user = users.FirstOrDefault(u => u.CharName == userToFind);
                }
                else
                {
                    return $"Необходимо указать пользователя или персонажа";
                }
                if (user == null || user.TgId == null)
                    return $"пользователь не найден";
                await SendTextMessage(user.TgId, giveOut.Text ?? "пустое сообщение");
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return String.Empty;
        }

        public async Task<List<string>> GetUserKnowledge(string userName)
        {
            var users = await _manager.Users.GetAllItems();
            userName = userName.ToLower();
            var user = users.FirstOrDefault(u => u.TgUser == userName);
            if (user == null)
                return new();
            var knowledge = await _manager.Logs.GetAllItems();
            return knowledge
                .Where(l => l.TgId == user.TgId && l.Result == "Success")
                .Select(k => k.DataKeyword ?? "")
                .Distinct()
                .ToList();
        }

        public async Task<bool> GetDataBank(string param, string chatId)
        {
            var items = await _manager.DataBank.GetAllItems();
            var item = items.FirstOrDefault(i => i.DataKeyWord?.ToLower() == param.ToLower());
            if (item != null)
            {
                await SendTextMessage(chatId, item.DataContext ?? "Пусто");
                return true;
            }
            await SendTextMessage(chatId, "ошибка");
            return false;
        }

        public async Task LogAction(string userId, string action, string param, bool result)
        {
            var log = new Log()
            {
                TgId = userId,
                Action = action,
                DataKeyword = param.ToLower(),
                DateTime = GetTime(),
                Result = result ? "Success" : "Error"
            };
            await _manager.Logs.PostItem(log);
        }

        private string GetTime()
        {
            TimeZoneInfo rstInfo = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            return TimeZoneInfo.ConvertTime(DateTimeOffset.Now.UtcDateTime, rstInfo).ToString("dd.MM : HH:mm:ss");
        }

        public async Task LogUser(string userId, string userName)
        {
            var users = await _manager.Users.GetAllItems();
            if (!users.Any(u => u.TgId == userId))
            {
                var user = new User
                {
                    TgId = userId,
                    TgUser = userName.ToLower()
                };
                await _manager.Users.PostItem(user);
            }
        }
    }
}