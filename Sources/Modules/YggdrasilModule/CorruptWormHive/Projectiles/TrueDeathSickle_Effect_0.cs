using Terraria.Audio;
using Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.VFXs;
using Everglow.Sources.Commons.Core.VFX;
using Terraria.GameContent;
using Everglow.Sources.Modules.MEACModule.Projectiles;
using Everglow.Sources.Modules.MythModule;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Function.Curves;
namespace Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.Projectiles
{
    public class TrueDeathSickle_Effect_0 : ModProjectile,IOcclusionProjectile, IWarpProjectile
    {
        internal int timer = 0;
        internal Vector2 mainVec;
        internal Queue<Vector2> trailVecs;
        internal int trailLength;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 15;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            trailLength = 240;
            trailVecs = new Queue<Vector2>(trailLength + 1);
        }
        public override void AI()
        {
            timer++;
            Player player = Main.player[Projectile.owner];
            trailVecs.Enqueue(mainVec);
            if (trailVecs.Count > trailLength)
            {
                trailVecs.Dequeue();
            }
            Projectile.velocity = player.velocity * 0.5f;

            switch (Projectile.ai[0])
            {
                case 0:
                    if (timer < 20)
                    {
                        float targetRot = -MathHelper.PiOver2 + player.direction * -1.6f;
                        mainVec = Vector2.Lerp(mainVec, Vector2Elipse(250, targetRot, 1.2f), 0.15f);
                        mainVec += Projectile.DirectionFrom(player.Center) * 3;
                        Projectile.rotation = mainVec.ToRotation();
                    }

                    if (timer == 50)
                    {
                        ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
                        Gsplayer.FlyCamPosition = new Vector2(0, 30).RotatedByRandom(6.283);
                    }

                    if (timer > 20 && timer < 52)
                    {
                        Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
                        Projectile.rotation += player.direction * 0.1f * (timer) / 20f;
                        mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);
                    }
                    break;
                case 1:
                    if (timer < 50)
                    {
                        float targetRot = -MathHelper.PiOver2 + player.direction * 1.2f;
                        mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.09f);
                        mainVec += Projectile.DirectionFrom(player.Center) * 3;
                        Projectile.rotation = mainVec.ToRotation();
                    }
                    if (timer == 70)
                    {
                        ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
                        Gsplayer.FlyCamPosition = new Vector2(0, 30).RotatedByRandom(6.283);
                    }
                    if (timer > 50 && timer < 75)
                    {
                        Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.15f);
                        Projectile.rotation -= player.direction * 0.24f;
                        mainVec = Vector2Elipse(250, Projectile.rotation, -1.2f, 0, 1000);
                    }
                    break;
                case 2:
                    trailLength = 400;
                    if (timer < 20)
                    {
                        float targetRot = 0;
                        if (player.direction == 1)
                        {
                            targetRot = -MathHelper.PiOver4 * 3;
                        }
                        mainVec = Vector2.Lerp(mainVec, Vector2Elipse(240, targetRot, 0.3f, 0.3f), 0.15f);
                        mainVec += Projectile.DirectionFrom(player.Center) * -3;
                        Projectile.rotation = mainVec.ToRotation();
                    }

                    if (timer == 50)
                    {
                        ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
                        Gsplayer.FlyCamPosition = new Vector2(0, 60).RotatedByRandom(6.283);
                    }
                    if (timer > 20 && timer < 55)
                    {
                        Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.0f, 0.75f);
                        Projectile.rotation += player.direction * 0.162f * (float)(1 - Math.Cos((timer - 20) / 17.5d * Math.PI));
                        mainVec = Vector2Elipse(240, Projectile.rotation, 0.3f, 0.3f, 10000);
                    }
                    break;
            }
        }
        public static Vector2 Vector2Elipse(float radius, float rot0, float rot1, float rot2 = 0, float viewZ = 1000)
        {
            Vector3 v = Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationZ(rot0)) * radius;
            v = Vector3.Transform(v, Matrix.CreateRotationX(-rot1));
            if (rot2 != 0)
            {
                v = Vector3.Transform(v, Matrix.CreateRotationZ(-rot2));
            }
            float k = -viewZ / (v.Z - viewZ);
            return k * new Vector2(v.X, v.Y);
        }
        public void DrawWarp()
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
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + trail[i] * Projectile.scale * 1.1f, new Color(dir, w, 0, 1), new Vector3(factor, 0, w)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/Warp").Value;//扭曲用贴图
            KEx.CurrentTechnique.Passes[0].Apply();
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            DrawOcclusion();
        }
        internal int timer2 = 0;
        public virtual float TrailAlpha(float factor)
        {
            float w;
            if (factor > 0.5f)
            {
                w = MathHelper.Lerp(0.5f, 0.7f, factor);
            }
            else
            {
                w = MathHelper.Lerp(0f, 0.5f, factor * 2f);
            }

            return w;
        }
        public string TrailShapeTex()
        {
            return "Everglow/Sources/Modules/YggdrasilModule/CorruptWormHive/Projectiles/FlameLine";
        }
        public string TrailColorTex()
        {
            return "Everglow/Sources/Modules/YggdrasilModule/CorruptWormHive/Projectiles/DeathSickle_Color";
        }
        public void DrawEffect()
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
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.05f * Projectile.scale, Color.White, new Vector3(factor * 1, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor * 2, 0, w)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            Effect MeleeTrail = ModContent.Request<Effect>("Everglow/Sources/Modules/YggdrasilModule/Effects/FlameTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            MeleeTrail.Parameters["uTime"].SetValue(timer2 * 0.007f);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MeleeTrail.CurrentTechnique.Passes["Trail"].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
            Player player = Main.player[Projectile.owner];
            Texture2D tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Handle");
            /*if (attackType == 1)
            {
                tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Shade_Handle_Flip");
            }
            if (attackType == 2)
            {
                tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Shade");
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (float)(Math.PI / 4d) * player.direction - 0.25f, new Vector2(0, player.direction == -1 ? 0 : tex.Height), 1.22f,player.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
                return;
            }*/
            float texWidth = 208;
            float texHeight = 188;
            float Size = 0.864f;
            double baseRotation = 0.7854;

            float exScale = 2;
            /*if (longHandle)
            {
                exScale += 1f;
            }*/
            //Vector2 origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);
            Vector2 origin = new Vector2(texWidth / 2, texHeight / 2);

            Vector2 Zoom = new Vector2(exScale * 1f * mainVec.Length() / tex.Width, 1.2f) * Projectile.scale;

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

            if (player.direction * player.gravDir == -1)
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
            return false;
        }
        public void DrawOcclusion()
        {
            Player player = Main.player[Projectile.owner];
            Texture2D tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Shade_Handle");
            /*if (attackType == 1)
            {
                tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Shade_Handle_Flip");
            }
            if (attackType == 2)
            {
                tex = YggdrasilContent.QuickTexture("CorruptWormHive/Projectiles/TrueDeathSickle_Shade");
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (float)(Math.PI / 4d) * player.direction - 0.25f, new Vector2(0, player.direction == -1 ? 0 : tex.Height), 1.22f,player.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
                return;
            }*/
            float texWidth = 208;
            float texHeight = 188;
            float Size = 0.864f;
            double baseRotation = 0.7854;

            float exScale = 2;
            /*if (longHandle)
            {
                exScale += 1f;
            }*/
            //Vector2 origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);
            Vector2 origin = new Vector2(texWidth / 2, texHeight / 2);

            Vector2 Zoom = new Vector2(exScale * 1f * mainVec.Length() / tex.Width, 1.2f) * Projectile.scale;

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

            if (player.direction * player.gravDir == -1)
            {
                sourceTopLeft = sourceBottomLeft;
                sourceTopRight = sourceBottomRight;
                sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
                sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            }

            Color lightColor = new Color(1f,1,1,1);
            if(timer > 50)
            {
                lightColor *= Math.Max(0, (225 - timer) / 175f);
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
        public Vector2 MainVec_WithoutGravDir
        {
            get
            {
                Player player = Main.player[Projectile.owner];
                Vector2 vec = mainVec;
                if (player.gravDir == -1)
                    vec.Y *= -1;
                return vec;
            }
        }
        public Vector2 MouseWorld_WithoutGravDir
        {
            get
            {
                Player player = Main.player[Projectile.owner];
                Vector2 vec = Main.MouseWorld;
                if (player.gravDir == -1)
                {
                    vec = WrapY(vec);
                }
                return vec;
            }
        }
        public Vector2 ProjCenter_WithoutGravDir
        {
            get
            {
                Player player = Main.player[Projectile.owner];
                Vector2 vec = Projectile.Center;
                if (player.gravDir == -1)
                {
                    vec = WrapY(vec);
                }
                return vec;
            }
        }
        private Vector2 WrapY(Vector2 vec)
        {
            vec.Y -= Main.screenPosition.Y;
            float d = vec.Y - Main.screenHeight / 2;
            vec.Y -= 2 * d;
            vec.Y += Main.screenPosition.Y;
            return vec;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 600);
        }
    }
}

