using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearchDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {

            var msg = context.MakeMessage();
            var attachment = GetMenu();
            msg.Attachments.Add(attachment);
            await context.PostAsync(msg);

            context.Wait(MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;

            // TODO: Put logic for handling user message here

            context.Wait(MessageReceivedAsync);
        }

        private static Attachment GetMenu()
        {
            var heroCard = new HeroCard
            {
                Title = "商品搜尋",
                Subtitle = "請選擇您要進行的搜尋方式...",
                Buttons = new List<CardAction>() {
                    new CardAction(ActionTypes.ImBack, "險種分類", value: "險種分類"),
                    new CardAction(ActionTypes.ImBack, "關鍵字", value: "關鍵字") }
            };

            return heroCard.ToAttachment();

        }
    }
}