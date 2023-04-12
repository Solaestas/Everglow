namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class TinStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			Color = new Color(196, 176, 117);
			TradeLength = 4;
			TradeShade = 0.7f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.70f;
			DrawWidth = 0.4f;
		}
    }
}