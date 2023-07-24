using Everglow.Commons.Weapons.StabbingSwords;
namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class EternalNight_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
			Color = new Color(119, 34, 255);
			TradeLength = 8;
			TradeShade = 0.8f;
			Shade = 0.9f;
			FadeTradeShade = 0.7f;
			FadeScale = 0.7f;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			MaxLength = 1.20f;
			DrawWidth = 0.4f;
		}
		public override void AI()
		{
			base.AI();
		}
	}
}