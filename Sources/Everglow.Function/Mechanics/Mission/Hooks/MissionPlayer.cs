using Everglow.Commons.Mechanics.Mission.PlayerSide.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Enums;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.Hooks;

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
			MissionManager.AddMission(new KillNPCMissionTest(), PlayerMissionState.Available);
			MissionManager.AddMission(new ParallelMissionTest(), PlayerMissionState.Available);
			MissionManager.AddMission(new MissionObjectivesTest(), PlayerMissionState.Available);
			MissionManager.AddMission(new OpenPanelMissionTest(), PlayerMissionState.Available);
			MissionManager.AddMission(new BranchingMissionTest(), PlayerMissionState.Available);
			MissionManager.AddMission(new NoneMission1(), PlayerMissionState.Available);
			MissionManager.AddMission(new NoneMission2(), PlayerMissionState.Available);
			MissionManager.AddMission(new NoneMission3(), PlayerMissionState.Available);
			MissionManager.AddMission(new NoneMission4(), PlayerMissionState.Available);
			MissionManager.AddMission(new NoneMission5(), PlayerMissionState.Available);
			MissionManager.AddMission(new NoneMission6(), PlayerMissionState.Available);
			MissionManager.AddMission(new MissionTimerTest(), PlayerMissionState.Available);
			MissionManager.AddMission(new MissionIconTest(), PlayerMissionState.Available);
			MissionManager.AddMission(new GiveItemMissionTest(), PlayerMissionState.Available);
			MissionManager.AddMission(new ExploreMissionTest(), PlayerMissionState.Available);
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

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!NetUtils.IsServer && Player.whoAmI == Main.myPlayer && !target.active)
		{
			MissionGlobalNPC.TriggerOnKillNPCEvent(target);
		}
	}
}