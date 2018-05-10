using AdaptiveCards;
using MerchandiserBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MerchandiserBot.PwdSetting.Dialogs
{
    [Serializable]
    public class ChoicePwdDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var msg = context.MakeMessage();
            var attachment = GetPwd();
            msg.Attachments.Add(attachment);
            await context.PostAsync(msg);
            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (message.Text.Equals("AD密碼"))
            {
                await context.PostAsync("AD密碼登入相關頁面如下,"+"\n\r"+"請選擇您要修改密碼之頁面");
                context.Done($"AD");

            }
            else if (message.Text.Equals("內網密碼"))
            {
                await context.PostAsync("內網密碼登入相關頁面如下," + "\n\r" + "請選擇您要修改密碼之頁面");
                context.Done($"內網");
            }
            else if (message.Text.Equals("不知道"))
            {
                await context.PostAsync("密碼登入相關頁面如下");
                context.Done($"Idk");
            }
            else if (RootDialog.GetBack2home()) //回首頁
            {
                context.Done("");
            }
            else
            {
                await context.PostAsync("請選擇表單中選項");
                var msg = context.MakeMessage();
                var attachment = GetPwd();
                msg.Attachments.Add(attachment);
                await context.PostAsync(msg);
                context.Wait(this.MessageReceivedAsync);
            }


        }

        private static Attachment GetPwd()
        {
            var heroCard = new HeroCard
            {
                Title = "請問要重設哪個密碼?",
                Subtitle = "選擇一個",
                Buttons = new List<CardAction>() {
                    new CardAction(ActionTypes.ImBack, "AD密碼", value: "AD密碼"),
                    new CardAction(ActionTypes.ImBack, "內網密碼", value: "內網密碼"),
                    new CardAction(ActionTypes.ImBack, "不知道", value: "不知道")
                }
            };
            return heroCard.ToAttachment();
        }

       
    }
}