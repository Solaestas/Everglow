using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class BloodGoldBayonet_Pro_Stab : StabbingProjectile_Stab
	{
		public NPC ProjTarget;
		public override void SetDefaults()
		{
			Color = Color.Red;
			TradeShade = 0.7f;
			Shade = 0.5f;
			FadeTradeShade = 0.6f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			DrawWidth = 0.4f;
			base.SetDefaults();
		}
		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}
		public override void AI()
		{
			if(Main.rand.NextBool(6))
			{
				Vector2 end = Projectile.Center + Projectile.velocity * 80 * MaxLength;
				if (EndPos != Vector2.zeroVector)
				{
					end = EndPos;
				}
				Dust dust = Dust.NewDustDirect(Vector2.Lerp(StartCenter, end, Main.rand.NextFloat(0.3f, 1f)) - new Vector2(4), 0, 0, ModContent.DustType<BloodShine>(), 0, 0, 0, default, Main.rand.NextFloat(0.85f, 1.2f));
				dust.velocity = new Vector2(0, Main.rand.NextFloat(2f)).RotateRandom(6.283);
			}

			base.AI();
		}
	}
}