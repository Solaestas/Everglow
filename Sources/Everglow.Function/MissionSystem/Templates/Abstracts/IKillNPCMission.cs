using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates.Abstracts;

/// <summary>
/// Represents a mission where the player needs to kill a specified NPC or a quantity of NPCs.
/// </summary>
public interface IKillNPCMission : IMissionObjective
{
	public abstract List<KillNPCRequirement> DemandNPCs { get; init; }

	public float CalculateProgress(IDictionary<int, int> nPCKillCounter)
	{
		if (DemandNPCs.Count == 0)
		{
			return 1f;
		}

		// The final progress is calculated as the average of the individual progress for each NPC type group
		return DemandNPCs.Select(x => x.Progress(nPCKillCounter)).Average();
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

	public IEnumerable<string> GetObjectivesString(IDictionary<int, int> nPCKillCounter)
	{
		var objectives = new List<string>();

		foreach (var demand in DemandNPCs)
		{
			string progress = demand.EnableIndividualCounter
				? $"({demand.Counter}/{demand.Requirement})"
				: $"({nPCKillCounter.Where((pair) => demand.NPCs.Contains(pair.Key)).Sum(pair => pair.Value)}/{demand.Requirement})";

			if (demand.NPCs.Count > 1)
			{
				var npcString = string.Join(',', demand.NPCs.ConvertAll(npcType =>
				{
					var npc = new NPC();
					npc.SetDefaults(npcType);
					return npc.TypeName;
				}));
				objectives.Add($"击杀 {npcString} 合计{demand.Requirement}个 {progress}\n");
			}
			else
			{
				var npc = new NPC();
				npc.SetDefaults(demand.NPCs.First());
				objectives.Add($"击杀 {npc.TypeName} {demand.Requirement}个 {progress}\n");
			}
		}

		return objectives;
	}
}