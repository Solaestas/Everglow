using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class BlossomThorn_Pro_Stab : StabbingProjectile_Stab
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			Color = new Color(209, 187, 107);
			TradeShade = 0.7f;
			Shade = 0.2f;
			FadeTradeShade = 0.44f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
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