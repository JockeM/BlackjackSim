using PlayingDeck;

namespace BlackjackSim;

public interface IPlayerTactic
{
    PlayerChoice GetPlayerChoice(Card dealerCard, Hand player);
}
