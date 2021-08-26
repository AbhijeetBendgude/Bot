using Microsoft.Bot.Schema;

namespace bot.Factories.Abstractions
{
    public interface IAdaptiveCardFactory
    {
        Attachment CreateOptionsCard();
        Attachment CreateTextCard();
        Attachment CreateCombinationCard(string message);
    }
}
