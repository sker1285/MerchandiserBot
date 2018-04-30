using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_KeywordDialog : IDialog<IMessageActivity>
    {
        static Boolean keywordCheck = false;
        static string keyword;
        public async Task StartAsync(IDialogContext context)
        {
            keywordCheck = true;
            await context.PostAsync("請輸入關鍵字：");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            keyword = activity.Text;
            if (result!=null)
            {
                await context.PostAsync($"將進行搜尋{keyword}...");
                context.Done(context);

            }
            // TODO: Put logic for handling user message here
        }

        public static void setcheck()
        {
            keywordCheck = false;
        }

        public static Boolean getcheck()
        {
            return keywordCheck;
        }
       
    }
}