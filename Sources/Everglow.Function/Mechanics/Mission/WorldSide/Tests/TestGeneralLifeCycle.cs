using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;

namespace Everglow.Commons.Mechanics.Mission.WorldSide.Tests;

public class TestGeneralLifeCycle : WorldMissionBase
{
	public override void Initialize()
	{
		Objectives.Add(new TestGeneralMissionLifeCycleObjective());
	}

	public class TestGeneralMissionLifeCycleObjective : WorldObjectiveBase
	{
		public override bool CheckCompletion() => true;

		public override string GetObjectiveText() => string.Empty;

		public override void GetObjectivesIcon(MissionIconGroup iconGroup)
		{
		}
	}
}
