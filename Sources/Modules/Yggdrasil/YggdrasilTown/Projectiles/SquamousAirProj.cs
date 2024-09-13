using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class SquamousAirProj : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 3600;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
	}
	internal int Target = -1;
	internal int TimeTokill = -1;
	public override void OnSpawn(IEntitySource source)
	{
		Target = (int)(Projectile.ai[0]);
	}
	public override void AI()
	{
		if (Target == -1)
		{
			Projectile.Kill();
			return;
		}
		Player player = Main.player[Target];
		Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		if (TimeTokill >= 0 && TimeTokill <= 2)
			Projectile.Kill();
		if (TimeTokill <= 15 && TimeTokill > 0)
			Projectile.velocity = Projectile.oldVelocity;
		TimeTokill--;
		if (TimeTokill < 0)
		{
			if(Projectile.timeLeft > 3480)
			{
				if((player.Center - Projectile.Center).Length() > 100)
				{
					float timeValue = (Projectile.timeLeft - 3480) / 120f;
					Vector2 aimVel = player.Center + player.velocity - Projectile.Center - Projectile.velocity;
					aimVel = Vector2.Normalize(aimVel);
					Projectile.velocity = Projectile.velocity * 0.9f + aimVel * 0.1f * (1 - timeValue) * 8f;
				}
			}
			Projectile.velocity.Y += 0.05f;
			Lighting.AddLight(Projectile.Center, 0, 0.4f, 0.7f);
		}
		else
		{
			if (TimeTokill < 10)
			{
				Projectile.damage = 0;
				Projectile.friendly = false;
			}
			Projectile.velocity *= 0f;
		}
	}
	public override void OnKill(int timeLeft)
	{
		base.OnKill(timeLeft);
	}
	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new AirFlameSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(37, 45),
				scale = Main.rand.NextFloat(40f, 55f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		AmmoHit();
		return false;
	}
	public void AmmoHit()
	{
		TimeTokill = 60;
		Projectile.velocity = Projectile.oldVelocity;
		GenerateSmog(8);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		if (TimeTokill > 0)
			return false;
		else
		{
			var TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, new Color(1f, 1f, 1f, 0), Projectile.rotation, TexMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
	public override void PostDraw(Color lightColor)
	{
		DrawTrail(lightColor);
	}
	public void DrawTrail(Color light)
	{
		float drawC = 0.4f;

		var bars = new List<Vertex2D>();
		int trueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				if (i == 1)
					return;
				break;
			}
			trueL = i;
		}
		float additiveFactor = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			float width = 17;
			if (Projectile.timeLeft <= 30)
				width *= Projectile.timeLeft / 30f;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = new Vector2(-normalDir.Y, normalDir.X).SafeNormalize(Vector2.Zero);

			var factor = i / (float)trueL;
			additiveFactor += (Projectile.oldPos[i - 1] - Projectile.oldPos[i]).Length() / 600f;
			float timer = (float)Main.time * 0.02f + Projectile.whoAmI * 0.77f;
			var color = Color.Lerp(new Color(drawC * light.R / 255f * 0.1f, drawC * light.G / 255f * 0.6f, drawC * light.B / 255f * 0.7f, 0), new Color(0, 0, 0, 0), factor);
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10) - Main.screenPosition, color, new Vector3(-additiveFactor * 2 + timer, 0, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10) - Main.screenPosition, color, new Vector3(-additiveFactor * 2 + timer, 1, 0)));

		}
		if (bars.Count > 2)
		{
			Texture2D t = Commons.ModAsset.Trail_2_thick.Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
	}
}