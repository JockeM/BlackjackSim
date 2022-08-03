// See https://aka.ms/new-console-template for more information


public interface IPlayerTactic
{
	int GetBet(int balance);
	PlayerChoice GetPlayerChoice(Hand player, Hand dealer);
}


class PlayerTactic : IPlayerTactic
{
	public int GetBet(int balance)
	{
		return balance / 2;
	}

	public PlayerChoice GetPlayerChoice(Hand player, Hand dealer)
	{
		if (player.GetPossibleHandValues().Any(x => x is > 17 and <= 21))
		{
			return PlayerChoice.Stand;
		}

		return PlayerChoice.Hit;
	}
}