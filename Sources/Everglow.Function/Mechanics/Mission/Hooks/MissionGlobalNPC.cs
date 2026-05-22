using Everglow.Commons.Mechanics.Mission.PlayerSide.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Enums;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Objectives;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Objectives;

namespace Everglow.Commons.Mechanics.Mission.Hooks;

public class MissionGlobalNPC : GlobalNPC
{
	public static event Action<NPC> OnKillNPCEvent;

	public static event Action<NPC> OnNPCKilled;

	public static void TriggerOnKillNPCEvent(NPC npc)
	{
		OnKillNPCEvent.Invoke(npc);
	}

	public override bool SpecialOnKill(NPC npc)
	{
		OnNPCKilled?.Invoke(npc);

		return base.SpecialOnKill(npc);
	}

	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var types = GetMissionNPCTypes();
		bool valid = types.Contains(npc.type);
		if (valid)
		{
			Texture2D tex = ModAsset.MissionExclamationMark.Value;
			float scale = (1f + (float)Math.Sin(Main.time * 0.24f) * 0.14f) * 0.16f;
			Color color = new Color(1f, 0.7f, 0.5f, 1f);
			spriteBatch.Draw(tex, new Vector2(npc.Center.X, npc.Center.Y - 36) - Main.screenPosition, null, color, 0f, tex.Size() / 2, scale, SpriteEffects.None, 0f);
		}
	}

	public static IEnumerable<int> GetMissionNPCTypes()
	{
		var missions = PlayerMissionManager.GetMissionPool(PlayerMissionState.Accepted);

		// Flatten all objectives recursively and filter for KillNPCObjective
		var playerSideNPCs = missions
			.SelectMany(mission => FlattenObjectives(mission.Objectives.AllObjectives))
			.OfType<KillNPCObjective>()
			.Where(o => !o.Completed)
			.SelectMany(killObjective => killObjective.DemandNPC.NPCs);

		var worldSideNPCs = WorldMissionManager.Instance.ActiveMissions
			.SelectMany(m => m.ActiveObjectives)
			.Select(o =>
			{
				if (o is WorldKillNPCObjective killObjective)
				{
					return killObjective.NPCType;
				}
				else if (o is WorldTalkObjective talkObjective)
				{
					return talkObjective.NPCType;
				}
				else if (o is WorldGiveObjective giveObjective)
				{
					return giveObjective.NPCType;
				}
				else
				{
					return NPCID.None;
				}
			}).Distinct();

		return playerSideNPCs.Concat(worldSideNPCs).Distinct();
	}

	/// <summary>
	/// Recursively flattens a collection of objectives, including nested objectives in ParallelObjective and BranchingObjective.
	/// </summary>
	private static IEnumerable<MissionObjectiveBase> FlattenObjectives(IEnumerable<MissionObjectiveBase> objectives)
	{
		foreach (var objective in objectives)
		{
			// Handle nested objectives in ParallelObjective and BranchingObjective
			if (objective is ParallelObjective parallelObjective)
			{
				foreach (var nestedObjective in FlattenObjectives(parallelObjective.Objectives))
				{
					yield return nestedObjective;
				}
			}
			else if (objective is BranchingObjective branchingObjective)
			{
				foreach (var nestedObjective in FlattenObjectives(branchingObjective.Objectives))
				{
					yield return nestedObjective;
				}
			}
			else
			{
				yield return objective;
			}
		}
	}
}