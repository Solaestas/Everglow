namespace Everglow.Myth.MiscItems.Buffs;

public class LaserWormBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = false;
	}
}
