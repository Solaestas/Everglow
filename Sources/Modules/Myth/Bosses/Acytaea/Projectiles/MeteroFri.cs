namespace Everglow.Myth.Bosses.Acytaea.Projectiles;

internal class MeteroFri : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 180;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
	}

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
	{
	}

	private float ka = 0;

	public override void AI()
	{
		ka = 1;
		if (Projectile.timeLeft < 60f)
			ka = Projectile.timeLeft / 60f;
		Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.2f / 50f * ka, 0, 0);
		float num2 = Projectile.Center.X;
		float num3 = Projectile.Center.Y;
		float num4 = 800f;
		bool flag = false;
		for (int j = 0; j < 200; j++)
		{
			if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
			{
				float num5 = Main.npc[j].position.X + Main.npc[j].width / 2;
				float num6 = Main.npc[j].position.Y + Main.npc[j].height / 2;
				float num7 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num5) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num6);
				if (num7 < num4)
				{
					num4 = num7;
					num2 = num5;
					num3 = num6;
					flag = true;
				}
			}
		}
		if (flag)
		{
			float num8 = 20f;
			var vector1 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float num9 = num2 - vector1.X;
			float num10 = num3 - vector1.Y;
			float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
			num11 = num8 / num11;
			num9 *= num11;
			num10 *= num11;
			Projectile.velocity.X = (Projectile.velocity.X * 200f + num9) / 201f;
			Projectile.velocity.Y = (Projectile.velocity.Y * 200f + num10) / 201f;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public static float Timer = 0;
	public static int WHOAMI = -1;
	public static int Typ = -1;
	private int TrueL = 1;

	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		int width = 60;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft;
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

			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 0, w)));
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
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/Projectiles/Metero").Value;
		if (Vx.Count >= 3)
		{
			Main.graphics.GraphicsDevice.Textures[0] = t;//GoldenBloodScaleMirror
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
		}
	}
}