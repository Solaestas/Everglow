using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class CrutchBayonet_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			AttackColor = new Color(155, 162, 164);
			base.SetDefaults();
			MaxOldAttackUnitCount = 4;
			OldShade = 0.3f;
			Shade = 0.2f;
			ShadeMultiplicative_Modifier = 0.64f;
			ScaleMultiplicative_Modifier = 1;
			OldLightColorValue = 1f;
			LightColorValueMultiplicative_Modifier = 0.4f;
			AttackLength = 1.05f;
			AttackEffectWidth = 0.4f;
		}

		internal bool FirstHit = false;

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (!FirstHit)
			{
				modifiers.CritDamage.Multiplicative *= 2.7f;
				FirstHit = true;
			}
			base.ModifyHitNPC(target, ref modifiers);
		}
	}
}