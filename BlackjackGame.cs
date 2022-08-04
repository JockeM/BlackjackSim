using Microsoft.Extensions.Logging;
using PlayingDeck;

namespace BlackjackSim;

public class Player
{
    public Hand Hand { get; } = new();
}

public class BlackjackGame
{
    private readonly ILogger _logger;
    private readonly IPlayerTactic _playerTactic;
    private readonly Deck _deck;

    private readonly AutomaticDealer _dealer = new();
    private readonly Player _player = new();
    
    public BlackjackGame(ILogger logger, IPlayerTactic tactic)
    {
        var deck = Deck.CreateNormalDeck();
        deck.Shuffle();
        _deck = deck;
        _logger = logger;
        _playerTactic = tactic;
    }

    public GameResult PlayRound()
    {
        _dealer.Hand.Add(_deck.Deal());
        _logger.LogInformation("Dealer has {Hand}", _dealer.Hand);

        _player.Hand.Add(_deck.Deal());
        _player.Hand.Add(_deck.Deal());
        _logger.LogInformation("Player has {Hand}", _player.Hand);

        if (_player.Hand.IsBlackjack())
        {
            _logger.LogInformation("Player has Blackjack");

            _dealer.Hand.Add(_deck.Deal());
            if (_dealer.Hand.IsBlackjack())
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
            choice = _playerTactic.GetPlayerChoice(_dealer.Hand.Cards.Single(), _player.Hand);
            _logger.LogInformation("Player chose {Choice}", choice);

            if (choice is PlayerChoice.Hit)
            {
                _player.Hand.Add(_deck.Deal());
                _logger.LogInformation("Player has {Hand}", _player.Hand);
            }

            if (_player.Hand.IsBust())
            {
                _logger.LogInformation("Player has busted");
                return GameResult.DealerWins;
            }
        } while (choice is not PlayerChoice.Stand);

        if (_player.Hand.IsBust())
        {
            _logger.LogInformation("Player has Bust");
            return GameResult.DealerWins;
        }

        // TODO: Move to dealer class
        while (_dealer.Hand.GetPossibleHandValues().All(v => v is < 17))
        {
            _dealer.Hand.Add(_deck.Deal());
            _logger.LogInformation("Dealer has {Hand}", _dealer.Hand);
        }

        if (_dealer.Hand.IsBust())
        {
            _logger.LogInformation("Dealer has busted");
            return GameResult.PlayerWins;
        }
        var winner = GetWinner(_player.Hand, _dealer.Hand);

        _player.Hand.Clear();
        _dealer.Hand.Clear();

        return winner;
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