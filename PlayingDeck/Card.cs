namespace PlayingDeck;

public readonly record struct Card
{
	public readonly Suit Suit { get; }
	public readonly Rank Rank { get; }

	public Card(Suit suit, Rank rank)
	{
		Suit = suit;
		Rank = rank;
	}

	public override string ToString() => $"{Suit.ToSymbol()} {Rank.ToSymbol()}";
}
