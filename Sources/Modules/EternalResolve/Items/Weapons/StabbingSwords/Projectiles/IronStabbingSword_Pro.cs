using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class IronStabbingSword_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			AttackColor = new Color(120, 108, 96);
			base.SetDefaults();
			MaxOldAttackUnitCount = 4;
			OldShade = 0.4f;
			Shade = 0.2f;
			ShadeMultiplicative_Modifier = 0.44f;
			ScaleMultiplicative_Modifier = 1;
			OldLightColorValue = 0.6f;
			LightColorValueMultiplicative_Modifier = 0.4f;
			AttackLength = 0.70f;
			AttackEffectWidth = 0.4f;
		}
	}
}