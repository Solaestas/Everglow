using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

namespace Everglow.Commons.Mechanics.Mission.WorldMission;

public class MissionObjectiveContainer_New
{
	public MissionObjectiveContainer_New() => AllObjectives = [];

	public ObjectiveBase this[int index] => AllObjectives[index];

	public List<ObjectiveBase> AllObjectives { get; private set; }

	/// <summary>
	/// The first objective.
	/// </summary>
	public ObjectiveBase First { get; private set; }

	/// <summary>
	/// The last objective.
	/// </summary>
	private ObjectiveBase Last { get; set; }

	public ObjectiveBase FirstIncomplete => AllObjectives.Count != 0 ?
		AllObjectives.First(x => !x.Completed) : null;

	public MissionObjectiveContainer_New Add(ObjectiveBase objective)
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
	public MissionObjectiveContainer_New AddParallel(params ObjectiveBase[] objectives)
	{
		return Add(new ParallelObjective_New(objectives));
	}

	/// <summary>
	/// Add a branching objective
	/// </summary>
	/// <param name="branches"></param>
	/// <returns></returns>
	public MissionObjectiveContainer_New AddBranches(params MissionObjectiveContainer_New[] branches)
	{
		// foreach (var builder in branches)
		// {
		// 	 CombineWith(builder);
		// }
		var branchingObjective = new BranchingObjective_New(branches.Select(t => t.First).ToArray());

		return Add(branchingObjective);
	}

	public void CombineWith(MissionObjectiveContainer_New data)
	{
		foreach (ObjectiveBase objective in data.AllObjectives)
		{
			objective.ObjectiveID = AllObjectives.Count;
			AllObjectives.Add(objective);
		}
	}
}