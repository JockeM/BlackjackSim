// See https://aka.ms/new-console-template for more information


var logger = new NullLogger();

var plays = Enumerable.Range(1, 1_000_000)
	.Select(_ =>
	{
		var game = new BlackjackGame(logger);
		return game.PlayRound();
	});

int playerWins = 0;
int dealerWins = 0;
int pushs = 0;

foreach (var play in plays)
{
	if (play is GameResult.PlayerWins)
	{
		playerWins++;
	}
	else if (play is GameResult.DealerWins)
	{
		dealerWins++;
	}
	else
	{
		pushs++;
	}
}

Console.WriteLine($"Player wins: {playerWins}");
Console.WriteLine($"Dealer wins: {dealerWins}");
Console.WriteLine($"Pushs: {pushs}");