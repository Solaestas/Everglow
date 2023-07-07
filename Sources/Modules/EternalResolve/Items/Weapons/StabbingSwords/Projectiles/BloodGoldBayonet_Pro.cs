using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class BloodGoldBayonet_Pro : StabbingProjectile
    {
		public NPC ProjTarget;
        public override void SetDefaults()
        {
            Color = Color.Red;
			TradeLength = 8;
			TradeShade = 0.7f;
			Shade = 0.5f;
			FadeTradeShade = 0.6f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			DrawWidth = 0.4f;
			base.SetDefaults();
		}
		public override void AI()
		{
			base.AI();
			Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
			Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f);
			if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
			{
				Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<BloodShine>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.7f));
				dust.velocity = vel;
			}
			Player player = Main.player[Projectile.owner];
			if (ProjTarget == null)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.BloodSwordStrikeBuff>());
			}
			else if(!ProjTarget.active)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.BloodSwordStrikeBuff>());
			}
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			player.ClearBuff(ModContent.BuffType<Buffs.BloodSwordStrikeBuff>());
			base.Kill(timeLeft);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(25))
			{
				if (target.type != NPCID.TargetDummy)
				{
					Projectile.NewProjectile(Projectile.GetItemSource_OnHit(target, ModContent.ItemType<BloodGoldBayonet>()), target.Center, Vector2.Zero, ProjectileID.VampireHeal, 5, 0, Projectile.owner, Projectile.owner, damageDone * 0.3f);
				}
			}
			ProjTarget = target;
			Player player = Main.player[Projectile.owner];
			if(target.life <= 0 || damageDone >= target.life)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.BloodSwordStrikeBuff>());
			}
			else
			{
				player.AddBuff(ModContent.BuffType<Buffs.BloodSwordStrikeBuff>(), 114514 * 60);
			}
		}
	}
}