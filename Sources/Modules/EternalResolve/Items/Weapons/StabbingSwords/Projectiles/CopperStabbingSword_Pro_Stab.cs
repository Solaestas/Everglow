using Everglow.Commons.Weapons.StabbingSwords;
namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class CopperStabbingSword_Pro_Stab : StabbingProjectile_Stab
    {
        public override void SetDefaults()
        {
			base.SetDefaults();
			Color = new Color(200, 101, 24);
			TradeShade = 0.7f;
			Shade = 2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.88f;
			DrawWidth = 0.4f;
		}
    }
}