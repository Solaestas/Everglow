using Everglow.Commons.Mechanics.MissionSystem.Core;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class BranchingObjective : MissionObjectiveBase
{
	public BranchingObjective()
	{
		Objectives = [];
	}

	public BranchingObjective(MissionObjectiveBase[] objectives)
	{
		Objectives = objectives;
	}

	public IEnumerable<MissionObjectiveBase> Objectives { get; }

	public override float Progress => Objectives.Max(x => x.Progress);

	public override void OnInitialize()
	{
		foreach (var item in Objectives)
		{
			item.OnInitialize();
		}
	}

	public override void Update()
	{
		if (Objectives.Any(o => o is ParallelObjective or BranchingObjective))
		{
			throw new InvalidDataException("Parallel objective or branching objective should not nest in itself or other.");
		}

		foreach (MissionObjectiveBase objective in Objectives)
		{
			if (objective.CheckCompletion())
			{
				Next = objective.Next;
				return;
			}
		}
	}

	public override void Complete()
	{
		Objectives.First(o => o.CheckCompletion()).Complete();

		base.Complete();
	}

	public override bool CheckCompletion() => Objectives.Any(x => x.CheckCompletion());

	public override void Activate(MissionBase fromQuest)
	{
		foreach (MissionObjectiveBase objective in Objectives)
		{
			objective.Activate(fromQuest);
		}
		base.Activate(fromQuest);
	}

	public override void Deactivate()
	{
		foreach (MissionObjectiveBase objective in Objectives)
		{
			objective.Deactivate();
		}
		base.Deactivate();
	}

	public override void GetObjectivesText(List<string> lines)
	{
		foreach (var objective in Objectives)
		{
			objective.GetObjectivesText(lines);
		}
	}

	public override void ResetProgress()
	{
		base.ResetProgress();
		Next = null;
		foreach (MissionObjectiveBase objective in Objectives)
		{
			objective.ResetProgress();
		}
	}

	public override void LoadData(TagCompound tag)
	{
		MissionBase.LoadObjectives(tag, Objectives);
	}

	public override void SaveData(TagCompound tag)
	{
		MissionBase.SaveObjectives(tag, Objectives);
	}
}