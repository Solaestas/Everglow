using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    public class TuskAim : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk Aim");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "瞄准");
        }
        public override void SetDefaults()
        {
            Projectile.width = 205;
            Projectile.height = 205;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((2.7f / (fade * fade)), (2.7f / (fade * fade)), (2.7f / (fade * fade)), (2.7f / (fade * fade))));
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft < 30)
            {
                fade = (Projectile.timeLeft) / 5f + 0.9f;
            }
            Projectile.scale = fade * 0.5f;
        }
        float fade = 4;

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 v0 = (Projectile.Center - player.Center) * 0.5f;
            Vector2 vPos = Projectile.Center - v0 - new Vector2(0, 1500);
            Vector2 v = (Projectile.Center - vPos) / 30f + Main.npc[(int)Projectile.ai[0]].velocity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Projectiles/Weapon/TuskAimGlow").Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 205, 205), new Color((1 / (fade * fade)), (1 / (fade * fade)), (1 / (fade * fade)), 0), 0, new Vector2(100.25f), Projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 205, 205), new Color((1 / (fade * fade)), (1 / (fade * fade)), (1 / (fade * fade)), 0), Projectile.timeLeft / 30f, new Vector2(100.25f), Projectile.scale * 0.9f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 205, 205), new Color((1 / (fade * fade)), (1 / (fade * fade)), (1 / (fade * fade)), 0), -Projectile.timeLeft / 30f, new Vector2(100.25f), Projectile.scale * 0.84f, SpriteEffects.None, 0f);
            return true;
        }
    }
}
