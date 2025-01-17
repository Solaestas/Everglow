using Everglow.Commons.MissionSystem.Tests;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem;

public class MissionPlayer : ModPlayer
{
	public override void OnEnterWorld()
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			// MissionManager.Clear();
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
				MissionManager.AddMission(new TestMission8(), MissionManager.PoolType.Available);

				MissionManager.AddMission(TextureMissionIconTestMission.Create(), MissionManager.PoolType.Available);
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
			MissionManager.LoadData(tag);
		}
	}

	public override bool OnPickup(Item item)
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			MissionManager.CountPick(item);
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
				MissionManager.CountKill(target.type);
			}
		}
	}
}