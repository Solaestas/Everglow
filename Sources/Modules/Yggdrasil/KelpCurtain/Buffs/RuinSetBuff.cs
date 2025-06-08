namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class RuinSetBuff : ModBuff
{
	public override string Texture => Commons.ModAsset.BuffTemplate_Mod;

	public override void SetStaticDefaults()
	{
		Main.persistentBuff[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.GetDamage<SummonDamageClass>() += 0.1f;
		player.moveSpeed += 0.25f;
	}
}