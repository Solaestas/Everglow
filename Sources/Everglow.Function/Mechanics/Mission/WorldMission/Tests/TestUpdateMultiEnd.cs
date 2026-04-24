using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Tests;

public class TestUpdateMultiEnd : WorldMissionBase
{
	public override void Initialize()
	{
		Objectives
			.Add(new WorldKillNPCObjective(NPCID.DemonEye, 10))
			.Add(new WorldKillNPCObjective(NPCID.Zombie, 10));
	}
}