using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class TrapYoyoLight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TrapYoyoLight");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "机关球");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 60;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.97f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 15; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(1.5f, 4f)).RotatedByRandom(MathHelper.TwoPi);
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 87, 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[num].velocity *= v;
                Main.dust[num].noGravity = true;
            }
            for (int i = 0; i < 4; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(3f, 9f)).RotatedByRandom(MathHelper.TwoPi);
                v += Projectile.velocity;
                int num2 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center - v, v, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.AuDust>(), Projectile.damage / 8, 0.2f, Main.myPlayer, 0f, 0f);
                Main.projectile[num2].timeLeft = 80;
            }
        }
        public override void Kill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/TrapYoyoLight").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
                Color color = new Color(240, 240, 240, 0);
                if (Projectile.timeLeft < 60)
                {
                    color = new Color(Projectile.timeLeft * 4, Projectile.timeLeft * 4, Projectile.timeLeft * 4, 0);
                }
                Color color2 = new Color((int)(color.R * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.G * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.B * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.A * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
                Main.spriteBatch.Draw(t, drawPos, null, color2, Projectile.rotation, drawOrigin, Projectile.scale * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
