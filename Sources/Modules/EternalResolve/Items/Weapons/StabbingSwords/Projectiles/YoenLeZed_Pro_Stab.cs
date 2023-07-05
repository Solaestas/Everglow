using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class YoenLeZed_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			Color = new Color(100, 180, 255);
			base.SetDefaults();
			TradeShade = 0.2f;
			Shade = 0.2f;
			FadeTradeShade = 0.74f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.8f;
			MaxLength = 1.15f;
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