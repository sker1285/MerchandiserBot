using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using MerchandiserBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_KeywordDialog : IDialog<IMessageActivity>
    {
        /// <summary>
        /// {keywordCheck}: 給　Messagecontroller是否需要進行 Luis 的判斷
        /// {LuisKWChwck} : 判斷 Luis是否有判斷正確
        /// {keyword}: 會被 Luis Intent 改變的　關鍵字
        /// {nonLuisKeyword}: 使用者輸入的原生關鍵字
        /// </summary>
        static Boolean keywordCheck = false;
        static Boolean LuisKWCheck = true;
        static string keyword;
        static string nonLuisKeyword;
        public async Task StartAsync(IDialogContext context)
        {
            keywordCheck = true;
            await context.PostAsync("請輸入關鍵字：");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            if (result!=null)
            {
                setnonLuisKeyword(activity.Text);
                context.Done(context);
            }
            else if (RootDialog.GetBack2home()) //回首頁
            {
                context.Done(context);
            }
        }

        public static void setcheck()
        {
            keywordCheck = false;
        }

        public static Boolean getcheck()
        {
            return keywordCheck;
        }

        public static void setLuisKWCheck_false()
        {
            LuisKWCheck = false;
        }

        public static void setLuisKWCheck_true()
        {
            LuisKWCheck = true;
        }

        public static Boolean getLuisKWCheck()
        {
            return LuisKWCheck;
        }

        public static void setKeyword(string kw)
        {
            keyword = kw;
        }

        public static string getKeyword()
        {
            return keyword;
        }

        public static void setnonLuisKeyword(string kw)
        {
            nonLuisKeyword = kw;
        }

        public static string getnonLuisKeyword()
        {
            return nonLuisKeyword;
        }
    }
}