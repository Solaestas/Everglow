using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class DreamStar_Pro_Stab : StabbingProjectile_Stab
	{
		internal int ContinuousHit = 0;
		public override void SetDefaults()
		{
			Color = Color.Gold;
			TradeShade = 0.4f;
			Shade = 0.2f;
			FadeTradeShade = 0.74f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.9f;
			MaxLength = 1.05f;
			DrawWidth = 0.4f;
			base.SetDefaults();
		}
	}
}
