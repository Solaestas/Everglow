namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class DarkLantern3 : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = 5280;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1.5f;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
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
			Projectile.timeLeft = 5280;
			Fy = Main.rand.Next(4);
			initialization = false;
		}
		num1 += 1;
		num4 += 0.01f;
		if (num1 > 0 && num1 <= 120)
			num = num1 / 120f;
		if (num1 >= 1)
			Projectile.hostile = true;
		if (Projectile.timeLeft < 240)
			Projectile.scale *= 0.99f;
	}
	private float num = 0;
	private int num1 = 0;
	private float num4 = 0;
	private int Fy = 0;
	private int fyc = 0;
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
		Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, texture2D.Width, nuM)), colorT, Projectile.rotation, new Vector2(texture2D.Width / 2f, nuM / 2f), Projectile.scale, SpriteEffects.None, 1f);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/LanternMoon/Projectiles/LanternKing/LanternFire").Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 30 * Fy, 20, 30)), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
		return false;
	}
}