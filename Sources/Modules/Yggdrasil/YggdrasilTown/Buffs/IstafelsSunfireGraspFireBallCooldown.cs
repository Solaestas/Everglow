namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class IstafelsSunfireGraspFireBallCooldown : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.pvpBuff[Type] = false;
		Main.buffNoSave[Type] = false;
		Main.debuff[Type] = false;
	}
}