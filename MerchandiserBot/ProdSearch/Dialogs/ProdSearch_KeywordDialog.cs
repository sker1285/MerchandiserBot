using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_KeywordDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("請輸入關鍵字：");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            if (result!=null)
            {
                context.Done(context);
            }
            // TODO: Put logic for handling user message here

            //context.Wait(MessageReceivedAsync);
        }
    }
}