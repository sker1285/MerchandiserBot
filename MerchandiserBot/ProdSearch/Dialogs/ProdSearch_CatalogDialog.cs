using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MerchandiserBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_CatalogDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //呼叫ThumbnailCard
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetMenu();
            await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (result!=null && TextCheck(message.Text))
            {
                ProdSearch_KeywordDialog.setKeyword(message.Text);
                context.Done(context);
            }
            else if (RootDialog.GetBack2home()) //回首頁
            {
                context.Done(context);
            }
            else
            {
                await context.PostAsync("無法辨識指令，請再次選擇您要的種類...");
                //呼叫ThumbnailCard
                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetMenu();
                await context.PostAsync(reply);

                context.Wait(MessageReceivedAsync);
            }

            // TODO: Put logic for handling user message here
        }

        //private static Attachment GetMenu()
        //{
        //    var heroCard = new HeroCard
        //    {
        //        Title = "選擇險種",
        //        Subtitle = "請選擇您要的種類...",
        //        Buttons = new List<CardAction>() {
        //            new CardAction(ActionTypes.ImBack, "意外傷害", value: "意外傷害"),
        //            new CardAction(ActionTypes.ImBack, "年金型", value: "年金型"),
        //            new CardAction(ActionTypes.ImBack, "利變壽", value: "利變壽"),
        //            new CardAction(ActionTypes.ImBack, "醫療型", value: "醫療型"),
        //            new CardAction(ActionTypes.ImBack, "壽險", value: "壽險") }
        //    };

        //    return heroCard.ToAttachment();

        //}
        public static IList<Attachment> GetMenu()
        {
            return new List<Attachment>()
            {
                GetThumbnailCard(
                    "選擇險種",
                    "請選擇您要的種類...",
                    null,
                    null,
                    new List<CardAction>(){
                    new CardAction(ActionTypes.ImBack, "意外傷害", value: "意外傷害"),
                    new CardAction(ActionTypes.ImBack, "年金型", value: "年金型"),                    
                    new CardAction(ActionTypes.ImBack, "醫療型", value: "醫療型"),
                    
                    }),
            };
        }
        private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = cardAction,
            };

            return heroCard.ToAttachment();
        }

        private Boolean TextCheck(string message)
        {
            if (message.Equals("意外傷害") || message.Equals("年金型") || message.Equals("利變壽") || message.Equals("醫療型") || message.Equals("壽險"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}