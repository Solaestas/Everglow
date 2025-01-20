using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Enums;
using Everglow.Commons.MissionSystem.Tests;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Hooks;

public class MissionPlayer : ModPlayer
{
	public override void OnEnterWorld()
	{
		if (Player.whoAmI == Main.myPlayer)
		{
			// MissionManager.Clear();
			if (!MissionManager.HasMission<MissionBase>())
			{
				MissionManager.AddMission(new TestMission1(), PoolType.Accepted);
				MissionManager.AddMission(new TestMission2(), PoolType.Accepted);
				MissionManager.AddMission(
					new TestMission3
					{
						SourceNPC = 1,
					}, PoolType.Available);
				MissionManager.AddMission(new TestMission4(), PoolType.Available);
				MissionManager.AddMission(new TestMission5(), PoolType.Available);

				MissionManager.AddMission(new TestMission6(), PoolType.Available);
				MissionManager.AddMission(new TestMission7(), PoolType.Available);
				MissionManager.AddMission(new TestMission8(), PoolType.Available);
				MissionManager.AddMission(new TestTalkToNPCMission(), PoolType.Available);
				MissionManager.AddMission(new TestGiveNPCItemMission(), PoolType.Available);

				MissionManager.AddMission(TextureMissionIconTestMission.Create(), PoolType.Available);
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