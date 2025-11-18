using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Projectiles
{
	public class EternalNight_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			AttackColor = new Color(189, 14, 255);
			MaxOldAttackUnitCount = 8;
			OldShade = 0.8f;
			Shade = 1;
			ShadeMultiplicative_Modifier = 0.7f;
			ScaleMultiplicative_Modifier = 0.7f;
			OldLightColorValue = 0.6f;
			LightColorValueMultiplicative_Modifier = 0.1f;
			AttackLength = 1.20f;
			AttackEffectWidth = 0.8f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(3))
			{
				var p0 = Projectile.NewProjectileDirect(
					Projectile.GetSource_FromAI(),
					Projectile.Center - new Vector2(0, Main.rand.NextFloat(115, 180) * Main.player[Projectile.owner].gravDir).RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)),
					Vector2.zeroVector, ModContent.ProjectileType<EternalNight_shadow>(),
					Projectile.damage, Projectile.knockBack * 0.6f, Projectile.owner, target.whoAmI);
				p0.timeLeft = 240;
			}
			base.OnHitNPC(target, hit, damageDone);
		}

		public override void VisualParticle()
		{
			Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
			Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f);
			if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
			{
				var dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<NightDustMoveWithPlayer>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.2f));
				dust.velocity = vel;
				dust.noGravity = true;
			}
		}

		public override void DrawEffect(Color lightColor)
		{
			Texture2D Shadow = Commons.ModAsset.Star2_black.Value;
			Texture2D light = Commons.ModAsset.StabbingProjectile.Value;
			Vector2 drawOrigin = light.Size() / 2f;
			Vector2 drawShadowOrigin = Shadow.Size() / 2f;

			if (OldShade > 0)
			{
				for (int f = MaxOldAttackUnitCount - 1; f > -1; f--)
				{
					Color fadeLight = AttackColor * (DarkDraw[f].Color.A / 255f);
					fadeLight.A = 0;
					fadeLight = fadeLight * OldLightColorValue * MathF.Pow(LightColorValueMultiplicative_Modifier, f);
					fadeLight = new Color(lightColor.R / 255f * fadeLight.R / 255f, lightColor.G / 255f * fadeLight.G / 255f, lightColor.B / 255f * fadeLight.B / 255f, 0);
					Vector2 lightSize = DarkDraw[f].Size;
					lightSize.Y *= 2;
					Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, fadeLight, DarkDraw[f].Rotation, drawOrigin, lightSize, SpriteEffects.None, 0f);
					Vector2 darkSize = DarkDraw[f].Size;
					darkSize.Y *= 0.4f;
					Main.spriteBatch.Draw(Shadow, DarkDraw[f].Postion - Main.screenPosition, null, Color.White * (DarkDraw[f].Color.A / 255f), DarkDraw[f].Rotation, drawShadowOrigin, darkSize, SpriteEffects.None, 0f);
				}
			}
			Vector2 size = LightDraw.Size;
			size.Y *= 1.1f;
			Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(lightColor.R / 255f * AttackColor.R / 255f, lightColor.G / 255f * AttackColor.G / 255f, lightColor.B / 255f * AttackColor.B / 255f, 0), LightDraw.Rotation, drawOrigin, size, SpriteEffects.None, 0f);
			size = LightDraw.Size;
			size.Y *= 0.4f;
			Main.spriteBatch.Draw(Shadow, LightDraw.Postion - Main.screenPosition, null, Color.White * Shade, LightDraw.Rotation, drawShadowOrigin, size, SpriteEffects.None, 0f);
		}
	}
}