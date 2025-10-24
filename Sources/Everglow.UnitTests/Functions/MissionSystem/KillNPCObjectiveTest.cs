using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Terraria;
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

			for (int i = 0; i < reqCount * 10; i++)
			{
				var npc = new NPC();
				npc.type = NPCID.BlueSlime;
				objective.DemandNPC.Count(npc);
			}
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
				var npc = new NPC();
				npc.type = NPCID.BlueSlime;
				killNPCMission.DemandNPC.Count(npc);
			}

			Assert.IsTrue(killNPCMission.Progress == 1f);
		}
	}

	[TestMethod]
	public void IndividualCounter_Should_NotCount_When_NPCTypeIsInvalid()
	{
		int reqCount = 10;
		var req = KillNPCRequirement.Create(
			[
				NPCID.BlueSlime,
				NPCID.IceSlime,
				NPCID.SpikedJungleSlime,
				NPCID.MotherSlime,
			], reqCount, false);

		var npc = new NPC
		{
			type = NPCID.Zombie,
		};
		req.Count(npc);

		npc = new NPC
		{
			type = NPCID.MoonLordCore,
		};
		req.Count(npc);
		Assert.IsTrue(req.Counter == 0);
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
		var npc = new NPC();
		npc.type = NPCID.BlueSlime;
		killNPCMission.DemandNPC.Count(npc);
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