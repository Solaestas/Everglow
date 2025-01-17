using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionIcons;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

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