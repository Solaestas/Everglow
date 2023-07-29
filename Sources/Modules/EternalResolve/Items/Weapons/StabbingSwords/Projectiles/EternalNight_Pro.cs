using Everglow.Commons.Weapons.StabbingSwords;
namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class EternalNight_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
			Color = new Color(119, 34, 255);
			TradeLength = 8;
			TradeShade = 0.8f;
			Shade = 0.9f;
			FadeTradeShade = 0.7f;
			FadeScale = 0.7f;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			MaxLength = 1.20f;
			DrawWidth = 0.4f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if(Main.rand.NextBool(3))
			{
				Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(),
					Projectile.Center - new Vector2(0, Main.rand.NextFloat(115, 180) * Main.player[Projectile.owner].gravDir).RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f))
					, Vector2.zeroVector, ModContent.ProjectileType<EternalNight_shadow>()
					, Projectile.damage, Projectile.knockBack * 0.6f, Projectile.owner, target.whoAmI);
				p0.timeLeft = 240;

			}
			base.OnHitNPC(target, hit, damageDone);
		}
		public override void AI()
		{
			base.AI();
		}
	}
}