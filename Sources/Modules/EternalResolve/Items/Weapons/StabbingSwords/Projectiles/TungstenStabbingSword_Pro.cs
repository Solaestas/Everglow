using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class TungstenStabbingSword_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			AttackColor = new Color(173, 218, 175);
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