namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class FractureDebuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
	}
}