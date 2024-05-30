namespace Everglow.Yggdrasil.Furnace.Buffs;

public class MelterGearCooldown : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}
}