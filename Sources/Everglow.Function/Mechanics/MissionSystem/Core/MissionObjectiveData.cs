using Everglow.Commons.Mechanics.MissionSystem.Objectives;

namespace Everglow.Commons.Mechanics.MissionSystem.Core;

public class MissionObjectiveData
{
	public MissionObjectiveData() => AllObjectives = [];

	public MissionObjectiveBase this[int index] => AllObjectives[index];

	public List<MissionObjectiveBase> AllObjectives { get; private set; }

	/// <summary>
	/// The first objective.
	/// </summary>
	public MissionObjectiveBase First { get; private set; }

	/// <summary>
	/// The last objective.
	/// </summary>
	private MissionObjectiveBase Last { get; set; }

	public MissionObjectiveBase FirstIncomplete => AllObjectives.Count != 0 ?
		AllObjectives.First(x => !x.Completed) : null;

	public MissionObjectiveData Add(MissionObjectiveBase objective)
	{
		objective.ObjectiveID = AllObjectives.Count;
		objective.OnInitialize(); // On create hook

		AllObjectives.Add(objective);

		if (First == null)
		{
			First = objective;
			Last = First;
			return this;
		}

		Last.Next = objective;
		Last = objective;
		return this;
	}

	/// <summary>
	/// Add a parallel objective. Shorthand for
	/// <code>
	/// Add(new ParallelObjective([]));
	/// </code>
	/// <para/>
	/// </summary>
	/// <param name="tasks">The array of objectives will be in the parallel objective</param>
	/// <returns></returns>
	public MissionObjectiveData AddParallel(params MissionObjectiveBase[] objectives)
	{
		return Add(new ParallelObjective(objectives));
	}

	/// <summary>
	/// Add a branching objective
	/// </summary>
	/// <param name="branches"></param>
	/// <returns></returns>
	public MissionObjectiveData AddBranches(params MissionObjectiveData[] branches)
	{
		//foreach (var builder in branches)
		//{
		//	CombineWith(builder);
		//}

		var branchingObjective = new BranchingObjective(branches.Select(t => t.First).ToArray());

		return Add(branchingObjective);
	}

	public void CombineWith(MissionObjectiveData data)
	{
		foreach (MissionObjectiveBase objective in data.AllObjectives)
		{
			objective.ObjectiveID = AllObjectives.Count;
			AllObjectives.Add(objective);
		}
	}
}