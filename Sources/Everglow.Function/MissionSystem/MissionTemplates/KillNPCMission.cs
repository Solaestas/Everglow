using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionIcons;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

/// <summary>
/// Represents a mission where the player needs to kill a specified NPC or a quantity of NPCs.
/// </summary>
public abstract class KillNPCMission : MissionBase
{
	private float progress = 0f;

	public override MissionIconGroup Icon => new MissionIconGroup(
		[
			NPCMissionIcon.Create(DemandNPCs.First()?.NPCs.First() ?? NPCID.BlueSlime)
		]);

	public override float Progress => progress;

	public string SourceContext => $"{nameof(Everglow)}.{nameof(GainItemMission)}.{Name}";

	public abstract List<KillNPCRequirement> DemandNPCs { get; init; }

	public abstract List<Item> RewardItems { get; }

	public override void PostComplete()
	{
		foreach (var item in RewardItems)
		{
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(SourceContext), item, item.stack);
		}
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

		if (DemandNPCs.Count == 0)
		{
			progress = 1f;
			return;
		}

		// The final progress is calculated as the average of the individual progress for each NPC type group
		progress = DemandNPCs.Select(x => x.Progress).Average();
	}

	/// <summary>
	/// Count kill for each demand group
	/// </summary>
	/// <param name="type">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void CountKill(int type, int count = 1)
	{
		foreach (var kmDemand in DemandNPCs.Where(x => x.NPCs.Contains(type)))
		{
			if (kmDemand.EnableIndividualCounter)
			{
				kmDemand.Count(count);
			}
		}
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		LoadVanillaNPCTextures(DemandNPCs.SelectMany(x => x.NPCs));
		LoadVanillaItemTextures(RewardItems.Select(x => x.type));
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
	}
}