using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Tests;

public class TestConsumeItem : WorldMissionBase
{
	public override void Initialize()
	{
		Objectives.Add(new WorldConsumeItemObjective(ItemID.WoodenArrow, 10));
	}
}