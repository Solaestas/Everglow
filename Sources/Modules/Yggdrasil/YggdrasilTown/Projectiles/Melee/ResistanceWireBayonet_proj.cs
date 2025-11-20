using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Melee
{
	public class ResistanceWireBayonet_proj : StabbingProjectile
	{
		public float Power = 0;

		public override void SetDefaults()
		{
			AttackColor = new Color(155, 162, 164);
			base.SetDefaults();
			MaxDarkAttackUnitCount = 4;
			OldColorFactor = 0.3f;
			CurrentColorFactor = 0.2f;
			ShadeMultiplicative_Modifier = 0.64f;
			ScaleMultiplicative_Modifier = 1;
			OldLightColorValue = 1f;
			LightColorValueMultiplicative_Modifier = 0.4f;
			AttackLength = 1.05f;
			AttackEffectWidth = 0.4f;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
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
			modifiers.FinalDamage *= mulDamage;
			Player player = Main.player[Projectile.owner];
			ResistanceWireBayonet rWB = null;
			if (player.HeldItem.ModItem is not null)
			{
				rWB = player.HeldItem.ModItem as ResistanceWireBayonet;
			}
			if (rWB != null)
			{
				if (rWB.Power > 10)
				{
					rWB.Power -= 10;
				}
				else
				{
					rWB.Power = 0;
				}
			}
			base.ModifyHitNPC(target, ref modifiers);
		}

		public override void DrawEffect(Color lightColor) => base.DrawEffect(lightColor);

		public override void DrawItem(Color lightColor)
		{
			base.DrawItem(lightColor);
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			// ResistanceWireBayonet rWB = null;
			// if (player.HeldItem.ModItem is not null)
			// {
			// rWB = player.HeldItem.ModItem as ResistanceWireBayonet;
			// }
			// if (rWB != null)
			// {
			// Power = rWB.Power;
			// }
			base.AI();
		}
	}
}