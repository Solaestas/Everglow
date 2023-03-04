namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing
{
    public class GoldLanternStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("灯笼星");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.light = 0.1f;
            Projectile.timeLeft = 400;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];
            Vector2 v = player.Center - Projectile.Center;
            if (Projectile.timeLeft == 400)
            {
                x = Main.rand.NextFloat(0f, 1000f);
                Projectile.timeLeft += -(int)Projectile.ai[0];
            }
            if (Projectile.timeLeft > 60)
            {
                if (sca < 1)
                {
                    sca += 0.03f;
                }
                else
                {
                    sca = 1;
                }
                if (Projectile.timeLeft < 220)
                {
                    Projectile.velocity *= 1.045f;
                }

            }
            else
            {
                if (sca > 0)
                {
                    sca -= 0.03f;
                }
                else
                {
                    sca = 0;
                }
                Projectile.velocity *= 0.95f;
            }
            if (Wid < 12f)
            {
                Wid += 0.2f;
            }
            else
            {
                Wid = 12;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.Kill();
        }
        private float Wid = 0;
        private float sca = 0;
        public override void PostDraw(Color lightColor)
        {
            x += 0.01f;
            float K = (float)(Math.Sin(x + Math.Sin(x) * 6) * (0.95 + Math.Sin(x + 0.24 + Math.Sin(x))) + 3) / 30f;
            float M = (float)(Math.Sin(x + Math.Tan(x) * 6) * (0.95 + Math.Cos(x + 0.24 + Math.Sin(x))) + 3) / 30f;/*ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value*/
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, 0, new Vector2(512f, 512f), K * 0.8f * sca, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, (float)(Math.PI * 0.5), new Vector2(512f, 512f), K * 0.8f * sca, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.75), new Vector2(512f, 512f), M * 0.8f * sca, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.25), new Vector2(512f, 512f), M * 0.8f * sca, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, x * 6f, new Vector2(512f, 512f), (M + K) * 0.8f * sca, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, -x * 6f, new Vector2(512f, 512f), (float)Math.Sqrt(M * M + K * K) * 0.8f * sca, SpriteEffects.None, 0f);
        }
        private float x = 0;
    }
}
