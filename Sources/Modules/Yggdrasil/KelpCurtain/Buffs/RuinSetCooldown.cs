namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class RuinSetCooldown : ModBuff
{

	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.persistentBuff[Type] = true;
	}
}