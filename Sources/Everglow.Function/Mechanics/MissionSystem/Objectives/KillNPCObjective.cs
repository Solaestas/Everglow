using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;
using Everglow.Commons.Mechanics.MissionSystem.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class KillNPCObjective : MissionObjectiveBase
{
	public KillNPCObjective()
	{
	}

	public KillNPCObjective(KillNPCRequirement requirements)
	{
		DemandNPC = requirements;
	}

	public KillNPCRequirement DemandNPC { get; set; }

	public override void OnInitialize()
	{
		base.OnInitialize();
		AssetUtils.LoadVanillaNPCTextures(DemandNPC.NPCs);
	}

	public override bool CheckCompletion() => Progress >= 1f;

	public override float Progress => DemandNPC.Progress(MissionManager.NPCKillCounter);

	public override void GetObjectivesText(List<string> lines)
	{
		string progress = DemandNPC.EnableIndividualCounter
				? $"({DemandNPC.Counter}/{DemandNPC.Requirement})"
				: $"({MissionManager.NPCKillCounter.Where((pair) => DemandNPC.NPCs.Contains(pair.Key)).Sum(pair => pair.Value)}/{DemandNPC.Requirement})";

		if (DemandNPC.NPCs.Count > 1)
		{
			var npcString = string.Join(',', DemandNPC.NPCs.ConvertAll(npcType =>
			{
				var npc = new NPC();
				npc.SetDefaults(npcType);
				return npc.TypeName;
			}));
			lines.Add($"击杀 {npcString} 合计{DemandNPC.Requirement}个 {progress}\n");
		}
		else
		{
			var npc = new NPC();
			npc.SetDefaults(DemandNPC.NPCs.First());
			lines.Add($"击杀 {npc.TypeName} {DemandNPC.Requirement}个 {progress}\n");
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
	/// <param name="npc">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void MissionPlayer_OnKillNPC(NPC npc)
	{
		DemandNPC.Count(npc);
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		tag.TryGet<KillNPCRequirement>(nameof(DemandNPC), out var demandNPC);
		if (DemandNPC.EnableIndividualCounter)
		{
			if (demandNPC != null && demandNPC.Counter > 0)
			{
				DemandNPC.SetInitialCount(demandNPC.Counter);
			}
		}
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		tag.Add(nameof(DemandNPC), DemandNPC);
	}
}