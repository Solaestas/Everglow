namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class CrossHair : ModBuff
{
	public override string Texture => Commons.ModAsset.BuffTemplate_Mod;

	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}
}