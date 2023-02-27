using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class Gear8 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gear");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "齿轮");
        }
        public override void SetDefaults()
        {
            Projectile.width = 230;
            Projectile.height = 230;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0));
        }
        float Ome = 0;
        float Ang = 0;
        Vector2 Inpos;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Inpos = player.Center - new Vector2(170 * player.direction, 150) - new Vector2(115, 0) + new Vector2(32 * Projectile.ai[0], 0);
            Projectile.position = Projectile.position * 0.498f + Inpos * 0.502f;
            Projectile.rotation -= Ome * 0.16f;
            if (Ome < 0.1f && Projectile.timeLeft > 55)
            {
                Ome += 0.0006f;
            }
            if (Projectile.timeLeft <= 55)
            {
                Ome *= 0.98f;
            }
            if (fade < 1f && Projectile.timeLeft > 55)
            {
                fade += 0.02f;
            }
            if (Projectile.timeLeft <= 55)
            {
                fade *= 0.93f;
            }
            if (Main.mouseLeft && player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.Legendary.MachineSkeGun>())
            {
                Projectile.timeLeft = 60;
            }
        }
        float fade = 0;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        int frequency = 30;
        int Acount = 1;
        private Effect ef;
        float Rota = 0;
        float CirPro0 = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            CirPro0 += 0.007f;
            Rota = (float)(-Main.time * 0.08f);
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearWave2").Value;
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/Gear8").Value;

            //Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.04) * 1.3f), new Vector2(115), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.04) * 0.5f), new Vector2(115), 1.3f, SpriteEffects.None, 0f);
            //Main.spriteBatch.Draw(t2, Projectile.Center - Main.screenPosition, null, new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.04) * 0.5f), new Vector2(115), 1f, SpriteEffects.None, 0f);
            ef.Parameters["radiu"].SetValue(Math.Abs(CirPro0 % 2f - 1f));
            ef.Parameters["Col"].SetValue(1);
            if (Projectile.timeLeft <= 55)
            {
                ef.Parameters["Col"].SetValue(fade);
            }
            ef.CurrentTechnique.Passes["Test"].Apply();
            return true;
        }
    }
}
