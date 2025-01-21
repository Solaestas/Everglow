using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared.Icons;
using Everglow.Commons.MissionSystem.Templates.Abstracts;

namespace Everglow.Commons.MissionSystem.Templates;

public abstract class TalkToNPCMission : MissionBase, ITalkToNPCMission, IRewardItemMission
{
	public abstract int NPCType { get; }

	public abstract string NPCText { get; }

	public override bool AutoComplete => true;

	private float progress;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup(NPCMissionIcon.Create(NPCType));

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
		(this as IRewardItemMission).GiveReward();
	}

	public override void UpdateProgress(params object[] objs)
	{
		progress = (this as ITalkToNPCMission).CalculateProgress();
	}

	public override string GetObjectives()
	{
		return (this as ITalkToNPCMission).GetObjectivesString();
	}
}