using static Terraria.ModLoader.PlayerDrawLayer;

namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;

public class AmbiguousNightHit : ModProjectile
{
	public override string Texture => "Everglow/Myth/MiscItems/Weapons/Slingshots/AmbiguousNight";
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 300;
		Projectile.extraUpdates = 6;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI()
	{

	}
	public override void PostDraw(Color lightColor)
	{
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D shadow = ModAsset.CursedHit.Value;
		Texture2D blackHole = ModAsset.BlackHole_BlackHole.Value;
		Texture2D blue = ModAsset.CorruptLight.Value;
		float Dark = Math.Max((Projectile.timeLeft) / 300f, 0);

		Main.spriteBatch.Draw(shadow, Projectile.Center - Main.screenPosition, null, Color.White, 0, shadow.Size() / 2f, 8f * Projectile.ai[0] * Dark * Dark, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(blackHole, Projectile.Center - Main.screenPosition, null, Color.White, 0, blackHole.Size() / 2f, 1.4f * Projectile.ai[0] * Dark, SpriteEffects.None, 0);
		Texture2D light = ModAsset.CursedHitStar.Value;
		Dark = Math.Max((Projectile.timeLeft - 240) / 70f, 0);
		Main.spriteBatch.Draw(blue, Projectile.Center - Main.screenPosition, null, new Color(1f,1f,1f,0), 0, blue.Size() / 2f, 6f * Projectile.ai[0] * Dark, SpriteEffects.None, 0);
		if (Projectile.timeLeft % 4 >= 2)
		{
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 5, 255, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark) * Projectile.ai[0] * 6f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 5, 255, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, Dark) * Projectile.ai[0] * 6f, SpriteEffects.None, 0);
		}
		else if(Projectile.timeLeft % 4 == 0)
		{
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 225, 255, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, Dark) * Projectile.ai[0] * 6f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 225, 255, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.3f, Dark) * Projectile.ai[0] * 6f, SpriteEffects.None, 0);
		}
		if(Projectile.timeLeft > 290)
		{
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(0.4f, Dark) * Projectile.ai[0] * 6f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.2f, Dark) * Projectile.ai[0] * 6f, SpriteEffects.None, 0);
			float lightValue = Projectile.timeLeft - 290;
			Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), lightValue * lightValue / 100f, lightValue * lightValue / 100f, lightValue);
		}
		return false;
	}
}
