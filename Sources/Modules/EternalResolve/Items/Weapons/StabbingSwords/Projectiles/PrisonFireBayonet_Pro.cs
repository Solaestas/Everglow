using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class PrisonFireBayonet_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Color = Color.Orange;
			base.SetDefaults();
			TradeLength = 6;
			TradeShade = 1f;
			Shade = 0.65f;
			FadeTradeShade = 0.84f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.90f;
			DrawWidth = 0.4f;
		}
		public int SuddenCooling = 0;
		public override void AI()
		{
			base.AI();
			Player player = Main.player[Projectile.owner];
			if (!player.wet || player.lavaWet)
			{
				for (int x = 0; x < 4; x++)
				{
					Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 6f);
					Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.2f, 0.4f);
					if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
					{
						Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<FlameShine>(), 0, 0, 0, default, Main.rand.NextFloat(0.35f, 0.6f));
						dust.velocity = vel;
					}
				}
				if (SuddenCooling > 0)
				{
					for (int x = 0; x < 4; x++)
					{
						Vector2 posII = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
						Vector2 velII = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f) + new Vector2(Main.rand.NextFloat(-1f, 1f), -SuddenCooling / 12f);
						if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, posII + velII, 0, 0))
						{
							Dust dust = Dust.NewDustDirect(posII, Projectile.width, Projectile.height, DustID.Smoke, 0, 0, 180, default, Main.rand.NextFloat(0.95f, 3.7f));
							dust.velocity = velII;
						}
					}
					SuddenCooling -= 1;
				}
				else
				{
					SuddenCooling = 0;
				}
			}
			else
			{
				Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
				Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f);
				if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
				{
					Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, DustID.Smoke, 0, 0, 200, default, Main.rand.NextFloat(0.95f, 1.7f));
					dust.velocity = vel;
				}
				if (SuddenCooling < 60)
				{
					SuddenCooling += 1;
					for (int x = 0; x < 4; x++)
					{
						Vector2 posII = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
						Vector2 velII = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f) + new Vector2(Main.rand.NextFloat(-1f, 1f), (SuddenCooling - 60f) / 12f);
						if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, posII + velII, 0, 0))
						{
							Dust dust = Dust.NewDustDirect(posII, Projectile.width, Projectile.height, DustID.Smoke, 0, 0, 180, default, Main.rand.NextFloat(0.95f, 3.7f));
							dust.velocity = velII;
						}
					}
				}
				else
				{
					SuddenCooling = 60;
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			if (!player.wet || player.lavaWet)
			{
				target.AddBuff(BuffID.OnFire, 150);
			}
        }
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			base.PostDraw(lightColor);
			Lighting.AddLight(Projectile.Center, 1f * Projectile.timeLeft / TradeLength, 0.4f * Projectile.timeLeft / TradeLength, 0f);
			Texture2D light = ModAsset.StabbingProjectile.Value;
			Vector2 drawOrigin = light.Size() / 2f;
			if (Main.myPlayer == Projectile.owner)
			{
				if (player.channel && !player.noItems && !player.CCed)
				{
					if(!player.wet || player.lavaWet)
					{
						Color = Color.Orange;
						TradeLength = 6;
						TradeShade = 1f;
						Shade = 0.65f;
						FadeTradeShade = 0.84f;
						FadeScale = 1;
						TradeLightColorValue = 1f;
						FadeLightColorValue = 0.4f;
						MaxLength = 0.90f;
						DrawWidth = 0.6f;

						for (int f = Projectile.timeLeft - 1; f > -1; f--)
						{
							float value = (TradeLength - f) / (float)TradeLength;
							Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, new Color(value, value * value * value * 0.5f, 0, 0), DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
						}
						Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(255, 155, 0, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
					}
					else
					{
						Color = new Color(80, 0, 0, 0);
						TradeLength = 6;
						TradeShade = 0.4f;
						Shade = 0.65f;
						FadeTradeShade = 0.74f;
						FadeScale = 1;
						TradeLightColorValue = 1f;
						FadeLightColorValue = 0.4f;
						MaxLength = 0.90f;
						DrawWidth = 0.6f;
						for (int f = Projectile.timeLeft - 1; f > -1; f--)
						{
							float value = (TradeLength - f) / (float)TradeLength;
							value *= value;
							Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, new Color(value * 0.1f, 0, 0, 0), DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
						}
						Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(20, 0, 0, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
					}
				}
			}
		}
	}
}