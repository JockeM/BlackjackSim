namespace PlayingDeck;

public class Deck
{
    private readonly List<Card> _cards;

    public Deck(List<Card> cards)
    {
        _cards = cards;
    }

    public void Shuffle(Random? random = null)
    {
        var rng = random ?? new();
        for (var i = 0; i < _cards.Count; i++)
        {
            var j = rng.Next(i, _cards.Count);
            (_cards[j], _cards[i]) = (_cards[i], _cards[j]);
        }
    }

    public Card Deal()
    {
        if (_cards.Count == 0)
        {
            throw new InvalidOperationException("Deck is empty");
        }

        var card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }

    private static Lazy<List<Card>> _normalDeck = new Lazy<List<Card>>(() =>
    {
        const int NormalDeckCount = 52;
        var cards = new List<Card>(NormalDeckCount);

        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(suit, rank));
            }
        }
        return cards;
    });

    public static Deck CreateNormalDeck() => new Deck(new List<Card>(_normalDeck.Value));
}
