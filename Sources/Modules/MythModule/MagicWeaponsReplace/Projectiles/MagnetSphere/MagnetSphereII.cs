using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;
using Everglow.Sources.Modules.MEACModule;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.MagnetSphere
{
    public class MagnetSphereII : ModProjectile, IWarpProjectile
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
            Projectile.timeLeft = 3000;
            Projectile.alpha = 0;
            Projectile.penetrate = 18;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 24;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 120;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0, 0.46f * Projectile.scale, 0.4f * Projectile.scale);
            Projectile.velocity *= 0.999f;
            Projectile.scale = 0.6f + (float)(Math.Sin(Main.timeForVisualEffects / 1.8f + Projectile.ai[0])) * 0.45f;
            Projectile.timeLeft -= player.ownedProjectileCounts[Projectile.type];
            if(Main.rand.NextBool(8))
            {
                foreach (NPC target in Main.npc)
                {
                    if(target.active)
                    {
                        if (!target.friendly && !target.dontTakeDamage)
                        {
                            Vector2 v = target.Center - Projectile.Center;
                            if(v.Length() < 400)
                            {
                                if (Main.rand.NextBool(6))
                                {
                                    int HitType = ModContent.ProjectileType<MagnetSphereLighting>();
                                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Vector2.One, HitType, (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, Projectile.whoAmI, Projectile.rotation + Main.rand.NextFloat(6.283f));
                                    p.CritChance = Projectile.CritChance;
                                    SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, target.Center);
                                    Projectile.penetrate--;
                                    if(Projectile.penetrate < 0)
                                    {
                                        Projectile.Kill();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/MagnetSphere/MagnetSphereII");
            Texture2D Light2 = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/MagnetSphere/Projectile_254");
            Texture2D Shade = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterBolt/NewWaterBoltShade");

            Color c0 = new Color(0, 199, 129, 0);

            List<Vertex2D> bars0 = new List<Vertex2D>();
            float width = 64;

            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                TrueL++;
            }
            if(Projectile.timeLeft < 2400)
            {
                TrueL = Projectile.timeLeft / 20;
            }
            if (TrueL < 1)
            {
                TrueL = 1;
            }
            for (int i = 1; i < TrueL; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 15d + Projectile.ai[0]) + 10000;
                x0 %= 1f;
                bars0.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, Color.White * 0.8f, new Vector3(x0, 1, w)));
                bars0.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, Color.White * 0.8f, new Vector3(x0, 0, w)));
            }
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkDark");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars0.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars0.ToArray(), 0, bars0.Count - 2);
            }

            Main.spriteBatch.Draw(Shade, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.White, Projectile.rotation, Shade.Size() / 2f, 1.08f * Projectile.scale, SpriteEffects.None, 0);

            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 1; i < TrueL; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float x0 = factor * 3.14159f / 0.9f - (float)(Main.timeForVisualEffects / 9d + Projectile.ai[0]) + 10000;
                x0 %= 1f;
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
            }
            t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkLight");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }

            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c0, Projectile.rotation, Light.Size() / 2f, 0.8f * Projectile.scale, SpriteEffects.None, 0);
            Rectangle rt = new Rectangle(0, 44 * (int)((Main.timeForVisualEffects / 6f) % 5), 38, 44);
            Main.spriteBatch.Draw(Light2, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rt, new Color(95,95,95,55), Projectile.rotation, rt.Size() / 2f, Projectile.scale * 0.2f + 1.2f, SpriteEffects.None, 0);
            return false;
        }

        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();

            Color c0 = new Color(0.6f, 0.6f, 0f);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 64;

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
                float x0 = factor * 1.6f - (float)(Main.timeForVisualEffects / 15d) + 10000;
                x0 %= 1f;
                float mul = 1f;
                if(i < 10)
                {
                    mul = i / 10f;
                }
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) * mul + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) * mul + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
            }
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTrace");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }


        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, Projectile.Center);
            for (int d = 0;d <28;d++)
            {
                Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Electric, 0, 0, 0, default, 0.6f);
                d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283) * 3;
            }
            int HitType = ModContent.ProjectileType<MagnetSphereHit>();
            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, (int)(Projectile.damage * 3f), Projectile.knockBack, Projectile.owner, 30, Projectile.rotation + Main.rand.NextFloat(6.283f));
            p.CritChance = Projectile.CritChance;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.Center);
            for (int d = 0; d < 28; d++)
            {
                Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Electric, 0, 0, 0, default, 0.6f);
                d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283);
            }
            Projectile.penetrate -= 5;
            if (Projectile.penetrate < 0)
            {
                Projectile.Kill();
            }
            int HitType = ModContent.ProjectileType<MagnetSphereHit>();
            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, (int)(Projectile.damage * 2f), Projectile.knockBack, Projectile.owner, 18, Projectile.rotation + Main.rand.NextFloat(6.283f));
            p.CritChance = Projectile.CritChance;
            Projectile.damage = (int)(Projectile.damage * 1.2);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.Center);
            for (int d = 0; d < 28; d++)
            {
                Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Electric, 0, 0, 0, default, 0.6f);
                d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283);
            }
            Projectile.penetrate -= 5;
            if (Projectile.penetrate < 0)
            {
                Projectile.Kill();
            }
            int HitType = ModContent.ProjectileType<MagnetSphereHit>();
            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, (int)(Projectile.damage * 2f), Projectile.knockBack, Projectile.owner, 18, Projectile.rotation + Main.rand.NextFloat(6.283f));
            p.CritChance = Projectile.CritChance;
            Projectile.damage = (int)(Projectile.damage * 1.2);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.Center);
            for (int d = 0; d < 28; d++)
            {
                Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Electric, 0, 0, 0, default, 0.6f);
                d0.velocity = new Vector2(0, Main.rand.NextFloat(1.65f, 5.5f)).RotatedByRandom(6.283);
            }
            int HitType = ModContent.ProjectileType<MagnetSphereHit>();
            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, (int)(Projectile.damage * 2f), Projectile.knockBack, Projectile.owner, 18, Projectile.rotation + Main.rand.NextFloat(6.283f));
            p.CritChance = Projectile.CritChance;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity *= 0.98f;
            Projectile.penetrate-=5;
            if(Projectile.penetrate < 0)
            {
                Projectile.Kill();
            }
            return false;
        }
    }
}