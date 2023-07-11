using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class RottenGoldBayonet_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Color = new Color(105, 105, 255);
			TradeShade = 0.7f;
			Shade = 0.5f;
			FadeTradeShade = 0.6f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.1f;
			DrawWidth = 0.4f;
			spAttCounts = 2;

		}
		public int spAttCounts = 2;//一次攻击最多触发两次特殊
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			
			if(spAttCounts>0)
				Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Vector2.zeroVector, ModContent.ProjectileType<RottenGoldBayonet_Mark>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack * 2.97f, Projectile.owner,1,target.whoAmI);
            spAttCounts--;
            base.OnHitNPC(target, hit, damageDone);
		}
		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}
		public override void AI()
		{
			base.AI();
		}
	}
}