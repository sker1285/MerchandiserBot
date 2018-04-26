using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_ShowProdDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetProdAttachment();
            await context.PostAsync(reply);
            context.Done(context);
            //context.Wait(MessageReceivedAsync);
        }

        //private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        //{
        //    var activity = await result;
        //    if (activity!=null)
        //    {
        //        context.Done(context);
        //    }
        //    context.Done(context);
        //    // TODO: Put logic for handling user message here
        //}

        private static IList<Attachment> GetProdAttachment()
        {
            return new List<Attachment>()
            {
                GetProduct(
                    "活力平安傷害保險",
                    "活力系列專區",
                    new CardImage(url: "https://www.energypark.org.tw/_admin/_upload/topGoal/GoalCom/245/photo4/%E6%96%B0%E5%85%89%E5%90%88%E7%BA%96LOGO.JPG"),
                    new CardAction(ActionTypes.OpenUrl, "查看更多", value: "https://www.google.com.tw/")),
                GetProduct(
                    "活力平安傷害保險",
                    "活力系列專區",
                    new CardImage(url: "https://www.energypark.org.tw/_admin/_upload/topGoal/GoalCom/245/photo4/%E6%96%B0%E5%85%89%E5%90%88%E7%BA%96LOGO.JPG"),
                    new CardAction(ActionTypes.OpenUrl, "查看更多", value: "https://www.google.com.tw/")),
                GetProduct(
                    "活力平安傷害保險",
                    "活力系列專區",
                    new CardImage(url: "https://www.energypark.org.tw/_admin/_upload/topGoal/GoalCom/245/photo4/%E6%96%B0%E5%85%89%E5%90%88%E7%BA%96LOGO.JPG"),
                    new CardAction(ActionTypes.OpenUrl, "查看更多", value: "https://www.google.com.tw/")),
                GetProduct(
                    "活力平安傷害保險",
                    "活力系列專區",
                    new CardImage(url: "https://www.energypark.org.tw/_admin/_upload/topGoal/GoalCom/245/photo4/%E6%96%B0%E5%85%89%E5%90%88%E7%BA%96LOGO.JPG"),
                    new CardAction(ActionTypes.OpenUrl, "查看更多", value: "https://www.google.com.tw/")),
            };
        }

        private static Attachment GetProduct(string title, string subtitle, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
    }
}