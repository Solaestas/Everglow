using Everglow.Commons.Mechanics.BiomesText;
using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Tests;

public class TestNewObj : WorldMissionBase
{
	public override void Initialize()
	{
		Objectives
			.Add(new WorldCollectItemObjective(ItemID.Ichor, 10))
			.Add(new WorldGiveObjective(NPCID.Guide, ItemID.Ichor, 10))
			.Add(new WorldReachObjective((p) => p.InVanillaBiome(VanillaBiomes.Desert)))
			.Add(new WorldTalkObjective(NPCID.Nurse));
	}
}