using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class BombShakeWave : ModProjectile, IWarpProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.hide = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        private static void DrawCircle(float radious, float width, Color color, Vector2 center)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4), color, new Vector3(h / (float)radious * 2f, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4), color, new Vector3(h / (float)radious * 2f, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious), color, new Vector3(1f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width), color, new Vector3(1f, 0, 0)));
            if (circle.Count > 0)
            {
                Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague");

                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        private static void DrawCircle(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, bool Black = false)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h += 5)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4), color, new Vector3(0.5f, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4), color, new Vector3(0.5f, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 0)
            {
                Texture2D t = MythContent.QuickTexture("OmniElementItems/Projectiles/Wave");
                if (Black)
                {
                    t = MythContent.QuickTexture("OmniElementItems/Projectiles/WaveBlack");
                }
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        public void DrawWarp(VFXBatch sb)
        {
           
            float value = (200 - Projectile.timeLeft) / (float)Projectile.timeLeft * 1.4f;
            float colorV = 0.6f * (1 - value) * Projectile.ai[0];
            float x0 = Projectile.ai[0];
            if (Projectile.ai[1] != 0)
            {
                colorV = 0.6f * (1 - value) * Projectile.ai[1];
                x0 = Projectile.ai[1] * 0.3f;
            }

            if (value < 1)
            {
                DrawCircle(sb,value * 1100 * Projectile.ai[0], 500 * x0 * (1 - value) + 130 * Projectile.ai[0], new Color(colorV, colorV, colorV, 0f), Projectile.Center - Main.screenPosition);
            }
            value -= 0.2f;
            if (value is < 1 and > 0)
            {
                DrawCircle(sb,value * 900 * Projectile.ai[0], 300 * x0 * (1 - value) + 130 * Projectile.ai[0], new Color(colorV, colorV, colorV, 0f), Projectile.Center - Main.screenPosition);
            }
        }
    }
}