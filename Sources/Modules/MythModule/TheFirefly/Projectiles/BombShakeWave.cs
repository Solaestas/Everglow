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
            Projectile.extraUpdates = 6;
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
        private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();

            for (int h = 0; h < radious / 2; h += 1)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
            if (circle.Count > 2)
            {
                spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
            }
        }
        public void DrawWarp(VFXBatch sb)
        {
            float value = (200 - Projectile.timeLeft) / (200f);
            value = MathF.Sqrt(value);
            float colorV = 0.9f * (1 - value);
            colorV *= 30f;
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague");
            float width = 60;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft;
            }

            DrawTexCircle_VFXBatch(sb, value * 450, width * 20, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
    }
}