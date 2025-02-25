using Everglow.Commons.Mechanics.MissionSystem.Core;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

[Obsolete("This objective is yet unfinished.", false)]
public class BranchingObjective : MissionObjectiveBase
{
	public BranchingObjective()
	{
	}

	public BranchingObjective(MissionObjectiveBase[] objectives)
	{
	}

	public override bool CheckCompletion() => throw new NotImplementedException();

	public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();
}