namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class SunstoneCooldown : ModBuff
{
	public override string Texture => Commons.ModAsset.DebuffTemplate_Mod;

	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.persistentBuff[Type] = true;
	}
}