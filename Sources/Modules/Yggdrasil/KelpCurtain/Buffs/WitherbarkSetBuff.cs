namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class WitherbarkSetBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
		Main.buffNoSave[Type] = true;
	}
}