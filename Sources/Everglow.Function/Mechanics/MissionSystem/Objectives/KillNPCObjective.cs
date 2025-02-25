using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class KillNPCObjective : MissionObjectiveBase
{
	public List<KillNPCRequirement> DemandNPCs { get; } = [];

	public override void OnInitialize()
	{
		base.OnInitialize();
		MissionBase.LoadVanillaNPCTextures(DemandNPCs.SelectMany(x => x.NPCs));
	}

	public override bool CheckCompletion() => DemandNPCs.All(n => n.Counter >= n.Requirement);

	public override float Progress => DemandNPCs.Count != 0
		? DemandNPCs.Average(x => x.Progress(MissionManager.NPCKillCounter))
		: 1f;

	public override void GetObjectivesText(List<string> lines)
	{
		foreach (var demand in DemandNPCs)
		{
			string progress = demand.EnableIndividualCounter
				? $"({demand.Counter}/{demand.Requirement})"
				: $"({MissionManager.NPCKillCounter.Where((pair) => demand.NPCs.Contains(pair.Key)).Sum(pair => pair.Value)}/{demand.Requirement})";

			if (demand.NPCs.Count > 1)
			{
				var npcString = string.Join(',', demand.NPCs.ConvertAll(npcType =>
				{
					var npc = new NPC();
					npc.SetDefaults(npcType);
					return npc.TypeName;
				}));
				lines.Add($"击杀 {npcString} 合计{demand.Requirement}个 {progress}\n");
			}
			else
			{
				var npc = new NPC();
				npc.SetDefaults(demand.NPCs.First());
				lines.Add($"击杀 {npc.TypeName} {demand.Requirement}个 {progress}\n");
			}
		}
	}

	public override void Activate(MissionBase sourceMission)
	{
		MissionPlayer.OnKillNPCEvent += MissionPlayer_OnKillNPC;
	}

	public override void Deactivate()
	{
		MissionPlayer.OnKillNPCEvent -= MissionPlayer_OnKillNPC;
	}

	/// <summary>
	/// Count kill for each demand group
	/// </summary>
	/// <param name="type">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void MissionPlayer_OnKillNPC(int type)
	{
		foreach (var kmDemand in DemandNPCs.Where(x => x.NPCs.Contains(type)))
		{
			if (kmDemand.EnableIndividualCounter)
			{
				kmDemand.Count(1);
			}
		}
	}

	public override void LoadData(TagCompound tag)
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
	}

	public override void SaveData(TagCompound tag)
	{
		tag.Add(nameof(DemandNPCs), DemandNPCs);
	}
}