using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Commons.Vertex;
using Everglow.EternalResolve.Items.Miscs;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class DreamStar_Pro : StabbingProjectile
	{
		NPC LastHitTarget = null;
		NPC StarNPC = null;
		internal int ContinuousHit = 0;
		public override int SoundTimer => 10;
		public override void SetDefaults()
		{
			Color = Color.Gold;
			TradeLength = 6;
			TradeShade = 0.4f;
			Shade = 0.2f;
			FadeShade = 0.74f;
			FadeScale = 1;
			TradeLightColorValue = 1f;

			FadeLightColorValue = 0.9f;
			MaxLength = 1.05f;
			DrawWidth = 0.4f;
			base.SetDefaults();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (LastHitTarget != null)
			{
				if (LastHitTarget == target)
				{
					if (StarNPC != target)
					{
						ContinuousHit++;
						if (ContinuousHit >= 5)
						{
							if (StarNPC != null)
							{
								Projectile.NewProjectile(Projectile.GetSource_FromAI(), StarNPC.Center, Vector2.zeroVector, ModContent.ProjectileType<DreamStar_Explosion>(), (int)(Projectile.damage * 4.26), Projectile.knockBack * 4.26f, Projectile.owner, 1.2f);
								Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.zeroVector, ModContent.ProjectileType<DreamStar_Explosion>(), (int)(Projectile.damage * 4.26), Projectile.knockBack * 4.26f, Projectile.owner, 1.2f);
							}
							StarNPC = target;
							ContinuousHit = 0;
						}
					}
				}
				else
				{
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), LastHitTarget.Center, Vector2.zeroVector, ModContent.ProjectileType<DreamStar_Explosion>(), (int)(Projectile.damage * 2.20 * (ContinuousHit / 5f + 0.00f)), Projectile.knockBack * 2.20f * (ContinuousHit / 5f + 0.05f), Projectile.owner, ContinuousHit / 5f + 0.05f);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.zeroVector, ModContent.ProjectileType<DreamStar_Explosion>(), (int)(Projectile.damage * 2.20 * (ContinuousHit / 5f + 0.00f)), Projectile.knockBack * 2.20f * (ContinuousHit / 5f + 0.05f), Projectile.owner, ContinuousHit / 5f + 0.05f);
					LastHitTarget = target;
					ContinuousHit = 0;
				}
			}
			else
			{
				LastHitTarget = target;
			}
		}
		public override void VisualParticle()
		{
			Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
			Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.1f, 0.2f);
			if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
			{
				int type = ModContent.DustType<StarShine_yellow>();
				if (Main.rand.NextBool(3))
				{
					type = ModContent.DustType<StarShine_purple>();
				}
				Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, type, 0, 0, 0, default, Main.rand.NextFloat(0.75f, 1.25f));
				dust.velocity = vel;
			}
			if (StarNPC != null)
			{
				if (!StarNPC.active)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), StarNPC.Center, Vector2.zeroVector, ModContent.ProjectileType<DreamStar_Explosion>(), (int)(Projectile.damage * 4.26), Projectile.knockBack * 4.26f);
					Item.NewItem(StarNPC.GetSource_Death(), StarNPC.Hitbox, ModContent.ItemType<StarSeed>());
					StarNPC = null;
				}
			}
			if (LastHitTarget != null)
			{
				if (!LastHitTarget.active)
				{
					LastHitTarget = null;
				}
			}
		}
		float bottomPos1 = 0f;
		float bottomPos2 = 0f;
		public override void DrawItem(Color lightColor)
		{
			if (!Main.gamePaused)
			{
				bottomPos1 = bottomPos1 * 0.8f + Main.rand.NextFloat(0.45f, 1.75f) * 0.2f;
				bottomPos2 = bottomPos2 * 0.8f + Main.rand.NextFloat(0.45f, 1.75f) * 0.2f;
			}
			else
			{
				//暂停的时候可以有一个渐停效果，看起来很好
				bottomPos1 = bottomPos1 * 0.9f;
				bottomPos2 = bottomPos2 * 0.9f;
			}
			float scale = MathF.Sin((float)Main.timeForVisualEffects);
			DrawFlags(lightColor, -8, 10, ModAsset.DreamStar_flag.Value, bottomPos1 * scale, bottomPos2 * scale);

			Texture2D itemTexture = ModAsset.DreamStar_withouFlag.Value;
			Main.spriteBatch.Draw(itemTexture, ItemDraw.Postion - Main.screenPosition, null, lightColor, ItemDraw.Rotation, itemTexture.Size() / 2f, ItemDraw.Size, ItemDraw.SpriteEffect, 0f);
			Main.spriteBatch.Draw(ModAsset.DreamStar_glow.Value, ItemDraw.Postion - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), ItemDraw.Rotation, itemTexture.Size() / 2f, ItemDraw.Size, ItemDraw.SpriteEffect, 0f);
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D Shadow = Commons.ModAsset.StabbingProjectileShade.Value;
			Texture2D light = Commons.ModAsset.StabbingProjectile.Value;
			Vector2 drawOrigin = light.Size() / 2f;
			Vector2 drawShadowOrigin = Shadow.Size() / 2f;
			DrawItem(lightColor);
			if (TradeShade > 0)
			{
				for (int f = TradeLength - 1; f > -1; f--)
				{
					Main.spriteBatch.Draw(Shadow, DarkDraw[f].Postion - Main.screenPosition, null, Color.White * (DarkDraw[f].Color.A / 255f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
					Color fadeLight = new Color(230, 120, 195) * (DarkDraw[f].Color.A / 255f);
					fadeLight.A = 0;
					fadeLight = fadeLight * TradeLightColorValue * MathF.Pow(FadeLightColorValue, f);
					if (fadeLight.G > 20)
					{
						fadeLight.G -= 20;
					}
					else
					{
						fadeLight.G = 0;
					}
					fadeLight = new Color(lightColor.R / 255f * fadeLight.R / 255f, lightColor.G / 255f * fadeLight.G / 255f, lightColor.B / 255f * fadeLight.B / 255f, 0);
					Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, fadeLight, DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
				}
			}
			if (Shade > 0)
			{
				Main.spriteBatch.Draw(Shadow, LightDraw.Postion - Main.screenPosition, null, Color.White * Shade, LightDraw.Rotation, drawShadowOrigin, LightDraw.Size, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(Color.R / 255f, Color.G / 255f, lightColor.B / 255f * Color.B / 255f, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			float drawTimer = (float)Main.time * 0.04f;
			if (StarNPC != null)
			{
				float starSize = MathF.Sqrt(StarNPC.width * StarNPC.height) * 0.8f;
				Vector2 drawStarCenter = StarNPC.Center - Main.screenPosition;
				Color drawColor = Color.Yellow;
				drawColor.A = 0;
				List<Vertex2D> bars = new List<Vertex2D>();
				for (int x = 0; x < 5; x++)
				{
					Vector2 bump = new Vector2(0, starSize).RotatedBy(x / 5.0 * Math.PI * 2 + drawTimer * -0.4);
					bars.Add(new Vertex2D(drawStarCenter + bump, drawColor, new Vector3(x / 5.0f + drawTimer, 0, 0)));
					bars.Add(new Vertex2D(drawStarCenter + bump * 0.1f, drawColor, new Vector3(x / 5.0f + drawTimer, 1, 0)));
					Vector2 dent = new Vector2(0, starSize * 0.5f).RotatedBy((x + 0.5) / 5.0 * Math.PI * 2 + drawTimer * -0.4);
					bars.Add(new Vertex2D(drawStarCenter + dent, drawColor, new Vector3((x + 0.5f) / 5.0f + drawTimer, 0, 0)));
					bars.Add(new Vertex2D(drawStarCenter + dent * 0.1f, drawColor, new Vector3((x + 0.5f) / 5.0f + drawTimer, 1, 0)));
					if (x == 4)
					{
						bump = new Vector2(0, starSize).RotatedBy((x + 1) / 5.0 * Math.PI * 2 + drawTimer * -0.4);
						bars.Add(new Vertex2D(drawStarCenter + bump, drawColor, new Vector3((x + 1) / 5.0f + drawTimer, 0, 0)));
						bars.Add(new Vertex2D(drawStarCenter + bump * 0.1f, drawColor, new Vector3((x + 1) / 5.0f + drawTimer, 1, 0)));
					}
				}
				Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.StabbingProjectile.Value;
				if (bars.Count > 3)
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
			if (LastHitTarget != null)
			{
				float lastHitSize = MathF.Sqrt(LastHitTarget.width * LastHitTarget.height) * 0.8f;
				Vector2 drawTargetCenter = LastHitTarget.Center - Main.screenPosition;
				Color drawColor = Color.Yellow;
				drawColor.A = 0;
				List<Vertex2D> bars = new List<Vertex2D>();
				for (int x = 0; x < ContinuousHit; x++)
				{
					Vector2 bump = new Vector2(0, lastHitSize).RotatedBy(x / 5.0 * Math.PI * 2 + drawTimer * -0.4);
					bars.Add(new Vertex2D(drawTargetCenter + bump, drawColor, new Vector3(x / 5.0f + drawTimer, 0, 0)));
					bars.Add(new Vertex2D(drawTargetCenter + bump * 0.1f, drawColor, new Vector3(x / 5.0f + drawTimer, 1, 0)));
					Vector2 dent = new Vector2(0, lastHitSize * 0.5f).RotatedBy((x + 0.5) / 5.0 * Math.PI * 2 + drawTimer * -0.4);
					bars.Add(new Vertex2D(drawTargetCenter + dent, drawColor, new Vector3((x + 0.5f) / 5.0f + drawTimer, 0, 0)));
					bars.Add(new Vertex2D(drawTargetCenter + dent * 0.1f, drawColor, new Vector3((x + 0.5f) / 5.0f + drawTimer, 1, 0)));
				}
				Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.StabbingProjectile.Value;
				if (bars.Count > 3)
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
	}
}
