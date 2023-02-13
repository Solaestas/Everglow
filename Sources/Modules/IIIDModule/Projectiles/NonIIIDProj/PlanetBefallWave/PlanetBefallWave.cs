using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.IIIDModule.Projectiles.NonIIIDProj.PlanetBefallWave
{
    public class PlanetBefallWave : ModProjectile, IWarpProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 3;
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
            float value = (200 - Projectile.timeLeft) / (200f);
            value = MathF.Sqrt(value);
            float colorV = 0.9f * (1 - value);
            colorV *= 10;
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Lightline");
            float width = 120;
            if (Projectile.timeLeft < 120)
            {
                width = Projectile.timeLeft;
            }
            VFXManager.spriteBatch.Begin();
            DrawTexCircle_VFXBatch(VFXManager.spriteBatch, value * 270 * Projectile.ai[0], width, new Color(colorV, colorV * 0.9f, 0, 0f) * 0.4f, Projectile.Center - Main.screenPosition, t);
            DrawTexCircle_VFXBatch(VFXManager.spriteBatch, value * 160 * Projectile.ai[0], width * 0.6f, new Color(colorV, colorV * 0.9f, 0, 0f) * 0.4f, Projectile.Center - Main.screenPosition, t);
            VFXManager.spriteBatch.End();
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
                circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0.8f, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0.2f, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 0.8f, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 0.2f, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
            if (circle.Count > 2)
            {
                spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
            }
        }
        public void DrawWarp(VFXBatch spriteBatch)
        {
            float value = (200 - Projectile.timeLeft) / (200f);
            value = MathF.Sqrt(value);
            float colorV = 0.9f * (1 - value);
            colorV *= 10;
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Lightline");
            float width = 120;
            if (Projectile.timeLeft < 120)
            {
                width = Projectile.timeLeft;
            }

            DrawTexCircle_VFXBatch(spriteBatch, value * 270 * Projectile.ai[0], width, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);

            DrawTexCircle_VFXBatch(spriteBatch, value * 160 * Projectile.ai[0], width * 0.6f, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
    }

}
