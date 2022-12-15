namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    class ToothMagicHit2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
        }

        Vector3[] CirclePoint = new Vector3[120];
        float Rad = 0;
        Vector2[] Circle2D = new Vector2[120];
        float[] Ro = new float[5];
        float[] Uy = new float[5];
        float[] AdUy = new float[5];
        private Effect ef;
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
            if (AdUy[0] == 0)
            {
                for (int i = 0; i < 5; ++i)
                {
                    AdUy[i] = Main.rand.NextFloat(-0.15f, 0.15f);
                }
            }
            if (DeltaRed[0] == 0)
            {
                for (int i = 0; i < 5; ++i)
                {
                    DeltaRed[i] = Main.rand.NextFloat(-0.3f, 0.3f);
                }
            }
            if (!Main.gamePaused)
            {
                for (int i = 0; i < 5; ++i)
                {
                    Uy[i] += AdUy[i];
                }
            }
            for (int g = 0; g < 5; g++)
            {
                float widx = Projectile.timeLeft / 120f;
                float widxM = 1f - widx;
                Rad = (float)(Math.Sqrt(5 * widxM) * 40);
                for (int d = 0; d < 120; d++)
                {
                    Circle2D[d] = new Vector2(30, 0).RotatedBy(d * Math.PI / 60d);
                    CirclePoint[d] = new Vector3(Circle2D[d].X, -Uy[g], 50 + Circle2D[d].Y);
                }
                for (int d = 0; d < 120; d++)
                {
                    Circle2D[d] = new Vector2(CirclePoint[d].X / (float)CirclePoint[d].Z, CirclePoint[d].Y / (float)CirclePoint[d].Z + Uy[g] / 50f) * Rad;
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                Vector2 Vbase = Projectile.Center;

                List<VertexBase.CustomVertexInfo> Vx3 = new List<VertexBase.CustomVertexInfo>();
                for (int h = 0; h < 120; h++)
                {
                    var factor = h / 120f;
                    Vx3.Add(new VertexBase.CustomVertexInfo(Vbase + Circle2D[h].RotatedBy(Ro[g]) * 0.5f, Color.Lime, new Vector3(factor, 1, 0.35f)));
                    Vx3.Add(new VertexBase.CustomVertexInfo(Vbase + Circle2D[h].RotatedBy(Ro[g]) * 1.2f, Color.Lime, new Vector3(factor, 0, 0.35f)));
                }
                ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeLaserRed").Value;
                List<VertexBase.CustomVertexInfo> triangleList = new List<VertexBase.CustomVertexInfo>();
                if (Vx3.Count > 2)
                {
                    triangleList.Add(Vx3[0]);
                    var vertex = new VertexBase.CustomVertexInfo((Vx3[0].Position + Vx3[1].Position) * 0.5f, Color.White, new Vector3(0, 0.5f, 1));
                    triangleList.Add(Vx3[1]);
                    triangleList.Add(vertex);
                    for (int i = 0; i < Vx3.Count - 2; i += 2)
                    {
                        triangleList.Add(Vx3[i]);
                        triangleList.Add(Vx3[i + 2]);
                        triangleList.Add(Vx3[i + 1]);

                        triangleList.Add(Vx3[i + 1]);
                        triangleList.Add(Vx3[i + 2]);
                        triangleList.Add(Vx3[i + 3]);
                    }
                    RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                    var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                    var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                    ef.Parameters["uTransform"].SetValue(model * projection);
                    ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                    ef.Parameters["maxr"].SetValue(Math.Clamp(widxM * widxM + DeltaRed[g], 0, 1));
                    Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/VisualTextures/heatmapTuskLine").Value;
                    Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/VisualTextures/LightCrackF").Value;
                    Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/VisualTextures/LightCrackF").Value;
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
        float[] DeltaRed = new float[5];
    }
}
