using Everglow.Commons.Mechanics.MissionSystem.Core;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class ParallelObjective : MissionObjectiveBase
{
	private IEnumerable<MissionObjectiveBase> _objectives;

	public ParallelObjective()
	{
	}

	public ParallelObjective(params MissionObjectiveBase[] objectives)
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

	public override void Complete()
	{
		foreach (var objective in _objectives)
		{
			objective.Complete();
		}

		base.Complete();
	}

	public override bool CheckCompletion() => _objectives.All(o => o.CheckCompletion());

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

	public override void Activate(MissionBase mission)
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
		foreach (var objective in _objectives)
		{
			objective.LoadData(tag);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		foreach (var objective in _objectives)
		{
			objective.SaveData(tag);
		}
	}
}