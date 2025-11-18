using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class BlossomThorn_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			AttackColor = new Color(209, 187, 107);
			MaxOldAttackUnitCount = 4;
			OldShade = 0.7f;
			Shade = 0.2f;
			ShadeMultiplicative_Modifier = 0.44f;
			ScaleMultiplicative_Modifier = 1;
			OldLightColorValue = 1f;
			LightColorValueMultiplicative_Modifier = 0.4f;
			AttackLength = 0.70f;
			AttackEffectWidth = 0.4f;
		}

		public override void AI()
		{
			base.AI();
		}

		public override void VisualParticle()
		{
			if (Main.rand.NextBool(3))
			{
				Vector2 vel = new Vector2(0, 12f).RotatedByRandom(6.283);
				Vector2 pos = Projectile.Center + Vector2.Normalize(Projectile.velocity) * Main.rand.NextFloat(Main.rand.NextFloat(30f), 120f) - vel * 3f;
				if (!Collision.SolidCollision(pos + vel * 3, 0, 0) && !Collision.SolidCollision(pos + vel * 6, 0, 0) && !Collision.SolidCollision(pos, 0, 0))
				{
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, vel, ModContent.ProjectileType<BlossomThorn_Spike>(), Projectile.damage / 2, 0, Projectile.owner);
				}
			}
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextBool(12))
				{
					Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.2f, 3f);
					Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f)) * Main.rand.NextFloat(0.4f, 0.8f);
					if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
					{
						Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<LeafDust>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.7f));
						dust.velocity = vel;
					}
				}
				else
				{
					Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.2f, 3f);
					Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f)) * Main.rand.NextFloat(0.4f, 0.8f);
					if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
					{
						Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<PlumBlossom>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.7f));
						dust.velocity = vel;
					}
				}
			}
		}

		public override void DrawEffect(Color lightColor)
		{
			Texture2D Shadow = Commons.ModAsset.Star2_black.Value;
			Texture2D light = ModAsset.BlossomThorn_Pro.Value;
			Vector2 drawOrigin = light.Size() / 2f;
			Vector2 drawShadowOrigin = Shadow.Size() / 2f;
			if (OldShade > 0)
			{
				for (int f = MaxOldAttackUnitCount - 1; f > -1; f--)
				{
					Main.spriteBatch.Draw(Shadow, DarkDraw[f].Postion - Main.screenPosition, null, Color.White * (DarkDraw[f].Color.A / 255f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
					Color fadeLight = AttackColor * (DarkDraw[f].Color.A / 255f);
					fadeLight.A = 0;
					fadeLight = fadeLight * OldLightColorValue * MathF.Pow(LightColorValueMultiplicative_Modifier, f);
					fadeLight = new Color(lightColor.R / 255f * fadeLight.R / 255f, lightColor.G / 255f * fadeLight.G / 255f, lightColor.B / 255f * fadeLight.B / 255f, 0);
					Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, fadeLight, DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
					if (GlowColor != Color.Transparent)
					{
						Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, GlowColor * MathF.Pow(FadeGlowColorValue, f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
					}
				}
			}
			if (Shade > 0)
			{
				Main.spriteBatch.Draw(Shadow, LightDraw.Postion - Main.screenPosition, null, Color.White * Shade, LightDraw.Rotation, drawShadowOrigin, LightDraw.Size, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(lightColor.R / 255f * AttackColor.R / 255f, lightColor.G / 255f * AttackColor.G / 255f, lightColor.B / 255f * AttackColor.B / 255f, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
			if (GlowColor != Color.Transparent)
			{
				Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, GlowColor, LightDraw.Rotation, drawShadowOrigin, LightDraw.Size, SpriteEffects.None, 0f);
			}
		}
	}
}