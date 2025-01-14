using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionAbstracts;

/// <summary>
/// Represents a mission where the player needs to kill a specified NPC or a quantity of NPCs.
/// </summary>
public interface IKillNPCMission
{
	public abstract List<KillNPCRequirement> DemandNPCs { get; init; }

	public float CalculateProgress()
	{
		if (DemandNPCs.Count == 0)
		{
			return 1f;
		}

		// The final progress is calculated as the average of the individual progress for each NPC type group
		return DemandNPCs.Select(x => x.Progress).Average();
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

	public void Load(TagCompound tag)
	{
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

		MissionBase.LoadVanillaNPCTextures(DemandNPCs.SelectMany(x => x.NPCs));
	}

	public void Save(TagCompound tag)
	{
		tag.Add(nameof(DemandNPCs), DemandNPCs);
	}
}