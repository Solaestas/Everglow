namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class BloodCultistBuff : ModBuff
{
	public override string Texture => Commons.ModAsset.BuffTemplate_Mod;

	public override void Update(Player player, ref int buffIndex)
	{
		player.lifeRegen += 1;
	}
}