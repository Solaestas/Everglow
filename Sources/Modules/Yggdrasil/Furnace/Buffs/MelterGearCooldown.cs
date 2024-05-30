namespace Everglow.Yggdrasil.Furnace.Buffs;

public class MelterGearCooldown : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}
}