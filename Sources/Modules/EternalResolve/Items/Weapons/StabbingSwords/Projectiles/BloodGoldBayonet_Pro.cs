namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class BloodGoldBayonet_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Color = Color.Red;
			TradeLength = 8;
			TradeShade = 0.7f;
			Shade = 0.5f;
			FadeTradeShade = 0.6f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			base.SetDefaults();
		}
    }
}