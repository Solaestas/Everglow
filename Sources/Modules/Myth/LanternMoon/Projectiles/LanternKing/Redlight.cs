namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

class Redlight : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 480;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1000;
	}
	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
	{
	}
	float ka = 0;
	int AIMNpc = -1;
	float kb = -1000;
	Vector2 AIMpos;
	public override void AI()
	{
		if (kb == -1000)
		{
			kb = Main.rand.NextFloat(-100f, 100f);
			if (Main.rand.NextFloat(-100f, 100f) > 0)
				AIMNpc = 1;
		}
		Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
		AIMpos = new Vector2(-500, 0).RotatedBy(Projectile.timeLeft / 18f * AIMNpc + kb * Math.Sign((Main.time + kb) % 300 - 150));
		ka = 1;
		if (Projectile.timeLeft < 60f)
			ka = Projectile.timeLeft / 60f;
		Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.2f / 50f * ka, 0, 0);
		Projectile.velocity *= 0.96f;
		Vector2 v0 = AIMpos + player.Center;
		var v1 = Vector2.Normalize(v0 - Projectile.Center);
		v1 = (v0 - Projectile.Center + v1 * 60f) / 480f;
		Projectile.velocity += v1;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public static float Timer = 0;
	public static int WHOAMI = -1;
	public static int Typ = -1;
	int TrueL = 1;
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		float width = 100;
		if (Projectile.timeLeft < 50)
			width = Projectile.timeLeft * 2f;
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
			float width2 = width;
			if (i < 40)
				width2 *= (float)Math.Sqrt(i / 40f);
			if (TrueL - i < 15)
			{
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width2 + new Vector2(10, 10) - Main.screenPosition, new Color(Math.Clamp((int)(255 * (TrueL - i - 1) / 15f * ka), 0, 255), 0, 0, 0), new Vector3(factor, 1, w)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width2 + new Vector2(10, 10) - Main.screenPosition, new Color(Math.Clamp((int)(255 * (TrueL - i - 1) / 15f * ka), 0, 255), 0, 0, 0), new Vector3(factor, 0, w)));
			}/*new Color(Math.Clamp((int)(255 * (Count - i - 1) / 5f), 0, 255), 0, 0, 0)*/
			else
			{
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width2 + new Vector2(10, 10) - Main.screenPosition, new Color((int)(255 * ka), 0, 0, 0), new Vector3(factor, 1, w)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width2 + new Vector2(10, 10) - Main.screenPosition, new Color((int)(255 * ka), 0, 0, 0), new Vector3(factor, 0, w)));
			}
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
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
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/LanternKing/FlameRiver").Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
	}
}
