using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_CatalogDialog : IDialog<IMessageActivity>
    {
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
            if (result!=null)
            {
                //if (message.Text.Equals("意外險"))
                //{
                //    ProdSearch_KeywordDialog.setKeyword("意外");
                //}
                //else if (message.Text.Equals("壽險"))
                //{
                //    ProdSearch_KeywordDialog.setKeyword("壽險");
                //}
                ProdSearch_KeywordDialog.setKeyword(message.Text);
                context.Done(context);
            }
            else
            {
                await context.PostAsync("無法辨識指令，請再次選擇您要的種類...");
                var msg = context.MakeMessage();
                var attachment = GetMenu();
                msg.Attachments.Add(attachment);
                await context.PostAsync(msg);

                context.Wait(MessageReceivedAsync);
            }

            // TODO: Put logic for handling user message here
        }

        private static Attachment GetMenu()
        {
            var heroCard = new HeroCard
            {
                Title = "選擇險種",
                Subtitle = "請選擇您要的種類...",
                Buttons = new List<CardAction>() {
                    new CardAction(ActionTypes.ImBack, "意外傷害", value: "意外傷害"),
                    new CardAction(ActionTypes.ImBack, "年金型", value: "年金型"),
                    new CardAction(ActionTypes.ImBack, "利變壽", value: "利變壽"),
                    new CardAction(ActionTypes.ImBack, "醫療型", value: "醫療型"),
                    new CardAction(ActionTypes.ImBack, "壽險", value: "壽險") }
            };

            return heroCard.ToAttachment();

        }
    }
}