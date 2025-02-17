using Everglow.Commons.MissionSystem.Shared;
using Everglow.UnitTests.Functions.MissionSystem.TestMissions;
using Terraria.ID;

namespace Everglow.UnitTests.Functions.MissionSystem;

[TestClass]
public class KillNPCMissionTest
{
	[TestMethod]
	public void IndividualCounter_Should_BeCappedAtRequirement()
	{
		int reqCount = 10;
		var killNPCMission = new UnitTestKillNPCMission1([
			KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], reqCount, true)]);
		foreach (var r in killNPCMission.DemandNPCs)
		{
			r.Count(count: 100);
			Assert.IsTrue(r.Counter == reqCount);
		}
	}

	[TestMethod]
	public void IndividualProgress_Should_CalculateProperly()
	{
		int reqCount = 10;
		var killNPCMission = new UnitTestKillNPCMission1([
			KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], reqCount, true)]);

		killNPCMission.UpdateProgress();

		for (int count = 0; count <= reqCount; count++)
		{
			Assert.IsTrue(killNPCMission.Progress == count / (float)reqCount);
			killNPCMission.DemandNPCs.First().Count(1);
			killNPCMission.UpdateProgress();
		}

		Assert.IsTrue(killNPCMission.Progress == 1f);
	}

	[TestMethod]
	public void IndividualCounter_Should_NotThrowInvalidOperationException_When_CountWithoutBeingEnabled()
	{
		int reqCount = 10;
		var killNPCMission = new UnitTestKillNPCMission1([
			KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], reqCount, false)]);

		killNPCMission.DemandNPCs.First().Count();
		Assert.IsTrue(true);
	}
}