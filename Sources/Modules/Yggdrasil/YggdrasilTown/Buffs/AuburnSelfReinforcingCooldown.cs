namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class AuburnSelfReinforcingCooldown : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}
}