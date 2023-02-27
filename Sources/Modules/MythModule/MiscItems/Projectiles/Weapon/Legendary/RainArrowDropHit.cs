using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    class RainArrowDropHit : ModProjectile
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
            List<Vertex2D> bars3 = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
            float widx = Projectile.timeLeft / 120f;
            float widxM = 1f - widx;
            radious = (float)(Math.Sqrt(5 * widxM) * 100);
            float width = widx * widx * 240f + 10;
            for (int i = 0; i < 201; ++i)
            {
                Vector2 vDp = new Vector2(0, radious).RotatedBy(i / 100d * Math.PI + FirstRo);
                var normalDir = Vector2.Normalize(vDp);

                var factor = i / 50f;
                var color = Color.Blue;
                var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
                float fx = 1 + (float)(Math.Sin(i / 20d * Math.PI) * 0.03f);
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
        }
    }
}
