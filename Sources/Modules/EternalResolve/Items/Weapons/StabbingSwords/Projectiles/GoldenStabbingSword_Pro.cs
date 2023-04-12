namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class GoldenStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			Color = new Color(255, 206, 48);
			TradeLength = 4;
			TradeShade = 0.7f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.88f;
			DrawWidth = 0.4f;
		}
    }
}