using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Shared.Icons;
using Everglow.Commons.MissionSystem.Templates.Abstracts;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates;

public abstract class CollectItemMission : MissionBase, ICollectItemMission, IRewardItemMission
{
	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup(
		DemandCollectItems.SelectMany(s => s.Items).Select(i => ItemMissionIcon.Create(i)));

	public abstract List<CollectItemRequirement> DemandCollectItems { get; init; }

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
		(this as IRewardItemMission).GiveReward();
	}

	public override void UpdateProgress(params object[] objs)
	{
		progress = (this as ICollectItemMission).CalculateProgress(Main.LocalPlayer.inventory);
	}

	public override string GetObjectives()
	{
		return (this as ICollectItemMission).GetObjectivesString(Main.LocalPlayer.inventory);
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		(this as ICollectItemMission).Load(tag);
		(this as IRewardItemMission).Load(tag);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);

		(this as ICollectItemMission).Save(tag);
	}
}