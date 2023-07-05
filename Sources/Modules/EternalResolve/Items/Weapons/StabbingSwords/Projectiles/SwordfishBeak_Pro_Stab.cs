using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.Audio;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class SwordfishBeak_Pro_Stab : StabbingProjectile_Stab
    {
        public override void SetDefaults()
        {
            Color = new Color(117, 134, 243);
            base.SetDefaults();
			TradeShade = 0.3f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.75f;
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