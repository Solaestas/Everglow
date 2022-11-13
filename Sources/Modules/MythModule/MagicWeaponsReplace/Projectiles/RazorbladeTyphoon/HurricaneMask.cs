using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.RazorbladeTyphoon
{
    public class HurricaneMask : ModProjectile, IWarpProjectile
    {
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 3;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
        }

        public override void AI()
        {
            if(Projectile.timeLeft == 1)
            {
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BlackHole.BlackHole>(), Projectile.damage * 12, 0, Projectile.owner, Projectile.ai[0]);
                p.CritChance = (int)Main.player[Projectile.owner].GetCritChance(DamageClass.Generic);
                p.timeLeft = 100 + (int)(Projectile.ai[0] * 240);
                if(Projectile.ai[0] < 0.5f)
                {
                    SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Sounds/TyphoonBlackHoleWeak").WithVolumeScale(Projectile.ai[0] * 2), Projectile.Center);
                }
                else
                {
                    SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Sounds/TyphoonBlackHoleStrong").WithVolumeScale(Projectile.ai[0]), Projectile.Center);
                }
                Projectile.Kill();
            }
            if(Projectile.timeLeft < 150)
            {
                Projectile.extraUpdates = 4;
            }
            if (Projectile.timeLeft < 100)
            {
                Projectile.extraUpdates = 9;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHit");
            float Dark = Math.Max(((Projectile.timeLeft - 150) / 50f), 0);
            Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, Color.White * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] * 6f * Dark, SpriteEffects.None, 0);
            Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitLight");
            Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, new Color(0, 225, 255, 0) * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] * 8f * Dark, SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHit");
            float Dark = Math.Max(((Projectile.timeLeft - 150) / 50f), 0);
            Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, Color.White * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f, SpriteEffects.None, 0);
            Texture2D light = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitStar");
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 225, 255, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark * Dark) * Projectile.ai[0] * 6f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 225, 255, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, Dark) * Projectile.ai[0] * 6f, SpriteEffects.None, 0);
            return false;
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

        public void DrawWarp()
        {
            float value = (float)Math.Sin(Projectile.timeLeft / 200d * Math.PI);
            float colorV = 1.6f * (1 - value) * Projectile.ai[0];
            float x0 = Projectile.ai[0];

            if (value < 1)
            {
                DrawTexCircle(value * 1100 * Projectile.ai[0], 150 * x0 * (1 - value) + 30 * Projectile.ai[0], new Color(colorV, colorV + 0.5f, 0, 0f), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("OmniElementItems/Projectiles/Wave"));
            }
            value -= 0.2f;
            if (value is < 1 and > 0)
            {
                DrawTexCircle(value * 1100 * Projectile.ai[0], 150 * x0 * (1 - value) + 30 * Projectile.ai[0], new Color(colorV, colorV + 0.5f, 0, 0f), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("OmniElementItems/Projectiles/Wave"));
            }
        }
    }
}