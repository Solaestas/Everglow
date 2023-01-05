using Everglow.Sources.Modules.MythModule.TheTusk;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon
{
    class XiaoHit : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
            Projectile.velocity *= 0;
            for (int i = 0; i < 23; i++)
            {
                if (DrawLine[i, 0] == Vector2.Zero)
                {
                    for (int j = 0; j < 18; j++)
                    {
                        DrawLine[i, j] = Projectile.Center;
                    }
                    DrawLineVelocity[i] = new Vector2(Main.rand.NextFloat(17f, 27f), 0).RotatedByRandom(6.283) * Projectile.ai[0];
                }
                DrawLine[i, 0] += DrawLineVelocity[i];
                DrawLineVelocity[i] *= 0.9f;
                for (int j = 17; j > 0; j--)//记录每一组流星火位置
                {
                    DrawLine[i, j] = DrawLine[i, j - 1];
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        private Effect ef;
        float radious = 0;
        float FirstRo = 0;
        float SecondRo = 0;
        Vector2[,] DrawLine = new Vector2[23, 18];
        Vector2[] DrawLineVelocity = new Vector2[23];
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
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeCurseGreen").Value;
            float widx = Projectile.timeLeft / 120f;
            float widxM = 1f - widx;
            radious = (float)(Math.Sqrt(5 * widxM) * 60) * Projectile.ai[0];
            /*float width = widx * widx * 80f + 10;
            for (int i = 0; i < 201; ++i)
            {
                Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 100d * Math.PI + FirstRo);
                var normalDir = Vector2.Normalize(vDp);

                var factor = i / 50f;
                var color = Color.Lime;
                var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                bars.Add(new VertexBase.CustomVertexInfo(vDp + Projectile.Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new VertexBase.CustomVertexInfo(vDp + Projectile.Center + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<VertexBase.CustomVertexInfo> triangleList = new List<VertexBase.CustomVertexInfo>();

            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
                ef.Parameters["maxr"].SetValue(widxM * widxM + 0.4f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapWindCyan").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ShakeWave").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ShakeWave").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }*/
            for (int i = 0; i < 23; i++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                List<VertexBase.CustomVertexInfo> barsII = new List<VertexBase.CustomVertexInfo>();


                for (int z = 1; z < 18; ++z)
                {
                    float widthII = Math.Clamp((DrawLine[i, z] - DrawLine[i, z - 1]).Length(), 0, 20 * Projectile.ai[0]);//宽度为距离(速度决定,上限20)
                    if (z > 13)
                    {
                        widthII *= (18 - z) / 5f;
                    }
                    var normalDir = Vector2.Normalize(DrawLine[i, z] - DrawLine[i, z - 1]).RotatedBy(1.57);
                    var factor = z / 18f;
                    var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                    barsII.Add(new VertexBase.CustomVertexInfo(DrawLine[i, z] + normalDir * widthII, Color.White, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    barsII.Add(new VertexBase.CustomVertexInfo(DrawLine[i, z] + normalDir * -widthII, Color.White, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }

                List<VertexBase.CustomVertexInfo> Vx = new List<VertexBase.CustomVertexInfo>();

                if (barsII.Count > 2)
                {
                    Vx.Add(barsII[0]);
                    var vertex = new VertexBase.CustomVertexInfo(DrawLine[i, 0], Color.White, new Vector3(0, 0.5f, 1));
                    Vx.Add(barsII[1]);
                    Vx.Add(vertex);
                    for (int z = 0; z < barsII.Count - 2; z += 2)
                    {
                        Vx.Add(barsII[z]);
                        Vx.Add(barsII[z + 2]);
                        Vx.Add(barsII[z + 1]);

                        Vx.Add(barsII[z + 1]);
                        Vx.Add(barsII[z + 2]);
                        Vx.Add(barsII[z + 3]);
                    }
                    RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                    var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                    var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                    ef.Parameters["uTransform"].SetValue(model * projection);
                    ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                    ef.Parameters["maxr"].SetValue(widxM * widxM);
                    Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapWindCyan").Value;
                    Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightlineOppo").Value;
                    Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightlineOppo").Value;
                    Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                    Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                    Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                    ef.CurrentTechnique.Passes[0].Apply();

                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);

                    Main.graphics.GraphicsDevice.RasterizerState = originalState;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
        }
    }
}
