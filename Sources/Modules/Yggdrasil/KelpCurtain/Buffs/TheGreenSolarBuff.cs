namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class TheGreenSolarBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true; // Use debuff tag to prevent being dispelled by right-clicking the buff icon.
		Main.buffNoTimeDisplay[Type] = true;
	}
}