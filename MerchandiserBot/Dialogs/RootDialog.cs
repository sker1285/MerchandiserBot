﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        static string option;
        static string prodOption;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

            //// calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            //// return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            context.Call(new HomeDialog(), SendWelcomeMessageAsync);


        }

        // HomeDialog
        private async Task SendWelcomeMessageAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync("Hi, I'm  Moviebot. Let's get started.");
            option = HomeDialog.getoption();
            if (option.Equals("1")) //忘記密碼
            {
                context.Call(new PwdSetting.Dialogs.CertifiedDialog(), CertifiedDialogResumeAfter);
            }
            else if (option.Equals("2")) //商品搜尋
            {
                context.Call(new ProdSearch.Dialogs.ProdSearchDialog(), ProdSearchDialogResumeAfter);
            }
            else if (option.Equals("3")) //推播訊息
            {

            }
        }



        /************************* PwdSetting *************************/
        // 身分認證
        private async Task CertifiedDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Call(new PwdSetting.Dialogs.VerificationDialog(), VerificationDialogResumeAfter);

        }

        // 資料驗證
        private async Task VerificationDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Call(new PwdSetting.Dialogs.PwdResetDialog(), PwdResetDialogResumeAfter);

        }
        private async Task PwdResetDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

        }


        /************************* ProdSearch *************************/
        //選擇商品搜尋方式 ---> 各自Dialog
        private async Task ProdSearchDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            prodOption = ProdSearch.Dialogs.ProdSearchDialog.GetProdSearchSelc();
            if (prodOption.Equals("險種分類"))
            {
                context.Call(new ProdSearch.Dialogs.ProdSearch_CatalogDialog(), PS_CataKeyDialogResumeAfter);
            }
            else if (prodOption.Equals("關鍵字"))
            {
                context.Call(new ProdSearch.Dialogs.ProdSearch_KeywordDialog(), PS_CataKeyDialogResumeAfter);
            }
            
        }

        //結束以 關鍵字 或 險種分類 後要填寫的表格
        private async Task PS_CataKeyDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Call(new ProdSearch.Dialogs.ProdSearch_FormDialog(), ProdSearchFormDialogResumeAfter);
        }

        //顯示產品
        private async Task ProdSearchFormDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Call(new ProdSearch.Dialogs.ProdSearch_ShowProdDialog(), null);
        }


        /************************* PushMsg *************************/


    }
}