using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class RottenGoldBayonet_Mark : ModProjectile
    {
		public override string Texture => "Everglow/EternalResolve/Items/Weapons/StabbingSwords/StabbingProjectile";
		public override void SetDefaults()
        {
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;

		}
		public override void AI()
		{
			Projectile.velocity *= 0;

			if(Projectile.timeLeft > 60)
			{

				for(int i = 0;i < 2; i++)
				{
					Vector2 deltaPos = new Vector2(0, Main.rand.NextFloat(30f, 100f)).RotatedByRandom(6.283);
					Dust dust = Dust.NewDustDirect(Projectile.Center + deltaPos, 0, 0, ModContent.DustType<RottenSmog>());
					dust.color.R = (byte)(Projectile.whoAmI % 256);
					dust.color.G = (byte)((Projectile.whoAmI - dust.color.R) / 256f);
					dust.rotation += Main.rand.NextFloat(6.283f);
					dust.velocity = Vector2.Normalize(deltaPos).RotatedBy(MathF.PI / 2f) * 10;
					dust.scale = 0.1f;
				}
			}
			else if(Projectile.timeLeft == 60)
			{
				Vector2 addPos = new Vector2(0, 120f).RotatedByRandom(6.283);
				for(int i = 0;i < 3; i++)
				{
					Vector2 newAddPos = addPos.RotatedBy(Math.PI / 3f * i * 2);
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + newAddPos, -newAddPos * 0.2f,ModContent.ProjectileType<RottenGoldBayonet_Slash>(),Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}