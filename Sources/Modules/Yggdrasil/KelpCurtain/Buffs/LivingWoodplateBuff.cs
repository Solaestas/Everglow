namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class LivingWoodplateBuff : ModBuff
{
	public override string Texture => Commons.ModAsset.BuffTemplate_Mod;

	public override void SetStaticDefaults()
	{
		Main.persistentBuff[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.lifeRegen += 8;
		player.statLifeMax2 += 40;
	}
}