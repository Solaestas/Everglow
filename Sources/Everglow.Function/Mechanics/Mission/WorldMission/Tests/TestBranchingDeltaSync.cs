using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Tests;

public class TestBranchingDeltaSync : WorldMissionBase
{
	public override void Initialize()
	{
		var b1 = new WorldObjectiveContainer();
		b1.Add(new WorldConsumeItemObjective(ItemID.FlamingArrow, 10));

		var b2 = new WorldObjectiveContainer()
			.Add(new WorldKillNPCObjective(NPCID.BlueSlime, 5))
			.Add(new WorldKillNPCObjective(NPCID.AngryBones, 5));

		//Objectives.AddBranches([b1, b2]);
	}
}