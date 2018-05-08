using AdaptiveCards;
using MerchandiserBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MerchandiserBot.PwdSetting.Dialogs
{

    [Serializable]
    public class PwdResetDialog : IDialog<IMessageActivity>
    {
        static string pwd;

        public async Task StartAsync(IDialogContext context)
        {
            DateTime localDate = DateTime.Now;
            string now = localDate.ToString("yyyy/MM/dd HH:mm:ss");
            if (pwd.Equals("AD"))
            {
                DataTable dt = new DbEntity().PwdRecord(PwdSetting.Dialogs.CertifiedDialog.getId(), now, "ADPwd");
                await context.PostAsync("AD密碼已重設，請至信箱收取");
                SendEmail();
                await ShowOptionsAsync(context);
                context.Done(context);
            }
            else if (pwd.Equals("內網"))
            {
                DataTable dt = new DbEntity().PwdRecord(PwdSetting.Dialogs.CertifiedDialog.getId(), now, "InwebPwd");
                await context.PostAsync("內網密碼已重設，請至信箱收取");
                SendEmail();
                await ShowOptionsAsync(context);
                context.Done(context);
            }

        }

        public void SendEmail()
        {
            DataTable Mdt = new DbEntity().MerchandiserData(PwdSetting.Dialogs.CertifiedDialog.getId());
            string birth = Convert.ToDateTime(Mdt.Rows[0]["Birth"]).ToString("yyyyMMdd");
            Mail.otp = birth;
            Mail.SendMail();
        }



        private async Task ShowOptionsAsync(IDialogContext context)
        {
            var card = new AdaptiveCard();
            var columnsBlock = new ColumnSet()
            {
                Separation = SeparationStyle.Default,
                Columns = new List<Column>
                {
                  new Column
                  {
                    Size = "1",
                    Items = new List<CardElement>
                    {
                      new TextBlock
                      {
                        Text = "收取信件後，請前往登入並 [修改密碼] ",
                        Weight = TextWeight.Bolder,
                        Size = TextSize.Large,
                        Wrap = true,
                      }
                      }
                      },
                       },
                        };
            card.Body.Add(columnsBlock);
            card.Actions = new List<ActionBase>()
            {
                new OpenUrlAction
                {
                  Title = "前往登入",
                  Url = "http://sklweb.skl.com.tw/MainWeb/IntraNet/Main_Page/Main/default.aspx",
                }
            };

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            var reply = context.MakeMessage();
            reply.Attachments.Add(attachment);

            await context.PostAsync(reply, CancellationToken.None);

        }

        public static void setpwd(string n)
        {
            pwd = n;
        }
    }
}