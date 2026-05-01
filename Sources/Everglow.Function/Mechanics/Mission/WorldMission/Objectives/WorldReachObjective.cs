using Everglow.Commons.Mechanics.Mission.WorldMission.Base;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public class WorldReachObjective : WorldObjectiveBase
{
	public WorldReachObjective()
	{
	}

	public WorldReachObjective(Func<Player, bool> condition)
	{
		Condition = condition;
	}

	private bool progress;

	public Func<Player, bool> Condition { get; private set; }

	public override float Progress => progress ? 1f : 0f;

	public override bool CheckCompletion() => progress;

	public override void Update()
	{
		foreach (var player in Main.ActivePlayers)
		{
			if (Condition(player))
			{
				progress = true;
				return;
			}
		}
	}

	public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();
}