using MerchandiserBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MerchandiserBot.PwdSetting.Dialogs
{
    [Serializable]
    public class InwebPwdDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();
            await context.PostAsync(reply);

            var msg = context.MakeMessage();
            var attachment = Getback();
            msg.Attachments.Add(attachment);
            await context.PostAsync(msg);

            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (message.Text.Equals("確認"))
            {
                await context.PostAsync("將進行修改 [ 內 網 ] 密碼");
                context.Done($"");
            }
            else if (message.Text.Equals("回上一步驟"))
            {
                context.Done($"goback");
            }
            else if (RootDialog.GetBack2home()) //回首頁
            {
                context.Done("");
            }
            else
            {
                await context.PostAsync("請選擇表單中選項");
            }
        }

        private static IList<Attachment> GetCardsAttachments()
        {
            DataTable dt = new DbEntity().Inweb();
            List<Attachment> listtt = new List<Attachment>();
            Attachment attt = new Attachment();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                attt = GetHeroCard(
                    dt.Rows[i]["name"].ToString(),
                    new CardImage(url: dt.Rows[i]["img"].ToString()),
                    new List<CardAction>() { new CardAction(ActionTypes.ImBack, "確認", value: "確認") });
                listtt.Add(attt);
            }
            return listtt;

        }

        private static Attachment GetHeroCard(string title, CardImage cardImage, List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Images = new List<CardImage>() { cardImage },
                Buttons = cardAction,

            };

            return heroCard.ToAttachment();
        }

        private static Attachment Getback()
        {
            var heroCard = new HeroCard
            {
                Title = "如上圖無須修改密碼之頁面請點選此按鈕",
                Buttons = new List<CardAction>() { new CardAction(ActionTypes.ImBack, "回上一步驟", value: "回上一步驟")
                    }
            };

            return heroCard.ToAttachment();

        }
    }
}