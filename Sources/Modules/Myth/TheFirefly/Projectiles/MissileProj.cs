using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class MissileProj : ModProjectile, IWarpProjectile
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
		Projectile.extraUpdates = 1;
		Projectile.timeLeft = 2400;
		Projectile.alpha = 0;
		Projectile.penetrate = 30;
		Projectile.scale = 1f;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 30;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 90;
	}
	internal float StartSpeed = 200;
	internal int Aimnpc = -1;
	public override void OnSpawn(IEntitySource source)
	{
		StartSpeed = Projectile.velocity.Length();
	}
	private void ChaseTarget()
	{
		float MinDis = 350;
		foreach (var target in Main.npc)
		{
			if (target.active && Main.rand.NextBool(2))
			{
				if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy())
				{
					Vector2 ToTarget = target.Center - Projectile.Center;
					float dis = ToTarget.Length();
					if (dis < 250 && ToTarget != Vector2.Zero)
					{
						if (dis < MinDis)
						{
							MinDis = dis;
							Aimnpc = target.whoAmI;
						}
					}
				}
			}
		}
		if (Aimnpc != -1)
		{
			NPC npc = Main.npc[Aimnpc];
			if (npc.active && (npc.Center - Projectile.Center).Length() < 350)
			{
				Vector2 v0 = npc.Center - Projectile.Center;
				Projectile.velocity += Vector2.Normalize(v0).RotatedBy(Projectile.ai[1]) * 0.5f;

				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * Math.Max(StartSpeed, 5);
			}
		}
	}
	private void GenerateDust()
	{
		if (Main.rand.NextBool((int)Math.Pow(Projectile.extraUpdates + Projectile.ai[0], 3)))
		{
			if (Projectile.extraUpdates == 1)
			{
				if (Projectile.timeLeft % 3 == 0)
				{
					int index = Dust.NewDust(Projectile.position - new Vector2(2), Projectile.width, Projectile.height, ModContent.DustType<BlueGlowAppearStoppedByTile>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.7f, 1.9f));
					Main.dust[index].velocity = Projectile.velocity * 0.5f;
				}
				int index2 = Dust.NewDust(Projectile.position - new Vector2(2), Projectile.width, Projectile.height, ModContent.DustType<BlueParticleDark2StoppedByTile>(), 0f, 0f, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
				Main.dust[index2].velocity = Projectile.velocity * 0.5f;
				Main.dust[index2].alpha = (int)(Main.dust[index2].scale * 50);
			}
		}
	}
	private void AddLight()
	{
		float kTime = 1f;
		if (Projectile.timeLeft < 90f)
			kTime = Projectile.timeLeft / 90f;
		Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0.32f * kTime, 0.23f * kTime, 0);
	}
	public override void AI()
	{
		Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		if (TimeTokill >= 0 && TimeTokill <= 2)
			Projectile.Kill();
		if (TimeTokill <= 15 && TimeTokill > 0)
			Projectile.velocity = Projectile.oldVelocity;
		TimeTokill--;
		if (TimeTokill < 0)
		{
			ChaseTarget();
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
		AddLight();
		GenerateDust();
		if (Projectile.timeLeft == 2310)
			Projectile.friendly = true;
	}
	private int TimeTokill = -1;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		HitToAnything();
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		HitToAnything();
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		HitToAnything();
		Projectile.tileCollide = false;
		return false;
	}
	private void HitToAnything()
	{
		Player player = Main.player[Projectile.owner];
		ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();


		TimeTokill = 90;
		Projectile.velocity = Projectile.oldVelocity;

		if (Projectile.ai[0] > 180)
		{
			Gsplayer.FlyCamPosition = new Vector2(0, 90).RotatedByRandom(6.283);
			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BombShakeWave>(), 0, 0, Projectile.owner, 1f, 0.5f);
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BeadShakeWave>(), 0, 0, Projectile.owner, 1.4f, 1f);
			foreach (NPC target in Main.npc)
			{
				float Dis = (target.Center - Projectile.Center).Length();

				if (Dis < 350)
				{
					if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy() && target.active)
					{
						bool crit = Main.rand.NextBool(33, 100);
						target.StrikeNPC(new NPC.HitInfo()
						{
							Damage = (int)(Projectile.damage * Main.rand.NextFloat(1.70f, 2.30f)),
							KnockBack = 2,
							HitDirection = 1,
							Crit = crit,
						});

						player.addDPS(Math.Max(0, target.defDamage));
					}
				}
			}
			for (int h = 0; h < 40; h += 3)
			{
				Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.8f, 1.4f);
				int r = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<PureBlue>(), 0, 0, 0, default, 15f * Main.rand.NextFloat(0.7f, 1.9f));
				Main.dust[r].noGravity = true;
				Main.dust[r].velocity = v3;
			}
			for (int y = 0; y < 40; y += 3)
			{
				int index = Dust.NewDust(Projectile.Center - new Vector2(8) + new Vector2(0, Main.rand.NextFloat(12f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(3.3f, 5.2f));
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 10.5f), 0).RotatedByRandom(Math.PI * 2d);
			}
			for (int y = 0; y < 40; y += 3)
			{
				int index = Dust.NewDust(Projectile.Center - new Vector2(8) + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(3.3f, 5.2f));
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(1.0f, 8.5f)).RotatedByRandom(Math.PI * 2d);
			}
			for (int y = 0; y < 16; y++)
			{
				int index = Dust.NewDust(Projectile.Center - new Vector2(8) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(0.8f, 7.5f)).RotatedByRandom(Math.PI * 2d);
			}
			Projectile.friendly = false;
			return;
		}
		SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolumeScale(0.5f).WithPitchOffset(1f), Projectile.Center);
		Gsplayer.FlyCamPosition = new Vector2(0, 30).RotatedByRandom(6.283);
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BombShakeWave>(), 0, 0, Projectile.owner, 0.5f, 0.2f);
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BeadShakeWave>(), 0, 0, Projectile.owner, 0.7f, 0.5f);
		foreach (NPC target in Main.npc)
		{
			float Dis = (target.Center - Projectile.Center).Length();

			if (Dis < 150)
			{
				if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy() && target.active)
				{
					bool crit = Main.rand.NextBool(33, 100);
					target.StrikeNPC(new NPC.HitInfo()
					{
						Damage = (int)(Projectile.damage * Main.rand.NextFloat(1.70f, 2.30f)),
						KnockBack = 2f,
						HitDirection = 1,
						Crit = crit
					});

					player.addDPS(Math.Max(0, target.defDamage));
				}
			}
		}
		for (int h = 0; h < 20; h += 3)
		{
			Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.8f, 1.4f);
			int r = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<PureBlue>(), 0, 0, 0, default, 5f * Main.rand.NextFloat(0.7f, 1.9f));
			Main.dust[r].noGravity = true;
			Main.dust[r].velocity = v3;
		}
		for (int y = 0; y < 20; y += 3)
		{
			int index = Dust.NewDust(Projectile.Center - new Vector2(8) + new Vector2(0, Main.rand.NextFloat(12f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 3.2f));
			Main.dust[index].noGravity = true;
			Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 5.5f), 0).RotatedByRandom(Math.PI * 2d);
		}
		for (int y = 0; y < 20; y += 3)
		{
			int index = Dust.NewDust(Projectile.Center - new Vector2(8) + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.7f, 3.2f));
			Main.dust[index].noGravity = true;
			Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(0.5f, 4.5f)).RotatedByRandom(Math.PI * 2d);
		}
		for (int y = 0; y < 12; y++)
		{
			int index = Dust.NewDust(Projectile.Center - new Vector2(8) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.8f, 2.2f));
			Main.dust[index].noGravity = true;
			Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 3.5f)).RotatedByRandom(Math.PI * 2d);
		}
		Projectile.friendly = false;
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
	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		Texture2D star = MythContent.QuickTexture("TheFirefly/Projectiles/MissileProj");

		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition - Projectile.velocity, null, new Color(150, 150, 150, 0), Projectile.rotation + 1.2f, star.Size() / 2f, 1f, SpriteEffects.None, 0);
		return false;
	}

	public void DrawWarp(VFXBatch spriteBatch)
	{
		float width = 16;

		int TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			TrueL++;
		}
		var bars = new List<Vertex2D>();
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			float MulColor = 1f;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			if (i == 1)
				MulColor = 0f;
			if (i >= 2)
			{
				var normalDirII = Projectile.oldPos[i - 2] - Projectile.oldPos[i - 1];
				normalDirII = Vector2.Normalize(new Vector2(-normalDirII.Y, normalDirII.X));
				if (Vector2.Dot(normalDirII, normalDir) <= 0.965f)
					MulColor = 0f;
			}
			if (i < Projectile.oldPos.Length - 1)
			{
				var normalDirII = Projectile.oldPos[i] - Projectile.oldPos[i + 1];
				normalDirII = Vector2.Normalize(new Vector2(-normalDirII.Y, normalDirII.X));
				if (Vector2.Dot(normalDirII, normalDir) <= 0.965f)
					MulColor = 0f;
			}

			float k0 = (float)Math.Atan2(normalDir.Y, normalDir.X);
			k0 += 3.14f + 1.57f;
			if (k0 > 6.28f)
				k0 -= 6.28f;
			var c0 = new Color(k0, 0.4f, 0, 0);

			var factor = i / (float)TrueL;
			float x0 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x0 %= 1f;
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(x0, 1, 0)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(x0, 0, 0)));
			var factorII = factor;
			factorII = (i + 1) / (float)TrueL;
			var x1 = factorII * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x1 %= 1f;
			if (x0 > x1)
			{
				float DeltaValue = 1 - x0;
				var factorIII = factorII * x0 + factor * DeltaValue;
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(1, 1, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(1, 0, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(0, 1, 0)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(0, 0, 0)));
			}
		}
		Texture2D t = MythContent.QuickTexture("TheFirefly/Projectiles/GoldLine");

		if (bars.Count > 3)
			spriteBatch.Draw(t, bars, PrimitiveType.TriangleStrip);
	}
}