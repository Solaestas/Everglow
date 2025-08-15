using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.KelpCurtain.Dusts;

public class SpongeOxygenBubble_Small : ModDust
{
	public override void OnSpawn(Dust dust)
	{
		dust.frame = new Rectangle(0, 8 * Main.rand.Next(4), 8, 8);
		base.OnSpawn(dust);
	}

	public override bool Update(Dust dust)
	{
		if (dust.customData is Projectile proj && proj.active)
		{
			Vector2 toOwner = proj.Center - dust.position + new Vector2(20 * MathF.Cos(dust.dustIndex + (float)Main.time * 0.05f), 20 * MathF.Sin(dust.dustIndex + (float)Main.time * 0.75f));
			float mulAcc = 1.3f - dust.frame.Y / 32f;
			dust.velocity += toOwner / 1000f * mulAcc ;
			dust.velocity *= 0.99f;
			dust.position += dust.velocity;
			Tile dustPosTile = YggdrasilWorldGeneration.SafeGetTile(dust.position.ToTileCoordinates());
			if (dust.position.Y % 16 > dustPosTile.LiquidAmount / 16f)
			{
				dust.active = false;
			}
		}
		else
		{
			dust.scale *= 0.95f;
			dust.velocity.Y += -0.05f;
			dust.velocity *= 0.99f;
			dust.position += dust.velocity;
			if (dust.scale <= 0.15f)
			{
				dust.active = false;
			}
			Tile dustPosTile = YggdrasilWorldGeneration.SafeGetTile(dust.position.ToTileCoordinates());
			if (dust.position.Y % 16 > dustPosTile.LiquidAmount / 16f)
			{
				dust.active = false;
			}
		}
		return false;
	}
}