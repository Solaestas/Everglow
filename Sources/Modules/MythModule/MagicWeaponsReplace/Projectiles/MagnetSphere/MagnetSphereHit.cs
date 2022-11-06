using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.MagnetSphere
{
    public class MagnetSphereHit : ModProjectile, IWarpProjectile
    {
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        public override void SetDefaults()
        {
            Projectile.width = 240;
            Projectile.height = 240;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
        }
        public void GenerateVFXExpolode(int Frequency, float mulVelocity = 1f)
        {
            for (int g = 0; g < Frequency * 3; g++)
            {
                Vector2 vel = new Vector2(0, Main.rand.NextFloat(4.65f, 5.5f)).RotatedByRandom(6.283) * mulVelocity;
                MagneticElectricity me = new MagneticElectricity
                {
                    velocity = vel,
                    Active = true,
                    Visible = true,
                    maxTime = Main.rand.Next(54, 180),
                    ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.01f, 0.01f), Main.rand.NextFloat(1.6f, 2f) * mulVelocity },
                    position = Projectile.Center - vel * 3
                };
                VFXManager.Add(me);
            }
            for (int g = 0; g < Frequency; g++)
            {
                Vector2 vel = new Vector2(0, Main.rand.NextFloat(6.65f, 10.5f)).RotatedByRandom(6.283) * mulVelocity;
                MagneticElectricity me = new MagneticElectricity
                {
                    velocity = vel,
                    Active = true,
                    Visible = true,
                    maxTime = Main.rand.Next(54, 180),
                    ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.01f, 0.01f), Main.rand.NextFloat(1.6f, 2f) * mulVelocity },
                    position = Projectile.Center - vel * 3
                };
                VFXManager.Add(me);
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            GenerateVFXExpolode(12, 0.6f);
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            if (Projectile.timeLeft <= 198)
            {
                Projectile.friendly = false;
            }
            float LightS = Projectile.timeLeft / 2f - 95f;
            if(LightS > 0)
            {
                Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0, LightS * 0.83f, LightS * 0.8f);
            }

            Projectile.velocity *= 0;
        }
        private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0.5f, 0, 0)));
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
            Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, new Color(0, 229, 206,0) * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f * Dark, SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHit");
            float Dark = Math.Max(((Projectile.timeLeft - 150) / 50f), 0);
            Main.spriteBatch.Draw(Shadow,Projectile.Center - Main.screenPosition,null,Color.White * Dark,0,Shadow.Size() / 2f,2.2f * Projectile.ai[0] / 15f, SpriteEffects.None,0);
            Texture2D light = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitStar");
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 120, 103, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark * Dark) * Projectile.ai[0] / 20f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 120, 103, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.8f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);

            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 120, 103, 0), (float)(Math.PI / 4d) + Projectile.ai[1], light.Size() / 2f, new Vector2(0.6f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 120, 103, 0), (float)(Math.PI / 4d * 3) + Projectile.ai[1], light.Size() / 2f, new Vector2(0.6f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);
            return false;
        }

        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            float value = (200 - Projectile.timeLeft) / (float)Projectile.timeLeft * 1.4f;
            float colorV = 0.9f * (1 - value);
            if(Projectile.ai[0] >= 10)
            {
                colorV *= Projectile.ai[0] / 10f;
            }
            Texture2D t = MythContent.QuickTexture("OmniElementItems/Projectiles/Wave");
            DrawTexCircle(value * 16 * Projectile.ai[0], 100, new Color(colorV, colorV * 0.2f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
        }
    }
}