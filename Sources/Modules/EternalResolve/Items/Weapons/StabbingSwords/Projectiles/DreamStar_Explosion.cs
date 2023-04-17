using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class DreamStar_Explosion : ModProjectile
    {
		public override string Texture => "Everglow/EternalResolve/Items/Weapons/StabbingSwords/StabbingProjectile";
		public override void SetDefaults()
        {
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 5;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
			Projectile.width = 60;
			Projectile.height = 60;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return base.Colliding(projHitbox, targetHitbox);
		}
		public override void OnSpawn(IEntitySource source)
		{
			for (int x = 0; x < 80 * Projectile.ai[0]; x++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StarShine_purple_withoutPlayer>());
				dust.scale = Main.rand.NextFloat(0.65f, 1.35f) * Projectile.ai[0];
				dust.velocity = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 4f), 4f)).RotatedByRandom(6.283) * Projectile.ai[0];
			}
			for (int x = 0; x < 30 * Projectile.ai[0]; x++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StarShine_yellow_withoutPlayer>());
				dust.scale = Main.rand.NextFloat(1f, 1.75f) * Projectile.ai[0];
				dust.velocity = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 7f), 7f)).RotatedByRandom(6.283) * Projectile.ai[0];
			}
		}
		public override void AI()
		{
			Projectile.velocity *= 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}