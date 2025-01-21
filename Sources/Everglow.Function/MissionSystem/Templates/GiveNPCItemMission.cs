using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Shared.Icons;
using Everglow.Commons.MissionSystem.Templates.Abstracts;

namespace Everglow.Commons.MissionSystem.Templates;

public abstract class GiveNPCItemMission : MissionBase, ITalkToNPCMission, IGiveItemMission, IRewardItemMission
{
	public float progress = 0f;

	public override float Progress => progress;

	public override bool AutoComplete => true;

	public bool SubmitItemsOnComplete => true;

	public abstract List<GiveItemRequirement> DemandGiveItems { get; init; }

	public abstract int NPCType { get; }

	public abstract string NPCText { get; }

	public abstract string CompleteText { get; }

	public override MissionIconGroup Icon => new MissionIconGroup(NPCMissionIcon.Create(NPCType));

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
		base.PostComplete();

		(this as IGiveItemMission).ConsumeItem(Main.LocalPlayer.inventory);
		(this as ITalkToNPCMission).UpdateNPCText(CompleteText);
		(this as IRewardItemMission).GiveReward();
	}

	public override void UpdateProgress(params object[] objs)
	{
		var talkProgress = (this as ITalkToNPCMission).CalculateProgress();
		var itemProgress = (this as IGiveItemMission).CalculateProgress(Main.LocalPlayer.inventory);

		progress = talkProgress == 1f && itemProgress == 1f ? 1f : 0f;
	}

	public override IEnumerable<string> GetObjectives()
	{
		return new List<string>() { (this as ITalkToNPCMission).GetObjectivesString() }
			.Concat((this as IGiveItemMission).GetObjectivesString(Main.LocalPlayer.inventory));
	}
}