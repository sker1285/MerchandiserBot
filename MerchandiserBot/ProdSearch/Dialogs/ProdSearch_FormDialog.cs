using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using AdaptiveCards;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MerchandiserBot.ProdSearch.Dialogs
{
    [Serializable]
    public class ProdSearch_FormDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await ShowOptionsAsync(context);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            dynamic value = message.Value;
            string submitType = value.Type.ToString();
            FormCheck query;
            try
            {
                query = FormCheck.GendernBirth(value);

                // Trigger validation using Data Annotations attributes from the FormCheck model
                List<ValidationResult> results = new List<ValidationResult>();
                bool valid = Validator.TryValidateObject(query, new ValidationContext(query, null, null), results, true);
                if (!valid)
                {
                    // Some field in the FormCheck are not valid
                    var errors = string.Join("\n", results.Select(o => " - " + o.ErrorMessage));
                    await context.PostAsync("Please complete all the search parameters:\n" + errors);
                    return;
                }
            }
            catch (InvalidCastException)
            {
                // FromCheck could not be parsed
                await context.PostAsync("請填寫完整");
                return;
            }

            context.Done(context);
            // TODO: Put logic for handling user message here
        }

        private async Task ShowOptionsAsync(IDialogContext context)
        {
            AdaptiveCard card = new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    new Container()
                    {
                        Speak = "請填寫相關內容:",
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
                                                Text = "請填寫相關內容:",
                                                Speak = "<s>請填寫相關內容:</s>",
                                                Weight = TextWeight.Bolder,
                                                Size = TextSize.Large
                                            },
                                            new TextBlock()
                                            {
                                                Text = "請選擇你的性別",
                                                Speak = "<s>請選擇你的性別</s>"
                                            },
                                            new ChoiceSet()
                                            {
                                                Id = "Gender",
                                                Style = ChoiceInputStyle.Expanded,
                                                IsMultiSelect = false,
                                                //Value = "1",
                                                Choices = new List<Choice>()
                                                {
                                                    new Choice(){ Title = "男",Value = "男"},
                                                    new Choice(){ Title = "女",Value = "女"}
                                                }
                                            },
                                            new TextBlock()
                                            {
                                                Text = "請選擇你的出生日",
                                                Speak = "<s>請選擇你的出生日</s>",
                                            },
                                            new TextBlock() { Text = "ex.1980/01/01" },
                                            new DateInput() { Id = "BirthCheck" }
                                        }
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

    }
}