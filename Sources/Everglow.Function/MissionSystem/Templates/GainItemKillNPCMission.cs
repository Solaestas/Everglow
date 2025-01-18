using Everglow.Commons.MissionSystem.Abstracts.Missions;
using Everglow.Commons.MissionSystem.MissionIcons;
using Everglow.Commons.MissionSystem.Shared;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates;

public abstract class GainItemKillNPCMission : MissionBase, IGainItemMission, IKillNPCMission, IRewardItemMission
{
	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup(
		DemandGainItems.SelectMany(s => s.Items).Select(i => ItemMissionIcon.Create(i)),
		DemandNPCs.SelectMany(s => s.NPCs).Select(i => NPCMissionIcon.Create(i)));

	public abstract List<Item> RewardItems { get; }

	public abstract List<KillNPCRequirement> DemandNPCs { get; init; }

	public abstract List<GainItemRequirement> DemandGainItems { get; init; }

	public bool SubmitItemsOnComplete => false;

	public override void PostComplete()
	{
		(this as IGainItemMission).ConsumeItem(Main.LocalPlayer.inventory);
		(this as IRewardItemMission).GiveReward();
	}

	public override void UpdateProgress(params object[] objs)
	{
		var progress1 = (this as IGainItemMission).CalculateProgress(Main.LocalPlayer.inventory);
		var progress2 = (this as IKillNPCMission).CalculateProgress(MissionManager.NPCKillCounter);
		progress = (progress1 + progress2) / 2f;
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		(this as IGainItemMission).Load(tag);
		(this as IKillNPCMission).Load(tag);
		(this as IRewardItemMission).Load(tag);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		(this as IKillNPCMission).Save(tag);
	}
}