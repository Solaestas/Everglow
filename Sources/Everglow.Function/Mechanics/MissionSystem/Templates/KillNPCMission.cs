using Everglow.Commons.Mechanics.MissionSystem;
using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.Mechanics.MissionSystem.Templates.Abstracts;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Templates;

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