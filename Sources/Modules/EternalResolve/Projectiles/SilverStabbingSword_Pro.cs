using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Projectiles
{
	public class SilverStabbingSword_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			AttackColor = new Color(180, 191, 193);
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
	}
}