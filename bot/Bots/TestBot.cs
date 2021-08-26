// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.14.0

using bot.Factories.Abstractions;
using bot.Services.Abstractions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bot.Bots
{
    public class TestBot : ActivityHandler
    {
        readonly IAdaptiveCardFactory _adaptiveCardFactory;
        readonly IJokeService _jokeService;

        public TestBot(IAdaptiveCardFactory adaptiveCardFactory, IJokeService jokeService)
        {
            _adaptiveCardFactory = adaptiveCardFactory;
            _jokeService = jokeService;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            Attachment optionsCard = null;

            if (turnContext.Activity.Text.Equals(ConstantStrings.return_to_start))
            {
                optionsCard = _adaptiveCardFactory.CreateTextCard();
            } 
            else if (turnContext.Activity.Text.Equals(ConstantStrings.get_joke)) 
            {
                var joke = await _jokeService.GetJoke();

                optionsCard = _adaptiveCardFactory.CreateCombinationCard(joke);
            }
            else
            {
                optionsCard = _adaptiveCardFactory.CreateOptionsCard();
            }

            await turnContext.SendActivityAsync(MessageFactory.Attachment(optionsCard), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeCard = _adaptiveCardFactory.CreateTextCard();

            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(welcomeCard), cancellationToken);
                }
            }
        }
    }
}
