namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class floatLantern5 : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = 7200;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;

	}
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(1f, 1f, 1f, 0.5f));
	}
	private bool initialization = true;
	public override void AI()
	{
		if (initialization)
		{
			num1 = Main.rand.Next(-120, 0);
			num4 = Main.rand.NextFloat(0.3f, 1800f);
			if (Projectile.timeLeft > 6000)
				Projectile.timeLeft = 3000;
			for (int i = 0; i < 4; i++)
			{
				V4[i] = new Vector2(0, Main.rand.NextFloat(5f, 40f)).RotatedByRandom(Math.PI * 2d);
				F4[i] = Main.rand.NextFloat(1f, 2.7f);
			}
			Fy = Main.rand.Next(4);
			initialization = false;
		}
		num1 += 1;
		num4 += 0.01f;
		Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - (float)Math.PI * 0.5f;

		if (num1 > 0 && num1 <= 120)
			num = num1 / 120f;
		if (Projectile.timeLeft < 120)
			num = Projectile.timeLeft / 120f;
	}
	private float num = 0;
	private int num1 = 0;
	private float num4 = 0;
	private int Fy = 0;
	private int fyc = 0;
	private Vector2[] V4 = new Vector2[4];
	private float[] F4 = new float[4];
	public override bool PreDraw(ref Color lightColor)
	{
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

		for (int i = 0; i < 4; i++)
		{
			Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + V4[i], new Rectangle?(new Rectangle(0, 0, texture2D.Width, nuM)), colorT, Projectile.rotation, new Vector2(texture2D.Width / 2f, nuM / 2f), Projectile.scale * F4[i], SpriteEffects.None, 1f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/LanternMoon/Projectiles/LanternKing/LanternFire").Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + V4[i], new Rectangle?(new Rectangle(0, 30 * Fy, 20, 30)), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f * F4[i], SpriteEffects.None, 1f);
		}
		return false;
	}
}