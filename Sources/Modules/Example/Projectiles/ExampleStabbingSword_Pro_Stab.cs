using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.Example.Projectiles
{
	public class ExampleStabbingSword_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Color = new Color(80, 34, 5);
			TradeShade = 0.7f;
			Shade = 2f;
			FadeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.70f;
			DrawWidth = 0.4f;
		}
	}
}