using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	public class FragransPedal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Pedal");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "花瓣");
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
        }
        private bool initialization = true;
        private double X;
        private float Omega;
        private float b;
        Vector2 vO = Vector2.Zero;
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            Projectile.velocity *= 0.93f;
            Projectile.scale = Projectile.ai[0];
            if (Projectile.timeLeft == 49)
            {
                vO = Projectile.Center - Projectile.velocity * 3f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 30)
            {
                return new Color?(new Color((int)(255 * (Projectile.timeLeft / 30f)), (int)(255 * (Projectile.timeLeft / 30f)), (int)(255 * (Projectile.timeLeft / 30f)), 0));
            }
            return new Color?(new Color(255, 255, 255, 0));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float offsetX = 20f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);
            Color drawColor = Projectile.GetAlpha(lightColor);
            for (int i = 0; i < 4; i++)
            {
                Main.EntitySpriteDraw(texture, vO - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + (Projectile.Center - vO).RotatedBy(Math.PI / 2d * i), sourceRectangle, drawColor, Projectile.rotation + (float)Math.PI / 2f * i, origin, Projectile.scale, spriteEffects, 0);
            }
            return false;
        }
    }
}