using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Curves;
using Everglow.Sources.Commons.Function.FeatureFlags;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MEACModule.Projectiles;
using Everglow.Sources.Modules.MythModule;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.VFXs;
using Terraria.Audio;
namespace Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.Projectiles
{
    public class TrueDeathSickle : MeleeProj, IOcclusionProjectile, IWarpProjectile, IBloomProjectile
    {
        public override void SetDef()
        {
            maxAttackType = 2;
            trailLength = 200;
            longHandle = true;
            shadertype = "Trail";
            AutoEnd = false;
            CanLongLeftClick = false;
            CanIgnoreTile = true;
            selfWarp = true;
        }
        public override string TrailShapeTex()
        {
            return "Everglow/Sources/Modules/YggdrasilModule/CorruptWormHive/Projectiles/FlameLine";
        }
        public override string TrailColorTex()
        {
            return "Everglow/Sources/Modules/YggdrasilModule/CorruptWormHive/Projectiles/DeathSickle_Color";
        }
        public override float TrailAlpha(float factor)
        {
            return base.TrailAlpha(factor) * 1.3f;
        }
        public override BlendState TrailBlendState()
        {
            return BlendState.AlphaBlend;
        }
        public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, float HorizontalWidth, float HorizontalHeight, float DrawScale, string GlowPath, double DrawRotation)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Handle");
            if (attackType == 1 && timer > 18)
            {
                tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Handle_Filp");
            }
            if (attackType == 2)
            {
                tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle");
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + ((float)(Math.PI / 4d) * player.direction) - 0.25f, new Vector2(0, player.direction == -1 ? 0 : tex.Height), 1.22f, player.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
                return;
            }
            float texWidth = 208;
            float texHeight = 188;
            float Size = 0.864f;
            double baseRotation = 0.7854;

            float exScale = 1;
            if (longHandle)
            {
                exScale += 1f;
            }
            Vector2 origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);

            Vector2 Zoom = new Vector2(exScale * drawScaleFactor * mainVec.Length() / tex.Width, 1.2f) * Projectile.scale;

            double ProjRotation = MainVec_WithoutGravDir.ToRotation() + (Math.PI / 4);

            float QuarterSqrtTwo = 0.35355f;

            Vector2 drawCenter = ProjCenter_WithoutGravDir - Main.screenPosition;
            Vector2 CenterMoveByPlayerRotation = new Vector2(6 * player.direction, -player.height * player.gravDir) - new Vector2(0, -player.height * player.gravDir).RotatedBy(player.fullRotation);
            Vector2 drawCenter2 = drawCenter - CenterMoveByPlayerRotation;

            Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - (Math.PI / 4))) * Zoom.Y * Size;
            Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + (Math.PI / 4))) * Zoom.X * Size;

            Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - (Math.PI / 4)));
            ITexNormal.X /= tex.Width;
            ITexNormal.Y /= tex.Height;
            Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + (Math.PI / 4)));
            JTexNormal.X /= tex.Width;
            JTexNormal.Y /= tex.Height;

            Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = (Vector2.Normalize(INormal) * origin.Y) - (Vector2.Normalize(JNormal) * origin.X);
            Vector2 TopRight = (Vector2.Normalize(JNormal) * ((JNormal.Length() * 2) - origin.X)) + (Vector2.Normalize(INormal) * origin.Y);
            Vector2 BottomLeft = (-Vector2.Normalize(INormal) * ((INormal.Length() * 2) - origin.Y)) - (Vector2.Normalize(JNormal) * origin.X);
            Vector2 BottomRight = (Vector2.Normalize(JNormal) * ((JNormal.Length() * 2) - origin.X)) - (Vector2.Normalize(INormal) * ((INormal.Length() * 2) - origin.Y));


            Vector2 sourceTopLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
            Vector2 sourceTopRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            Vector2 sourceBottomLeft = new Vector2(0.5f) - ITexNormal - JTexNormal;
            Vector2 sourceBottomRight = new Vector2(0.5f) - ITexNormal + JTexNormal;

            if (Player.direction * Player.gravDir == -1)
            {
                sourceTopLeft = sourceBottomLeft;
                sourceTopRight = sourceBottomRight;
                sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
                sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            }

            List<Vertex2D> vertex2Ds = new List<Vertex2D>
            {
                    new Vertex2D(drawCenter2 + TopLeft, lightColor, new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
                    new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),

                    new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + BottomRight, lightColor, new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
            };

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);

        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];


            ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
            float ShakeStrength = 3f;
            Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 24f * ShakeStrength, 100)).RotatedByRandom(6.283);
            knockback *= 5f;
            int HitType = ModContent.ProjectileType<TrueDeathSickleHit>();
            if (attackType == 2)
            {
                damage = (int)(damage * 2.3f);
                knockback *= 2.3f;
                GenerateVFXFromTarget(target, 36, 2.6f);
                if (player.ownedProjectileCounts[HitType] < 5)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.One, HitType, 0, 0, Projectile.owner, 30, Projectile.rotation);
                }
            }
            else
            {
                GenerateVFXFromTarget(target, 18, 2f);
                if (player.ownedProjectileCounts[HitType] < 5)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.One, HitType, 0, 0, Projectile.owner, 15, Projectile.rotation);
                }
            }
        }
        public override void Attack()
        {
            useTrail = true;
            Player player = Main.player[Projectile.owner];
            TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
            Tplayer.HideLeg = true;
            if (attackType == 0)
            {

                if (timer < 20)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 + (Player.direction * -1.6f);
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(250, targetRot, 1.2f), 0.15f);
                    mainVec += Projectile.DirectionFrom(Player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 20)
                {
                    if (!EverglowConfig.DebugMode) { AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing")); }
                    else { AttSound(SoundID.Item71.WithPitchOffset(-0.3f)); }
                }

                if (timer == 50)
                {
                    ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
                    Gsplayer.FlyCamPosition = new Vector2(0, 30).RotatedByRandom(6.283);
                }

                if (timer is > 20 and < 52)
                {
                    GenerateVFX(4);
                    Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
                    isAttacking = true;
                    Projectile.rotation += player.direction * 0.1f * timer / 20f;
                    mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);

                    float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
                    player.fullRotation = BodyRotation;
                    player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
                    player.legRotation = -BodyRotation;
                    player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
                    Tplayer.HeadRotation = -BodyRotation;
                }
                if (timer >= 52)
                {
                    Projectile.extraUpdates = 16;
                }
                if (timer > 320)
                {
                    Projectile.extraUpdates = 2;
                    NextAttackType();
                }
            }

            if (attackType == 1)
            {
                if (timer < 50)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 + (Player.direction * 1.2f);
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.09f);
                    mainVec += Projectile.DirectionFrom(Player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 50)
                {
                    if (!EverglowConfig.DebugMode) { AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing")); }
                    else { AttSound(SoundID.Item71.WithPitchOffset(-0.3f)); }
                }

                if (timer == 70)
                {
                    ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
                    Gsplayer.FlyCamPosition = new Vector2(0, 30).RotatedByRandom(6.283);
                }


                if (timer is > 50 and < 75)
                {
                    GenerateVFX(4);
                    Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
                    isAttacking = true;
                    Projectile.rotation -= player.direction * 0.24f;
                    mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);

                    float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
                    player.fullRotation = BodyRotation;
                    player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
                    player.legRotation = -BodyRotation;
                    player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
                    Tplayer.HeadRotation = -BodyRotation;
                }
                if (timer >= 75)
                {
                    Projectile.extraUpdates = 16;
                }
                if (timer > 400)
                {
                    Projectile.extraUpdates = 2;
                    NextAttackType();
                }
            }
            if (attackType == 2)
            {
                trailLength = 400;
                longHandle = false;
                drawScaleFactor = 1.6f;
                if (timer < 20)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = 0;
                    if (player.direction == 1)
                    {
                        targetRot = -MathHelper.PiOver4 * 3;
                    }
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(240, targetRot, 0.3f, 0.3f), 0.15f);
                    mainVec += Projectile.DirectionFrom(Player.Center) * -3;
                    Projectile.rotation = mainVec.ToRotation();
                    disFromPlayer = -10;
                }
                if (timer == 30)
                {
                    if (!EverglowConfig.DebugMode) { AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleePowerSwing")); }
                    else { AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleePowerSwing")); SoundEngine.PlaySound(SoundID.Item71); }
                }
                if (timer == 50)
                {
                    ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
                    Gsplayer.FlyCamPosition = new Vector2(0, 60).RotatedByRandom(6.283);
                }
                if (timer is > 20 and < 55)
                {
                    GenerateVFX(timer / 5);
                    Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
                    isAttacking = true;
                    Projectile.rotation += player.direction * 0.162f * (float)(1 - Math.Cos((timer - 20) / 17.5d * Math.PI));
                    mainVec = Vector2Elipse(240, Projectile.rotation, 0.3f, 0.3f, 10000);

                    float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
                    player.fullRotation = BodyRotation;
                    player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
                    player.legRotation = -BodyRotation;
                    player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
                    Tplayer.HeadRotation = -BodyRotation;
                }
                if (timer >= 55)
                {
                    Projectile.extraUpdates = 6;
                }
                if (timer > 380)
                {
                    trailLength = 200;
                    longHandle = true;
                    drawScaleFactor = 1f;
                    NextAttackType();
                }
            }
        }
        public void GenerateVFX(int Frequency)
        {
            Player player = Main.player[Projectile.owner];
            float dir = player.direction * (((attackType + 1) % 2) - 0.5f) * 2;
            float mulVelocity = 1f;
            mulVelocity *= Frequency / 5f;
            if (attackType == 2)
            {
                mulVelocity = 2.4f;
            }
            for (int g = 0; g < Frequency; g++)
            {
                DevilFlameDust df = new DevilFlameDust
                {
                    velocity = Vector2.Normalize(mainVec).RotatedBy(1.57 * dir) * 250f / mainVec.Length() * Main.rand.NextFloat(0.65f, 8.6f) * mulVelocity,
                    Active = true,
                    Visible = true,
                    position = Projectile.Center + (mainVec * Main.rand.NextFloat(0.85f, 1.3f)) + new Vector2(Main.rand.NextFloat(0.05f, 36f), 0).RotatedByRandom(6.283),
                    maxTime = Main.rand.Next(9, 24),
                    ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(0.01f, 0.1f) * dir, Main.rand.NextFloat(8f, 32f) }
                };
                VFXManager.Add(df);
            }
        }
        public static void GenerateVFXFromTarget(NPC target, int Frequency, float mulVelocity = 1f)
        {
            for (int g = 0; g < Frequency; g++)
            {
                DevilFlameDust df = new DevilFlameDust
                {
                    velocity = new Vector2(0, Main.rand.NextFloat(4.5f, 9f)).RotatedByRandom(6.283) * mulVelocity,
                    Active = true,
                    Visible = true,
                    position = target.Center,
                    maxTime = Main.rand.Next(12, 30),
                    ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(8f, 32f) }
                };
                VFXManager.Add(df);
            }
        }
        public void DrawBloom(SpriteBatch spriteBatch)
        {
            if (spriteBatch == Main.spriteBatch)
            {
                VFXManager.spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
                DrawEffect(VFXManager.spriteBatch);
                DrawOcclusion(VFXManager.spriteBatch);
                VFXManager.spriteBatch.End();
            }
        }

        public new void DrawWarp(VFXBatch spriteBatch)
        {
            List<Vector2> SmoothTrailX = CatmullRom.SmoothPath(trailVecs.ToList());//平滑
            List<Vector2> SmoothTrail = new List<Vector2>();
            for (int x = 0; x < SmoothTrailX.Count - 1; x++)
            {
                SmoothTrail.Add(SmoothTrailX[x]);
            }
            if (trailVecs.Count != 0)
            {
                SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);
            }
            int length = SmoothTrail.Count;
            if (length <= 3)
            {
                return;
            }
            Vector2[] trail = SmoothTrail.ToArray();
            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1f;
                float d = trail[i].ToRotation() + 1.57f;
                float dir = d / MathHelper.TwoPi;
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(dir, w, 0, 1), new Vector3(factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + (trail[i] * Projectile.scale * 1.1f), new Color(dir, w, 0, 1), new Vector3(factor, 0, w)));
            }

            spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/Warp").Value, bars, PrimitiveType.TriangleStrip);

            DrawOcclusion(spriteBatch);
            return;
        }
        public override void DrawTrail(Color color)
        {

        }
        internal int timer2 = 0;
        public void DrawEffect(VFXBatch spriteBatch)
        {
            timer2++;
            List<Vector2> SmoothTrailX = CatmullRom.SmoothPath(trailVecs.ToList());//平滑
            List<Vector2> SmoothTrail = new List<Vector2>();
            for (int x = 0; x < SmoothTrailX.Count - 1; x++)
            {
                SmoothTrail.Add(SmoothTrailX[x]);
            }
            if (trailVecs.Count != 0)
            {
                SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);
            }

            int length = SmoothTrail.Count;
            if (length <= 3)
            {
                return;
            }
            Vector2[] trail = SmoothTrail.ToArray();
            List<Vertex2D> bars = new List<Vertex2D>();

            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = TrailAlpha(factor);
                if (attackType == 2)
                {
                    w *= 1.2f;
                }
                if (!longHandle)
                {
                    bars.Add(new Vertex2D(Projectile.Center + (trail[i] * 0.15f * Projectile.scale), Color.White, new Vector3(factor * 1, 1, 0f)));
                    bars.Add(new Vertex2D(Projectile.Center + (trail[i] * Projectile.scale), Color.White, new Vector3(factor * 2, 0, w)));
                }
                else
                {
                    bars.Add(new Vertex2D(Projectile.Center + (trail[i] * 0.05f * Projectile.scale), Color.White, new Vector3(factor * 1, 1, 0f)));
                    bars.Add(new Vertex2D(Projectile.Center + (trail[i] * Projectile.scale), Color.White, new Vector3(factor * 2, 0, w)));
                }
            }

            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            Effect MeleeTrail = YggdrasilContent.QuickEffect("Effects/FlameTrail");
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            MeleeTrail.Parameters["uTime"].SetValue(timer2 * 0.007f);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();

            spriteBatch.Draw(ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, bars, PrimitiveType.TriangleStrip);
        }
        public void DrawOcclusion(VFXBatch spriteBatch)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Shade_Handle");
            if (attackType == 1)
            {
                tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Shade_Handle_Flip");
            }
            if (attackType == 2)
            {
                tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Shade");
                spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(255, 0, 0, 255), Projectile.rotation + ((float)(Math.PI / 4d) * player.direction) - 0.25f, new Vector2(0, player.direction == -1 ? 0 : tex.Height), 1.22f, player.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);

                return;
            }
            float texWidth = 208;
            float texHeight = 188;
            float Size = 0.864f;
            double baseRotation = 0.7854;

            float exScale = 1;
            if (longHandle)
            {
                exScale += 1f;
            }
            Vector2 origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);

            Vector2 Zoom = new Vector2(exScale * drawScaleFactor * mainVec.Length() / tex.Width, 1.2f) * Projectile.scale;

            double ProjRotation = MainVec_WithoutGravDir.ToRotation() + (Math.PI / 4);

            float QuarterSqrtTwo = 0.35355f;

            Vector2 drawCenter = ProjCenter_WithoutGravDir - Main.screenPosition;
            Vector2 CenterMoveByPlayerRotation = new Vector2(6 * player.direction, -player.height * player.gravDir) - new Vector2(0, -player.height * player.gravDir).RotatedBy(player.fullRotation);
            Vector2 drawCenter2 = drawCenter - CenterMoveByPlayerRotation;

            Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - (Math.PI / 4))) * Zoom.Y * Size;
            Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + (Math.PI / 4))) * Zoom.X * Size;

            Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - (Math.PI / 4)));
            ITexNormal.X /= tex.Width;
            ITexNormal.Y /= tex.Height;
            Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + (Math.PI / 4)));
            JTexNormal.X /= tex.Width;
            JTexNormal.Y /= tex.Height;

            Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = (Vector2.Normalize(INormal) * origin.Y) - (Vector2.Normalize(JNormal) * origin.X);
            Vector2 TopRight = (Vector2.Normalize(JNormal) * ((JNormal.Length() * 2) - origin.X)) + (Vector2.Normalize(INormal) * origin.Y);
            Vector2 BottomLeft = (-Vector2.Normalize(INormal) * ((INormal.Length() * 2) - origin.Y)) - (Vector2.Normalize(JNormal) * origin.X);
            Vector2 BottomRight = (Vector2.Normalize(JNormal) * ((JNormal.Length() * 2) - origin.X)) - (Vector2.Normalize(INormal) * ((INormal.Length() * 2) - origin.Y));


            Vector2 sourceTopLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
            Vector2 sourceTopRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            Vector2 sourceBottomLeft = new Vector2(0.5f) - ITexNormal - JTexNormal;
            Vector2 sourceBottomRight = new Vector2(0.5f) - ITexNormal + JTexNormal;

            if (Player.direction * Player.gravDir == -1)
            {
                sourceTopLeft = sourceBottomLeft;
                sourceTopRight = sourceBottomRight;
                sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
                sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            }

            Color lightColor = new Color(255, 0, 0, 255);
            List<Vertex2D> vertex2Ds = new List<Vertex2D>
            {
                    new Vertex2D(drawCenter2 + TopLeft, lightColor, new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
                    new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),

                    new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + BottomRight, lightColor, new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
            };

            spriteBatch.Draw(tex, vertex2Ds, PrimitiveType.TriangleStrip);

        }
        public override void End()
        {
            Player player = Main.player[Projectile.owner];
            TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
            player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
            player.fullRotation = 0;
            player.legRotation = 0;
            Tplayer.HeadRotation = 0;
            Tplayer.HideLeg = false;
            player.legPosition = Vector2.Zero;
            Projectile.Kill();
            player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.DeathFlame>(), 1800);
        }
    }
}

