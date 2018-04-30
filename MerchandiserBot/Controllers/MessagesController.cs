using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace MerchandiserBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
         
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
                    Models.LUIS objLUISRes = JsonConvert.DeserializeObject<Models.LUIS>(json);

                    string strReply = "無法識別的內容";

                    if (objLUISRes.intents.Count > 0)
                    {
                        string strIntent = objLUISRes.intents[0].intent;
                        strReply = LuisIntent.intent(strIntent);
                    }

                    Activity reply = activity.CreateReply(strReply);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                    ProdSearch.Dialogs.ProdSearch_KeywordDialog.setcheck();
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