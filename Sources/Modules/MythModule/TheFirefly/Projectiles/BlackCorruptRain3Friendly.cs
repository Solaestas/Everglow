using Terraria.Localization;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class BlackCorruptRain3Friendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Corrupt Ball");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "黑萤球");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 700;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() < 5f)
            {
                Projectile.velocity *= 1.018f;
            }
            Lighting.AddLight(base.Projectile.Center, 0,0.4f,0.9f);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTraceLight");
            float width = 20;
            if (Projectile.timeLeft < 120)
            {
                width = Projectile.timeLeft / 6f;
            }
            VFXManager.spriteBatch.Begin();
            DrawTexCircle_VFXBatch(VFXManager.spriteBatch, 30 + 7 * MathF.Sin((float)(Main.timeForVisualEffects / 3f + Projectile.ai[0])), width, new Color(0, 150,255, 0) * 0.4f, Projectile.Center - Main.screenPosition,t , (float)(Main.timeForVisualEffects / 3.8f + Projectile.ai[0]));
            VFXManager.spriteBatch.End();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("TheFirefly/Projectiles/FixCoinLight3");
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition, null, new Color(1f,1f,1f, 0), Projectile.rotation, Light.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return true;
        }
        private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();

            for (int h = 0; h < radious / 2; h += 1)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0.8f, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0.2f, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 0.8f, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0.2f, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 0.2f, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0.8f, 0)));
            if (circle.Count > 2)
            {
                spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
            }
        }
    }
}