using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class EnchantedBayonet_Pro_Stab : StabbingProjectile_Stab
	{
        public override void SetDefaults()
        {
			base.SetDefaults();
			Color = new Color(76, 126, 255);
			TradeShade = 0.8f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.5f;
			MaxLength = 0.88f;
			DrawWidth = 0.4f;
		}
	}
}