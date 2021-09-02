using bot.Factories.Abstractions;
using bot.Models.MSTeams;
using bot.Services.Abstractions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bot.Bots
{
    public class MSTeamsBot : TeamsActivityHandler
    {
        readonly IAdaptiveCardFactory _adaptiveCardFactory;
        readonly IJokeService _jokeService;

        public MSTeamsBot(IAdaptiveCardFactory adaptiveCardFactory, IJokeService jokeService)
        {
            _adaptiveCardFactory = adaptiveCardFactory;
            _jokeService = jokeService;
        }
        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action, CancellationToken cancellationToken)
        {
            return await CreateAdaptiveCardResponse(turnContext, action);
        }

        private async Task<MessagingExtensionActionResponse> CreateAdaptiveCardResponse(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            var userResponse = ((JObject)action.Data).ToObject<TeamsResponse>();
            Attachment optionsCard;

            if (userResponse.Text.Equals(ConstantStrings.return_message))
            {
                optionsCard = _adaptiveCardFactory.CreateTextCard();
            }
            else if (userResponse.Text.Equals(ConstantStrings.joke_request))
            {
                var joke = await _jokeService.GetJoke();

                optionsCard = _adaptiveCardFactory.CreateCombinationCard(joke);
            }
            else
            {
                optionsCard = _adaptiveCardFactory.CreateOptionsCard();
            }

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    AttachmentLayout = "list",
                    Type = "result",
                    Attachments = new List<MessagingExtensionAttachment> { optionsCard.ToMessagingExtensionAttachment() },
                },
            };
        }
    }
}
