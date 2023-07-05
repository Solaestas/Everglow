using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class PrisonFireBayonet_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			Color = Color.Orange;
			base.SetDefaults();
			TradeShade = 1f;
			Shade = 0.65f;
			FadeTradeShade = 0.84f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.90f;
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