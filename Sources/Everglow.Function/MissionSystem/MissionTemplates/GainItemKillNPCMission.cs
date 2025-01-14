using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionIcons;
using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

public abstract class GainItemKillNPCMission : MissionBase, IGainItemMission, IKillNPCMission, IRewardItemMission
{
	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup([
		TextureMissionIcon.Create(TextureAssets.MagicPixel.Value),
		]);

	public abstract List<Item> RewardItems { get; }

	public abstract List<KillNPCRequirement> DemandNPCs { get; init; }

	public abstract List<GainItemRequirement> DemandItems { get; }

	public bool Consume => false;

	public override void PostComplete()
	{
		(this as IGainItemMission).ConsumeItem(Main.LocalPlayer.inventory);
		(this as IRewardItemMission).GiveReward();
	}

	public override void Update()
	{
		base.Update();

		UpdateProgress();
	}

	public override void UpdateProgress(params object[] objs)
	{
		if (PoolType != MissionManager.PoolType.Accepted)
		{
			return;
		}

		var progress1 = (this as IGainItemMission).CalculateProgress(Main.LocalPlayer.inventory);
		var progress2 = (this as IKillNPCMission).CalculateProgress();
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