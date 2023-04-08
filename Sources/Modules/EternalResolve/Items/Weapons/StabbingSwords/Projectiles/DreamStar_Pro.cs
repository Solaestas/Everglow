namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class DreamStar_Pro : StabbingProjectile
    {
        public override int SoundTimer => 10;
        public override void SetDefaults()
        {
            Color = Color.Gold;
			TradeLength = 4;
			TradeShade = 0.3f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 1.05f;
			base.SetDefaults();
        }
    }
}
