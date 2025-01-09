using Everglow.Commons.MissionSystem.MissionTemplates;
using static Everglow.Commons.MissionSystem.MissionTemplates.KillNPCMission;
using Terraria.ID;
using Terraria;
using Everglow.Commons.MissionSystem;

namespace Everglow.UnitTests.Functions.MissionSystem;

[TestClass]
public class KillNPCMissionTest
{
	public MissionManager MissionManager { get; set; }

	[TestInitialize]
	public void Initialize()
	{
		MissionManager = new MissionManager();
	}

	[TestMethod]
	public void IndividualCounter_Should_BeCappedAtRequirement()
	{
		var killNPCMission = new KillNPCMission();
		killNPCMission.SetInfo("Test4", "击杀10个史莱姆", "测试介绍: \n" + "[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
		int reqCount = 10;
		killNPCMission.DemandNPCs.AddRange([
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
		var killNPCMission = new KillNPCMission();
		killNPCMission.SetInfo("Test4", "击杀10个史莱姆", "123");
		int reqCount = 10;
		killNPCMission.DemandNPCs.AddRange([
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
		var killNPCMission = new KillNPCMission();
		killNPCMission.SetInfo("Test4", "击杀10个史莱姆", "123");

		int reqCount = 10;
		killNPCMission.DemandNPCs.AddRange([
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