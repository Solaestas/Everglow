using Everglow.Commons.Mechanics.Quest.PlayerSide.Objectives;
using Terraria.GameContent.Personalities;

namespace Everglow.UnitTests.Function.QuestSystem;

[TestClass]
public class ExploreObjectiveTest
{
	[TestMethod]
	public void Constructor_NegativeMoveRequirement_Throws()
	{
		Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ExploreObjective(new ForestBiome(), -1f));
	}
}
