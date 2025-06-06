namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class RayBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = false;
		Main.pvpBuff[Type] = true;
		Main.buffNoSave[Type] = false;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		if (Main.myPlayer == player.whoAmI)
		{
			Lighting.GlobalBrightness += 0.5f;
		}
	}
}