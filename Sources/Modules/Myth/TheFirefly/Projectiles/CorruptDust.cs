using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class CorruptDust : ModProjectile
{
	public override void SetStaticDefaults()
	{
	}

	public override void SetDefaults()
	{
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 1080;
		Projectile.alpha = 0;
		Projectile.penetrate = 1;
		Projectile.scale = 1;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 90;
	}

	public override void AI()
	{
		if (Projectile.timeLeft % 3 == 0)
		{
			int index = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, ModContent.DustType<BlueGlowAppear>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.7f, 1.9f));
			Main.dust[index].velocity = Projectile.velocity * 0.5f;
		}
		int index2 = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, ModContent.DustType<BlueParticleDark2>(), 0f, 0f, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
		Main.dust[index2].velocity = Projectile.velocity * 0.5f;
		Main.dust[index2].alpha = (int)(Main.dust[index2].scale * 50);
	}

	public override void Kill(int timeLeft)
	{
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
	}

	public override void PostDraw(Color lightColor)
	{
		DrawTrail();
	}
	private void DrawTrail()
	{
		float k1 = 60f;
		float k0 = (2400 - Projectile.timeLeft) / k1;

		if (Projectile.timeLeft <= 2400 - k1)
			k0 = 1;

		var c0 = new Color(0, k0 * k0 * 0.4f + 0.2f, k0 * 0.8f + 0.2f, 0);
		var bars = new List<Vertex2D>();


		int TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			TrueL++;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			float width = 36;
			if (Projectile.timeLeft <= 40)
				width = Projectile.timeLeft * 0.9f;
			if (i < 10)
				width *= i / 10f;
			if (Projectile.ai[0] == 3)
				width *= 0.5f;
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);
			float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
			x0 %= 1f;
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
			var factorII = (i + 1) / (float)TrueL;
			var x1 = factorII * 0.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
			x1 %= 1f;
			if (x0 > x1)
			{
				float DeltaValue = 1 - x0;
				var factorIII = factorII * x0 + factor * DeltaValue;
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(1, 1, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(1, 0, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(0, 1, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0, new Vector3(0, 0, 0)));
			}
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Texture2D t = MythContent.QuickTexture("TheFirefly/Projectiles/GoldLine");
		Main.graphics.GraphicsDevice.Textures[0] = t;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

	}
}