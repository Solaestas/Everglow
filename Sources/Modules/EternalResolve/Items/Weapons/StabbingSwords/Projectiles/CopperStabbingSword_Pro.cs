using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class CopperStabbingSword_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			AttackColor = new Color(200, 101, 24);
			MaxOldAttackUnitCount = 4;
			OldShade = 0.7f;
			Shade = 0.2f;
			ShadeMultiplicative_Modifier = 0.64f;
			ScaleMultiplicative_Modifier = 1;
			OldLightColorValue = 1f;
			LightColorValueMultiplicative_Modifier = 0.4f;
			AttackLength = 0.70f;
			AttackEffectWidth = 0.4f;
		}
	}
}