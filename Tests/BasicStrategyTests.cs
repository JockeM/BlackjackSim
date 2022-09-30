using PlayingDeck;
using Xunit;

namespace BlackjackSim.Tests;

public class BasicStrategyTests
{
    static Hand Hand(params Rank[] ranks) => new(ranks.Select(x => new Card(Suit.Clubs, x)));

    static IEnumerable<Card> OneOfEachRank()
    {
        foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        {
            yield return new Card(Suit.Clubs, rank);
        }
    }

    [Fact]
    public void Test()
    {
        var strategy = new BasicStrategy();
        foreach (var card in OneOfEachRank())
        {
            var result = strategy.GetPlayerChoice(card, Hand(Rank.Ace, Rank.Ace));
        }
    }
}
