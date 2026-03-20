using Everglow.Commons.Mechanics.MissionSystem.Objectives;

namespace Everglow.UnitTests.Functions.MissionSystem;

[TestClass]
public class TalkNPCObjectiveTest
{
	[TestMethod]
	public void Constructor_Should_ThrowException_When_NPCTypeIsLessThanOne()
	{
		for (int i = 0; i > -100; i--)
		{
			Assert.ThrowsException<InvalidDataException>(() => new TalkNPCObjective(i, "111"));
		}
	}

	[TestMethod]
	public void Constructor_Should_ThrowException_When_NPCTextIsNullOrEmpty()
	{
		Assert.ThrowsException<ArgumentNullException>(() => new TalkNPCObjective(1, string.Empty));
		Assert.ThrowsException<ArgumentNullException>(() => new TalkNPCObjective(1, null));
	}
}