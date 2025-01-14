using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionIcons;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

public abstract class KillNPCMission : MissionBase, IKillNPCMission, IRewardItemMission
{
	private float progress = 0f;

	public override MissionIconGroup Icon => new MissionIconGroup(
		[
			NPCMissionIcon.Create(DemandNPCs.First()?.NPCs.First() ?? NPCID.BlueSlime)
		]);

	public override float Progress => progress;

	public abstract List<KillNPCRequirement> DemandNPCs { get; init; }

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
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

		progress = (this as IKillNPCMission).CalculateProgress();
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		tag.TryGet<List<KillNPCRequirement>>(nameof(DemandNPCs), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			foreach (var demand in DemandNPCs.Where(d => d.EnableIndividualCounter))
			{
				demand.Count(
					demandNPCs
						.Where(d => d.EnableIndividualCounter && d.NPCs.Intersect(demand.NPCs).Any())
						.Sum(x => x.Counter));
			}
		}

		LoadVanillaNPCTextures(DemandNPCs.SelectMany(x => x.NPCs));
		LoadVanillaItemTextures(RewardItems.Select(x => x.type));
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);

		tag.Add(nameof(DemandNPCs), DemandNPCs);
	}
}