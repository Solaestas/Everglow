using Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Auburn;

namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class AuburnSelfReinforcing : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.lifeRegen -= 2;
		player.maxMinions += 1;

		if (player.buffTime[buffIndex] == 1)
		{
			player.AddBuff(ModContent.BuffType<AuburnSelfReinforcingCooldown>(), AuburnHoodie.BuffCooldown);
		}
	}
}