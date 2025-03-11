using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;
using Terraria.ID;

namespace Everglow.UnitTests.Functions.MissionSystem;

[TestClass]
public class KillNPCObjectiveTest
{
	[TestMethod]
	public void IndividualCounter_Should_BeCappedAtRequirement()
	{
		for (int reqCount = 1; reqCount < 30; reqCount++)
		{
			var objective = new KillNPCObjective(KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], reqCount, true));

			objective.DemandNPC.Count(reqCount * 10);
			Assert.IsTrue(objective.DemandNPC.Counter == reqCount);
		}
	}

	[TestMethod]
	public void IndividualProgress_Should_CalculateProperly()
	{
		for (int reqCount = 1; reqCount < 30; reqCount++)
		{
			var killNPCMission = new KillNPCObjective(
			KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], reqCount, true));

			for (int count = 0; count <= reqCount; count++)
			{
				Assert.IsTrue(killNPCMission.Progress == count / (float)reqCount);
				killNPCMission.DemandNPC.Count();
			}

			Assert.IsTrue(killNPCMission.Progress == 1f);
		}
	}

	[TestMethod]
	public void IndividualCounter_Should_NotThrowInvalidOperationException_When_CountWithoutBeingEnabled()
	{
		int reqCount = 10;
		var killNPCMission = new KillNPCObjective(
			KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], reqCount, false));

		killNPCMission.DemandNPC.Count();
		Assert.IsTrue(true);
	}

	[TestMethod]
	public void CreateRequirement_Should_ThrowInvalidDataException_When_ParamIsLessThanZero()
	{
		Assert.ThrowsException<InvalidDataException>(() =>
		{
			KillNPCRequirement.Create([], 1);
		});

		Assert.ThrowsException<InvalidDataException>(() =>
		{
			KillNPCRequirement.Create([NPCID.Worm], 0);
		});
	}
}