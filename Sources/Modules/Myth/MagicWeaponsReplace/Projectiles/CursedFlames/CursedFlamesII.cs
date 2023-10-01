using Everglow.Myth.Common;
using Terraria;
using Terraria.Audio;

namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.CursedFlames;

public class CursedFlamesII : ModProjectile, IWarpProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = 1000;
		Projectile.alpha = 0;
		Projectile.penetrate = 3;
		Projectile.scale = 1f;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 120;
	}

	public override void AI()
	{
		Projectile.velocity.Y += 0.010f;
		if (Main.rand.NextBool(4))
		{
			GenerateVFX(1);
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.CursedTorch, 0, 0, 0, default, 2.2f);
			d0.noGravity = true;
		}
	}
	public void GenerateVFX(int Frequency)
	{
		float mulVelocity = 1f;
		for (int g = 0; g < Frequency; g++)
		{
			var cf = new CursedFlameDust
			{
				velocity = Projectile.velocity * Main.rand.NextFloat(0.65f, 2.5f) * mulVelocity + Projectile.velocity.SafeNormalize(new Vector2(0, -1)),
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * 1,
				maxTime = Main.rand.Next(27, 72),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.01f, 0.01f), Main.rand.NextFloat(3.6f, 10f) * mulVelocity }
			};
			Ins.VFXManager.Add(cf);
		}
	}

	public void GenerateVFXExpolode(int Frequency, float mulVelocity = 1f)
	{
		for (int g = 0; g < Frequency * 3; g++)
		{
			var cf = new CursedFlameDust
			{
				velocity = new Vector2(0, Main.rand.NextFloat(4.65f, 5.5f)).RotatedByRandom(6.283) * mulVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-56f, 56f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(16, 36),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.18f, 0.18f), Main.rand.NextFloat(4f, 12f) }
			};
			Ins.VFXManager.Add(cf);
		}
		for (int g = 0; g < Frequency; g++)
		{
			var cf = new CursedFlameDust
			{
				velocity = new Vector2(0, Main.rand.NextFloat(6.65f, 10.5f)).RotatedByRandom(6.283) * mulVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(12, 30),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(4f, 32f) }
			};
			Ins.VFXManager.Add(cf);
		}

	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D Light = MythContent.QuickTexture("TheFirefly/Projectiles/GlowStar");
		Texture2D Shade = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterBolt/NewWaterBoltShade");
		float k0 = 1f;
		var c0 = new Color(k0 * 0.2f + 0.2f, k0 * k0 * 0.3f + 0.6f, 0, 0);

		var bars0 = new List<Vertex2D>();
		float width = 24;

		int TrueL = 0;
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
			float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
			x0 %= 1f;
			bars0.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, Color.White, new Vector3(x0, 1, w)));
			bars0.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, Color.White, new Vector3(x0, 0, w)));
		}
		Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTraceShade");
		Main.graphics.GraphicsDevice.Textures[0] = t;
		if (bars0.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars0.ToArray(), 0, bars0.Count - 2);

		Main.spriteBatch.Draw(Shade, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.White, Projectile.rotation, Light.Size() / 2f, (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 2.5f, SpriteEffects.None, 0);

		var bars = new List<Vertex2D>();
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;

			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);
			float x0 = factor * 1.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
			x0 %= 1f;
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
		}
		t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTrace");
		Main.graphics.GraphicsDevice.Textures[0] = t;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c0, Projectile.rotation, Light.Size() / 2f, (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 2.5f, SpriteEffects.None, 0);
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c0, Projectile.rotation, Light.Size() / 2f, (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 2.5f, SpriteEffects.None, 0);
		return false;
	}

	public void DrawWarp(VFXBatch spriteBatch)
	{
		var c0 = new Color(0.6f, 0.6f, 0f);
		var bars = new List<Vertex2D>();
		float width = 64;

		int TrueL = 0;
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
			float x0 = factor * 1.6f - (float)(Main.timeForVisualEffects / 15d) + 10000;
			x0 %= 1f;
			float mul = 1f;
			if (i < 10)
				mul = i / 10f;
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) * mul + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) * mul + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
		}
		Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTrace");
		// Main.graphics.GraphicsDevice.Textures[0] = t;
		if (bars.Count > 3)
			//Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			spriteBatch.Draw(t, bars, PrimitiveType.TriangleStrip);
	}


	public override void OnKill(int timeLeft)
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 33).RotatedByRandom(6.283);

		GenerateVFXExpolode(24, 2.2f);

		for (int d = 0; d < 70; d++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.CursedTorch, 0, 0, 0, default, 0.6f);
			d0.velocity = new Vector2(0, Main.rand.NextFloat(3.65f, 7.5f)).RotatedByRandom(6.283);
		}
		int HitType = ModContent.ProjectileType<CursedFlameHit>();
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, Projectile.damage, Projectile.knockBack * 6, Projectile.owner, 18, Projectile.rotation + Main.rand.NextFloat(6.283f));

		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithVolumeScale(0.4f), Projectile.Center);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 11).RotatedByRandom(6.283);
		GenerateVFXExpolode(14, 1.2f);
		for (int d = 0; d < 28; d++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.CursedTorch, 0, 0, 0, default, 0.6f);
			d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283);
		}

		int HitType = ModContent.ProjectileType<CursedFlameHit>();
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, Projectile.damage, Projectile.knockBack * 2, Projectile.owner, 10, Projectile.rotation + Main.rand.NextFloat(6.283f));
		target.AddBuff(BuffID.CursedInferno, 900);
		Projectile.damage = (int)(Projectile.damage * 1.2);

		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithVolumeScale(0.4f), Projectile.Center);
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 11).RotatedByRandom(6.283);
		GenerateVFXExpolode(14, 1.2f);
		for (int d = 0; d < 28; d++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.CursedTorch, 0, 0, 0, default, 0.6f);
			d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283);
		}
		int HitType = ModContent.ProjectileType<CursedFlameHit>();
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, Projectile.damage, Projectile.knockBack * 2, Projectile.owner, 10, Projectile.rotation + Main.rand.NextFloat(6.283f));
		target.AddBuff(BuffID.CursedInferno, 900);
		Projectile.damage = (int)(Projectile.damage * 1.2);

		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithVolumeScale(0.4f), Projectile.Center);
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 11).RotatedByRandom(6.283);
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithVolumeScale(0.4f), Projectile.Center);
		GenerateVFXExpolode(14, 1.2f);
		for (int d = 0; d < 28; d++)
		{
			Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
			var d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.CursedTorch, 0, 0, 0, default, 0.6f);
			d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283);
		}
		int HitType = ModContent.ProjectileType<CursedFlameHit>();
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, Projectile.damage, Projectile.knockBack * 2, Projectile.owner, 10, Projectile.rotation + Main.rand.NextFloat(6.283f));
		if (Projectile.velocity.X != oldVelocity.X)
			Projectile.velocity.X = -oldVelocity.X;
		if (Projectile.velocity.Y != oldVelocity.Y)
			Projectile.velocity.Y = -oldVelocity.Y;
		Projectile.velocity *= 0.98f;
		Projectile.penetrate--;
		Projectile.damage = (int)(Projectile.damage * 1.2);
		return false;
	}
}