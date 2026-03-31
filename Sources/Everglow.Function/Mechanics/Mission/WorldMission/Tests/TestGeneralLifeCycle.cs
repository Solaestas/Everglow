using Everglow.Commons.Mechanics.Mission.WorldMission.Base;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Tests;

public class TestGeneralLifeCycle : MissionBase_New
{
	public override string Name => nameof(TestGeneralLifeCycle);

	public override void Initialize()
	{
		Objectives.Add(new TestGeneralMissionLifeCycleObjective());
	}

	public class TestGeneralMissionLifeCycleObjective : ObjectiveBase
	{
		public override bool CheckCompletion() => true;

		public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();
	}
}