namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    class RainArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.alpha = 255;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        int Ran = -1;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
        }
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
            if (Ran == -1)
            {
                Ran = Main.rand.Next(9);
            }
            int addi = 90 - Projectile.timeLeft;
            if (Projectile.timeLeft < 40)
            {
                addi = Projectile.timeLeft;
            }
            int Alp = 255 - addi * 3;
            if (Alp < 150)
            {
                Alp = 150;
            }
            Projectile.alpha = Alp;
            if (Projectile.ai[0] >= 1)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * (255 - Projectile.alpha) / 100f);
            }
            if (Projectile.timeLeft % 9 == Ran)
            {
                if (Projectile.ai[0] >= 1)
                {
                    Vector2 v = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
                    int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, Projectile.type, Projectile.damage * 2 / 3, Projectile.knockBack * 2 / 3f, Projectile.owner, Projectile.ai[0] - 1, 0f);
                    Main.projectile[h].timeLeft = Projectile.timeLeft;
                }
            }
            if (Projectile.ai[0] < 1)
            {
                Projectile.velocity.Y += 0.35f;
                Projectile.velocity *= 0.99f;
            }
            else
            {
                Projectile.velocity.Y += 0.15f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
            }
            for (int j = 0; j < 4; j++)
            {
                int num20 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default(Color), 1f);
            }
        }
    }
}
