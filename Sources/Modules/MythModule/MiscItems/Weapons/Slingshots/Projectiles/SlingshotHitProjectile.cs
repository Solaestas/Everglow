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
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
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
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitLight");
            float Dark = Math.Max(((Projectile.timeLeft - 150) / 50f), 0);
            Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, new Color(55, 255,0,0) * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f * Dark, SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Shadow =
                
                MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHit");
            float Dark = Math.Max(((Projectile.timeLeft - 150) / 50f), 0);
            Main.spriteBatch.Draw(Shadow,Projectile.Center - Main.screenPosition,null,Color.White * Dark,0,Shadow.Size() / 2f,2.2f * Projectile.ai[0] / 15f, SpriteEffects.None,0);
            Texture2D light = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitStar");
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(55, 255, 0, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark * Dark) * Projectile.ai[0] / 40f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(55, 255, 0, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, Dark) * Projectile.ai[0] / 40f, SpriteEffects.None, 0);
            return false;
        }

  
        public void DrawWarp()
        {
            float value = (200 - Projectile.timeLeft) / (float)Projectile.timeLeft * 1.4f;
            float colorV = 0.9f * (1 - value);
            if (Projectile.ai[0] >= 10)
            {
                colorV *= Projectile.ai[0] / 10f;
            }
            Texture2D t = MythContent.QuickTexture("OmniElementItems/Projectiles/Wave");
            DrawTexCircle(value * 16 * Projectile.ai[0], 100, new Color(colorV, colorV * 0.4f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
    }
}