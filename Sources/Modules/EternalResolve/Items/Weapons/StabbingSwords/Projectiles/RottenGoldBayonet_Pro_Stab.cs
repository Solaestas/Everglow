using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

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
			FadeShade = 0.6f;
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
			if (Main.rand.NextBool(6))
			{
				Vector2 end = Projectile.Center + Projectile.velocity * 80 * MaxLength;
				if (EndPos != Vector2.zeroVector)
				{
					end = EndPos;
				}
				Dust dust = Dust.NewDustDirect(Vector2.Lerp(StartCenter, end, Main.rand.NextFloat(0.3f, 1f)) - new Vector2(4), 0, 0, ModContent.DustType<CorruptShine>(), 0, 0, 0, default, Main.rand.NextFloat(0.85f, 1.2f));
				dust.velocity = new Vector2(0, Main.rand.NextFloat(2f)).RotateRandom(6.283);
			}
		}
	}
}