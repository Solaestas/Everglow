using Everglow.Myth.Common;

namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.LunarFlare
{
	public class LunarFlareHit : ModProjectile, IWarpProjectile, IBloomProjectile
	{
		protected override bool CloneNewInstances => false;
		public override bool IsCloneable => false;

		public override void SetDefaults()
		{
			Projectile.width = 120;
			Projectile.height = 120;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 200;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.95f;

			if (Projectile.timeLeft <= 198)
				Projectile.friendly = false;


			int MaxC = (int)(Projectile.ai[0] / 6 + 5);
			MaxC = Math.Min(26, MaxC);
			if (Projectile.timeLeft >= 200)
			{
				for (int x = 0; x < MaxC; x++)
				{
					SparkVelocity[x] = new Vector2(0, Projectile.ai[0]).RotatedByRandom(6.283) * Main.rand.NextFloat(0.05f, 1.2f);
					SparkOldPos[x, 0] = Projectile.Center;
				}
			}

			for (int x = 0; x < MaxC; x++)
			{
				for (int y = 39; y > 0; y--)
				{
					SparkOldPos[x, y] = SparkOldPos[x, y - 1];
				}
				SparkOldPos[x, 0] += SparkVelocity[x];

				if (SparkVelocity[x].Length() > 0.3f)
					SparkVelocity[x] *= 0.95f;
				SparkVelocity[x].Y += 0.04f;
			}
			Projectile.velocity *= 0;
			Lighting.AddLight(Projectile.Center, 0, Projectile.timeLeft / 100f, Projectile.timeLeft / 100f);
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitLight");
			float Dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
			Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, new Color(0, 255, 255, 0) * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f * Dark, SpriteEffects.None, 0);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHit");
			float Dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
			Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, Color.White * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f, SpriteEffects.None, 0);
			Texture2D light = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitStar");
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 255, 255, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark * Dark) * Projectile.ai[0] / 20f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 255, 255, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, Dark) * Projectile.ai[0] / 20f, SpriteEffects.None, 0);
			float size = Math.Clamp(Projectile.timeLeft / 8f - 10, 0f, 20f);
			if (size > 0)
			{
				DrawSpark(Color.White * 0.5f, size, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkDark"));
				DrawSpark(new Color(0, 255, 255, 0), size, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkLight"));
			}
			return false;
		}

		private Vector2[,] SparkOldPos = new Vector2[27, 40];
		private Vector2[] SparkVelocity = new Vector2[27];
		internal void DrawSpark(Color c0, float width, Texture2D tex)
		{
			int MaxC = (int)(Projectile.ai[0] / 6 + 5);
			MaxC = Math.Min(26, MaxC);
			var bars = new List<Vertex2D>();
			for (int x = 0; x < MaxC; x++)
			{
				int TrueL = 0;
				for (int i = 1; i < 40; ++i)
				{
					if (SparkOldPos[x, i] == Vector2.Zero)
						break;

					TrueL++;
				}
				for (int i = 1; i < 40; ++i)
				{
					if (SparkOldPos[x, i] == Vector2.Zero)
						break;

					var normalDir = SparkOldPos[x, i - 1] - SparkOldPos[x, i];
					normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
					var factor = i / (float)TrueL;
					var w = MathHelper.Lerp(1f, 0.05f, factor);
					float x0 = 1 - factor;
					if (i == 1)
					{
						bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 1, w)));
						bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 0, w)));
					}
					bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
					bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
					if (i == 39)
					{
						bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 1, w)));
						bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 0, w)));
					}
				}
				Texture2D t = tex;
				Main.graphics.GraphicsDevice.Textures[0] = t;
			}
			if (bars.Count > 3)
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
		{
			var circle = new List<Vertex2D>();

			for (int h = 0; h < radious / 2; h += 1)
			{
				circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
			}
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
			if (circle.Count > 2)
				spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
		}
		public void DrawWarp(VFXBatch spriteBatch)
		{
			float value = (200 - Projectile.timeLeft) / 200f;
			float colorV = 0.9f * (1 - value);
			if (Projectile.ai[0] >= 10)
				colorV *= Projectile.ai[0] / 10f;
			Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague");
			float width = 60;
			if (Projectile.timeLeft < 60)
				width = Projectile.timeLeft;

			DrawTexCircle_VFXBatch(spriteBatch, value * 27 * Projectile.ai[0], width, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
		}
		public void DrawBloom()
		{
			float size = Math.Clamp(Projectile.timeLeft / 8f - 60, 0f, 20f);
			if (size > 0)
				DrawSpark(new Color(255, 255, 255, 0), size, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkLight"));
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
		}
	}
}