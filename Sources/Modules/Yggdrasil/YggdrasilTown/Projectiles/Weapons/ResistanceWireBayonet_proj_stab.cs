using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons
{
	public class ResistanceWireBayonet_proj_stab : StabbingProjectile_Stab
	{
		public float Power = 0;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Color = new Color(71, 89, 99);
			TradeShade = 0.7f;
			Shade = 0.2f;
			FadeShade = 0.44f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.70f;
			DrawWidth = 0.4f;
		}

		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Player player = Main.player[Projectile.owner];
			float distanceValue = (player.Center - target.Center).Length();
			if (Power > 10)
			{
				target.AddBuff(BuffID.OnFire, (int)Power * 10);
			}

			bool hasHit = target.HasBuff(ModContent.BuffType<HasHitByResistenceWireBayonet>());
			float mulDamage = 1f;
			mulDamage += Power * 0.8f / 100f;
			if (!hasHit)
			{
				target.AddBuff(ModContent.BuffType<HasHitByResistenceWireBayonet>(), 360000000);
				mulDamage *= 2.6f;
			}
			modifiers.FinalDamage += Math.Max(0, 180 - distanceValue) / 180f;
			modifiers.FinalDamage *= mulDamage;
			//ResistanceWireBayonet rWB = null;
			//if (player.HeldItem.ModItem is not null)
			//{
			//	rWB = player.HeldItem.ModItem as ResistanceWireBayonet;
			//}
			//if (rWB != null)
			//{
			//	if (rWB.Power > 10)
			//	{
			//		rWB.Power -= 10;
			//	}
			//	else
			//	{
			//		rWB.Power = 0;
			//	}
			//}
			base.ModifyHitNPC(target, ref modifiers);
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			ResistanceWireBayonet rWB = null;
			if (player.HeldItem.ModItem is not null)
			{
				rWB = player.HeldItem.ModItem as ResistanceWireBayonet;
			}
			if (rWB != null)
			{
				Power = rWB.Power;
			}
			base.AI();
		}
	}
}