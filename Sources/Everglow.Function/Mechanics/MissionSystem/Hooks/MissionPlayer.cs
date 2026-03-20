using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Tests;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Hooks;

public class MissionPlayer : ModPlayer
{
	/// <summary>
	/// Called on local client only.
	/// </summary>
	public static event Action<Item> OnPickupEvent;

	/// <summary>
	/// Called on local client only.
	/// </summary>
	public static event Action<Player, Item> OnDropSelectedItemEvent;

	private MissionManagerData missionManagerData;

	/// <summary>
	/// Indicate to apply player mission info into <see cref="MissionManager"/>. Defaults to <c>false</c>.
	/// <br/>Set to <c>true</c> after data applying, to <c>false</c> after player loading.
	/// </summary>
	private bool missionManagerDataInitialized = false;

	public override void Load()
	{
		On_Player.DropSelectedItem_int_refItem += On_Player_DropSelectedItem_int_refItem;
	}

	public override void OnEnterWorld()
	{
		if (!missionManagerDataInitialized) // Prevent load being called when OnEnterWorld is called by subworldlibrary
		{
			MissionManager.ApplyData(missionManagerData);
			missionManagerDataInitialized = true;
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
		missionManagerData = MissionManager.LoadData(tag);
		missionManagerDataInitialized = false;
	}

	public override bool OnPickup(Item item)
	{
		OnPickupEvent?.Invoke(item);
		return true;
	}

	private void On_Player_DropSelectedItem_int_refItem(On_Player.orig_DropSelectedItem_int_refItem orig, Player self, int slot, ref Item theItemWeDrop)
	{
		orig(self, slot, ref theItemWeDrop);
		OnDropSelectedItemEvent?.Invoke(self, theItemWeDrop);
	}

	public override void OnConsumeAmmo(Item weapon, Item ammo)
	{
		MissionGlobalItem.InvokeOnConsumeItemEvent(ammo, Player);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!NetUtils.IsServer && Player.whoAmI == Main.myPlayer && !target.active)
		{
			MissionGlobalNPC.TriggerOnKillNPCEvent(target);
		}
	}
}