using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MerchandiserBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        /// <summary>
        /// {option}: 使用者在主選單　選擇的項目
        /// {prodOption}: 使用者在商品搜尋選單　選擇的項目
        /// {state}: 
        /// {error}: 
        /// {Back2home}: 是否要回首頁選單
        /// {Open2home}: 是否要執行　回首頁Luis判斷
        /// </summary>
        static string option;
        static string prodOption;
        static string state;
        static int error;

        static Boolean Back2home = true;
        static Boolean Open2home = false;

        public Task StartAsync(IDialogContext context)
        {
            
            context.Wait(MessageReceivedAsync);
            
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            if (Back2home)
            {
                context.Call(new HomeDialog(), SendWelcomeMessageAsync);
                SetBack2home(false);
                //SetOpen2home(false);
            }
            else
            {
                await context.PostAsync("顯示選單請下指令～（例如：\"首頁\"、\"表單\"、\"累了\"...等）");
                SetOpen2home(true);
                context.Wait(MessageReceivedAsync);
            }
            


        }

        // HomeDialog
        private async Task SendWelcomeMessageAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            option = HomeDialog.getoption();
            if (option.Equals("1")) //忘記密碼
            {
                context.Call(new PwdSetting.Dialogs.ChoicePwdDialog(), ChoicePwdDialogsResumeAfter);
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

        //選擇須修改密碼類型
        private async Task ChoicePwdDialogsResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var msg = await result;
            if ($"{msg}".Equals("AD"))
            {
                PwdSetting.Dialogs.PwdResetDialog.setpwd("AD");
                context.Call(new PwdSetting.Dialogs.ADPwdDialog(), PwdDialogResumeAfter);
            }
            else if ($"{msg}".Equals("內網"))
            {
                PwdSetting.Dialogs.PwdResetDialog.setpwd("內網");
                context.Call(new PwdSetting.Dialogs.InwebPwdDialog(), PwdDialogResumeAfter);
            }
            else if ($"{msg}".Equals("Idk"))
            {
                PwdSetting.Dialogs.PwdResetDialog.setpwd("AD");
                context.Call(new PwdSetting.Dialogs.IdkPwdDialog(), PwdDialogResumeAfter);
            }

        }
        //AD密碼和內網密碼頁面選擇結束
        private async Task PwdDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {

            var msg = await result;
            if ($"{msg}".Equals("goback"))  //如密碼類別選擇錯誤回上一頁
            {
                context.Call(new PwdSetting.Dialogs.ChoicePwdDialog(), ChoicePwdDialogsResumeAfter);
            }
            else
            {
                context.Call(new PwdSetting.Dialogs.CertifiedDialog(), CertifiedDialogResumeAfter);
            }
        }
        // 身分認證結束
        private async Task CertifiedDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {          
            DataTable dt = new DbEntity().MerchandiserData(PwdSetting.Dialogs.CertifiedDialog.getId());
            if (dt.Rows.Count!=0)
            {
                error = int.Parse(dt.Rows[0]["errortimes"].ToString());
                if (error > 3)
                {
                    await context.PostAsync("您已輸入超過3次錯誤,請致電21080進行修改");
                    context.Call(new HomeDialog(), SendWelcomeMessageAsync);
                }
                else
                {
                    context.Call(new PwdSetting.Dialogs.VerificationDialog(), VerificationDialogResumeAfter);
                }
            } 
            else
            {
                await context.PostAsync("查無此人");
                context.Call(new PwdSetting.Dialogs.CertifiedDialog(), CertifiedDialogResumeAfter);
            }
        }

        // 資料驗證結束
        private async Task VerificationDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            bool Mcheck = PwdSetting.Dialogs.VerificationDialog.CheckMerchandiser();
            
            if (Mcheck)
            {
                string Id = PwdSetting.Dialogs.CertifiedDialog.getId();
                DataTable dt = new DbEntity().MerchandiserData(Id);
                string name = dt.Rows[0]["Name"].ToString();
                await context.PostAsync($"{name}你好，身份已驗證成功!");
                context.Call(new PwdSetting.Dialogs.PwdResetDialog(), PwdResetDialogResumeAfter);
            }
            else
            {
                await context.PostAsync("身份驗證失敗!");
                DataTable dt = new DbEntity().errortimes(error+1);
                context.Call(new PwdSetting.Dialogs.CertifiedDialog(), CertifiedDialogResumeAfter);
            }


        }
        private async Task PwdResetDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            SetOpen2home(true);
            context.Wait(MessageReceivedAsync);
        }


        /************************* ProdSearch *************************/
        //選擇商品搜尋方式 ---> 各自Dialog
        private async Task ProdSearchDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            prodOption = ProdSearch.Dialogs.ProdSearchDialog.GetProdSearchSelc();
            if (prodOption.Equals("險種分類"))
            {
                context.Call(new ProdSearch.Dialogs.ProdSearch_CatalogDialog(), ProdSearchFormDialogResumeAfter);
            }
            else if (prodOption.Equals("關鍵字"))
            {
                context.Call(new ProdSearch.Dialogs.ProdSearch_KeywordDialog(), ProdSearchFormDialogResumeAfter);
                //context.Call(new ProdSearch.Dialogs.ProdSearch_KeywordDialog(), PS_KeywordDialogResumeAfter);
            }
            
        }

        //結束以 關鍵字 或 險種分類 後要填寫的表格
        //private async Task PS_KeywordDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        //{
        //    if (ProdSearch.Dialogs.ProdSearch_KeywordDialog.getLuisKWCheck())
        //    {
        //        context.Call(new ProdSearch.Dialogs.ProdSearch_FormDialog(), ProdSearchFormDialogResumeAfter);
        //    }
        //    else
        //    {
        //        context.Call(new ProdSearch.Dialogs.ProdSearch_KeywordDialog(), PS_KeywordDialogResumeAfter);
        //    }
            
        //}

        //顯示產品

        private async Task ProdSearchFormDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (ProdSearch.Dialogs.ProdSearchDialog.GetProdSearchSelc().Equals("關鍵字"))
            {
                if (ProdSearch.Dialogs.ProdSearch_KeywordDialog.getLuisKWCheck())
                {
                    context.Call(new ProdSearch.Dialogs.ProdSearch_ShowProdDialog(), ProdSearchShowProdDialogResumeAfter);
                }
                else
                {
                    context.Call(new ProdSearch.Dialogs.ProdSearch_KeywordDialog(), ProdSearchFormDialogResumeAfter);
                }
            }
            else
            {
                context.Call(new ProdSearch.Dialogs.ProdSearch_ShowProdDialog(), ProdSearchShowProdDialogResumeAfter);
            }
            //context.Call(new ProdSearch.Dialogs.ProdSearch_ShowProdDialog(), ProdSearchShowProdDialogResumeAfter);
        }

        private async Task ProdSearchShowProdDialogResumeAfter(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            SetOpen2home(true);
            context.Wait(MessageReceivedAsync);
        }

        /************************* PushMsg *************************/




        /************************** GetSet *************************/
        public static void SetBack2home(Boolean check)
        {
            Back2home = check;
        }

        public static Boolean GetBack2home()
        {
            return Back2home;
        }

        public static void SetOpen2home(Boolean check)
        {
            Open2home = check;
        }

        public static Boolean GetOpen2home()
        {
            return Open2home;
        }

    }

}