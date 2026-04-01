using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public class WorldParallelObjective : WorldObjectiveBase
{
	private IEnumerable<WorldObjectiveBase> _objectives;

	public IEnumerable<WorldObjectiveBase> Objectives => _objectives;

	public WorldParallelObjective()
	{
	}

	public WorldParallelObjective(params WorldObjectiveBase[] objectives)
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
		if (_objectives.Any(o => o is WorldParallelObjective or WorldBranchingObjective))
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

	public override void Activate(WorldMissionBase mission)
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
		WorldMissionBase.LoadObjectives(tag, Objectives);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		WorldMissionBase.SaveObjectives(tag, Objectives);
	}
}