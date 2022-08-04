using PlayingDeck;

public class Hand
{
	private readonly List<Card> _cards;
	public IReadOnlyList<Card> Cards => _cards;

	public Hand(IEnumerable<Card>? cards = null) => _cards = cards?.ToList() ?? new List<Card>();

	public void Add(Card card) => _cards.Add(card);
	public void Clear() => _cards.Clear();

	public IEnumerable<int> GetPossibleHandValues()
	{
		int GetRankValue(Rank rank) => rank switch
		{
			Rank.Two => 2,
			Rank.Three => 3,
			Rank.Four => 4,
			Rank.Five => 5,
			Rank.Six => 6,
			Rank.Seven => 7,
			Rank.Eight => 8,
			Rank.Nine => 9,
			Rank.Ten or Rank.Jack or Rank.Queen or Rank.King => 10,
			_ => throw new ArgumentOutOfRangeException(nameof(rank)),
		};

		var aces = _cards.Count(x => x.Rank is Rank.Ace);
		var nonAceSum = _cards.Where(x => x.Rank is not Rank.Ace).Sum(x => GetRankValue(x.Rank));

		if (aces is 0)
		{
			yield return nonAceSum;
		}
		else
		{
			for (var i = 0; i <= aces; i++)
			{
				const int LowAceValue = 1;
				const int HighAceValue = 11;

				var highAces = i;
				var lowAces = (aces - i);
				yield return nonAceSum + (lowAces * LowAceValue) + (highAces * HighAceValue);
			}
		}
	}

	public bool IsBlackjack() => _cards.Count is 2 && GetPossibleHandValues().Contains(21);
	public bool IsBust() => GetPossibleHandValues().All(x => x > 21);

	public override string ToString() => $"{string.Join(", ", _cards)} ({string.Join(", ", GetPossibleHandValues())})";
}