namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class DevilHeartSetCooldown : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}
}