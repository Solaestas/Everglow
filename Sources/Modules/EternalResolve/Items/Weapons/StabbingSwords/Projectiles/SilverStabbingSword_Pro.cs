using Everglow.Commons.Weapons.StabbingSwords;
namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class SilverStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Color = new Color(180, 191, 193);
            base.SetDefaults();
			TradeLength = 4;
			TradeShade = 0.3f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.75f;
			DrawWidth = 0.4f;
		}
    }
}