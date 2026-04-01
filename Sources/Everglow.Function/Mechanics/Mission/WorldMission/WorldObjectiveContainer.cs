using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

namespace Everglow.Commons.Mechanics.Mission.WorldMission;

public class WorldObjectiveContainer
{
	public WorldObjectiveContainer() => AllObjectives = [];

	public WorldObjectiveBase this[int index] => AllObjectives[index];

	public List<WorldObjectiveBase> AllObjectives { get; private set; }

	/// <summary>
	/// The first objective.
	/// </summary>
	public WorldObjectiveBase First { get; private set; }

	/// <summary>
	/// The last objective.
	/// </summary>
	private WorldObjectiveBase Last { get; set; }

	public WorldObjectiveBase FirstIncomplete => AllObjectives.FirstOrDefault(x => !x.Completed);

	public WorldObjectiveContainer Add(WorldObjectiveBase objective)
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
	public WorldObjectiveContainer AddParallel(params WorldObjectiveBase[] objectives)
	{
		return Add(new WorldParallelObjective(objectives));
	}

	/// <summary>
	/// Add a branching objective
	/// </summary>
	/// <param name="branches"></param>
	/// <returns></returns>
	public WorldObjectiveContainer AddBranches(params WorldObjectiveContainer[] branches)
	{
		// foreach (var builder in branches)
		// {
		// 	 CombineWith(builder);
		// }
		var branchingObjective = new WorldBranchingObjective(branches.Select(t => t.First).ToArray());

		return Add(branchingObjective);
	}

	public void CombineWith(WorldObjectiveContainer data)
	{
		foreach (WorldObjectiveBase objective in data.AllObjectives)
		{
			objective.ObjectiveID = AllObjectives.Count;
			AllObjectives.Add(objective);
		}
	}
}