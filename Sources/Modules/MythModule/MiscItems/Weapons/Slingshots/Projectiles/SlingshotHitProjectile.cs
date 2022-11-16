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
            Projectile.extraUpdates = 3;
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
            float value = (200 - Projectile.timeLeft) / (float)Projectile.timeLeft * 1.4f;
            float colorV = 0.02f * MathF.Sqrt(Projectile.ai[0]) * (1 - value);
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/EShoot");
            DrawTexCircle(value * 32 * MathF.Sqrt(Projectile.ai[0]), 4 * MathF.Sqrt(Projectile.ai[0]) * value, new Color(colorV, colorV, colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            return false;
        }

  
        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            float value = (200 - Projectile.timeLeft) / (float)Projectile.timeLeft * 1.4f;
            float colorV = 0.9f * (1 - value);
            if (Projectile.ai[0] >= 10)
            {
                colorV *= 10f / Projectile.ai[0];
            }
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/EShoot");
            DrawTexCircle(value * 32 * MathF.Sqrt(Projectile.ai[0]), 10 * MathF.Sqrt(Projectile.ai[0]), new Color(colorV, colorV * MathF.Sqrt(Projectile.ai[0]) * 0.1f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}