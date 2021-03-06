﻿using AdaptiveCards;
using MerchandiserBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MerchandiserBot.PwdSetting.Dialogs
{
    [Serializable]
    public class VerificationDialog : IDialog<IMessageActivity>
    {
        static string name,IDnum,birth;
        static int state;
        
        public async Task StartAsync(IDialogContext context)
        {
            state = 0;
            await context.PostAsync("請輸入您的[身分證字號]");

            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            state = state + 1;           

            
            if (state == 1 )
            {
                IDnum = message.Text;

                if (IsValidID(IDnum)) //驗證輸入格式
                {
                    //await context.PostAsync("請輸入您的[生日]" +
                    //                   "\n\r" + " OOO年OO月OO日(ex.070/01月01日)");
                    //await ShowOptionsAsync(context);
                    await context.PostAsync("身分驗證中...");
                    context.Done(context);
                }
                else if (RootDialog.GetBack2home()) //回首頁
                {
                    context.Done(context);
                }
                else {
                    await context.PostAsync("輸入格式不符，請重新輸入[身分證字號]");
                    state--;
                }              
            }
            //if (state == 2 )
            //{
            //    dynamic value = message.Value;
            //    string submitType = value.Type.ToString();
            //    FormBirth query;
            //    try
            //    {
            //        query = FormBirth.Birth(value);

            //        // Trigger validation using Data Annotations attributes from the HotelsQuery model
            //        List<ValidationResult> results = new List<ValidationResult>();
            //        bool valid = Validator.TryValidateObject(query, new ValidationContext(query, null, null), results, true);
            //        if (!valid)
            //        {
            //            // Some field in the Hotel Query are not valid
            //            var errors = string.Join("\n", results.Select(o => " - " + o.ErrorMessage));
            //            await context.PostAsync("Please complete all the search parameters:\n" + errors);
            //            return;
            //        }
            //    }
            //    catch (InvalidCastException)
            //    {
            //        // Hotel Query could not be parsed
            //        //await context.PostAsync("Please complete all the search parameters");
            //        await context.PostAsync("請填寫完整");
            //        await ShowOptionsAsync(context);


            //        return;
            //    }
            //    var queryform = FormBirth.Birth(message.Value);
            //    birth = queryform.Checkin.Value.ToString("yyyy/MM/dd");
            //    await context.PostAsync("身分驗證中...");
            //    context.Done(context);

            //}
          
        }

        private async Task ShowOptionsAsync(IDialogContext context)
        {
            AdaptiveCard card = new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    new Container()
                    {
                        Speak = "<s>Hey</s><s>請選擇你的出生日</s>",
                        Items = new List<CardElement>()
                        {
                            new ColumnSet()
                            {
                                Columns = new List<Column>()
                                {
                                    new Column()
                                    {
                                         Size = ColumnSize.Stretch,
                                        Items = new List<CardElement>()
                                        {
                                            new TextBlock()
                        {
                            Text = "請選擇你的出生日",
                            Speak = "<s>請選擇你的出生日</s>",
                            Weight = TextWeight.Bolder,
                            Size = TextSize.Large
                        },

                        new TextBlock() { Text = "ex.1980/01/01" },
                        new DateInput()
                        {
                            Id = "Checkin",
                            //Speak = "<s>When do you want to check in?</s>"
                        },
                },

                                        }

                                    }
                                }

                            }
                        }
                    },
                Actions = new List<ActionBase>()
                {
                    new SubmitAction()
                    {
                        Title = "確認",
                        Speak = "<s>確認</s>",
                        DataJson = "{ \"Type\": \"Check\" }"
                    }
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

            context.Wait(MessageReceivedAsync);
        }

        public static bool IsValidID(string strIn) //身分證格式確認
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"[a-zA-Z]\d{9}$");
        }


        public static bool CheckMerchandiser()
        {
            string Id = PwdSetting.Dialogs.CertifiedDialog.getId();
            DataTable dt = new DbEntity().M_Check(Id,IDnum,birth);
            if (dt.Rows.Count > 0) { return true; }
            return false;
        }


        public static string getname() { return name; }
        public static string getID() { return IDnum; }
        public static string getbirth() { return birth; }

    }
}