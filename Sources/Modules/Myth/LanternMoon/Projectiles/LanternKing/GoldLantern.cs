namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class GoldLantern : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("金灯笼");
	}
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 600;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;

	}
	//55555
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(1f, 1f, 1f, 0.5f));
	}
	private bool initialization = true;
	private float Sca = 0;
	public override void AI()
	{
		if (initialization)
		{
			num1 = Main.rand.Next(-120, 0);
			num4 = Main.rand.NextFloat(0.3f, 1800f);
			Fy = Main.rand.Next(4);
			initialization = false;
		}
		num1 += 1;
		num4 += 0.01f;
		if (Projectile.timeLeft > 510)
		{
			Projectile.velocity *= 0.8f;
			Projectile.velocity = Projectile.velocity.RotatedBy(1 / 105d * Math.PI);
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - (float)Math.PI * 0.5f;
			Sca += 0.01111111f;
		}
		else
		{
			Projectile.velocity *= 0;
			if (Projectile.timeLeft == 510)
			{
				Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GoldLanternRay>(), 0, 0, Main.myPlayer, Projectile.rotation + 1, Projectile.rotation + 0.3f);
				Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GoldLanternRay>(), 0, 0, Main.myPlayer, Projectile.rotation - 1, Projectile.rotation - 0.3f);
			}
		}
		if (num1 > 0 && num1 <= 120)
			num = num1 / 120f;
		if (Projectile.timeLeft < 120)
		{
			num = Projectile.timeLeft / 120f;
			Sca -= 0.01f;
		}
	}
	private float num = 0;
	private int num1 = 0;
	private float num4 = 0;
	private float x = 0;
	private int Fy = 0;
	private int fyc = 0;
	float ka = 0;
	public override bool PreDraw(ref Color lightColor)
	{
		ka = 1;
		if (Projectile.timeLeft < 60f)
			ka = Projectile.timeLeft / 60f;
		var texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture);
		int nuM = texture2D.Height;
		fyc += 1;
		if (fyc == 8)
		{
			fyc = 0;
			Fy += 1;
		}
		if (Fy > 3)
			Fy = 0;
		var colorT = new Color(1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 0.5f * num * (float)(Math.Sin(num4) + 2) / 3f);
		Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, texture2D.Width, nuM)), colorT, Projectile.rotation, new Vector2(texture2D.Width / 2f, nuM / 2f), Projectile.scale, SpriteEffects.None, 1f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/LanternKing/LanternFire").Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 30 * Fy, 20, 30)), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
		x += 0.01f;
		float K = (float)(Math.Sin(x + Math.Sin(x) * 6) * (0.95 + Math.Sin(x + 0.24 + Math.Sin(x))) + 3) / 30f;
		float M = (float)(Math.Sin(x + Math.Tan(x) * 6) * (0.95 + Math.Cos(x + 0.24 + Math.Sin(x))) + 3) / 30f;
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, 0, new Vector2(128f, 128f), K * 2.8f * 6, SpriteEffects.None, 0f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, (float)(Math.PI * 0.5), new Vector2(128f, 128f), K * 2.8f * Sca * 6 * ka, SpriteEffects.None, 0f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.75), new Vector2(128f, 128f), M * 2.8f * Sca * 6 * ka, SpriteEffects.None, 0f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.25), new Vector2(128f, 128f), M * 2.8f * Sca * 6 * ka, SpriteEffects.None, 0f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, x * 6f, new Vector2(128f, 128f), (M + K) * 2.8f * Sca * 6 * ka, SpriteEffects.None, 0f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, -x * 6f, new Vector2(128f, 128f), (float)Math.Sqrt(M * M + K * K) * 2.8f * Sca * 6 * ka, SpriteEffects.None, 0f);
		return false;
	}
}