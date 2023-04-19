namespace Everglow.Myth.TheFirefly.Buffs;

public class ShadowPotionBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}
	public override void Update(Player player, ref int buffIndex)
	{
		Color playerLight = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16));
		if(Math.Max(Math.Max(playerLight.R, playerLight.G), playerLight.B) <= 100)
		{
			player.nightVision = true;
			player.statDefense += 15;
			player.allDamage += 0.2f;
		}
		if (Math.Max(Math.Max(playerLight.R, playerLight.G), playerLight.B) >= 200)
		{
			player.lifeRegen -= (int)(player.statLifeMax2 * 0.01f);
			player.lifeRegenTime = 0;
		}
	}
}