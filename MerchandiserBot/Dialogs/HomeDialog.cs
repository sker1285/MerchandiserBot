using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MerchandiserBot.Dialogs
{
    [Serializable]

    public class HomeDialog
    {
       
            public async Task StartAsync(IDialogContext context)
            {
                /* Wait until the first message is received from the conversation and call MessageReceviedAsync 
                 *  to process that message. */

                var reply = context.MakeMessage();

                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetCardsMovie();

                await context.PostAsync(reply);
                context.Wait(this.MessageReceivedAsync);


            }
            private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
            {
                var message = await result;

                if (message.Text.ToLower().Contains("找影片"))
                {
                    context.Done(context);
                }
            }
            public static IList<Attachment> GetCardsMovie()
            {
                return new List<Attachment>()
            {
                GetThumbnailCard(
                    "  MovieBot",
                    "Hi~我是電影達人" +"\n\r"+"找電影就找我",
                    null,
                    new CardImage(url: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSvDTnf1qo83gNohIMygd0b-zvlEL2apcgrjkRoSWW21pc0L2M6"),
                    new List<CardAction>(){ new CardAction(ActionTypes.ImBack, "找影片", value: "找影片"),new CardAction(ActionTypes.ImBack, "找影院", value: "找影院"),new CardAction(ActionTypes.ImBack, "有線電視", value: "有線電視") }),
            };
            }

            private static Attachment GetHeroCard(CardImage cardImage, List<CardAction> cardAction)
            {
                var heroCard = new HeroCard
                {

                    Images = new List<CardImage>() { cardImage },
                    Buttons = cardAction,
                };

                return heroCard.ToAttachment();
            }

            private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, List<CardAction> cardAction)
            {
                var heroCard = new ThumbnailCard
                {
                    Title = title,
                    Subtitle = subtitle,
                    Text = text,
                    Images = new List<CardImage>() { cardImage },
                    Buttons = cardAction,
                };

                return heroCard.ToAttachment();
            }
        }
    }