// See https://aka.ms/new-console-template for more information

using BlackjackSim;
using System.Diagnostics;

var basicStrategy = new BasicStrategy();
var playerTactic = new PlayerTactic();

Console.WriteLine("BasicStrategy");
NewMethod(basicStrategy);
Console.WriteLine();
Console.WriteLine("Shitty Player Stategy");
NewMethod(playerTactic);

static void NewMethod(IPlayerTactic tactic)
{
	var logger = new NullLogger();

	var stopwatch = new Stopwatch();
	stopwatch.Start();

	var plays = Enumerable.Range(1, 1_000_000)
		.AsParallel()
		.Select(_ =>
		{
			var game = new BlackjackGame(logger, tactic);
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

	stopwatch.Stop();
	Console.WriteLine($"Sim time: {stopwatch.ElapsedMilliseconds}");
	Console.WriteLine($"Player wins: {playerWins}");
	Console.WriteLine($"Dealer wins: {dealerWins}");
	Console.WriteLine($"Pushs: {pushs}");
}