using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.Example.Projectiles
{
	public class ExampleStabbingSword_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
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

		public override void CustomBehavior()
		{
			float timeValue = (float)(Main.time / 120f);
			AttackColor = Main.hslToRgb(timeValue % 1.0f, 1, 0.5f);
		}
	}
}