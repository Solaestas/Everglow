using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class Gear3 : ModProjectile
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
            Projectile.rotation -= Ome * 0.66f;
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
                fade *= 0.98f;
            }
            if (Main.mouseLeft && player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.Legendary.MachineSkeGun>() && player.HasAmmo(player.HeldItem) == true)
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
        public override bool PreDraw(ref Color lightColor)
        {
            Rota = 0;
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Vortex").Value;
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/Gear5").Value;
            Vector2 v0 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + Projectile.rotation + 0.52);
            Vector2 v1 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + 2.0943951023932 + Projectile.rotation + 0.52);
            Vector2 v2 = Projectile.Center + new Vector2(0, 100).RotatedBy(-Rota + 4.1887902047864 + Projectile.rotation + 0.52);
            Main.spriteBatch.Draw(t, v0 - Main.screenPosition, new Rectangle(0, 0, 80, 80), new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.04) * 1.3f), new Vector2(40), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(t, v1 - Main.screenPosition, new Rectangle(0, 0, 80, 80), new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.06) * 2), new Vector2(40), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(t, v2 - Main.screenPosition, new Rectangle(0, 0, 80, 80), new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.09) * 1.7f), new Vector2(40), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ef.Parameters["uRot"].SetValue(Rota);
            ef.Parameters["Col"].SetValue((float)(fade * fade));
            ef.CurrentTechnique.Passes["Test"].Apply();
            return true;
        }
    }
}
