using Terraria.Localization;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class MothGrey : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("");
			}

	public override void SetDefaults()
	{
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 90;
		Projectile.alpha = 0;
		Projectile.penetrate = 1;
		Projectile.scale = 1;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
	}

	public override void AI()
	{
		Projectile.velocity = Projectile.velocity.RotatedBy(5f / Projectile.timeLeft * Projectile.ai[0] + Math.Sin(Main.time / 10f) * 0.17f);
		Projectile.velocity *= 0.95f;
	}

	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(0, 0, 0, 0));
	}

	private int TrueL = 1;

	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		float width = 2;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft / 30f;
		TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			TrueL++;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(1f, 1f) - Main.screenPosition, Color.White, new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(1f, 1f) - Main.screenPosition, Color.White, new Vector3(factor, 0, w)));
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity), Color.White, new Vector3(0, 0.5f, 1));
			Vx.Add(bars[1]);
			Vx.Add(vertex);
			for (int i = 0; i < bars.Count - 2; i += 2)
			{
				Vx.Add(bars[i]);
				Vx.Add(bars[i + 2]);
				Vx.Add(bars[i + 1]);

				Vx.Add(bars[i + 1]);
				Vx.Add(bars[i + 2]);
				Vx.Add(bars[i + 3]);
			}
		}
		Texture2D t = Common.MythContent.QuickTexture("TheFirefly/Projectiles/MothGreyLine");
		Main.graphics.GraphicsDevice.Textures[0] = t;
		if (Vx.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
	}
}