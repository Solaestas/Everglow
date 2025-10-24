namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class EvilMusicRemnant : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}
}