
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles
{
    class AcytaeaArrow2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 720;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        private float K = 10;
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center);
            for (int j = 0; j < 6; j++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(6.28);
                int f = Projectile.NewProjectile(null, Projectile.Center + v * 2f, v, ModContent.ProjectileType<BrokenAcytaea2>(), 0, 1, Main.myPlayer);
                Main.projectile[f].scale = Sca / 100f;
            }
            for (int j = 0; j < 30; j++)
            {
                Vector2 v0 = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(Math.PI * 2);
                int num22 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 8f)).RotatedByRandom(Math.PI * 2), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), v0.X, v0.Y, 0, default, 1.5f * Sca / 100f);
            }
        }
        int Sca = 100;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(Sca / 100f, Sca / 100f, Sca / 100f, 0));
        }
        float ka = 1;
        public override void AI()
        {
            ka = 1;
            if (Projectile.timeLeft < 60f)
            {
                ka = Projectile.timeLeft / 60f;
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.2f / 250f * ka, 0, 0);
            Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
            if (Projectile.timeLeft < 19)
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.timeLeft < 100)
            {
                Sca = Projectile.timeLeft;
            }
            int num22 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 8f)).RotatedByRandom(Math.PI * 2), 2, 2, ModContent.DustType<Dusts.RedEffect2>(), 0, 0, 0, default, 1.5f * Sca / 100f);
            Main.dust[num22].velocity *= 0.2f;
            if (Projectile.ai[0] == 1)
            {
                if (Projectile.timeLeft <= 700)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 vf = new Vector2(-1, 0).RotatedBy(i / 2d * Math.PI);
                        Projectile.NewProjectile(null, Projectile.Center, vf * 14, ModContent.ProjectileType<Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles.AcytaeaArrow2>(), Projectile.damage, 3, player.whoAmI, 3);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 vf = new Vector2(0.3f, 0.3f).RotatedBy(i / 2d * Math.PI);
                        Projectile.NewProjectile(null, Projectile.Center, vf * 14, ModContent.ProjectileType<Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles.AcytaeaArrow2>(), Projectile.damage, 3, player.whoAmI, 2);
                    }
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] == 2)
            {
                if (Projectile.timeLeft <= 708)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        Vector2 vf = Projectile.velocity.RotatedBy(i / 14d * Math.PI);
                        Projectile.NewProjectile(null, Projectile.Center, vf * 0.9f, ModContent.ProjectileType<Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles.AcytaeaArrow2>(), Projectile.damage, 3, player.whoAmI, 4);
                    }
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] == 3)
            {
                if (Projectile.timeLeft <= 712)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        Vector2 vf = Projectile.velocity.RotatedBy(i / 6d * Math.PI);
                        Projectile.NewProjectile(null, Projectile.Center, vf * 0.6f, ModContent.ProjectileType<Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles.AcytaeaArrow2>(), Projectile.damage, 3, player.whoAmI, 5);
                    }
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] == 4)
            {
                if (Projectile.timeLeft <= 705)
                {
                    for (int i = -2; i < 3; i++)
                    {
                        Vector2 vf = Projectile.velocity.RotatedBy(i / 7d * Math.PI);
                        int f = Projectile.NewProjectile(null, Projectile.Center, vf * (10 + i) / 10f, ModContent.ProjectileType<Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles.AcytaeaArrow2>(), Projectile.damage, 3, player.whoAmI, 6);
                        Main.projectile[f].timeLeft = 120;
                    }
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] == 5)
            {
                if (Projectile.timeLeft <= 702)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        if (i != 0)
                        {
                            Vector2 vf = Projectile.velocity.RotatedBy(i / 4.8d * Math.PI);
                            int f = Projectile.NewProjectile(null, Projectile.Center, vf * 2.6f, ModContent.ProjectileType<Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles.AcytaeaArrow2>(), Projectile.damage, 3, player.whoAmI, 7);
                            Main.projectile[f].timeLeft = 120;
                        }
                    }
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] >= 6)
            {
                Projectile.tileCollide = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = ModContent.Request<Texture2D>("MythMod/Projectiles/Acytaea/AcytaeaArrow").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
                Color color = new Color(255, 255, 255, 0);
                float Fad = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color color2 = new Color((int)(color.R * Fad * Fad), (int)(color.G * Fad * Fad), (int)(color.B * Fad), (int)(color.A * Fad));
                if (Projectile.timeLeft < 100)
                {
                    color2.R = (byte)(color2.R * Projectile.timeLeft / 100f);
                    color2.G = (byte)(color2.G * Projectile.timeLeft / 100f);
                    color2.B = (byte)(color2.B * Projectile.timeLeft / 100f);
                }
                Main.spriteBatch.Draw(t, drawPos, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
