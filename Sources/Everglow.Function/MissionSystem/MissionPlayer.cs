using Everglow.Commons.MissionSystem.MissionTemplates;
using Terraria.ModLoader.IO;
using static Everglow.Commons.MissionSystem.MissionTemplates.GainItemMission;
using static Everglow.Commons.MissionSystem.MissionTemplates.KillNPCMission;

namespace Everglow.Commons.MissionSystem;

public class MissionPlayer : ModPlayer
{
	// public override bool CloneNewInstances => true;
	public MissionManager MissionManager = new MissionManager();

	public override void OnEnterWorld()
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			MissionManager.Clear();
			if (!MissionManager.Instance.HasMission<MissionBase>())
			{
				var mission = new GainItemMission();
				mission.SetInfo("Test1", "获取10个土块", "测试[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
				mission.DemandItems.AddRange([
					GainItemRequirement.Create([ItemID.DirtBlock], 10)]);
				mission.RewardItems.AddRange([
					new Item(ItemID.Wood, 10)]);
				mission.IsVisible = false;
				mission.AutoComplete = true;
				mission.MissionType = MissionType.Daily;
				MissionManager.AddMission(mission, MissionManager.PoolType.Accepted);

				mission = new GainItemMission();
				mission.SetInfo("Test2", "获取10个木头", "测试介绍2\n" +
					"[TimerIconDrawer,MissionName='Test2'] 剩余时间:[TimerStringDrawer,MissionName='Test2']", 30000);
				mission.DemandItems.AddRange([
					GainItemRequirement.Create([ItemID.Wood], 10)]);
				mission.RewardItems.AddRange([
					new Item(ItemID.IronOre, 10)]);
				mission.MissionType = MissionType.Achievement;
				MissionManager.AddMission(mission, MissionManager.PoolType.Accepted);

				mission = new GainItemMission();
				mission.SetInfo("Test3", "获取10个铁矿", "测试介绍3");
				mission.DemandItems.AddRange([
					GainItemRequirement.Create([ItemID.IronOre], 1000)]);
				mission.RewardItems.AddRange([
					new Item(ItemID.Zenith, 1000)]);
				mission.MissionType = MissionType.MainStory;
				mission.SourceNPC = 1;
				MissionManager.AddMission(mission, MissionManager.PoolType.Available);

				var killNPCMission = new KillNPCMission();
				killNPCMission.SetInfo("Test4", "击杀10个史莱姆", "测试介绍: \n" + "[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
				killNPCMission.DemandNPCs.AddRange([
					KillNPCRequirement.Create(
					[
						NPCID.BlueSlime,
						NPCID.IceSlime,
						NPCID.SpikedJungleSlime,
						NPCID.MotherSlime,
					], 10, true),
				KillNPCRequirement.Create(
					[
						NPCID.DemonEye,
					], 3, true),
				]);
				killNPCMission.RewardItems.AddRange([
					new Item(ItemID.Zenith),
				new Item(ItemID.GoldAxe, 10),
				]);
				killNPCMission.MissionType = MissionType.SideStory;
				MissionManager.AddMission(killNPCMission, MissionManager.PoolType.Available);

				killNPCMission = new KillNPCMission();
				killNPCMission.SetInfo("Test5", "击杀12个史莱姆", "测试介绍: \n" + "[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
				killNPCMission.DemandNPCs.AddRange([
					KillNPCRequirement.Create(
					[
						NPCID.BlueSlime,
						NPCID.IceSlime,
						NPCID.SpikedJungleSlime,
						NPCID.MotherSlime,
					], 12, true),
				KillNPCRequirement.Create(
					[
						NPCID.DemonEye,
					], 3, true),
				]);
				killNPCMission.RewardItems.AddRange([
					new Item(ItemID.Zenith),
				new Item(ItemID.GoldAxe, 10),
				]);
				killNPCMission.MissionType = MissionType.Challenge;
				MissionManager.AddMission(killNPCMission, MissionManager.PoolType.Available);

				killNPCMission = new KillNPCMission();
				killNPCMission.SetInfo("Test5", "击杀13个史莱姆", "测试介绍: \n" + "[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
				killNPCMission.DemandNPCs.AddRange([
					KillNPCRequirement.Create(
					[
						NPCID.BlueSlime,
						NPCID.IceSlime,
						NPCID.SpikedJungleSlime,
						NPCID.MotherSlime,
					], 12, true),
				KillNPCRequirement.Create(
					[
						NPCID.DemonEye,
					], 3, true),
				]);
				killNPCMission.RewardItems.AddRange([
					new Item(ItemID.Zenith),
				new Item(ItemID.GoldAxe, 10),
				]);
				killNPCMission.MissionType = MissionType.Legendary;
				MissionManager.AddMission(killNPCMission, MissionManager.PoolType.Available);
			}
		}
	}

	public override void PostUpdate()
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			MissionManager.Update();
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
			MissionManager.LoadData(tag);
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			// If player killed the target, then count this kill
			if (!target.active)
			{
				MissionManager.CountKill(target.type);
			}
		}
	}
}