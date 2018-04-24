using AdaptiveCards;
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
    public class PwdResetDialog : IDialog<IMessageActivity>
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
            if (message.Text.Contains("AD密碼"))
            {
                await context.PostAsync("AD密碼已重設，請至信箱收取");
                await ShowOptionsAsync(context);
                context.Done(context);

            }
            else if (message.Text.Contains("內網密碼"))
            {
                await context.PostAsync("內網密碼已重設，請至信箱收取");
                await ShowOptionsAsync(context);
                context.Done(context);
            }
            else
            {
                await context.PostAsync("請選擇表單中選項");
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
                    new CardAction(ActionTypes.ImBack, "內網密碼", value: "內網密碼")
                }
            };
            return heroCard.ToAttachment();
        }

        private async Task ShowOptionsAsync(IDialogContext context)
        {
            var card = new AdaptiveCard();
            card.Actions = new List<ActionBase>()
            {
                new OpenUrlAction
                {
                  Title = "前往登入",
                  Url = "http://sklweb.skl.com.tw/MainWeb/IntraNet/Main_Page/Main/default.aspx",
                }
            };

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            var reply = context.MakeMessage();
            reply.Attachments.Add(attachment);

            await context.PostAsync(reply, CancellationToken.None);

            context.Wait(MessageReceivedAsync);
        }
    }
}