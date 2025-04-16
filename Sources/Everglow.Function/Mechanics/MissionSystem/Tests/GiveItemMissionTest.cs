using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Tests.ExampleNPC;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class GiveItemMissionTest : MissionBase
{
	public override string DisplayName => GetType().Name;

	public GiveItemMissionTest()
	{
		Objectives.Add(new GiveItemObjective(new ItemRequirement([ItemID.DirtBlock], 10), ModContent.NPCType<ExamplePerson>(), "Give me xxxxx.", "Thank you"));
	}
}