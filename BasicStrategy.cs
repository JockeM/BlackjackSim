using PlayingDeck;

namespace BlackjackSim;

public class BasicStrategy : IPlayerTactic
{
    enum StrategyActions
    {
        Hit,
        Stand,
        Split,
        SplitDas, // Double after split
        DoubleDown, // hit otherwise
        DoubleDownOrStand,
        Surrnder,
    }

    static StrategyActions GetChoice(char c) =>
        c switch
        {
            'H' => StrategyActions.Hit,
            'S' => StrategyActions.Stand,
            'D' => StrategyActions.DoubleDown,
            'B' => StrategyActions.DoubleDownOrStand,
            'Y' => StrategyActions.Split,
            'F' => StrategyActions.Surrnder,
            _ => throw new ArgumentException($"Invalid choice: {c}"),
        };

    static StrategyActions GetHardTotalAction(Rank rank, Hand hand)
    {
        var value = hand.GetPossibleHandValues().Where(v => v is <= 21).Max();
        if (value is >= 17)
        {
            return StrategyActions.Stand;
        }
        else if (value is <= 8)
        {
            return StrategyActions.Hit;
        }

        var s = StrategyMap[value];

        var c = rank switch
        {
            Rank.Ace => s[^1],
            Rank.King or Rank.Queen or Rank.Jack => s[^2],
            _ => s[(int)rank - 2],
        };

        return GetChoice(c);
    }

    private static readonly Dictionary<int, string> StrategyMap =
        new()
        {
            { 16, "SSSSSHHHHH" },
            { 15, "SSSSSHHHHH" },
            { 14, "SSSSSHHHHH" },
            { 13, "SSSSSHHHHH" },
            { 12, "HHSSSHHHHH" },
            { 11, "DDDDDDDDDD" },
            { 10, "DDDDDDDDHH" },
            { 9, "HDDDDHHHHH" },
        };

    static PlayerChoice ToChoice(StrategyActions action) =>
        action switch
        {
            StrategyActions.Hit => PlayerChoice.Hit,
            StrategyActions.Stand => PlayerChoice.Stand,
            StrategyActions.Split => PlayerChoice.Split,
            StrategyActions.DoubleDown => PlayerChoice.Hit,
            StrategyActions.DoubleDownOrStand => PlayerChoice.Stand,
            StrategyActions.Surrnder => PlayerChoice.Stand,
            _ => throw new ArgumentException($"Invalid choice: {action}"),
        };

    static bool IsPair(Hand hand) => hand.Cards.GroupBy(x => x.Rank).Count() is 1;

    static bool ContainsSingleAce(Hand hand) => hand.Cards.Count(x => x.Rank is Rank.Ace) is 1;

    public PlayerChoice GetPlayerChoice(Card dealerCard, Hand playerHand)
    {
        if (playerHand.Cards.Count is 2)
        {
            if (ContainsSingleAce(playerHand))
            {
                // TODO: Check ace strategy

                return PlayerChoice.Hit;
            }

            if (IsPair(playerHand))
            {
                // TODO: Check pair strategy
            }
        }

        var action = GetHardTotalAction(dealerCard.Rank, playerHand);
        return ToChoice(action);
    }
}
