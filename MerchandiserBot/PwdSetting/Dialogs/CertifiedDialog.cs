using MerchandiserBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MerchandiserBot.PwdSetting.Dialogs
{
    [Serializable]
    public class CertifiedDialog : IDialog<IMessageActivity>
    { 
        static string Id;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("請問您的員編是多少?");

            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (1 != 1)     //此員編第一次登入
            {

            }
            else if (RootDialog.GetBack2home()) //回首頁
            {
                context.Done(context);
            }
            else
            {
                Id = message.Text;
                

                context.Done(context);
            }
        }

        public static string getId()
        {
            return Id;
        }
    }
}