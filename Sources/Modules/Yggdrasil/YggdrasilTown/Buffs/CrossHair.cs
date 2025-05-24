namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class CrossHair : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}
}