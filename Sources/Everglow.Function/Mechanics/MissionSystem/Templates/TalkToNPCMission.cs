using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.Mechanics.MissionSystem.Templates.Abstracts;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Templates;

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

	public override IEnumerable<string> GetObjectives()
	{
		return [(this as ITalkToNPCMission).GetObjectivesString()];
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		(this as IRewardItemMission).Load(tag);
	}
}