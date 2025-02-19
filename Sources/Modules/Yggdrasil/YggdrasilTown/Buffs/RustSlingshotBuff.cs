namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public abstract class RustSlingshotBuff : ModBuff
{
	public override string Texture => ModAsset.RustSlingshotBuff_Mod;

	public override void SetStaticDefaults()
	{
		Main.pvpBuff[Type] = true;
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = false;
	}
}

public class RustSlingshotBuffOne : RustSlingshotBuff
{
}

public class RustSlingshotBuffTwo : RustSlingshotBuff
{
}

public class RustSlingshotBuffThree : RustSlingshotBuff
{
}