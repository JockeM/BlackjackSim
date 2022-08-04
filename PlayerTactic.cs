// See https://aka.ms/new-console-template for more information


using PlayingDeck;

namespace BlackjackSim;

class PlayerTactic : IPlayerTactic
{
    public PlayerChoice GetPlayerChoice(Card dealerCard, Hand player)
    {
        if (player.GetPossibleHandValues().Any(x => x is > 17 and <= 21))
        {
            return PlayerChoice.Stand;
        }

        return PlayerChoice.Hit;
    }
}
