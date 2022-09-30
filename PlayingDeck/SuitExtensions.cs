namespace PlayingDeck;

public static class SuitExtensions
{
    public static char ToSymbol(this Suit suit)
    {
        return suit switch
        {
            Suit.Clubs => '♣',
            Suit.Diamonds => '♦',
            Suit.Hearts => '♥',
            Suit.Spades => '♠',
            _ => throw new ArgumentOutOfRangeException(nameof(suit))
        };
    }

    public static Color GetColor(this Suit suit)
    {
        return suit switch
        {
            Suit.Clubs or Suit.Spades => Color.Black,
            Suit.Diamonds or Suit.Hearts => Color.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(suit))
        };
    }
}
