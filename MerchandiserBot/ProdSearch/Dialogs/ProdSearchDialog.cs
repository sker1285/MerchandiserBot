using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearchDialog : IDialog<IMessageActivity>
    {
        static string ProdSearch_Selc;

        public async Task StartAsync(IDialogContext context)
        {
            var msg = context.MakeMessage();
            var attachment = GetMenu();
            msg.Attachments.Add(attachment);
            await context.PostAsync(msg);

            context.Wait(MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            ProdSearch_Selc = message.Text;

            if (message!=null && (message.Text.Equals("險種分類") || message.Text.Equals("關鍵字")))
            {
                context.Done(context);
            }
            else
            {
                await context.PostAsync("無法辨識指令，請再次選擇您要進行的搜尋方式...");
                var msg = context.MakeMessage();
                var attachment = GetMenu();
                msg.Attachments.Add(attachment);
                await context.PostAsync(msg);

                context.Wait(MessageReceivedAsync);
            }

            // TODO: Put logic for handling user message here
            
            //context.Wait(MessageReceivedAsync);
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

        public static string GetProdSearchSelc()
        {
            return ProdSearch_Selc;
        }
    }
}