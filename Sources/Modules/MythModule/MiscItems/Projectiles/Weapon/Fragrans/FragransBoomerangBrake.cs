using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	public class FragransBoomerangBrake : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Boomerang Brake");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "旋云舞月");
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 3600;
            Projectile.hostile = false;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void AI()
        {
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.03f, 0, 0.18f);
            if (Tokill >= 0 && Tokill <= 2)
            {
                Projectile.Kill();
            }
            Player player = Main.player[Projectile.owner];
            if (Tokill <= 44 && Tokill > 0)
            {
                Projectile.position = Projectile.oldPosition;
                Projectile.velocity = Projectile.oldVelocity;
            }
            if (Sta == 0)
            {
                Sta = Main.rand.NextFloat(-1f, 1f);
            }
            if (Tokill < 0)
            {
                Projectile.rotation += Sta * Projectile.velocity.Y;
                Projectile.velocity.Y += 0.25f;
                if (Main.rand.NextBool(6))
                {
                    int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                    Main.dust[num90].noGravity = true;
                    Main.dust[num90].velocity = Projectile.velocity * 0.5f;
                }
                if (Main.rand.NextBool(18))
                {
                    int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                    Main.dust[num90].noGravity = true;
                    Main.dust[num90].velocity = Projectile.velocity * 0.5f;
                }
            }
            Tokill--;
        }
        private bool Nul = false;
        private float Sta = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Fragrans/FragransBoomerangBrake").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            if (Tokill < 0)
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
                    Color color = new Color(255, 255, 255, 0);
                    float Fad = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                    Color color2 = new Color((int)(color.R * Fad * Fad), (int)(color.G * Fad * Fad), (int)(color.B * Fad), (int)(color.A * Fad));
                    Main.spriteBatch.Draw(t, drawPos, null, color2, Projectile.rotation - Sta * (Projectile.velocity.Y - 0.25f * k), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            else
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
                    Color color = new Color(Tokill / 45f, Tokill / 45f, Tokill / 45f, 0);
                    float Fad = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                    Color color2 = new Color((int)(color.R * Fad * Fad), (int)(color.G * Fad * Fad), (int)(color.B * Fad), (int)(color.A * Fad));
                    Main.spriteBatch.Draw(t, drawPos, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Projectile.oldVelocity;
            Tokill = 45;
            for (int y = 0; y < 6; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 2; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 7.5f)).RotatedByRandom(Math.PI * 2d);
            }
            float a = Main.rand.NextFloat(0, 500.5f);
            Player player = Main.player[Projectile.owner];
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Nul = true;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int y = 0; y < 6; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 2; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 7.5f)).RotatedByRandom(Math.PI * 2d);
            }
        }
        int Tokill = -1;
        public override Color? GetAlpha(Color lightColor)
        {
            if (Nul)
            {
                return new Color(0, 0, 0, 0);
            }
            return new Color(255, 255, 255, 0);
        }
        private Effect ef;
        public override void Kill(int timeLeft)
        {

        }
    }
}
