namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class MelterGearCooldown : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}
}