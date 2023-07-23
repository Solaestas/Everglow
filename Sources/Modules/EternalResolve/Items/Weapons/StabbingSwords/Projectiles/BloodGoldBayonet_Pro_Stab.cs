using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Buffs;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Buffs;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class BloodGoldBayonet_Pro_Stab : StabbingProjectile_Stab
	{
		public NPC ProjTarget;
		public override void SetDefaults()
		{
			Color = Color.Red;
			TradeShade = 0.7f;
			Shade = 0.5f;
			FadeTradeShade = 0.6f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			DrawWidth = 0.4f;
			base.SetDefaults();
		}
		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}
		public override void AI()
		{
			if(Main.rand.NextBool(6))
			{
				Vector2 end = Projectile.Center + Projectile.velocity * 80 * MaxLength;
				if (EndPos != Vector2.zeroVector)
				{
					end = EndPos;
				}
				Dust dust = Dust.NewDustDirect(Vector2.Lerp(StartCenter, end, Main.rand.NextFloat(0.3f, 1f)) - new Vector2(4), 0, 0, ModContent.DustType<BloodShine>(), 0, 0, 0, default, Main.rand.NextFloat(0.85f, 1.2f));
				dust.velocity = new Vector2(0, Main.rand.NextFloat(2f)).RotateRandom(6.283);
			}

			base.AI();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if(target.type != NPCID.TargetDummy)
			{
				target.AddBuff(ModContent.BuffType<BloodDrinking>(), 180);
			}
			base.OnHitNPC(target, hit, damageDone);
		}
	}
	public class BloodDrinkingTarget : GlobalNPC
	{
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			base.PostDraw(npc, spriteBatch, screenPos, drawColor);
			if(npc.HasBuff(ModContent.BuffType<BloodDrinking>()))
			{
				float light = 0;
				float dark = 0;
				for (int t = 0; t < 5; t++)
				{
					if (npc.buffType[t] == ModContent.BuffType<OnMoth>())
					{
						light = Math.Clamp((npc.buffTime[t] - 280) / 20f, 0, 1);
						dark = Math.Clamp(npc.buffTime[t] / 120f, 0, 0.3f) * 0.5f;
						break;
					}
				}
				Texture2D bloodMark = ModAsset.BloodDrinkingMark.Value;
				Texture2D bloodMarkBlack = ModAsset.BloodDrinkingMark_dark.Value;
				if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
				{
					spriteBatch.Draw(bloodMark, npc.Center - Main.screenPosition, null, new Color(light, light, light, 0), 0, bloodMark.Size(), 3f, SpriteEffects.None, 0f);
					spriteBatch.Draw(bloodMarkBlack, npc.Center - Main.screenPosition, null, new Color(dark, dark, dark, 0), 0, bloodMark.Size(), 3f, SpriteEffects.None, 0f);
				}
				else
				{
					spriteBatch.Draw(bloodMark, npc.Center - Main.screenPosition, null, new Color(light, light, light, 0), 0, bloodMark.Size(), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
					spriteBatch.Draw(bloodMarkBlack, npc.Center - Main.screenPosition, null, new Color(dark, dark, dark, 0), 0, bloodMark.Size(), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
				}
			}
		}
	}
}