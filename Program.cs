using BlackjackSim;
using System.Diagnostics;

var basicStrategy = new BasicStrategy();
var playerTactic = new PlayerTactic();

Console.WriteLine("BasicStrategy");
Sim(basicStrategy);
Console.WriteLine();
Console.WriteLine("Shitty Player Strategy");
Sim(playerTactic);

static void Sim(IPlayerTactic tactic)
{
    var logger = new NullLogger();

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    const int Runs = 1_000_000;

    var plays = Enumerable
        .Range(1, Runs)
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
