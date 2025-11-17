using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.Example.Projectiles
{
	public class ExampleStabbingSword_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Color = new Color(255, 0, 0);
			TradeLength = 4;
			TradeShade = 0.7f;
			Shade = 0.2f;
			FadeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.70f;
			DrawWidth = 0.4f;
		}

		public override void CustomBehavior()
		{
			float timeValue = (float)(Main.time / 120f);
			Color = Main.hslToRgb(timeValue % 1.0f, 1, 0.5f);
		}
	}
}