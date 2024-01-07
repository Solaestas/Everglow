using Everglow.Myth.Common;

namespace Everglow.Myth.LanternMoon.Projectiles;

class LBloodEffect : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 180;
		Projectile.tileCollide = false;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
	}
	Vector2 AIMpos = Vector2.Zero;
	int TrueL = 1;
	Vector2 Acc = Vector2.Zero;
	float Omega = 0;
	int AimProj = -1;
	public override ModProjectile Clone(Projectile projectile)
	{
		var clone = base.Clone(projectile) as LBloodEffect;
		//值类型不必重新赋值
		//AIMpos = Vector2.Zero;
		//TrueL = 1;
		//Acc = Vector2.Zero;
		//Omega = 0;
		//AimProj = -1;
		return clone;
	}
	public override void AI()
	{
		if (Omega == 0)
			Omega = Main.rand.NextFloat(-0.4f, 0.4f);
		if (Projectile.timeLeft <= 160)
			Omega *= 0.96f;
		if (AimProj == -1)
		{
			AIMpos = Main.projectile[(int)Projectile.ai[0]].Center;
			for (int f = 0; f < Main.projectile.Length; f++)
			{
				if (Main.projectile[f].active && Main.projectile[f].type == ModContent.ProjectileType<LMeteor>())
				{
					AimProj = f;
					break;
				}
			}
			if (Projectile.velocity.Length() > 7f)
				Projectile.velocity *= 0.95f;
		}
		else
		{
			AIMpos = Main.projectile[AimProj].Center;
		}

		if (Projectile.timeLeft >= 140 && AimProj == -1)
		{
			Acc = (AIMpos - Projectile.Center) / 530f;
			Projectile.velocity += Acc;
			if (Projectile.timeLeft <= 160)
				Projectile.velocity = Projectile.velocity.RotatedBy(Omega);
		}
		else
		{
			Acc = (AIMpos - Projectile.Center) / 30f;
			Projectile.velocity += Acc;
			float kv = Math.Clamp(Projectile.velocity.Length() / 3f, 1, 100);

			Projectile.velocity = Projectile.velocity.RotatedBy(Omega / kv);
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public override void PostDraw(Color lightColor)
	{
		var bars = new List<Vertex2D>();
		float width = 6;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft / 10f;
		TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			TrueL++;
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
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
		if (Vx.Count > 2)
		{
			Texture2D t = ModAsset.LBloodEffect.Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
		}
	}
}
