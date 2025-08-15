using Everglow.Yggdrasil.KelpCurtain.Items.Weapons;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Magic;

public class GreenSungloShield_B : ModProjectile
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MagicProjectiles;

	public override string Texture => ModAsset.GreenSungloThorns_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 48;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1200;
		Projectile.penetrate = -1;
	}

	private int timer = 0;

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.Center = Main.player[Projectile.owner].Center;
		Projectile.timeLeft++;
		timer++;
		if (player.HeldItem.type != ModContent.ItemType<GreenSungloStaff>())
		{
			Projectile.Kill();
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		int a = 3;
		int b = 2;
		int width = 20;
		int count = Math.Min(628, timer * 2);
		var backVertices = new List<Vertex2D>();

		for (float i = 0; i <= count; i++)
		{
			Vector2 pos0 = new Vector2(MathF.Cos(a * (i + 1) * 0.01f + (float)Main.time * 0.02f), MathF.Sin(b * (i + 1) * 0.01f + (float)Main.time * 0.02f)) * 30;
			Vector2 pos = new Vector2(MathF.Cos(a * i * 0.01f + (float)Main.time * 0.02f), MathF.Sin(b * i * 0.01f + (float)Main.time * 0.02f)) * 30;
			Vector2 pos2 = new Vector2(MathF.Cos(a * (i - 1) * 0.01f + (float)Main.time * 0.02f), MathF.Sin(b * (i - 1) * 0.01f + (float)Main.time * 0.02f)) * 30;
			Vector2 normal = (pos2 - pos0).NormalizeSafe().RotatedBy(MathF.PI + 0.5f);
			normal = MathUtils.Lerp(0.5f, normal, Vector2.UnitY);
			backVertices.Add(new Vertex2D(Projectile.Center + pos - normal * width - Main.screenPosition, Color.White, new Vector3(0, (float)(i / 100f), 0)));
			backVertices.Add(new Vertex2D(Projectile.Center + pos + normal * width - Main.screenPosition, Color.White, new Vector3(1, (float)(i / 100f), 0)));
		}

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.GreenSungloThorns.Value;
		if (backVertices.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, backVertices.ToArray(), 0, backVertices.Count - 2);
		}

		return false;
	}
}