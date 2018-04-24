using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MerchandiserBot.Dialogs
{
    [Serializable]

    public class HomeDialog : IDialog<IMessageActivity>
    {
        static string option;

        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceviedAsync 
             *  to process that message. */

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsMovie();

            await context.PostAsync(reply);
            context.Wait(this.MessageReceivedAsync);


        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("忘記密碼"))
            {
                option = "1";
                context.Done(context);
            }
            else if (message.Text.ToLower().Contains("商品搜尋"))
            {
                option = "2";
                context.Done(context);
            }
            else if (message.Text.ToLower().Contains("推播訊息"))
            {
                option = "3";
                context.Done(context);
            }
            else
            {
                await context.PostAsync("請選擇表單中選項");
            }
        }
        public static IList<Attachment> GetCardsMovie()
        {
            return new List<Attachment>()
            {
                GetThumbnailCard(
                    "  小光機器人",
                    "你好~我是小光" +"\n\r"+"很高興為你服務",
                    null,
                    new CardImage(url: "https://www.energypark.org.tw/_admin/_upload/topGoal/GoalCom/245/photo4/%E6%96%B0%E5%85%89%E5%90%88%E7%BA%96LOGO.JPG"),
                    new List<CardAction>(){ new CardAction(ActionTypes.ImBack, "忘記密碼", value: "忘記密碼"),
                        new CardAction(ActionTypes.ImBack, "商品搜尋", value: "商品搜尋"),
                        new CardAction(ActionTypes.ImBack, "推播訊息", value: "推播訊息") }),
            };
        }

        private static Attachment GetHeroCard(CardImage cardImage, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {

                Images = new List<CardImage>() { cardImage },
                Buttons = cardAction,
            };

            return heroCard.ToAttachment();
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

        public static string getoption()
        {
            return option;
        }
    }


}