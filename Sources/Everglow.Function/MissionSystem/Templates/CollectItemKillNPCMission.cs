using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Shared.Icons;
using Everglow.Commons.MissionSystem.Templates.Abstracts;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates;

public abstract class CollectItemKillNPCMission : MissionBase, ICollectItemMission, IKillNPCMission, IRewardItemMission
{
	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup(
		DemandCollectItems.SelectMany(s => s.Items).Select(i => ItemMissionIcon.Create(i)),
		DemandNPCs.SelectMany(s => s.NPCs).Select(i => NPCMissionIcon.Create(i)));

	public abstract List<Item> RewardItems { get; }

	public abstract List<KillNPCRequirement> DemandNPCs { get; init; }

	public abstract List<CollectItemRequirement> DemandCollectItems { get; init; }

	public override void PostComplete()
	{
		(this as IRewardItemMission).GiveReward();
	}

	public override void UpdateProgress(params object[] objs)
	{
		var progress1 = (this as ICollectItemMission).CalculateProgress(Main.LocalPlayer.inventory);
		var progress2 = (this as IKillNPCMission).CalculateProgress(MissionManager.NPCKillCounter);
		progress = (progress1 + progress2) / 2f;
	}

	public override string GetObjectives()
	{
		var objectives = string.Empty;
		objectives += (this as ICollectItemMission).GetObjectivesString(Main.LocalPlayer.inventory);
		objectives += (this as IKillNPCMission).GetObjectivesString();
		return objectives;
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		(this as ICollectItemMission).Load(tag);
		(this as IKillNPCMission).Load(tag);
		(this as IRewardItemMission).Load(tag);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		(this as IKillNPCMission).Save(tag);
	}
}