using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.MissionSystem.Hooks;

public class MissionGlobalNPC : GlobalNPC
{
	public static event Action<NPC> OnKillNPCEvent;

	/// <summary>
	/// On kill hook for multiplayer missions
	/// </summary>
	public static event Action<NPC> GlobalOnKillNPCEvent;

	public static void TriggerOnKillNPCEvent(NPC npc)
	{
		OnKillNPCEvent.Invoke(npc);
	}

	public override void OnKill(NPC npc)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
		{
			SingleOnKill(npc);
		}
		else if (Main.netMode == NetmodeID.Server)
		{
			ServerOnKill(npc);
		}
	}

	public override bool SpecialOnKill(NPC npc)
	{
		if (npc.lastInteraction == Main.myPlayer && !NetUtils.IsServer)
		{
			//OnKillNPCEvent?.Invoke(npc);
		}

		return base.SpecialOnKill(npc);
	}

	public void SingleOnKill(NPC npc)
	{
		GlobalOnKillNPCEvent?.Invoke(npc);
	}

	public void ServerOnKill(NPC npc)
	{
		GlobalOnKillNPCEvent?.Invoke(npc);
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
		var missions = MissionManager.GetMissionPool(PoolType.Accepted);

		// Flatten all objectives recursively and filter for KillNPCObjective
		return missions
			.SelectMany(mission => FlattenObjectives(mission.Objectives.AllObjectives))
			.OfType<KillNPCObjective>()
			.Where(o => !o.Completed)
			.SelectMany(killObjective => killObjective.DemandNPC.NPCs);
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