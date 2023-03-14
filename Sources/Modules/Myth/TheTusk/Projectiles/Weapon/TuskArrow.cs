using Everglow.Myth.TheTusk.Gores;
using Terraria;

namespace Everglow.Myth.TheTusk.Projectiles.Weapon;

class TuskArrow : ModProjectile, IWarpProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = 3;
		Projectile.timeLeft = 600;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Ranged;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		for (int f = 0; f < 8; f++)
		{
			var g = Gore.NewGoreDirect(null, target.Center, new Vector2(0, Main.rand.NextFloat(10f)).RotatedByRandom(6.283), ModContent.GoreType<Blood>(), Main.rand.NextFloat(0.65f, Main.rand.NextFloat(2.5f, 3.75f)));
			g.timeLeft = Main.rand.Next(250, 500);
		}
		for (int y = 0; y < 16; y++)
		{
			int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 3f));
			Main.dust[num90].noGravity = false;
			Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d) * 0.5f;
		}
		if (Projectile.penetrate <= 1)
		{
			Projectile.timeLeft -= 40;
			Projectile.friendly = false;
		}
	}
	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
	}
	internal int TargetWhoAmI = -1;
	private void FindEnemies()
	{
		Vector2 detectCenter = Projectile.Center;
		float minDistance = 200;
		foreach (NPC npc in Main.npc)
		{
			if (npc.active)
			{
				if (!npc.dontTakeDamage && !npc.friendly && npc.CanBeChasedBy() && Collision.CanHit(Projectile, npc))
				{
					if ((npc.Center - detectCenter).Length() < minDistance)
					{
						minDistance = (npc.Center - detectCenter).Length();
						TargetWhoAmI = npc.whoAmI;
					}
				}
			}
		}
	}
	private void MoveTo(Vector2 aim, float speedValue = 0.1f)
	{
		Vector2 v0 = aim - Projectile.velocity - Projectile.Center;
		float val = v0.Length();
		if (val > 100f)
			val = 100f;
		Projectile.velocity = (aim - Projectile.velocity - Projectile.Center).SafeNormalize(Vector2.Zero) * val * speedValue * 0.2f + Projectile.velocity * 0.8f;
	}
	private void Attack()
	{
		NPC target;
		if (TargetWhoAmI is >= 0 and <= 200)
		{
			target = Main.npc[TargetWhoAmI];
			if (!target.active)
				return;
		}
		else
		{
			return;
		}
		MoveTo(target.Center, 0.2f);
	}
	public override void AI()
	{
		Projectile.hide = true;
		if (TargetWhoAmI <= -1)
		{
			FindEnemies();
			Player player = Main.player[Projectile.owner];
			Vector2 v0 = player.Center + new Vector2(-20 * player.direction, -40) - Projectile.Center;
			float vscale = MathF.Pow(v0.Length(), 0.6f);
			Vector2 v1 = v0 / v0.Length();
			Projectile.velocity += v1 * vscale / 10f;
			if (Projectile.velocity.Length() > 20)
				Projectile.velocity *= 0.99f;
			if (Projectile.velocity.Length() > 30)
				Projectile.velocity *= 0.99f;
			if (Projectile.velocity.Length() > 40)
				Projectile.velocity *= 0.99f;
		}
		else
		{
			Attack();
		}
		for (int y = 0; y < 8; y++)
		{
			var blood = Dust.NewDustDirect(Projectile.Center - new Vector2(4) + Projectile.velocity * Main.rand.NextFloat(-4f, 1f), 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 2f) * 0.4f);
			blood.noGravity = true;
			blood.velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d);
		}
		if (Main.rand.NextBool(6))
		{
			var g = Gore.NewGoreDirect(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(10f)).RotatedByRandom(6.283), ModContent.GoreType<Blood>(), Main.rand.NextFloat(0.65f, Main.rand.NextFloat(1.0f, 1.75f)));
			g.timeLeft = Main.rand.Next(150, 200);
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		var c0 = new Color(193, 0, 29, 0);
		float width = 24;
		float MulByTimeLeft = 1f;
		if (Projectile.timeLeft < 500)
			MulByTimeLeft = Projectile.timeLeft / 500f;
		width *= MulByTimeLeft;
		c0 *= MulByTimeLeft;
		int TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			TrueL++;
		}
		DrawFlameTrail(TrueL, width, true, new Color(255, 255, 255, 180));
		DrawFlameTrail(TrueL, width, false, c0);
		return false;
	}
	private void DrawFlameTrail(int TrueL, float width, bool Shade = false, Color c0 = new Color(), float Mulfactor = 1.6f)
	{
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
			Vector2 DrawCenter = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2f;
			Color c2 = Lighting.GetColor((int)DrawCenter.X / 16, (int)DrawCenter.Y / 16);
			MulColor *= (c2.R + c2.G + c2.B) / 765f;
			var factor = i / (float)TrueL;

			float x0 = factor * Mulfactor - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x0 %= 1f;
			factor = MathF.Sin(MathF.Sqrt(factor) * MathF.PI);
			bars.Add(new Vertex2D(DrawCenter + normalDir * -width * factor - Main.screenPosition, c0 * MulColor, new Vector3(x0, 1, 0)));
			bars.Add(new Vertex2D(DrawCenter + normalDir * width * factor - Main.screenPosition, c0 * MulColor, new Vector3(x0, 0, 0)));
			var factorII = i / (float)TrueL;
			factor = (i + 1) / (float)TrueL;
			var x1 = factor * Mulfactor - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x1 %= 1f;
			if (x0 > x1)
			{
				float DeltaValue = 1 - x0;
				var factorIII = factorII * x0 + factor * DeltaValue;
				factorIII = MathF.Sin(MathF.Sqrt(factorIII) * MathF.PI);
				bars.Add(new Vertex2D(DrawCenter + normalDir * -width * factorIII - Main.screenPosition, c0 * MulColor, new Vector3(1, 1, 0)));
				bars.Add(new Vertex2D(DrawCenter + normalDir * width * factorIII - Main.screenPosition, c0 * MulColor, new Vector3(1, 0, 0)));
				bars.Add(new Vertex2D(DrawCenter + normalDir * -width * factorIII - Main.screenPosition, c0 * MulColor, new Vector3(0, 1, 0)));
				bars.Add(new Vertex2D(DrawCenter + normalDir * width * factorIII - Main.screenPosition, c0 * MulColor, new Vector3(0, 0, 0)));
			}
		}
		Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTrace");
		if (Shade)
			t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTraceShade5xDark");
		Main.graphics.GraphicsDevice.Textures[0] = t;

		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		float width = 24;
		float MulByTimeLeft = 1f;
		if (Projectile.timeLeft < 500)
			MulByTimeLeft = Projectile.timeLeft / 500f;
		width *= MulByTimeLeft;
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
			Vector2 DrawCenter = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2f;

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
			Color c0 = new Color(k0, 0.2f, 0, 0) * MulByTimeLeft;
			var factor = i / (float)TrueL;

			float x0 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x0 %= 1f;
			factor = MathF.Sin(MathF.Sqrt(factor) * MathF.PI);
			bars.Add(new Vertex2D(DrawCenter + normalDir * -width * factor - Main.screenPosition, c0 * MulColor, new Vector3(x0, 1, 0)));
			bars.Add(new Vertex2D(DrawCenter + normalDir * width * factor - Main.screenPosition, c0 * MulColor, new Vector3(x0, 0, 0)));
			var factorII = i / (float)TrueL;
			factor = (i + 1) / (float)TrueL;
			var x1 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
			x1 %= 1f;
			if (x0 > x1)
			{
				float DeltaValue = 1 - x0;
				var factorIII = factorII * x0 + factor * DeltaValue;
				factorIII = MathF.Sin(MathF.Sqrt(factorIII) * MathF.PI);
				bars.Add(new Vertex2D(DrawCenter + normalDir * -width * factorIII - Main.screenPosition, c0 * MulColor, new Vector3(1, 1, 0)));
				bars.Add(new Vertex2D(DrawCenter + normalDir * width * factorIII - Main.screenPosition, c0 * MulColor, new Vector3(1, 0, 0)));
				bars.Add(new Vertex2D(DrawCenter + normalDir * -width * factorIII - Main.screenPosition, c0 * MulColor, new Vector3(0, 1, 0)));
				bars.Add(new Vertex2D(DrawCenter + normalDir * width * factorIII - Main.screenPosition, c0 * MulColor, new Vector3(0, 0, 0)));
			}
		}
		Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTraceLight");
		if (bars.Count > 3)
			spriteBatch.Draw(t, bars, PrimitiveType.TriangleStrip);
	}
}
