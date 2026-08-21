using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;

namespace Everglow.Commons.Mechanics.Mission.WorldSide.Tests;

public class TestMissionTime : WorldMissionBase
{
	public override int TimeLimit => 600;

	public override void Initialize()
	{
		Objectives.Add(new TestMissionTimeObjective());
	}

	public class TestMissionTimeObjective : WorldObjectiveBase
	{
		public override bool CheckCompletion() => false;

		public override string GetObjectiveText() => string.Empty;

		public override void GetObjectivesIcon(MissionIconGroup iconGroup)
		{
		}
	}
}
