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
        static string pwd;

        public async Task StartAsync(IDialogContext context)
        {
            if (pwd.Equals("AD"))
            {
                await context.PostAsync("AD密碼已重設，請至信箱收取");
                await ShowOptionsAsync(context);
                context.Done(context);
            }
            else if (pwd.Equals("內網"))
            {
                await context.PostAsync("內網密碼已重設，請至信箱收取");
                await ShowOptionsAsync(context);
                context.Done(context);
            }
            
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

        }

        public static void setpwd(string n)
        {
            pwd = n;
        }
    }
}