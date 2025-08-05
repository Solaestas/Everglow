namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class HardenedSkinBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
	}
}