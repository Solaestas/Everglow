using Everglow.EternalResolve.Items.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Bosses.Projectiles
{
    public class CrutchRapier_Hostile : Rapier_Hostile
    {
        public override void SetDefaults()
        {
            Color = new Color(155, 162, 164);
            base.SetDefaults();
			TradeLength = 4;
			TradeShade = 0.3f;
			Shade = 0.2f;
			FadeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 1.05f;
			DrawWidth = 0.4f;
			itemType = ModContent.ItemType<CrutchBayonet>();
		}

	}
}