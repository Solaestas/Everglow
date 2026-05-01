using Everglow.Commons.Mechanics.Mission.WorldMission.Base;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public class WorldTalkObjective : WorldObjectiveBase
{
	public WorldTalkObjective()
	{
	}

	public WorldTalkObjective(int npcType)
	{
		NpcType = npcType;
	}

	private bool progress;

	public int NpcType { get; private set; }

	public override float Progress => progress ? 1f : 0f;

	public override bool CheckCompletion() => progress;

	public override void Update()
	{
		foreach (var player in Main.ActivePlayers)
		{
			if (player.TalkNPC?.type == NpcType)
			{
				progress = true;
				return;
			}
		}
	}

	public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();
}