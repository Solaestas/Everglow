using Terraria.Audio;
using Terraria.GameContent;
using Everglow.Sources.Modules.MEACModule.Projectiles;
using Everglow.Sources.Modules.MythModule;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Function.Curves;
namespace Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.Projectiles
{
    public class TrueDeathSickle : MeleeProj, IOcclusionProjectile
    {
        public override void SetDef()
        {
            maxAttackType = 2;
            trailLength = 24;
            longHandle = true;
            shadertype = "Trail";
            AutoEnd = false;
            CanLongLeftClick = false;
            CanIgnoreTile = true;
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
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + (float)(Math.PI / 4d) * player.direction - 0.25f, new Vector2(0, player.direction == -1 ? 0 : tex.Height), 1.28f,player.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
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

            double ProjRotation = MainVec_WithoutGravDir.ToRotation() + Math.PI / 4;

            float QuarterSqrtTwo = 0.35355f;

            Vector2 drawCenter = ProjCenter_WithoutGravDir - Main.screenPosition;
            Vector2 CenterMoveByPlayerRotation = new Vector2(6 * player.direction, -player.height * player.gravDir) - new Vector2(0, -player.height * player.gravDir).RotatedBy(player.fullRotation);
            Vector2 drawCenter2 = drawCenter - CenterMoveByPlayerRotation;

            Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - Math.PI / 4)) * Zoom.Y * Size;
            Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + Math.PI / 4)) * Zoom.X * Size;

            Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - Math.PI / 4));
            ITexNormal.X /= tex.Width;
            ITexNormal.Y /= tex.Height;
            Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + Math.PI / 4));
            JTexNormal.X /= tex.Width;
            JTexNormal.Y /= tex.Height;

            Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = Vector2.Normalize(INormal) * origin.Y - Vector2.Normalize(JNormal) * origin.X;
            Vector2 TopRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) + Vector2.Normalize(INormal) * origin.Y;
            Vector2 BottomLeft = -Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y) - Vector2.Normalize(JNormal) * origin.X;
            Vector2 BottomRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) - Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y);


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

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //伤害倍率
            ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
            float ShakeStrength = 1f;
            
            Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12f * ShakeStrength, 100)).RotatedByRandom(6.283);
        }
        public override void Attack()
        {
            Player player = Main.player[Projectile.owner];
            TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
            Tplayer.HideLeg = true;
            if (Main.myPlayer == Projectile.owner && Main.mouseRight && Main.mouseRightRelease)
            {
                timer = 0;
            }

            useTrail = true;


            if (attackType == 0)
            {
                if (timer < 20)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 + Player.direction * -1.6f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(250, targetRot, 1.2f), 0.15f);
                    mainVec += Projectile.DirectionFrom(Player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 20)
                {
                    AttSound(new SoundStyle(
                "Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing"));
                }

                if (timer > 20 && timer < 52)
                {
                    Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.1f * (timer) / 20f;
                    mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);
                }
                if (timer > 70)
                {
                    NextAttackType();
                }
            }

            if (attackType == 1)
            {
                if (timer < 50)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 + Player.direction * 1.2f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.09f);
                    mainVec += Projectile.DirectionFrom(Player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 50)
                {
                    AttSound(new SoundStyle(
                "Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing"));
                }
                if (timer > 50 && timer < 75)
                {
                    Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.15f);
                    isAttacking = true;
                    Projectile.rotation -= Projectile.spriteDirection * 0.24f;
                    mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f,0,1000);
                }
                if (timer > 120)
                {
                    NextAttackType();
                }
            }
            if (attackType == 2)
            {
                trailLength = 40;
                longHandle = false;
                drawScaleFactor = 1.6f;
                if (timer < 20)
                {
                    useTrail = false;
                    LockPlayerDir(Player);
                    float targetRot = -MathHelper.PiOver2 + Player.direction * -1.6f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(240, targetRot, 0.3f, 0.3f), 0.15f);
                    mainVec += Projectile.DirectionFrom(Player.Center) * -3;
                    Projectile.rotation = mainVec.ToRotation();
                    disFromPlayer = -10;
                }
                if (timer == 20)
                {
                    AttSound(new SoundStyle(
                "Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleePowerSwing"));
                }
                if (timer > 20 && timer < 55)
                {
                    Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.162f * (float)(1 - Math.Cos((timer - 20) / 17.5d * Math.PI));
                    mainVec = Vector2Elipse(240, Projectile.rotation, 0.3f, 0.3f, 10000);
                }
                if (timer > 190)
                {
                    trailLength = 24;
                    longHandle = true;
                    drawScaleFactor = 1f;
                    NextAttackType();
                }
            }
        }
        public override void DrawTrail(Color color)
        {

        }
        public void DrawEffect()
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
                float w = TrailAlpha(factor);
                if(attackType == 2)
                {
                    w *= 1.2f;
                }
                if (!longHandle)
                {
                    bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.15f * Projectile.scale, Color.White, new Vector3(factor * 2, 1, 0f)));
                    bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor * 4, 0, w)));
                }
                else
                {
                    bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.3f * Projectile.scale, Color.White, new Vector3(factor * 2, 1, 0f)));
                    bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor * 4, 0, w)));
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            Effect MeleeTrail = ModContent.Request<Effect>("Everglow/Sources/Modules/YggdrasilModule/Effects/FlameTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            MeleeTrail.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects * -0.3f);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            //Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void DrawOcclusion()
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
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (float)(Math.PI / 4d) * player.direction - 0.25f, new Vector2(0, player.direction == -1 ? 0 : tex.Height), 1.28f,player.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
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

            double ProjRotation = MainVec_WithoutGravDir.ToRotation() + Math.PI / 4;

            float QuarterSqrtTwo = 0.35355f;

            Vector2 drawCenter = ProjCenter_WithoutGravDir - Main.screenPosition;
            Vector2 CenterMoveByPlayerRotation = new Vector2(6 * player.direction, -player.height * player.gravDir) - new Vector2(0, -player.height * player.gravDir).RotatedBy(player.fullRotation);
            Vector2 drawCenter2 = drawCenter - CenterMoveByPlayerRotation;

            Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - Math.PI / 4)) * Zoom.Y * Size;
            Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + Math.PI / 4)) * Zoom.X * Size;

            Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - Math.PI / 4));
            ITexNormal.X /= tex.Width;
            ITexNormal.Y /= tex.Height;
            Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + Math.PI / 4));
            JTexNormal.X /= tex.Width;
            JTexNormal.Y /= tex.Height;

            Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = Vector2.Normalize(INormal) * origin.Y - Vector2.Normalize(JNormal) * origin.X;
            Vector2 TopRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) + Vector2.Normalize(INormal) * origin.Y;
            Vector2 BottomLeft = -Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y) - Vector2.Normalize(JNormal) * origin.X;
            Vector2 BottomRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) - Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y);


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

            Color lightColor = Color.White;
            List<Vertex2D> vertex2Ds = new List<Vertex2D>
            {
                    new Vertex2D(drawCenter2 + TopLeft, lightColor, new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
                    new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),

                    new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + BottomRight, lightColor, new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
            };

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
            target.AddBuff(BuffID.Poisoned, 600);
        }
    }
}

