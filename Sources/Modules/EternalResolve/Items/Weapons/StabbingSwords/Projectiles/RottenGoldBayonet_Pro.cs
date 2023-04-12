using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Buffs;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.Utilities;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class RottenGoldBayonet_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
			Color = new Color(105, 105, 255);
			TradeLength = 8;
			TradeShade = 0.7f;
			Shade = 0.5f;
			FadeTradeShade = 0.6f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			DrawWidth = 0.4f;

		}
		private int SummonProjPreTick = 0;
		public override void AI()
		{
			base.AI();
			Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
			Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f);
			if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
			{
				Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<CorruptShine>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.7f));
				dust.velocity = vel;
			}
			SummonProjPreTick--;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if(SummonProjPreTick <= 0)
			{
				if (!target.HasBuff<LifeRotten>())
				{
					target.buffImmune[ModContent.BuffType<LifeRotten>()] = false;
					target.AddBuff(ModContent.BuffType<LifeRotten>(), 114514 * 60);
					target.defense -= 2;
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Vector2.zeroVector, ModContent.ProjectileType<RottenGoldBayonet_Mark>(), (int)(Projectile.damage * 2.97f), Projectile.knockBack * 2.97f, Projectile.owner);
					SummonProjPreTick += 18;
				}
			}
		}
	}
}