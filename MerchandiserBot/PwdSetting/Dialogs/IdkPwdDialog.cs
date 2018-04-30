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
    public class IdkPwdDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {

            //先顯示AD頁面
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachmentsAD();
            await context.PostAsync(reply);
            await context.PostAsync("以上有您要修改密碼之頁面嗎 ? (如有請選擇確認鍵)");

            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (message.Text.Equals("確認 (AD)"))
            {
                PwdSetting.Dialogs.PwdResetDialog.setpwd("AD");
                await context.PostAsync("這是 [ AD ] 密碼,將為您進行修改 ");
                context.Done($"");
            }
            else if (message.Text.Equals("確認 (內網)"))
            {
                PwdSetting.Dialogs.PwdResetDialog.setpwd("內網");
                await context.PostAsync("這是 [ 內 網 ] 密碼,將為您進行修改 ");
                context.Done($"");
            }
            else
            {
                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetCardsAttachmentsInweb();
                await context.PostAsync(reply);
                await context.PostAsync("以上有您要修改密碼之頁面嗎 ? (如有請選擇確認鍵)");
            }
           
        }

        private static IList<Attachment> GetCardsAttachmentsAD()
        {
            DataTable dt = new DbEntity().AD();
            List<Attachment> listtt = new List<Attachment>();
            Attachment attt = new Attachment();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                attt = GetHeroCard(
                    dt.Rows[i]["name"].ToString(),
                    new CardImage(url: dt.Rows[i]["img"].ToString()),
                    new List<CardAction>() { new CardAction(ActionTypes.ImBack, "確認", value: "確認 (AD)") });
                listtt.Add(attt);
            }
            return listtt;

        }

        private static IList<Attachment> GetCardsAttachmentsInweb()
        {
            DataTable dt = new DbEntity().Inweb();
            List<Attachment> listtt = new List<Attachment>();
            Attachment attt = new Attachment();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                attt = GetHeroCard(
                    dt.Rows[i]["name"].ToString(),
                    new CardImage(url: dt.Rows[i]["img"].ToString()),
                    new List<CardAction>() { new CardAction(ActionTypes.ImBack, "確認", value: "確認 (內網)") });
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

       
    }
}