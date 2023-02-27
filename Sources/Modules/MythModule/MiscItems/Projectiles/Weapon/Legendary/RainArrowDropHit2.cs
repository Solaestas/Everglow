using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    class RainArrowDropHit2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
            Projectile.velocity *= 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        private Effect ef;
        float radious = 0;
        float FirstRo = 0;
        float SecondRo = 0;
        public override void PostDraw(Color lightColor)
        {
            if (FirstRo == 0)
            {
                FirstRo = Main.rand.NextFloat(0, 6.283f);
            }
            if (SecondRo == 0)
            {
                SecondRo = Main.rand.NextFloat(0, 6.283f);
            }
            Projectile.ai[0] = FirstRo;
            Projectile.ai[1] = SecondRo;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            List<Vertex2D> bars2 = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
            float widx = Projectile.timeLeft / 120f;
            float widxM = 1f - widx;
            radious = (float)(Math.Sqrt(5 * widxM) * 60);
            float width = widx * widx * 200f + 10;
            for (int i = 0; i < 201; ++i)
            {
                Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 100d * Math.PI + FirstRo);
                var normalDir = Vector2.Normalize(vDp);

                var factor = i / 50f;
                var color = Color.Lime;
                var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                float fx = 1 + (float)(Math.Sin(i / 20d * Math.PI) * 0.02f);
                bars.Add(new Vertex2D(vDp * fx + Projectile.Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                if (width > vDp.Length() * fx)
                {
                    bars.Add(new Vertex2D(Projectile.Center, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                else
                {
                    bars.Add(new Vertex2D(vDp * fx + Projectile.Center + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }
            for (int i = 0; i < 201; ++i)
            {
                Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 100d * Math.PI + SecondRo);
                var normalDir = Vector2.Normalize(vDp);

                var factor = i / 50f;
                var color = Color.Blue;
                var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                float fx = 1 + (float)(Math.Sin((i + 50 - widxM * 40f) / 20d * Math.PI) * 0.02f);
                bars2.Add(new Vertex2D(vDp * fx + Projectile.Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                if (width > vDp.Length() * fx)
                {
                    bars2.Add(new Vertex2D(Projectile.Center, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                else
                {
                    bars2.Add(new Vertex2D(vDp * fx + Projectile.Center + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }

            List<Vertex2D> triangleList = new List<Vertex2D>();
            List<Vertex2D> triangleList2 = new List<Vertex2D>();


            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
                triangleList.Add(bars[1]);
                triangleList.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                ef.Parameters["maxr"].SetValue(widxM * widxM - 0.2f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlueRain").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightWave").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightWave").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }

            if (bars2.Count > 2)
            {
                triangleList2.Add(bars2[0]);
                var vertex = new Vertex2D((bars2[0].position + bars2[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
                triangleList2.Add(bars2[1]);
                triangleList2.Add(vertex);
                for (int i = 0; i < bars2.Count - 2; i += 2)
                {
                    triangleList2.Add(bars2[i]);
                    triangleList2.Add(bars2[i + 2]);
                    triangleList2.Add(bars2[i + 1]);

                    triangleList2.Add(bars2[i + 1]);
                    triangleList2.Add(bars2[i + 2]);
                    triangleList2.Add(bars2[i + 3]);
                }
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                ef.Parameters["maxr"].SetValue(widxM * widxM - 0.1f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlueRain").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceEpsilon").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceEpsilon").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        public static float[,] widHepuyuan = new float[1000, 60];
        public static Vector2[,] OldplCenHepuyuan = new Vector2[1000, 60];
        public static void DrawAll(SpriteBatch sb)
        {
            for (int s = 0; s < Main.projectile.Length; s++)
            {
                if (Main.projectile[s].active)
                {
                    if (Main.projectile[s].type == ModContent.ProjectileType<SlingshotHitProjectile>() || Main.projectile[s].type == ModContent.ProjectileType<KSSlingshotHit>()) //Was ModContent.ProjectileType<Slingshots.SlingshotHit>()
					{
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                        List<Vertex2D> bars = new List<Vertex2D>();
                        Effect ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeSlingshot").Value;
                        float widx = Main.projectile[s].timeLeft / 120f;
                        float widxM = 1f - widx;
                        float radious = (float)(Math.Sqrt(5 * widxM) * 20);
                        float width = widx * widx * 80f + 10;
                        for (int i = 0; i < 41; ++i)
                        {
                            Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 20d * Math.PI);
                            var normalDir = Vector2.Normalize(vDp);

                            var factor = i / 12.5f;
                            var color = Color.Lime;
                            var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                            float delk0 = (width - radious) / (float)(width) / 2f;
                            if (delk0 < 0)
                            {
                                delk0 = 0;
                            }
                            bars.Add(new Vertex2D(vDp + Main.projectile[s].Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                            bars.Add(new Vertex2D(vDp + Main.projectile[s].Center + normalDir * -(Math.Clamp(width, 0, radious)), color, new Vector3((float)Math.Sqrt(factor), delk0, w)));
                        }

                        List<Vertex2D> triangleList = new List<Vertex2D>();

                        if (bars.Count > 2)
                        {
                            triangleList.Add(bars[0]);
                            var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Main.projectile[s].velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
                            triangleList.Add(bars[1]);
                            triangleList.Add(vertex);
                            for (int i = 0; i < bars.Count - 2; i += 2)
                            {
                                triangleList.Add(bars[i]);
                                triangleList.Add(bars[i + 2]);
                                triangleList.Add(bars[i + 1]);

                                triangleList.Add(bars[i + 1]);
                                triangleList.Add(bars[i + 2]);
                                triangleList.Add(bars[i + 3]);
                            }
                            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                            ef.Parameters["uTransform"].SetValue(model * projection);
                            ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                            ef.Parameters["maxr"].SetValue(widxM * widxM);
                            ef.Parameters["stre"].SetValue(Main.projectile[s].ai[0]);
                            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapGrey").Value;
                            Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceGamma").Value;
                            Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceGamma").Value;
                            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                            ef.CurrentTechnique.Passes[0].Apply();

                            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                            Main.graphics.GraphicsDevice.RasterizerState = originalState;
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        }
                    }
                    if (Main.projectile[s].type == ModContent.ProjectileType<SlingshotAmmo>() || Main.projectile[s].type == ModContent.ProjectileType<GelBall>())
                    {
                        float DrawC = Math.Clamp((Main.projectile[s].velocity.Length() - 12) / 24f, 0, 1f);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        List<Vertex2D> bars = new List<Vertex2D>();
                        int TrueL = 1;
                        for (int i = 1; i < Main.projectile[s].oldPos.Length; ++i)
                        {
                            if (Main.projectile[s].oldPos[i] == Vector2.Zero)
                                break;
                            TrueL++;
                        }
                        for (int i = 1; i < TrueL; ++i)
                        {
                            if (Main.projectile[s].oldPos[i] == Vector2.Zero)
                                break;
                            float width = 6;
                            if (Main.projectile[s].timeLeft > 30)
                            {
                                width = 6;
                            }
                            else
                            {
                                width = Main.projectile[s].timeLeft / 5f;
                            }
                            var normalDir = Main.projectile[s].oldPos[i - 1] - Main.projectile[s].oldPos[i];
                            if (normalDir.Length() < 0.2f)
                            {
                                normalDir = Main.projectile[s].velocity / Main.projectile[s].velocity.Length();
                            }
                            normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                            var factor = i / (float)TrueL;
                            var color = Color.Lerp(new Color(DrawC, DrawC, DrawC, 0), new Color(0, 0, 0, 0), factor);
                            var w = MathHelper.Lerp(1f, 0.05f, factor);

                            bars.Add(new Vertex2D(Main.projectile[s].oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                            bars.Add(new Vertex2D(Main.projectile[s].oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                        }
                        List<Vertex2D> triangleList = new List<Vertex2D>();
                        if (bars.Count > 2)
                        {
                            triangleList.Add(bars[0]);
                            Vector2 va = Main.projectile[s].velocity * 1.5f;
                            if (Main.projectile[s].ai[0] <= 44 && Main.projectile[s].ai[0] > 0)
                            {
                                va = Main.projectile[s].velocity * 0.05f;
                            }
                            var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + va, new Color(DrawC, DrawC, DrawC, 0), new Vector3(0, 0.5f, 1));
                            triangleList.Add(bars[1]);
                            triangleList.Add(vertex);
                            for (int i = 0; i < bars.Count - 2; i += 2)
                            {
                                triangleList.Add(bars[i]);
                                triangleList.Add(bars[i + 2]);
                                triangleList.Add(bars[i + 1]);

                                triangleList.Add(bars[i + 1]);
                                triangleList.Add(bars[i + 2]);
                                triangleList.Add(bars[i + 3]);
                            }
                            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/EShootDark").Value;
                            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        }
                    }
                    if (Main.projectile[s].type == ModContent.ProjectileType<RainArrowDropHit2>())
                    {
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                        List<Vertex2D> bars = new List<Vertex2D>();
                        List<Vertex2D> bars2 = new List<Vertex2D>();
                        Effect ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
                        float widx = Main.projectile[s].timeLeft / 120f;
                        float widxM = 1f - widx;
                        float radious = (float)(Math.Sqrt(5 * widxM) * 60);
                        float width = widx * widx * 200f + 10;
                        for (int i = 0; i < 201; ++i)
                        {
                            Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 100d * Math.PI + Main.projectile[s].ai[0]);
                            var normalDir = Vector2.Normalize(vDp);

                            var factor = i / 50f;
                            var color = Color.Lime;
                            var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                            float fx = 1 + (float)(Math.Sin(i / 20d * Math.PI) * 0.02f);
                            bars.Add(new Vertex2D(vDp * fx + Main.projectile[s].Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                            if (width > vDp.Length() * fx)
                            {
                                bars.Add(new Vertex2D(Main.projectile[s].Center, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                            }
                            else
                            {
                                bars.Add(new Vertex2D(vDp * fx + Main.projectile[s].Center + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                            }
                        }
                        for (int i = 0; i < 201; ++i)
                        {
                            Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 100d * Math.PI + Main.projectile[s].ai[1]);
                            var normalDir = Vector2.Normalize(vDp);

                            var factor = i / 50f;
                            var color = Color.Blue;
                            var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                            float fx = 1 + (float)(Math.Sin((i + 50 - widxM * 40f) / 20d * Math.PI) * 0.02f);
                            bars2.Add(new Vertex2D(vDp * fx + Main.projectile[s].Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                            if (width > vDp.Length() * fx)
                            {
                                bars2.Add(new Vertex2D(Main.projectile[s].Center, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                            }
                            else
                            {
                                bars2.Add(new Vertex2D(vDp * fx + Main.projectile[s].Center + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                            }
                        }

                        List<Vertex2D> triangleList = new List<Vertex2D>();
                        List<Vertex2D> triangleList2 = new List<Vertex2D>();


                        if (bars.Count > 2)
                        {
                            triangleList.Add(bars[0]);
                            var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f, Color.White, new Vector3(0, 0.5f, 1));
                            triangleList.Add(bars[1]);
                            triangleList.Add(vertex);
                            for (int i = 0; i < bars.Count - 2; i += 2)
                            {
                                triangleList.Add(bars[i]);
                                triangleList.Add(bars[i + 2]);
                                triangleList.Add(bars[i + 1]);

                                triangleList.Add(bars[i + 1]);
                                triangleList.Add(bars[i + 2]);
                                triangleList.Add(bars[i + 3]);
                            }
                            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                            ef.Parameters["uTransform"].SetValue(model * projection);
                            ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                            ef.Parameters["maxr"].SetValue(widxM * widxM - 0.2f);
                            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlueRain").Value;
                            Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightWave").Value;
                            Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightWave").Value;
                            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                            ef.CurrentTechnique.Passes[0].Apply();

                            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                            Main.graphics.GraphicsDevice.RasterizerState = originalState;
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        }

                        if (bars2.Count > 2)
                        {
                            triangleList2.Add(bars2[0]);
                            var vertex = new Vertex2D((bars2[0].position + bars2[1].position) * 0.5f, Color.White, new Vector3(0, 0.5f, 1));
                            triangleList2.Add(bars2[1]);
                            triangleList2.Add(vertex);
                            for (int i = 0; i < bars2.Count - 2; i += 2)
                            {
                                triangleList2.Add(bars2[i]);
                                triangleList2.Add(bars2[i + 2]);
                                triangleList2.Add(bars2[i + 1]);

                                triangleList2.Add(bars2[i + 1]);
                                triangleList2.Add(bars2[i + 2]);
                                triangleList2.Add(bars2[i + 3]);
                            }
                            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                            ef.Parameters["uTransform"].SetValue(model * projection);
                            ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                            ef.Parameters["maxr"].SetValue(widxM * widxM - 0.1f);
                            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlueRain").Value;
                            Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceEpsilon").Value;
                            Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceEpsilon").Value;
                            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                            ef.CurrentTechnique.Passes[0].Apply();

                            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);

                            Main.graphics.GraphicsDevice.RasterizerState = originalState;
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        }
                    }
                    if (Main.projectile[s].type == ModContent.ProjectileType<RainArrowDropHit>())
                    {
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                        List<Vertex2D> bars = new List<Vertex2D>();
                        List<Vertex2D> bars2 = new List<Vertex2D>();
                        Effect ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
                        float widx = Main.projectile[s].timeLeft / 120f;
                        float widxM = 1f - widx;
                        float radious = (float)(Math.Sqrt(5 * widxM) * 100);
                        float width = widx * widx * 200f + 10;
                        for (int i = 0; i < 201; ++i)
                        {
                            Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 100d * Math.PI + Main.projectile[s].ai[0]);
                            var normalDir = Vector2.Normalize(vDp);

                            var factor = i / 50f;
                            var color = Color.Lime;
                            var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                            float fx = 1 + (float)(Math.Sin(i / 20d * Math.PI) * 0.02f);
                            bars.Add(new Vertex2D(vDp * fx + Main.projectile[s].Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                            if (width > vDp.Length() * fx)
                            {
                                bars.Add(new Vertex2D(Main.projectile[s].Center, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                            }
                            else
                            {
                                bars.Add(new Vertex2D(vDp * fx + Main.projectile[s].Center + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                            }
                        }
                        for (int i = 0; i < 201; ++i)
                        {
                            Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 100d * Math.PI + Main.projectile[s].ai[1]);
                            var normalDir = Vector2.Normalize(vDp);

                            var factor = i / 50f;
                            var color = Color.Blue;
                            var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                            float fx = 1 + (float)(Math.Sin((i + 50 - widxM * 40f) / 20d * Math.PI) * 0.02f);
                            bars2.Add(new Vertex2D(vDp * fx + Main.projectile[s].Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                            if (width > vDp.Length() * fx)
                            {
                                bars2.Add(new Vertex2D(Main.projectile[s].Center, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                            }
                            else
                            {
                                bars2.Add(new Vertex2D(vDp * fx + Main.projectile[s].Center + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                            }
                        }

                        List<Vertex2D> triangleList = new List<Vertex2D>();
                        List<Vertex2D> triangleList2 = new List<Vertex2D>();


                        if (bars.Count > 2)
                        {
                            triangleList.Add(bars[0]);
                            var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f, Color.White, new Vector3(0, 0.5f, 1));
                            triangleList.Add(bars[1]);
                            triangleList.Add(vertex);
                            for (int i = 0; i < bars.Count - 2; i += 2)
                            {
                                triangleList.Add(bars[i]);
                                triangleList.Add(bars[i + 2]);
                                triangleList.Add(bars[i + 1]);

                                triangleList.Add(bars[i + 1]);
                                triangleList.Add(bars[i + 2]);
                                triangleList.Add(bars[i + 3]);
                            }
                            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                            ef.Parameters["uTransform"].SetValue(model * projection);
                            ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                            ef.Parameters["maxr"].SetValue(widxM * widxM - 0.2f);
                            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlueRain").Value;
                            Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightWave").Value;
                            Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightWave").Value;
                            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                            ef.CurrentTechnique.Passes[0].Apply();

                            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                            Main.graphics.GraphicsDevice.RasterizerState = originalState;
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        }

                        if (bars2.Count > 2)
                        {
                            triangleList2.Add(bars2[0]);
                            var vertex = new Vertex2D((bars2[0].position + bars2[1].position) * 0.5f, Color.White, new Vector3(0, 0.5f, 1));
                            triangleList2.Add(bars2[1]);
                            triangleList2.Add(vertex);
                            for (int i = 0; i < bars2.Count - 2; i += 2)
                            {
                                triangleList2.Add(bars2[i]);
                                triangleList2.Add(bars2[i + 2]);
                                triangleList2.Add(bars2[i + 1]);

                                triangleList2.Add(bars2[i + 1]);
                                triangleList2.Add(bars2[i + 2]);
                                triangleList2.Add(bars2[i + 3]);
                            }
                            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                            ef.Parameters["uTransform"].SetValue(model * projection);
                            ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                            ef.Parameters["maxr"].SetValue(widxM * widxM - 0.1f);
                            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlueRain").Value;
                            Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceEpsilon").Value;
                            Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceEpsilon").Value;
                            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                            ef.CurrentTechnique.Passes[0].Apply();

                            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);

                            Main.graphics.GraphicsDevice.RasterizerState = originalState;
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        }
                    }
                    //TODO: Port the "World" sword, Hepuyuan Spear, and Ichor Ring
                    //if (Main.projectile[s].type == ModContent.ProjectileType<Accessory.IchorRing>())
                    //{
                    //    float Radius = (float)Math.Sin(Main.time / 140d) * 5 + 170f;
                    //    if (Main.projectile[s].timeLeft >= 660)
                    //    {
                    //        Radius = ((float)Math.Sin(Main.time / 140d) * 5 + 170f) * (720 - Main.projectile[s].timeLeft) / 60f;

                    //    }
                    //    if (Main.projectile[s].timeLeft <= 60)
                    //    {
                    //        Radius = ((float)Math.Sin(Main.time / 140d) * 5 + 170f) * (Main.projectile[s].timeLeft) / 60f;
                    //    }
                    //    Accessory.IchorRing.aiii += 0.04f;
                    //    Main.spriteBatch.End();
                    //    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    //    List<Vertex2D> Vx = new List<Vertex2D>();

                    //    Vector2 vf = Main.projectile[s].Center - Main.screenPosition;
                    //    for (int h = 0; h < 90; h++)
                    //    {
                    //        Vector2 v0 = new Vector2(Radius, 0).RotatedBy(h / 45d * Math.PI + Accessory.IchorRing.aiii);
                    //        Vector2 v1 = new Vector2(Radius, 0).RotatedBy((h + 1) / 45d * Math.PI + Accessory.IchorRing.aiii);
                    //        float Color4R = Radius / 420f;
                    //        float Color4G = ((float)((h) / 45d * Math.PI + Accessory.IchorRing.aiii + Math.PI * 2000000) % 6.28f) / 6.283f;
                    //        float Color5R = Radius / 420f;
                    //        float Color5G = ((float)((h + 1) / 45d * Math.PI + Accessory.IchorRing.aiii + Math.PI * 2000000) % 6.28f) / 6.283f;
                    //        Color color4 = new Color(Color4R, Color4G, 0, 0);
                    //        Color color5 = new Color(Color5R, Color5G, 0, 0);
                    //        Vx.Add(new Vertex2D(vf + v0, color4, new Vector3(h / 90f, 0, 0)));
                    //        Vx.Add(new Vertex2D(vf + v1, color5, new Vector3((h + 1) / 90f, 0, 0)));
                    //        Vx.Add(new Vertex2D(vf, Color.Transparent, new Vector3((h + 0.5f) / 90f, 1, 0)));
                    //    }
                    //    Texture2D t = ModContent.Request<Texture2D>("MythMod/Projectiles/Accessory/IchorBlue").Value;
                    //    Main.graphics.GraphicsDevice.Textures[0] = t;
                    //    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
                    //    Main.spriteBatch.End();
                    //    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                    //    Main.spriteBatch.End();
                    //    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    //    List<Vertex2D> Vx2 = new List<Vertex2D>();

                    //    for (int h = 0; h < 90; h++)
                    //    {
                    //        Vector2 v0 = new Vector2(0, Radius).RotatedBy(h / 45d * Math.PI - Accessory.IchorRing.aiii);
                    //        Vector2 v1 = new Vector2(0, Radius).RotatedBy((h + 1) / 45d * Math.PI - Accessory.IchorRing.aiii);
                    //        float Color4R = Radius / 420f;
                    //        float Color4G = ((float)((h) / 45d * Math.PI - Accessory.IchorRing.aiii + Math.PI * 2000000) % 6.28f) / 6.283f;
                    //        float Color5R = Radius / 420f;
                    //        float Color5G = ((float)((h + 1) / 45d * Math.PI - Accessory.IchorRing.aiii + Math.PI * 2000000) % 6.28f) / 6.283f;
                    //        Color color4 = new Color(Color4R, Color4G, 0, 0);
                    //        Color color5 = new Color(Color5R, Color5G, 0, 0);
                    //        Vx2.Add(new Vertex2D(vf + v0, color4, new Vector3(h / 90f, 0, 0)));
                    //        Vx2.Add(new Vertex2D(vf + v1, color5, new Vector3((h + 1) / 90f, 0, 0)));
                    //        Vx2.Add(new Vertex2D(vf, Color.Transparent, new Vector3((h + 0.5f) / 90f, 1, 0)));
                    //    }
                    //    t = ModContent.Request<Texture2D>("MythMod/Projectiles/Accessory/IchorBlue").Value;
                    //    Main.graphics.GraphicsDevice.Textures[0] = t;
                    //    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);
                    //    Main.spriteBatch.End();
                    //    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    //}


                    if (Main.projectile[s].type == ModContent.ProjectileType<Melee.Hepuyuan.Hepuyuan>() || Main.projectile[s].type == ModContent.ProjectileType<Melee.Hepuyuan.HepuyuanDown>())
                    {
                        Player player = Main.player[Main.projectile[s].owner];
                        Vector2 FirstVel = Vector2.Zero;
                        if (!Main.gamePaused)
                        {
                            OldplCenHepuyuan[s, 0] = Main.projectile[s].Center - Vector2.Normalize(Main.projectile[s].velocity) * 15f;//记录位置
                            for (int f = 59; f > 0; f--)
                            {
                                OldplCenHepuyuan[s, f] = OldplCenHepuyuan[s, f - 1];
                            }
                            widHepuyuan[s, 0] = Math.Clamp(Main.projectile[s].velocity.Length() / 6f, 0, 60);//宽度
                            for (int f = 59; f > 0; f--)
                            {
                                widHepuyuan[s, f] = widHepuyuan[s, f - 1];
                            }
                        }
                        if (FirstVel == Vector2.Zero)
                        {
                            FirstVel = Vector2.Normalize(Main.projectile[s].velocity);
                        }
                        if (Main.projectile[s].timeLeft == 1)
                        {
                            for (int f = 59; f >= 0; f--)
                            {
                                OldplCenHepuyuan[s, f] = Vector2.Zero;
                            }
                        }
                        Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);
                        for (int d = 0; d < 7; d++)
                        {
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                            List<Vertex2D> VxII = new List<Vertex2D>();
                            List<Vertex2D> barsII = new List<Vertex2D>();
                            Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 3d * Math.PI + Main.time / 7d);
                            float widk = Vector2.Dot(Vector2.Normalize(deltaPos), Vector2.Normalize(Main.projectile[s].velocity)) + 1.2f;
                            float widV = (float)Math.Clamp(1.6 - Math.Sqrt(Main.projectile[s].velocity.Length() / 16f), 0, 1.6f);
                            if (d == 0)
                            {
                                deltaPos *= 0;
                                widk = 4f * Main.projectile[s].timeLeft / 60f;
                            }
                            for (int i = 1; i < 60; ++i)
                            {
                                if (OldplCenHepuyuan[s, i] == Vector2.Zero)
                                    break;
                                var factor = i / 60f;
                                float Color4R = Math.Clamp((float)Math.Sqrt(widV) / 3f, 0, 1);
                                float Color4G = (float)((Math.Atan2(FlipVel.Y, FlipVel.X) + 62.83) % 62.83) / 6.283f;
                                Color color4 = new Color(Color4R, Color4G, 0, 0);
                                barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV * 1.5f + OldplCenHepuyuan[s, i] + FlipVel * widHepuyuan[s, i] * widk * 2 - Main.screenPosition, color4, new Vector3((float)Math.Sqrt(factor), 1, 0)));
                                barsII.Add(new Vertex2D(deltaPos * (float)(Math.Clamp(Math.Sqrt(factor * 3), 0, 1)) * widV * 1.5f + OldplCenHepuyuan[s, i] - FlipVel * widHepuyuan[s, i] * widk * 2 - Main.screenPosition, color4, new Vector3((float)Math.Sqrt(factor), 0, 0)));
                            }
                            if (barsII.Count > 2)
                            {
                                VxII.Add(barsII[0]);
                                var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vector2.Normalize(Main.projectile[s].velocity) * 30, new Color(255, 255, 255, 255), new Vector3(0, 0.5f, 1));
                                VxII.Add(barsII[1]);
                                VxII.Add(vertex);
                                for (int i = 0; i < barsII.Count - 2; i += 2)
                                {
                                    VxII.Add(barsII[i]);
                                    VxII.Add(barsII[i + 2]);
                                    VxII.Add(barsII[i + 1]);

                                    VxII.Add(barsII[i + 1]);
                                    VxII.Add(barsII[i + 2]);
                                    VxII.Add(barsII[i + 3]);
                                }
                            }

                            Texture2D t0 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/WorldFade").Value;
                            Main.graphics.GraphicsDevice.Textures[0] = t0;//GlodenBloodScaleMirror
                            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);
                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                        }
                    }
                }
            }
        }
    }
}
