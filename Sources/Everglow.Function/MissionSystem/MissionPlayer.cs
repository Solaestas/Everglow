using Everglow.Commons.MissionSystem.Tests;
using Terraria.ModLoader.IO;

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
			if (!MissionManager.HasMission<MissionBase>())
			{
				MissionManager.AddMission(new TestMission1(), MissionManager.PoolType.Accepted);
				MissionManager.AddMission(new TestMission2(), MissionManager.PoolType.Accepted);
				MissionManager.AddMission(
					new TestMission3
				{
					SourceNPC = 1,
				}, MissionManager.PoolType.Available);
				MissionManager.AddMission(new TestMission4(), MissionManager.PoolType.Available);
				MissionManager.AddMission(new TestMission5(), MissionManager.PoolType.Available);

				MissionManager.AddMission(new TestMission6(), MissionManager.PoolType.Available);
				MissionManager.AddMission(new TestMission7(), MissionManager.PoolType.Available);

				MissionManager.Instance.AddMission(TextureMissionIconTestMission.Create(), MissionManager.PoolType.Available);
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