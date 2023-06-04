namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class IronStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Color = new Color(120, 108, 96);
            base.SetDefaults();
			TradeLength = 4;
			TradeShade = 0.4f;
			Shade = 0.2f;
			FadeTradeShade = 0.44f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.70f;
			DrawWidth = 0.4f;
		}
    }
}