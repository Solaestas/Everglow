namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    class ToothMagicHit : ModProjectile
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
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        private Effect ef;
        float radious = 0;
        float FirstRo = 0;
        float SecondRo = 0;
        float[] Ro = new float[5];
        float[] Uy = new float[5];
        public override void PostDraw(Color lightColor)
        {
            if (Ro[0] == 0)
            {
                for (int i = 0; i < 5; ++i)
                {
                    Ro[i] = Main.rand.NextFloat(0, 6.283f);
                }
            }
            if (Uy[0] == 0)
            {
                for (int i = 0; i < 5; ++i)
                {
                    Uy[i] = Main.rand.NextFloat(8f, 50f);
                }
            }

            if (FirstRo == 0)
            {
                FirstRo = Main.rand.NextFloat(0, 6.283f);
            }
            if (SecondRo == 0)
            {
                SecondRo = Main.rand.NextFloat(0, 6.283f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeLaserRed").Value;
            float widx = Projectile.timeLeft / 120f;
            float widxM = 1f - widx;
            radious = (float)(Math.Sqrt(5 * widxM) * 40);
            float width = widx * widx * 120f + 10;
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
                ef.Parameters["maxr"].SetValue(widxM * widxM);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapTuskLine").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightCrackF").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightCrackF").Value;
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
    }
}
