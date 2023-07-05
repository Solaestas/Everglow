using Everglow.Commons.Weapons.StabbingSwords;
namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class HolyBayonet_Pro_Stab : StabbingProjectile_Stab
    {
        public override void SetDefaults()
        {
            Color = new Color(243, 175, 105);
            base.SetDefaults();
			TradeShade = 0.3f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 1.25f;
			DrawWidth = 0.4f;
		}
	}
}