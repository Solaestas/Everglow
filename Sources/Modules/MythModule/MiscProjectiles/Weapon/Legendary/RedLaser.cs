using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class RedLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RedLaser");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "红激光");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 9;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 90;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.9f;
        }
        public override void Kill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/RedLaser").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
            Color color = new Color(255, 255, 255, 0);
            Main.spriteBatch.Draw(t, drawPos, new Rectangle(0, 0, 150, 8), color, (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X), drawOrigin, Projectile.velocity.Length() / 45f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
