using Everglow.Commons.Weapons.StabbingSwords;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class EternalNight_Pro_Stab : StabbingProjectile_Stab
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
			Color = new Color(119, 34, 255);
			TradeShade = 0.8f;
			Shade = 0.9f;
			FadeTradeShade = 0.7f;
			FadeScale = 0.7f;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			MaxLength = 1.20f;
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int k = 0; k < 3; k++)
			{
				Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(),
					target.Center - new Vector2(0, Main.rand.NextFloat(115, 180) * Main.player[Projectile.owner].gravDir).RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f))
					, Vector2.zeroVector, ModContent.ProjectileType<EternalNight_shadow>()
					, Projectile.damage / 3, Projectile.knockBack * 0.6f, Projectile.owner, target.whoAmI);
				p0.timeLeft = 240 + k * 5;

			}
			base.OnHitNPC(target, hit, damageDone);
		}
		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			for (int k = 0; k < 7; k++)
			{
				Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(),
					Projectile.Center + Projectile.velocity * k * 25f - new Vector2(0, Main.rand.NextFloat(80, 180) * Main.player[Projectile.owner].gravDir).RotatedBy(Main.rand.NextFloat(-0.7f, 0.7f))
					, Vector2.zeroVector, ModContent.ProjectileType<EternalNight_shadow>()
					, Projectile.damage / 3, Projectile.knockBack * 0.6f, Projectile.owner, -1);
				p0.timeLeft = 240 + k * 4;
				p0.rotation = MathF.PI * 0.75f + Main.rand.NextFloat(-1f, 1f);
			}
		}
	}
}