using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Tests;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Hooks;

public class MissionPlayer : ModPlayer
{
	public static event Action<NPC> OnKillNPCEvent;

	public static event Action<Item> OnPickupEvent;

	public static event Action<Item> GlobalOnPickupEvent;

	private MissionManager.MissionManagerInfo missionInfo;

	private bool initialMissionInfoLoaded = false;

	public override void OnEnterWorld()
	{
		if (!initialMissionInfoLoaded) // Prevent load being called when on enter world is called by subworldlibrary
		{
			MissionManager.LoadPlayerInfo(missionInfo);
			initialMissionInfoLoaded = true;
		}

#if DEBUG
		if (!MissionManager.HasMission<MissionBase>())
		{
			MissionManager.AddMission(new KillNPCMissionTest(), PoolType.Available);
			MissionManager.AddMission(new ParallelMissionTest(), PoolType.Available);
			MissionManager.AddMission(new MissionObjectivesTest(), PoolType.Available);
			MissionManager.AddMission(new OpenPanelMissionTest(), PoolType.Available);
			MissionManager.AddMission(new BranchingMissionTest(), PoolType.Available);
			MissionManager.AddMission(new NoneMission1(), PoolType.Available);
			MissionManager.AddMission(new NoneMission2(), PoolType.Available);
			MissionManager.AddMission(new NoneMission3(), PoolType.Available);
			MissionManager.AddMission(new NoneMission4(), PoolType.Available);
			MissionManager.AddMission(new NoneMission5(), PoolType.Available);
			MissionManager.AddMission(new NoneMission6(), PoolType.Available);
			MissionManager.AddMission(new MissionTimerTest(), PoolType.Available);
			MissionManager.AddMission(new MissionIconTest(), PoolType.Available);
			MissionManager.AddMission(new GiveItemMissionTest(), PoolType.Available);
			MissionManager.AddMission(new ExploreMissionTest(), PoolType.Available);
		}
#endif
	}

	public override void SaveData(TagCompound tag)
	{
		MissionManager.SaveData(tag);
	}

	public override void LoadData(TagCompound tag)
	{
		missionInfo = MissionManager.LoadData(tag);
		initialMissionInfoLoaded = false;
	}

	public override bool OnPickup(Item item)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			OnPickupEvent?.Invoke(item);
		}

		GlobalOnPickupEvent?.Invoke(item);

		return true;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			// If player killed this npc
			if (!target.active)
			{
				OnKillNPCEvent?.Invoke(target);
			}
		}
	}

	public override void OnConsumeAmmo(Item weapon, Item ammo)
	{
		MissionGlobalItem.InvokeOnConsumeItemEvent(ammo, Player);
	}
}