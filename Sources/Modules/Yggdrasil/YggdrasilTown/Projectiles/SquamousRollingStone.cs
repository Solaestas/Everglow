using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class SquamousRollingStone : ModProjectile
{
	public int StartDirection = 0;
	public int StopTimer = 0;

	public override void OnSpawn(IEntitySource source)
	{
		StartDirection = Math.Sign(Projectile.ai[0]);
		for (int t = 0; t < 100; t++)
		{
			if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			{
				Projectile.position.Y -= 1;
			}
			else
			{
				Projectile.position.Y -= 5;
				break;
			}
		}
	}

	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.width = 100;
		Projectile.height = 100;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 1500;
		Projectile.aiStyle = -1;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
	}

	public override void AI()
	{
		if(Projectile.scale < 0.5)
		{
			return;
		}
		if (Projectile.velocity.Length() <= 0.01f)
		{
			StopTimer++;
			if (StopTimer > 60)
			{
				if (Projectile.timeLeft > 60)
				{
					Projectile.timeLeft = 60;
				}
			}
		}
		else
		{
			StopTimer = 0;
		}
		Projectile.velocity.X += StartDirection * 0.04f;
		Projectile.velocity.Y += 0.2f;
		Projectile.rotation += Projectile.velocity.X * 0.01f;
		if (Projectile.velocity.Length() > 4)
		{
			//Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SquamousShellStone>(), Projectile.velocity.X * Main.rand.NextFloat(0.3f, 0.6f), Projectile.velocity.Y * Main.rand.NextFloat(0.3f, 0.6f), 0, default, Main.rand.NextFloat(0.9f, 1.6f));
			//dust.noGravity = false;
			//GenerateSmogAtBottom(1);
		}
	}

	public void GenerateSmogAtBottom(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(0f, 1f))).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Bottom + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(17, 45),
				scale = Main.rand.NextFloat(10f, 25f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (MathF.Abs(Projectile.velocity.Y) > 2)
		{
			for (int x = 0; x < Projectile.velocity.Length() * 5; x++)
			{
				Vector2 newVel = Projectile.velocity.RotateRandom(6.283);
				//Dust dust = Dust.NewDustDirect(Projectile.Bottom, 0, 0, ModContent.DustType<SquamousShellStone>(), newVel.X * Main.rand.NextFloat(0.3f, 0.6f), newVel.Y * Main.rand.NextFloat(0.3f, 0.6f), 0, default, Main.rand.NextFloat(0.9f, 1.6f));
				//dust.noGravity = false;
			}
		}
		if (Projectile.velocity.X * Projectile.oldVelocity.X < 0)
		{
			Projectile.velocity.X = 0;
		}
		return false;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D drawTex = ModAsset.SquamousRollingStone.Value;
		Texture2D drawGlow = ModAsset.SquamousRollingStone_glow.Value;
		Main.spriteBatch.Draw(drawTex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, drawTex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		if (Projectile.timeLeft <= 60)
		{
			float glowValue = Projectile.timeLeft / 60f;
			glowValue = (MathF.Sin(1 / (glowValue + 0.01f)) + 0.5f) * (1 - glowValue);
			Main.spriteBatch.Draw(drawGlow, Projectile.Center - Main.screenPosition, null, new Color(glowValue, glowValue, glowValue, 0), Projectile.rotation, drawTex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		}
		return false;
	}

	public override void OnKill(int timeLeft)
	{
		for (int x = 0; x < 16; x++)
		{
			Dust.NewDust(Projectile.Center - Projectile.velocity * 2 - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<SquamousShellStone>(), 0f, 0f, 0, default, 0.7f);
		}
		GenerateSmog(8);
		Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<SquamousRockExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 16);
		ShakerManager.AddShaker(Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.Pi), 120, 20f, 120, 0.9f, 0.8f, 150);
	}

	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(37, 45),
				scale = Main.rand.NextFloat(40f, 55f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
		for (int g = 0; g < Frequency * 10; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(25f, 46f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmog_ConeDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(20, 62),
				scale = Main.rand.NextFloat(0.6f, 25f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
		for (int g = 0; g < Frequency * 20; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(1.0f, 34f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new Spark_MoonBladeDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(70, 125),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(9f, 47.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				noGravity = true,
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) },
			};
			Ins.VFXManager.Add(spark);
		}
	}
}