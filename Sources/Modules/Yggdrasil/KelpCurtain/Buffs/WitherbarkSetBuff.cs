namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class WitherbarkSetBuff : ModBuff
{
	public override string Texture => Commons.ModAsset.BuffTemplate_Mod;

	public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
		Main.buffNoSave[Type] = true;
	}
}