using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.WaterBolt
{
    public class NewWaterBolt : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 1000;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.9993f;
            Projectile.velocity.Y += 0.02f;
            float k0 = 1f / (Projectile.ai[0] + 2) * 2;
            Vector2 v0 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * Projectile.scale * 0.3f;
            Dust.NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<BlueGlowAppear>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * Projectile.scale * 0.4f * k0);
            if (Projectile.ai[1] > 0)
            {
                Projectile.ai[1] -= 1;
                if (Projectile.ai[1] == 1)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("TheFirefly/Projectiles/GlowStar");
            Texture2D Shade = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterBolt/NewWaterBoltShade");
            float k1 = (100f + Projectile.ai[0] * 25) * 0.3f;
            float k0 = (1000 - Projectile.timeLeft) / k1;
            float k2 = 1f;
            if (Projectile.timeLeft <= 1000 - k1)
            {
                k0 = 1;
            }
            if (Projectile.timeLeft < 200)
            {
                k2 = Projectile.timeLeft / 200f;
            }
            Color c0 = new Color(0, k0 * k0 * 0.3f, k0 * 0.8f + 0.8f, 0);

            List<Vertex2D> bars0 = new List<Vertex2D>();
            float width = 24;

            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                TrueL++;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
                x0 %= 1f;
                bars0.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, Color.White, new Vector3(x0, 1, w)));
                bars0.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, Color.White, new Vector3(x0, 0, w)));
            }
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlack");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars0.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars0.ToArray(), 0, bars0.Count - 2);
            }

            Main.spriteBatch.Draw(Shade, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.White, Projectile.rotation, Light.Size() / 2f, (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 3.5f * k2, SpriteEffects.None, 0);

            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
                x0 %= 1f;
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
            }
            t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c0, Projectile.rotation, Light.Size() / 2f, (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 3.5f * k2, SpriteEffects.None, 0);
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c0, Projectile.rotation, Light.Size() / 2f, (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 3.5f * k2, SpriteEffects.None, 0);
            return false;
        }

        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            float k1 = (100f + Projectile.ai[0] * 25) * 0.3f;
            float k0 = (1000 - Projectile.timeLeft) / k1;

            if (Projectile.timeLeft <= 1000 - k1)
            {
                k0 = 1;
            }

            Color c0 = new Color(0.6f, 0.3f, 0f);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 48;

            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                TrueL++;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 15d) + 10000;
                x0 %= 1f;
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
            }
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override bool PreKill(int timeLeft)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                float value = Math.Min(Projectile.damage / 30f, 1f);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BeadShakeWave>(), 0, 0, Projectile.owner, 0.2f, 3f);
            }
            if (timeLeft <= 0)
            {
                return;
            }
            switch (Main.rand.Next(2))
            {
                case 0:
                    SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Sounds/WaterBolt1"), Projectile.Center);
                    break;

                case 1:
                    SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Sounds/WaterBolt2"), Projectile.Center);
                    break;
            }
            float k1 = 1;
            float k0 = 5;
            for (int j = 0; j < 8 * k0; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale * k1;
                int dust0 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * Projectile.scale * 0.4f * k0);
                Main.dust[dust0].noGravity = true;
            }
            for (int j = 0; j < 16 * k0; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale * k1;
                int dust1 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(3.7f, 5.1f) * k0);
                Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / k0);
                Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
            }
            foreach (NPC target in Main.npc)
            {
                float Dis = (target.Center - Projectile.Center).Length();

                if (Dis < k0 * 50)
                {
                    if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy() && target.active)
                    {
                        target.StrikeNPC((int)(Projectile.damage / (Dis + 35f) * 35f), 0.2f, 1);
                    }
                }
            }
        }
    }
}