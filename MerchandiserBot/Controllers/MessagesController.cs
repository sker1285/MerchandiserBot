using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MerchandiserBot.Dialogs;
using MerchandiserBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Models;
using Newtonsoft.Json;

namespace MerchandiserBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        static public List<Product> ltProd = GetJson.GetProdList(@"C:\Users\er1307\source\repos\MerchandiserBot\MerchandiserBot\App_Data\product.json");
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                if (ProdSearch.Dialogs.ProdSearch_KeywordDialog.getcheck())
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    string strLuisKey = ConfigurationManager.AppSettings["LUISAPIKey"].ToString();
                    string strLuisAppId = ConfigurationManager.AppSettings["LUISAppId"].ToString();
                    string strMessage = HttpUtility.UrlEncode(activity.Text);
                    string strLuisUrl = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{strLuisAppId}?subscription-key={strLuisKey}&verbose=true&timezoneOffset=0&q={strMessage}";

                    // 收到文字訊息後，往LUIS送
                    WebRequest request = WebRequest.Create(strLuisUrl);
                    HttpWebResponse hwresponse = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = hwresponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string json = reader.ReadToEnd();
                    LUIS objLUISRes = JsonConvert.DeserializeObject<LUIS>(json);

                    string strReply = "無法識別的內容";
                    ProdSearch.Dialogs.ProdSearch_KeywordDialog.setLuisKWCheck_true();

                    if (objLUISRes.intents.Count > 0)
                    {
                        string strIntent = objLUISRes.intents[0].intent;
                        if (strIntent.Equals("搜尋壽險"))
                        {
                            strReply = "將進行搜尋壽險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("壽險");
                        }
                        else if (strIntent.Equals("搜尋投資型保險"))
                        {
                            strReply = "將進行搜尋投資型保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("投資");
                        }
                        else if (strIntent.Equals("搜尋年金保險"))
                        {
                            strReply = "將進行搜尋年金保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("年金");
                        }
                        else if (strIntent.Equals("搜尋小額終老保險"))
                        {
                            strReply = "將進行搜尋小額終老保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("小額終老");
                        }
                        else if (strIntent.Equals("搜尋實物給付型保險"))
                        {
                            strReply = "將進行搜尋實物給付型保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("實物給付");
                        }
                        else if (strIntent.Equals("搜尋長照"))
                        {
                            strReply = "將進行搜尋長照...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("長照專區");
                        }
                        else if (strIntent.Equals("搜尋大男子保險"))
                        {
                            strReply = "將進行搜尋大男子保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("大男子保險");
                        }
                        else if (strIntent.Equals("搜尋生死合險"))
                        {
                            strReply = "將進行搜尋生死合險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("生死合險");
                        }
                        else if (strIntent.Equals("搜尋HER大女子保險"))
                        {
                            strReply = "將進行搜尋HER大女子保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("HER大女子保險");
                        }
                        else if (strIntent.Equals("搜尋OIU保險"))
                        {
                            strReply = "將進行搜尋OIU保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("OIU專區");
                        }
                        else if (strIntent.Equals("搜尋利變壽"))
                        {
                            strReply = "將進行搜尋利變壽...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("利變壽");
                        }
                        else if (strIntent.Equals("搜尋展新人生保險"))
                        {
                            strReply = "將進行搜尋展新人生保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("展新人生");
                        }
                        else if (strIntent.Equals("搜尋意外傷害險"))
                        {
                            strReply = "將進行搜尋意外傷害險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("意外傷害");
                        }
                        else if (strIntent.Equals("搜尋活力系列保險"))
                        {
                            strReply = "將進行搜尋活力系列保險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("活力系列");
                        }
                        else if (strIntent.Equals("搜尋醫療險"))
                        {
                            strReply = "將進行搜尋醫療險...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setKeyword("醫療");
                        }
                        else
                        {
                            strReply = "無法識別的內容，請重新輸入...";
                            ProdSearch.Dialogs.ProdSearch_KeywordDialog.setLuisKWCheck_false();
                        }

                    }

                    Activity reply = activity.CreateReply(strReply);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                    ProdSearch.Dialogs.ProdSearch_KeywordDialog.setcheck();
                }
                else if (RootDialog.GetOpen2home())
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    string strLuisKey = ConfigurationManager.AppSettings["LUISAPIKey"].ToString();
                    string strLuisAppId = ConfigurationManager.AppSettings["LUISAppId"].ToString();
                    string strMessage = HttpUtility.UrlEncode(activity.Text);
                    string strLuisUrl = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{strLuisAppId}?subscription-key={strLuisKey}&verbose=true&timezoneOffset=0&q={strMessage}";

                    // 收到文字訊息後，往LUIS送
                    WebRequest request = WebRequest.Create(strLuisUrl);
                    HttpWebResponse hwresponse = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = hwresponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string json = reader.ReadToEnd();
                    LUIS objLUISRes = JsonConvert.DeserializeObject<LUIS>(json);

                    //string strReply = "無法識別的內容";

                    if (objLUISRes.intents.Count > 0)
                    {
                        string strIntent = objLUISRes.intents[0].intent;
                        if (strIntent.Equals("回首頁選單"))
                        {
                            RootDialog.SetBack2home(true);
                        }
                    }

                    //Activity reply = activity.CreateReply(strReply);
                    //await connector.Conversations.ReplyToActivityAsync(reply);
                }

                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());

            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels

                IConversationUpdateActivity iConversationUpdated = message as IConversationUpdateActivity;
                if (iConversationUpdated != null)
                {
                    ConnectorClient connector = new ConnectorClient(new System.Uri(message.ServiceUrl));

                    foreach (var member in iConversationUpdated.MembersAdded ?? System.Array.Empty<ChannelAccount>())
                    {
                        // if the bot is added, then
                        if (member.Id == iConversationUpdated.Recipient.Id)
                        {


                            var reply = ((Activity)iConversationUpdated).CreateReply($"您好~ 我是小光機器人");
                            connector.Conversations.ReplyToActivity(reply);


                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}