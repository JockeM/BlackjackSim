using PlayingDeck;
using Xunit;

namespace BlackjackSim.Tests;

public class BlackjackHandTests
{
	Hand Hand(params Rank[] ranks) => new Hand(ranks.Select(x => new Card(Suit.Clubs, x)));

	[Fact]
	public void GetPossibleHandValues_TwoAces_ReturnsSequence()
	{
		// Given
		var hand = Hand(Rank.Ace, Rank.Ace);
		// When
		var result = hand.GetPossibleHandValues();
		// Then
		Assert.Equal(new[] { 2, 12, 22 }, result);
	}

	[Fact]
	public void GetPossibleHandValues_ThreeAces_ReturnsSequence()
	{
		var hand = Hand(Rank.Ace, Rank.Ace, Rank.Ace);

		var result = hand.GetPossibleHandValues();

		Assert.Equal(new[] { 3, 13, 23, 33 }, result);
	}

	[Fact]
	public void GetPossibleHandValues_OneAceAndATwo_ReturnsSequence()
	{
		var hand = Hand(Rank.Ace, Rank.Two);

		var result = hand.GetPossibleHandValues();

		Assert.Equal(new[] { 3, 13 }, result);
	}

	[Fact]
	public void GetPossibleHandValues_TwoFives_ReturnsTen()
	{
		var hand = Hand(Rank.King, Rank.Five);

		var result = hand.GetPossibleHandValues();

		Assert.Equal(new[] { 15 }, result);
	}

	[Fact]
	public void IsBlackjack_GivenKingAndAce_ReturnsTrue()
	{
		var hand = Hand(Rank.King, Rank.Ace);

		var result = hand.IsBlackjack();

		Assert.True(result);
	}

	[Fact]
	public void IsBlackjack_GivenTwoAces_ReturnsFalse()
	{
		var hand = Hand(Rank.Ace, Rank.Ace);

		var result = hand.IsBlackjack();

		Assert.False(result);
	}

	[Fact]
	public void IsBlackjack_GivenAceAndNine_ReturnsFalse()
	{
		var hand = Hand(Rank.Ace, Rank.Nine);

		var result = hand.IsBlackjack();

		Assert.False(result);
	}

	[Fact]
	public void IsBlackjack_GivenTreeCardSwith21Sum_ReturnsFalse()
	{
		var hand = Hand(Rank.Eight, Rank.Eight, Rank.Three);

		var result = hand.IsBlackjack();

		Assert.False(result);
	}

	[Fact]
	public void IsBust_GivenTreeCardSwith21Sum_ReturnsFalse()
	{
		var hand = Hand(Rank.Eight, Rank.Eight, Rank.Three);

		var result = hand.IsBust();

		Assert.False(result);
	}

	[Fact]
	public void IsBust_GivenBustHandWithTwoAces_ReturnsFalse()
	{
		var hand = Hand(Rank.Ace, Rank.Ace, Rank.Ten, Rank.Ten);

		var result = hand.IsBust();

		Assert.True(result);
	}

	[Fact]
	public void IsBust_GivenBustHand_ReturnsFalse()
	{
		var hand = Hand(Rank.King, Rank.Jack, Rank.Two);

		var result = hand.IsBust();

		Assert.True(result);
	}
}