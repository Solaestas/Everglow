using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class MechanicMosquito_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			Color = new Color(187, 196, 196);
			TradeShade = 0.4f;
			Shade = 0.5f;
			FadeTradeShade = 0.6f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			DrawWidth = 0.4f;
			base.SetDefaults();
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