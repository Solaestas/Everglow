using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Shared.Icons;
using Everglow.Commons.MissionSystem.Templates.Abstracts;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates;

public abstract class ConsumeItemMission : MissionBase, IConsumeItemMission, IRewardItemMission
{
	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup(
		DemandConsumeItems.SelectMany(s => s.Items).Select(i => ItemMissionIcon.Create(i)));

	public abstract List<ItemRequirement> DemandConsumeItems { get; init; }

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
		(this as IRewardItemMission).GiveReward();
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		(this as IConsumeItemMission).Load(tag);
		(this as IRewardItemMission).Load(tag);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);

		(this as IConsumeItemMission).Save(tag);
	}

	public override void UpdateProgress(params object[] objs)
	{
		progress = (this as IConsumeItemMission).CalculateProgress();
	}
}