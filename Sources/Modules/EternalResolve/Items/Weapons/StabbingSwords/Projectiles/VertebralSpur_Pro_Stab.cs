using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class VertebralSpur_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Color = new Color(105, 92, 76);
			TradeShade = 0.7f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.70f;
			DrawWidth = 0.4f;
		}
		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}
		public override void AI()
		{
			base.AI();
		}
	}
}