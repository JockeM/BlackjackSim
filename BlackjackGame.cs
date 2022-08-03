using Microsoft.Extensions.Logging;
using PlayingDeck;

public class Player
{
	public Hand Hand { get; } = new();
}

public class BlackjackGame
{
	public IPlayerTactic PlayerTactic { get; } = new PlayerTactic();
	public AutomaticDealer Dealer { get; } = new();
	public Player Player { get; } = new();
	public Deck Deck { get; }

	private readonly ILogger _logger;

	public BlackjackGame(ILogger logger)
	{
		var deck = Deck.CreateNormalDeck();
		deck.Shuffle();
		Deck = deck;

		_logger = logger;
	}

	public GameResult PlayRound()
	{
		Dealer.Hand.Add(Deck.Deal());
		_logger.LogInformation("Dealer has {Hand}", Dealer.Hand);

		Player.Hand.Add(Deck.Deal());
		Player.Hand.Add(Deck.Deal());
		_logger.LogInformation("Player has {Hand}", Player.Hand);

		if (Player.Hand.IsBlackjack())
		{
			_logger.LogInformation("Player has Blackjack");

			Dealer.Hand.Add(Deck.Deal());
			if (Dealer.Hand.IsBlackjack())
			{
				_logger.LogInformation("Dealer has Blackjack");
				return GameResult.Push;
			}

			return GameResult.PlayerWins;
		}

		// TODO: Move to player class
		PlayerChoice choice;
		do
		{
			choice = PlayerTactic.GetPlayerChoice(Player.Hand, Dealer.Hand);
			_logger.LogInformation("Player chose {Choice}", choice);

			if (choice is PlayerChoice.Hit)
			{
				Player.Hand.Add(Deck.Deal());
				_logger.LogInformation("Player has {Hand}", Player.Hand);
			}

			if (Player.Hand.IsBust())
			{
				_logger.LogInformation("Player has busted");
				return GameResult.DealerWins;
			}
		} while (choice is not PlayerChoice.Stand);

		if (Player.Hand.IsBust())
		{
			_logger.LogInformation("Player has Bust");
			return GameResult.DealerWins;
		}

		// TODO: Move to dealer class
		while (Dealer.Hand.GetPossibleHandValues().All(v => v is < 17))
		{
			Dealer.Hand.Add(Deck.Deal());
			_logger.LogInformation("Dealer has {Hand}", Dealer.Hand);
		}

		if (Dealer.Hand.IsBust())
		{
			_logger.LogInformation("Dealer has busted");
			return GameResult.PlayerWins;
		}

		return GetWinner(Player.Hand, Dealer.Hand);
	}

	public GameResult GetWinner(Hand player, Hand dealer)
	{
		if (player.IsBust() || dealer.IsBust())
		{
			throw new InvalidOperationException("Player and dealer must not be bust");
		}

		if (player.IsBlackjack() && dealer.IsBlackjack())
		{
			return GameResult.Push;
		}
		else if (player.IsBlackjack())
		{
			return GameResult.PlayerWins;
		}
		else if (dealer.IsBlackjack())
		{
			return GameResult.DealerWins;
		}

		var playerBest = player.GetPossibleHandValues().Where(v => v is <= 21).Max();
		var dealerBest = dealer.GetPossibleHandValues().Where(v => v is <= 21).Max();

		if (playerBest == dealerBest)
		{
			return GameResult.Push;
		}
		else if (playerBest > dealerBest)
		{
			return GameResult.PlayerWins;
		}
		else
		{
			return GameResult.DealerWins;
		}
	}
}


public enum GameResult
{
	PlayerWins,
	DealerWins,
	Push,
}
