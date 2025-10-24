using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
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

	// TODO: This progress calculation is bad
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
		var completed = Objectives.First(o => o.CheckCompletion());
		Next = completed.Next;
		completed.Complete();

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

	public override void GetObjectivesIcon(MissionIconGroup iconGroup)
	{
		if (Next == null)
		{
			foreach (var objective in Objectives)
			{
				objective.GetObjectivesIcon(iconGroup);
			}
		}
		else
		{
			Next.GetObjectivesIcon(iconGroup);
		}
	}

	public override void GetObjectivesText(List<string> lines)
	{
		var index = 1;
		foreach (var objective in Objectives)
		{
			var tempLines = new List<string>();
			objective.GetObjectivesText(tempLines);

			for (int i = 0; i < tempLines.Count; i++)
			{
				if (Completed)
				{
					if (objective.Next == Next)
					{
						tempLines[i] = $"[TextDrawer,Text='(Branch {index})',Color='100,255,100,255']" + " " + tempLines[i];
					}
					else
					{
						tempLines[i] = $"[TextDrawer,Text='(Branch {index})',Color='100,100,100,255']" + " " + tempLines[i];
					}
				}
				else
				{
					tempLines[i] = $"[TextDrawer,Text='(Branch {index})',Color='100,180,120,255']" + " " + tempLines[i];
				}
			}

			lines.AddRange(tempLines);

			index++;
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
		base.LoadData(tag);

		MissionBase.LoadObjectives(tag, Objectives);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);

		MissionBase.SaveObjectives(tag, Objectives);
	}
}