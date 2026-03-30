using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public class ParallelObjective_New : ObjectiveBase
{
	private IEnumerable<ObjectiveBase> _objectives;

	public IEnumerable<ObjectiveBase> Objectives => _objectives;

	public ParallelObjective_New()
	{
	}

	public ParallelObjective_New(params ObjectiveBase[] objectives)
	{
		_objectives = objectives;
	}

	public override float Progress => _objectives.Average(o => o.Progress);

	public override void OnInitialize()
	{
		foreach (var item in _objectives)
		{
			item.OnInitialize();
		}
	}

	public override void Update()
	{
		if (_objectives.Any(o => o is ParallelObjective_New or BranchingObjective_New))
		{
			throw new InvalidDataException("Parallel objective or branching objective should not nest in itself or other.");
		}
	}

	public override void Complete()
	{
		foreach (var objective in _objectives)
		{
			objective.Complete();
		}

		base.Complete();
	}

	public override bool CheckCompletion() => _objectives.All(o => o.CheckCompletion());

	//public override void GetObjectivesIcon(MissionIconGroup iconGroup)
	//{
	//	foreach (var objective in _objectives)
	//	{
	//		objective.GetObjectivesIcon(iconGroup);
	//	}
	//}

	public override void GetObjectivesText(List<string> lines)
	{
		foreach (var objective in _objectives)
		{
			objective.GetObjectivesText(lines);
		}
	}

	public override void ResetProgress()
	{
		base.ResetProgress();

		foreach (var objective in _objectives)
		{
			objective.ResetProgress();
		}
	}

	public override void Activate(MissionBase_New mission)
	{
		foreach (var objective in _objectives)
		{
			objective.Activate(mission);
		}
		base.Activate(mission);
	}

	public override void Deactivate()
	{
		base.Deactivate();
		foreach (var objective in _objectives)
		{
			objective.Deactivate();
		}
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		MissionBase_New.LoadObjectives(tag, Objectives);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		MissionBase_New.SaveObjectives(tag, Objectives);
	}
}