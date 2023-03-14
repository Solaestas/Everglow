using Terraria.Localization;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class GoldLanternLine4 : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Gold Lantern Line");
		DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "灯笼须4");
	}
	public override void SetDefaults()
	{
		Projectile.width = 1;
		Projectile.height = 1;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;

		Projectile.timeLeft = 600;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
	}
	float ka = 0;
	private float Z = 0;
	private Vector2 Vk;
	public override void AI()
	{
		Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
		if (Projectile.timeLeft == 600)
		{
			Z = Main.rand.NextFloat(0, (float)(Math.PI * 2));
			Projectile.timeLeft = Main.rand.Next(80, 82);
		}
		Vector2 v = new Vector2(0, 3).RotatedBy(Z);
		Z += Main.rand.NextFloat(-0.04f, 0.04f);
		if (Projectile.timeLeft == 60)
			Vk = (player.Center - Projectile.Center) / (player.Center - Projectile.Center).Length() * 24;
		if (Projectile.timeLeft <= 60)
			v = Vk;
		Projectile.velocity = Projectile.velocity * 0.96f + v * 0.04f;
		ka = 0.2f;
		if (Projectile.timeLeft < 60f)
			ka = Projectile.timeLeft / 300f;
	}


	int TrueL = 1;
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

			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);

			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
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
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapLanternLine").Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
	}
}
