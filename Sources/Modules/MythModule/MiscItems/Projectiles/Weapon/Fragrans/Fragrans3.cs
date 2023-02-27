namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	class Fragrans3 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 116;
            Projectile.height = 116;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((int)(100 * fade), (int)(100 * fade), (int)(100 * fade), 0));
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = player.Center - new Vector2(58);
            if (Projectile.timeLeft < 30)
            {
                fade = (Projectile.timeLeft / 30f) * (Projectile.timeLeft / 30f);
            }
            Projectile.scale += 0.07f;
        }
        float fade = 1;
    }
}
