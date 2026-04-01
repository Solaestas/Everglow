using Everglow.Commons.Mechanics.Mission.WorldMission.Base;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Tests;

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

		public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();
	}
}