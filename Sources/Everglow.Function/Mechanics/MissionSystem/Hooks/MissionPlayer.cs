using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Tests;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Hooks;

public class MissionPlayer : ModPlayer
{
	public static event Action<int> OnKillNPCEvent;

	public static event Action<Item> OnPickupEvent;

	private MissionManager.MissionManagerInfo missionInfo;

	public override void OnEnterWorld()
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			MissionManager.OnEnterWorld(missionInfo);

			if (!MissionManager.HasMission<MissionBase>())
			{
				MissionManager.AddMission(new KillNPCMissionTest(), PoolType.Available);
				MissionManager.AddMission(new ParallelMissionTest(), PoolType.Available);
				MissionManager.AddMission(new MissionObjectivesTest(), PoolType.Available);
				MissionManager.AddMission(new OpenPanelMissionTest(), PoolType.Available);
			}
		}
	}

	public override void SaveData(TagCompound tag)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			MissionManager.SaveData(tag);
		}
	}

	public override void LoadData(TagCompound tag)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			missionInfo = MissionManager.LoadData(tag);
		}
	}

	public override bool OnPickup(Item item)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			OnPickupEvent?.Invoke(item);
		}

		return true;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			// If player killed the target, then count this kill
			if (!target.active)
			{
				OnKillNPCEvent?.Invoke(target.type);
			}
		}
	}
}