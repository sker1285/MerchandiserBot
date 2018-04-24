using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_FormDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;

            // TODO: Put logic for handling user message here

            context.Wait(MessageReceivedAsync);
        }

        private async Task ShowOptionsAsync(IDialogContext context)
        {
            
        }
    }
}