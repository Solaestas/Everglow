using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Objectives;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Objectives.Requirements;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Tests.ExampleNPC;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;

public class GiveItemMissionTest : PlayerMissionBase
{
	public override string DisplayName => GetType().Name;

	public GiveItemMissionTest()
	{
		Objectives.Add(new GiveItemObjective(new ItemRequirement([ItemID.DirtBlock], 10), ModContent.NPCType<ExamplePerson>(), "Give me xxxxx.", "Thank you"));
	}
}
