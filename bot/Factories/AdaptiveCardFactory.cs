using AdaptiveCards;
using bot.Factories.Abstractions;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace bot.Factories
{
    public sealed class AdaptiveCardFactory : IAdaptiveCardFactory
    {
        //The default schema for the adaptive cards, it tells the client what version of adaptive cards to render
        static AdaptiveSchemaVersion defaultSchema = new(1, 0);

        /// <summary>
        /// Creates a simple adaptive card with two submit actions.
        /// </summary>
        public Attachment CreateOptionsCard()
        {
            var options = new string[]
            {
                ConstantStrings.get_joke,
                ConstantStrings.return_to_start
            };

            var card_actions = new List<AdaptiveAction>();

            foreach (var option in options) 
            {
                card_actions.Add(new AdaptiveSubmitAction 
                {
                    Title = option,
                    Data = option
                });
            }

            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveActionSet
                    {
                        Actions = card_actions
                    }
                }
            };

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        /// <summary>
        /// Creates an adaptive card that contains a textbox only
        /// </summary>
        public Attachment CreateTextCard()
        {
            var message = ConstantStrings.welcome_message;

            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default,
                        Wrap = true
                    }
                }
            };

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        /// <summary>
        /// Creates an adaptive card that has both text and a submit action
        /// </summary>
        public Attachment CreateCombinationCard(string message)
        {
            var action = ConstantStrings.return_to_start;

            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default
                    },
                    new AdaptiveActionSet
                    {
                        Actions = new List<AdaptiveAction>
                        {
                            new AdaptiveSubmitAction
                            {
                                Title = action,
                                Data = action
                            }
                        }
                    }
                }
            };

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        /// <summary>
        /// Using a JSON representation of the adaptive card return an attachment that can be sent to the client
        /// </summary>
        Attachment CreateAdaptiveCardAttachment(string jsonData)
        {
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(jsonData),
            };

            return adaptiveCardAttachment;
        }
    }
}
