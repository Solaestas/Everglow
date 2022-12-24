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
            float colorV = value * value;
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague");
            float width = 60;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft;
            }

            DrawTexCircle_VFXBatch(sb, value * 450 * Projectile.ai[0], width * 20 * Projectile.ai[0], new Color(colorV, colorV * 0.7f * Projectile.ai[1], colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
    }
}