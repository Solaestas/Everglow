using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Shared.Icons;
using Everglow.Commons.MissionSystem.Templates.Abstracts;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates;

public abstract class KillNPCMission : MissionBase, IKillNPCMission, IRewardItemMission
{
	private float progress = 0f;

	public override MissionIconGroup Icon => new MissionIconGroup(
		DemandNPCs.SelectMany(s => s.NPCs).Select(i => NPCMissionIcon.Create(i)));

	public override float Progress => progress;

	public abstract List<KillNPCRequirement> DemandNPCs { get; init; }

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
		(this as IRewardItemMission).GiveReward();
	}

	public override void UpdateProgress(params object[] objs)
	{
		progress = (this as IKillNPCMission).CalculateProgress(MissionManager.NPCKillCounter);
	}

	public override IEnumerable<string> GetObjectives()
	{
		return (this as IKillNPCMission).GetObjectivesString(MissionManager.NPCKillCounter);
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		(this as IKillNPCMission).Load(tag);
		(this as IRewardItemMission).Load(tag);
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);

		(this as IKillNPCMission).Save(tag);
	}
}