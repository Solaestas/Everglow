using Everglow.Commons.Mechanics.Mission.PlayerMission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Tests.ExampleNPC;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Objectives;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Primitives;

namespace Everglow.Commons.Mechanics.Mission.PlayerMission.Tests;

public class GiveItemMissionTest : MissionBase
{
	public override string DisplayName => GetType().Name;

	public GiveItemMissionTest()
	{
		Objectives.Add(new GiveItemObjective(new ItemRequirement([ItemID.DirtBlock], 10), ModContent.NPCType<ExamplePerson>(), "Give me xxxxx.", "Thank you"));
	}
}