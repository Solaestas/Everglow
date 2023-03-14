namespace Everglow.Myth.LanternMoon.Projectiles.DashCore;

public class BlueStarTrail : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("蓝色荧火");
	}
	public override void SetDefaults()
	{
		Projectile.width = 1;
		Projectile.height = 1;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;

		Projectile.timeLeft = 600;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
	}
	private float Z = 0;
	public override void AI()
	{
		if (Projectile.timeLeft == 600)
		{
			Z = Main.rand.NextFloat(0, (float)(Math.PI * 2));
			Projectile.timeLeft = Main.rand.Next(20, 40);
		}
		Vector2 v = new Vector2(0, 3).RotatedBy(Z);
		Z += Main.rand.NextFloat(-0.04f, 0.04f);
		Projectile.velocity = Projectile.velocity * 0.9f + v * 0.1f;
		ka = 0.2f;
		if (Projectile.timeLeft < 60f)
			ka = Projectile.timeLeft / 300f;
	}

	int TrueL = 1;
	float ka = 0;
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		float width = 12;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft / 5f;
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
			var factor = 1f;
			if (Projectile.oldPos.Length > 0)
				factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);
			Lighting.AddLight(Projectile.oldPos[i], (255 - Projectile.alpha) * 0f / 50f * ka * (1 - factor), (255 - Projectile.alpha) * 0.3f / 50f * ka * (1 - factor), (255 - Projectile.alpha) * 1f / 50f * ka * (1 - factor));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(0.5f, 0.5f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(0.5f, 0.5f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f, new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
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
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/DashCore/StarTrail").Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
	}
}
