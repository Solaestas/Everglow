using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.Audio;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class SwordfishBeak_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			AttackColor = new Color(117, 134, 243);
			base.SetDefaults();
			MaxOldAttackUnitCount = 4;
			OldShade = 0.3f;
			Shade = 0.2f;
			ShadeMultiplicative_Modifier = 0.64f;
			ScaleMultiplicative_Modifier = 1;
			OldLightColorValue = 1f;
			LightColorValueMultiplicative_Modifier = 0.4f;
			AttackLength = 0.75f;
			AttackEffectWidth = 0.4f;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (!target.wet)
			{
				modifiers.FinalDamage.Multiplicative *= 1.4f;
			}
			else
			{
				modifiers.FinalDamage.Multiplicative *= 0.6f;
				modifiers.CritDamage *= 1.4f;
				modifiers.ArmorPenetration += 4;
			}
		}

		public override void VisualParticle()
		{
			Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
			Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.24f);
			if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
			{
				Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<SeaWater>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.7f));
				dust.velocity = vel;
			}
			Player player = Main.player[Projectile.owner];
			if (player.wet)
			{
				AttackLength = 1.125f;
			}
			else
			{
				AttackLength = 0.75f;
			}
		}

		public override void HitTileSound(float scale)
		{
			SoundEngine.PlaySound(SoundID.Dig.WithVolume(1 - scale / 2.42f).WithPitchOffset(Main.rand.NextFloat(0.6f, 1f)), Projectile.Center);
			Projectile.soundDelay = SoundTimer;
		}
	}
}