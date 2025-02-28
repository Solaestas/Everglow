using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;

namespace Everglow.Commons.Mechanics.MissionSystem.Hooks;

public class MissionGlobalNPC : GlobalNPC
{
	/// <summary>
	/// On kill hook for multiplayer missions
	/// </summary>
	public static event Action<NPC> OnNPCKill;

	public override void OnKill(NPC npc)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
		{
			ClientOnKill(npc);
		}
		else if (Main.netMode == NetmodeID.Server)
		{
			ServerOnKill(npc);
		}
	}

	public void ClientOnKill(NPC npc)
	{
		// TODO: Count kill for all player
	}

	public void ServerOnKill(NPC npc)
	{
		// TODO: Send packet to client to sync the kill of npc.
	}

	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var types = GetMissionNPCTypes();
		bool valid = types.Contains(npc.type);
		if (valid)
		{
			Texture2D tex = ModAsset.ExclamationMark.Value;
			float scale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
			spriteBatch.Draw(tex, new Vector2(npc.Center.X - 2, npc.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + scale, SpriteEffects.None, 0f);
		}
	}

	public static IEnumerable<int> GetMissionNPCTypes()
	{
		var missions = MissionManager.GetMissionPool(PoolType.Accepted);

		// Flatten all objectives recursively and filter for KillNPCObjective
		return missions
			.SelectMany(mission => FlattenObjectives(mission.Objectives.AllObjectives))
			.OfType<KillNPCObjective>()
			.SelectMany(killObjective => killObjective.DemandNPCs.SelectMany(demand => demand.NPCs));
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