// See https://aka.ms/new-console-template for more information


using PlayingDeck;

namespace BlackjackSim;

public class BasicStrategy : IPlayerTactic
{
    public enum PlayerAction
    {
        Hit,
        Stand,
        Split,
        SplitDas, // Double after split
        DoubleDown, // hit otherwise
        DoubleDownOrStand,
        Surrnder,
    }

    static PlayerAction GetChoice(char c) => c switch
    {
        'H' => PlayerAction.Hit,
        'S' => PlayerAction.Stand,
        'D' => PlayerAction.DoubleDown,
        'B' => PlayerAction.DoubleDownOrStand,
        'Y' => PlayerAction.Split,
        'F' => PlayerAction.Surrnder,
        _ => throw new ArgumentException($"Invalid choice: {c}"),
    };

    static PlayerAction GetChoice(Rank rank, string s) => rank switch
    {
        Rank.Ace => GetChoice(s[^1]),
        Rank.King or Rank.Queen or Rank.Jack => GetChoice(s[^2]),
        _ => GetChoice(s[(int)rank - 2]),
    };

    private static readonly Dictionary<int, string> StrategyMap = new()
    {
        {21, "SSSSSSSSSS"},
        {20, "SSSSSSSSSS"},
        {19, "SSSSSSSSSS"},
        {18, "SSSSSSSSSS"},
        {17, "SSSSSSSSSS"},
        {16, "SSSSSHHHHH"},
        {15, "SSSSSHHHHH"},
        {14, "SSSSSHHHHH"},
        {13, "SSSSSHHHHH"},
        {12, "HHSSSHHHHH"},
        {11, "DDDDDDDDDD"},
        {10, "DDDDDDDDHH"},
        {9,  "HDDDDHHHHH"},
        {8,  "HHHHHHHHHH"},
        {7,  "HHHHHHHHHH"},
        {6,  "HHHHHHHHHH"},
        {5,  "HHHHHHHHHH"},
        {4,  "HHHHHHHHHH"},
    };

    private PlayerChoice ToChoice(PlayerAction action) => action switch
    {
        PlayerAction.Hit => PlayerChoice.Hit,
        PlayerAction.Stand => PlayerChoice.Stand,
        PlayerAction.Split => PlayerChoice.Split,
        PlayerAction.DoubleDown => PlayerChoice.Hit,
        PlayerAction.DoubleDownOrStand => PlayerChoice.Stand,
        PlayerAction.Surrnder => PlayerChoice.Stand,
        _ => throw new ArgumentException($"Invalid choice: {action}"),
    };

    public PlayerChoice GetPlayerChoice(Card dealerCard, Hand player)
    {
        var value = player.GetPossibleHandValues().Where(v => v is <= 21).Max();
        var action = GetChoice(dealerCard.Rank, StrategyMap[value]);
        return ToChoice(action);
    }
}