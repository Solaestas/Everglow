using Everglow.Commons.MissionSystem.MissionTemplates;
using Terraria.ModLoader.IO;
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

			var mMission = new MultipleMission();
			mMission.SetInfo("测试层级任务", "含2个子任务", "测试[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");

			var subM1 = new GainItemMission();
			subM1.SetInfo("子任务1", "子任务1", "测试[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
			subM1.DemandItems.AddRange([
				new Item(ItemID.DirtBlock, 10)]);
			subM1.RewardItems.AddRange([
				new Item(ItemID.Wood, 10)]);
			subM1.Consume = true;

			var subM2 = new GainItemMission();
			subM2.SetInfo("子任务2", "子任务2", "测试[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
			subM2.DemandItems.AddRange([
				new Item(ItemID.DirtBlock, 8)]);
			subM2.RewardItems.AddRange([
				new Item(ItemID.Wood, 8)]);

			mMission.SubMissions.Add(subM1);
			mMission.SubMissions.Add(subM2);

			MissionManager.AddMission(mMission, MissionManager.PoolType.Available);

			return;

			if (!MissionManager.HasMission<GainItemMission>())
			{
				var mission = new GainItemMission();
				mission.SetInfo("Test1", "获取10个土块", "测试[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
				mission.DemandItems.AddRange([
					new Item(ItemID.DirtBlock, 10)]);
				mission.RewardItems.AddRange([
					new Item(ItemID.Wood, 10)]);
				MissionManager.AddMission(mission, MissionManager.PoolType.Available);

				mission = new GainItemMission();
				mission.SetInfo("Test2", "获取10个木头", "测试介绍2\n" +
					"[TimerIconDrawer,MissionName='Test2'] 剩余时间:[TimerStringDrawer,MissionName='Test2']", 30000);
				mission.DemandItems.AddRange([
					new Item(ItemID.Wood, 10)
					]);
				mission.RewardItems.AddRange([
					new Item(ItemID.IronOre, 10)]);
				MissionManager.AddMission(mission, MissionManager.PoolType.Accepted);

				mission = new GainItemMission();
				mission.SetInfo("Test3", "获取10个铁矿", "测试介绍3");
				mission.DemandItems.AddRange([
					new Item(ItemID.IronOre, 10)]);
				mission.RewardItems.AddRange([
					new Item(ItemID.Zenith, 10)]);
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
			MissionManager.Save(tag);
		}
	}

	public override void LoadData(TagCompound tag)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			MissionManager.Load(tag);
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