namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    class ToothMagicHit : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
            Projectile.velocity *= 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        private Effect ef;
        float radious = 0;
        float FirstRo = 0;
        float SecondRo = 0;
        float[] Ro = new float[5];
        float[] Uy = new float[5];
        public override void PostDraw(Color lightColor)
        {
        }
    }
}
