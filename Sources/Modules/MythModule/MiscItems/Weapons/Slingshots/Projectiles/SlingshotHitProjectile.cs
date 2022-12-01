using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    /// <summary>
    /// ai[0]强度ai[1]角度
    /// </summary>
    public abstract class SlingshotHitProjectile : ModProjectile, IWarpProjectile
    {
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 9;
            Projectile.DamageType = DamageClass.Ranged;
            SetDef();
        }
        public virtual void SetDef()
        {

        }
        private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            float value = (200 - Projectile.timeLeft) / 200f;
            float colorV = 0.02f * MathF.Sqrt(Projectile.ai[0]) * (1 - value);
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/EShoot");
            DrawTexCircle(value * 32 * MathF.Sqrt(Projectile.ai[0]), 4 * MathF.Sqrt(Projectile.ai[0]) * value, new Color(colorV, colorV, colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            return false;
        }
        private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();

            for (float h = 0; h < radious / 2; h += 1f)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 2)
            {
                spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
            }
        }
        public void DrawWarp(VFXBatch spriteBatch)
        {
            float value = (200 - Projectile.timeLeft) / 200f;
            float colorV = 0.6f * MathF.Sqrt(Projectile.ai[0]) * (1 - value);
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/EShoot");
            DrawTexCircle_VFXBatch(spriteBatch, value * 32 * MathF.Sqrt(Projectile.ai[0]), 8 * MathF.Sqrt(Projectile.ai[0]) * value, new Color(colorV, colorV * 0.4f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
    }
}