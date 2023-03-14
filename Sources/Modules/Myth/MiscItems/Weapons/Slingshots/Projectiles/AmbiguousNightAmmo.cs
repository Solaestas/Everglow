using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
	public class AmbiguousNightAmmo : SlingshotAmmo
	{
		public override void SetDef()
		{
		}
		public override void DrawTrail()
		{
			DrawShade();
			List<Vertex2D> bars = new List<Vertex2D>();
			int TrueL = 0;
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
				{
					if (i == 1)
					{
						return;
					}
					break;
				}

				TrueL = i;
			}
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
				{
					break;
				}

				float width = 3;
				if (Projectile.timeLeft <= 30)
				{
					width *= Projectile.timeLeft / 30f;
				}
				var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
				normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);

				var factor = i / (float)TrueL;
				width *= (1 - factor);
				var color = new Color(255, 255, 255, 0);

				float fac1 = factor * 3 + (float)(-Main.timeForVisualEffects * 0.03) + 100000;
				float fac2 = ((i + 1) / (float)TrueL) * 3 + (float)(-Main.timeForVisualEffects * 0.03) + 100000;
				//TODO:925分钟之后会炸

				fac1 %= 1f;
				fac2 %= 1f;
				if (fac1 > fac2)
				{
					bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 0, 0)));
					bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 1, 0)));
					if (i < Projectile.oldPos.Length - 1)
					{
						float fac3 = 1 - fac1;
						fac3 /= 3 / (float)TrueL;
						normalDir = Projectile.oldPos[i] - Projectile.oldPos[i + 1];
						normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);
						bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
						bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 1, 0)));

						bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(0, 0, 0)));
						bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(0, 1, 0)));
					}
				}
				else
				{
					bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 0, 0)));
					bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 1, 0)));
				}
			}

			if (bars.Count > 2)
			{
				Texture2D t = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/ShadowTrail");

				Main.graphics.GraphicsDevice.Textures[0] = t;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
			DrawShade(1);
		}
		private void DrawShade(int times = 0)
		{
			List<Vertex2D> bars = new List<Vertex2D>();
			float TrueL = 0;
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
				{
					if (i == 1)
					{
						return;
					}
					break;
				}

				TrueL = i;
			}
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
				{
					break;
				}

				float width = 12;
				if (Projectile.timeLeft <= 30)
				{
					width *= Projectile.timeLeft / 30f;
				}
				var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
				normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);

				var factor = i / (float)TrueL;
				width *= (1 - factor);
				var color = Color.White;

				float fac1 = factor * 3 + (float)(-Main.timeForVisualEffects * 0.09) + 100000;
				float fac2 = ((i + 1) / (float)TrueL) * 3 + (float)(-Main.timeForVisualEffects * 0.09) + 100000;

				fac1 %= 1f;
				fac2 %= 1f;
				if (fac1 > fac2)
				{
					bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 0, 0)));
					bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 1, 0)));
					if (i < Projectile.oldPos.Length - 1)
					{
						float fac3 = 1 - fac1;
						fac3 /= 3 / (float)TrueL;
						normalDir = Projectile.oldPos[i] - Projectile.oldPos[i + 1];
						normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);
						bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
						bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 1, 0)));

						bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(0, 0, 0)));
						bars.Add(new Vertex2D(Projectile.oldPos[i + 1] * (1 - fac3) + Projectile.oldPos[i] * fac3 + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(0, 1, 0)));
					}
				}
				else
				{
					bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 0, 0)));
					bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(fac1, 1, 0)));
				}
			}
			if (bars.Count > 2)
			{
				Texture2D t = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailBlack");
				if (times == 1)
				{
					t = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/ShadowTrailFlame");
				}
				Main.graphics.GraphicsDevice.Textures[0] = t;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
		}
		public override void AmmoHit()
		{
			SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithVolumeScale(Projectile.ai[0]), Projectile.Center);
			Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NormalHit>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.velocity.Length(), Main.rand.NextFloat(6.283f));
			float Power = Projectile.ai[0] + 0.5f;

			for (int x = 0; x < 100 * Power; x++)
			{
				int index = Dust.NewDust(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, DustID.WaterCandle, 0f, 0f, 0, default, Power * Main.rand.NextFloat(1.7f, 2.3f));
				Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.5f, 4f)).RotatedByRandom(6.283) * Power;

			}
			Projectile.friendly = false;
			TimeTokill = 30;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.ShadowSupervisor>(), (int)(600 * Projectile.ai[0]) + 120);
		}
	}
}
