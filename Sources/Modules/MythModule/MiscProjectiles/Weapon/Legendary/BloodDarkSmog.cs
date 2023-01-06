namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    class BloodDarkSmog : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(0, 0, 0, 0));
        }
        private float Vd = 0;
        private float Sc = 0;
        public override void AI()
        {
            Projectile.rotation += 0.3f;
            if (Projectile.timeLeft > 60)
            {
                Vd = (120 - Projectile.timeLeft) / 60f;
                Sc = (120 - Projectile.timeLeft) / 60f;
            }
            if (Projectile.timeLeft < 60)
            {
                Vd = Projectile.timeLeft / 60f;
                Sc = Projectile.timeLeft / 60f;
            }
            float num2 = Projectile.Center.X;
            float num3 = Projectile.Center.Y;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num5) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num6);
                    if (num7 < 500)
                    {
                        Vector2 v = Main.npc[j].Center - Projectile.Center;
                        v = v / v.Length() * 36f;
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<BloodDark>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                        Projectile.Kill();
                    }
                }
            }
            int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + new Vector2(29, 29).RotatedBy(Projectile.rotation), 4, 4, ModContent.DustType<MiscDusts.BlackSmog>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 9f) * Sc);
            Main.dust[num90].noGravity = true;
            Main.dust[num90].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * Vd * 0.5f;
            if (Main.rand.Next(5) == 1)
            {
                int num91 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + new Vector2(29, 29).RotatedBy(Projectile.rotation), 4, 4, 183, 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 9f) * Sc * 0.4f);
                Main.dust[num91].noGravity = true;
                Main.dust[num91].velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * Vd * 0.5f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player p = Main.player[Projectile.owner];
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/BloodDark").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            for (int k = 1; k < Sc * 15 + 1; k++)
            {
                Vector2 drawPos = Projectile.Center - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY) - new Vector2(29);
                Color color2 = new Color((int)(510 / k * Sc), (int)(510 / k * Sc), (int)(510 / k * Sc), (int)(510 / k * Sc));
                SpriteEffects S = SpriteEffects.None;
                Main.spriteBatch.Draw(t, drawPos, null, color2, Projectile.rotation - k * 0.4f, drawOrigin, 1, S, 0f);
            }
            return false;
        }
    }
}
