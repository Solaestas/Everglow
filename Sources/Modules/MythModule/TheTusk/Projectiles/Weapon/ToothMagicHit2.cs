namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    class ToothMagicHit2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
        }

        Vector3[] CirclePoint = new Vector3[120];
        float Rad = 0;
        Vector2[] Circle2D = new Vector2[120];
        float[] Ro = new float[5];
        float[] Uy = new float[5];
        float[] AdUy = new float[5];
        private Effect ef;
        public override void PostDraw(Color lightColor)
        {
        }
        float[] DeltaRed = new float[5];
    }
}
