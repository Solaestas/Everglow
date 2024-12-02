using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

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
			FadeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 1.25f;
			DrawWidth = 0.4f;
		}
		public override void AI()
		{
			if(Main.rand.NextBool(8))
			{
				Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 48f);
				Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f);
				if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
				{
					Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<HolyDust>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.2f));
					dust.velocity = vel;
					dust.noGravity= true;
				}
			}
			base.AI();
		}
		private int hitNPCCount = 0;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			hitNPCCount++;
			if(hitNPCCount > 3)
			{
				return;
			}
			Vector2 addPos = new Vector2(0, 120f).RotatedByRandom(6.283);
			Vector2 newAddPos = addPos.RotatedBy(Math.PI / 2f);
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center + newAddPos * 0.9f, -newAddPos * 0.14f, ModContent.ProjectileType<HolyBayonet_Slash>(), Projectile.damage / 6, Projectile.knockBack, Projectile.owner);
			newAddPos = addPos;
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center + newAddPos * 1.2f, -newAddPos * 0.3f, ModContent.ProjectileType<HolyBayonet_Slash>(), Projectile.damage / 6, Projectile.knockBack, Projectile.owner);
			base.OnHitNPC(target, hit, damageDone);
		}
	}
}