using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MerchandiserBot.Dialogs;
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
           var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetMenu();
                await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message!=null && (message.Text.Equals("險種分類") || message.Text.Equals("關鍵字")))
            {
                ProdSearch_Selc = message.Text;
                context.Done(context);
            }
            else if (RootDialog.GetBack2home()) //回首頁
            {
                context.Done(context);
            }
            else
            {
                await context.PostAsync("無法辨識指令，請再次選擇您要進行的搜尋方式...");
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
        //    var heroCard = new ThumbnailCard
        //    {
        //        Title = "商品搜尋",
        //        Subtitle = "請選擇您要進行的搜尋方式...",
        //        Buttons = new List<CardAction>() {
        //            new CardAction(ActionTypes.ImBack, "險種分類", value: "險種分類"),
        //            new CardAction(ActionTypes.ImBack, "關鍵字", value: "關鍵字") }
        //    };

        //    return heroCard.ToAttachment();

        //}

        public static IList<Attachment> GetMenu()
        {
            return new List<Attachment>()
            {
                GetThumbnailCard(
                    "商品搜尋",
                    "請選擇您要進行的搜尋方式...",
                    null,  
                    null,
                    new List<CardAction>(){ new CardAction(ActionTypes.ImBack, "險種分類", value: "險種分類"),
                    new CardAction(ActionTypes.ImBack, "關鍵字", value: "關鍵字") }),
            };
        }
        private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, List<CardAction> cardAction)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = cardAction,
            };

            return heroCard.ToAttachment();
        }

        public static string GetProdSearchSelc()
        {
            return ProdSearch_Selc;
        }
    }
}