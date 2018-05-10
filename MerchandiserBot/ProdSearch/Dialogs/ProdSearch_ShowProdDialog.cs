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

        /// <summary>
        /// 搜尋商品
        /// 1.判斷是以 "關鍵字" 或是 "險種" 進行搜尋
        /// 2.1 關鍵字--> 檢查是否有與關鍵字符合的 "商品名稱"
        ///     否-->　搜尋 LUIS 判斷出來的商品 "類別"
        ///     是-->　搜尋商品符合 "關鍵字" 且與LUIS判別結果相同的 "類別"
        /// 2.2 類別--> 搜尋 LUIS 判別出來的商品類別
        /// </summary>
        /// <returns></returns>
        private static IList<Attachment> GetProdAttachment()
        {
            if (ProdSearchDialog.GetProdSearchSelc().Equals("關鍵字"))
            {
                var result = MessagesController.ltProd.FindAll(x => x.Cata.Contains(keyword));
                int count = 0;
                if (Checkdata())
                {
                    result = MessagesController.ltProd.FindAll(x => x.Cata.Contains(keyword) && x.Name.Contains(ProdSearch_KeywordDialog.getnonLuisKeyword()));
                }
                //var result = MessagesController.ltProd.FindAll(x => x.Cata.Contains(keyword) && x.Name.Contains(keyword));
                List<Attachment> list = new List<Attachment>();
                foreach (var item in result)
                {
                    if (count <10)
                    {
                        list.Add(
                            GetProduct(item.Name.ToString(), item.Cata.ToString() + "  " + item.PublishDate.ToString(), new CardImage(url: item.ImgURL.ToString()), new CardAction(ActionTypes.OpenUrl, "查看更多", value: item.DMURL.ToString()))
                            );
                    }
                    count++;
                }
                count = 0;
                return list;
            }
            else
            {
                var result = MessagesController.ltProd.FindAll(x => x.Cata.Contains(keyword));
                int count = 0;
                List<Attachment> list = new List<Attachment>();
                foreach (var item in result)
                {
                    if (count < 10)
                    {
                        list.Add(
                            GetProduct(item.Name.ToString(), item.Cata.ToString() + "  " + item.PublishDate.ToString(), new CardImage(url: item.ImgURL.ToString()), new CardAction(ActionTypes.OpenUrl, "查看更多", value: item.DMURL.ToString()))
                            );
                    }
                    count++;
                }
                count = 0;
                return list;
            }

        }

        //檢查是否有此筆商品
        private static Boolean Checkdata()
        {
            var ss = ProdSearch_KeywordDialog.getnonLuisKeyword();
            var result = MessagesController.ltProd.FindAll(x => x.Name.Contains(ProdSearch_KeywordDialog.getnonLuisKeyword()));
            if (result.Count.Equals(0))
            {
                return false;
            }
            else
            {
                return true;
            }
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