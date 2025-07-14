namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class DevilHeartWeaponBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.lifeRegen += 5;
	}
}