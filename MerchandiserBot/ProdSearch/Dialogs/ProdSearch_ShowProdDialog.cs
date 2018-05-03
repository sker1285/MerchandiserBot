using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_ShowProdDialog : IDialog<IMessageActivity>
    {
        static string keyword;
        public async Task StartAsync(IDialogContext context)
        {
            keyword = ProdSearch_KeywordDialog.getKeyword();

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetProdAttachment();
            await context.PostAsync(reply);
            context.Done(context);
        }

        private static IList<Attachment> GetProdAttachment()
        {
            var result = MessagesController.ltProd.FindAll(x => x.Cata.Contains(keyword));
            List<Attachment> list = new List<Attachment>();
            foreach (var item in result)
            {
                list.Add(
                    GetProduct(item.Name.ToString(), item.Cata.ToString()+"  "+item.PublishDate.ToString(), new CardImage(url: "https://sklbot.blob.core.windows.net/merchandiserbot/A01.PNG"), new CardAction(ActionTypes.OpenUrl, "查看更多", value: item.DMURL.ToString()))
                    );
            }
            return list;
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