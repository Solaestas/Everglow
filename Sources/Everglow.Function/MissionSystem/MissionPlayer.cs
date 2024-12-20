using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem;

public class MissionPlayer : ModPlayer
{
	// public override bool CloneNewInstances => true;
	public MissionManager MissionManager = new MissionManager();

	public override void OnEnterWorld()
	{
		base.OnEnterWorld();
		if (!MissionManager.HasMission<GainItemMission>())
		{
			Item item = new Item();
			var mission = new GainItemMission();
			mission.SetInfo("Test1", "获取10个土块", "测试[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']");
			item = new Item();
			item.SetDefaults(2);
			item.stack = 10;
			mission.DemandItem = [item];
			MissionManager.AddMission(mission, MissionManager.PoolType.CanTaken);

			mission = new GainItemMission();
			mission.SetInfo("Test2", "获取10个木头", "测试介绍2\n" +
				"[TimerIconDrawer,MissionName='Test2'] 剩余时间:[TimerStringDrawer,MissionName='Test2']", 30000);
			item = new Item();
			item.SetDefaults(9);
			item.stack = 10;
			mission.DemandItem = [item];
			MissionManager.AddMission(mission, MissionManager.PoolType.BeenTaken);

			mission = new GainItemMission();
			mission.SetInfo("Test3", "获取10个铁矿", "测试介绍3");
			item = new Item();
			item.SetDefaults(11);
			item.stack = 10;
			mission.DemandItem = [item];
			MissionManager.AddMission(mission, MissionManager.PoolType.CanTaken);
		}
	}

	public override void PostUpdate()
	{
		MissionManager.Update();
		base.PostUpdate();
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		MissionManager.Save(tag);
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		MissionManager.Load(tag);
	}
}