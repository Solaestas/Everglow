using Terraria.Graphics.Effects;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheTusk.WorldGeneration;
namespace Everglow.Sources.Modules.MythModule.TheTusk.Backgrounds
{
    public class TuskBiomeSky : CustomSky
    {
        public static bool Open = false;
        //public static bool HasSky = false;
        //private bool skyActive;
        public override void OnLoad()
        {

        }
        public override void Deactivate(params object[] args)
        {
            //HasSky = false;
            //this.skyActive = false;
        }

        public override void Reset()
        {
            //HasSky = false;
            //this.skyActive = false;
        }

        public override bool IsActive()
        {
            return /*this.skyActive ||*/ this.opacity > 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            //HasSky = true;
            //this.skyActive = true;
        }
        /// <summary>
        /// 获取绘制矩形
        /// </summary>
        /// <param name="texSize"></param>
        /// <param name="MoveStep"></param>
        /// <returns></returns>
        public Rectangle GetDrawRect(Vector2 texSize, float MoveStep, float MulSize = 1)
        {
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + (texSize / 2);
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / MulSize;
            TuskGen tuskGen = ModContent.GetInstance<TuskGen>();
            Vector2 TuskBiomeCenter = new Vector2(tuskGen.tuskCenterX, tuskGen.tuskCenterY) * 16;
            Vector2 deltaPos = Main.screenPosition - TuskBiomeCenter;
            deltaPos *= MoveStep;
            int RX = (int)(sampleCenter.X - screenSize.X / 2f + deltaPos.X);
            int RY = (int)(sampleCenter.Y - screenSize.Y / 2f + deltaPos.Y);

            return new Rectangle(RX, RY, (int)(screenSize.X), (int)(screenSize.Y));
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            TuskGen tuskGen = ModContent.GetInstance<TuskGen>();
            Vector2 TuskBiomeCenter = new Vector2(tuskGen.tuskCenterX, tuskGen.tuskCenterY) * 16;
            Vector2 TuskBiomeCenterToScreenPosition = Main.screenPosition - TuskBiomeCenter;

            Color DrawC = Main.ColorOfTheSkies * opacity;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 SkyBase = new Vector2(Main.screenWidth / 2f, Main.screenHeight * 1.0f - 160f) - TuskBiomeCenterToScreenPosition * 0.01f;
            Vector2 SkyVortex = SkyBase + new Vector2(1210, 161)/*图心偏移坐标*/ - new Vector2(1024, 600)/*绘制中心*/;

            Vector2[] SkyVortexOvalCenter = new Vector2[20];
            SkyVortexOvalCenter[1] = SkyVortex + new Vector2(-184, -347)/*椭圆中心偏移坐标*/;
            SkyVortexOvalCenter[2] = SkyVortex + new Vector2(-184, -347)/*椭圆中心偏移坐标*/;
            SkyVortexOvalCenter[3] = SkyVortex + new Vector2(-77, -264)/*椭圆中心偏移坐标*/;
            SkyVortexOvalCenter[4] = SkyVortex + new Vector2(-38, -127)/*椭圆中心偏移坐标*/;
            SkyVortexOvalCenter[5] = SkyVortex + new Vector2(-38, -127)/*椭圆中心偏移坐标*/;
            SkyVortexOvalCenter[6] = SkyVortex + new Vector2(-9, -75)/*椭圆中心偏移坐标*/;
            SkyVortexOvalCenter[7] = SkyVortex + new Vector2(-1, -28)/*椭圆中心偏移坐标*/;
            SkyVortexOvalCenter[14] = SkyVortex + new Vector2(0)/*椭圆中心偏移坐标*/;
            SkyVortexOvalCenter[15] = SkyVortex + new Vector2(0)/*椭圆中心偏移坐标*/;

            Vector2[,] SkyHalo = new Vector2[20, 200];
            for (int i = 0; i < 20; i++)
            {
                float Blength = 2242;
                float ABdevide = 0.32085f;
                double OneDevideRotaSpeed = 2500d;
                if (i == 1)
                {
                    Blength = 2102;
                    ABdevide = 0.32085f;
                    OneDevideRotaSpeed = 10500d;
                }
                if (i == 2)
                {
                    Blength = 1842;
                    ABdevide = 0.32085f;
                    OneDevideRotaSpeed = 7000d;
                }
                if (i == 3)
                {
                    Blength = 1330;
                    ABdevide = 0.31429f;
                    OneDevideRotaSpeed = 4300d;
                }
                if (i == 4)
                {
                    Blength = 611;
                    ABdevide = 0.37643f;
                    OneDevideRotaSpeed = 2200d;
                }
                if (i == 5)
                {
                    Blength = 611;
                    ABdevide = 0.37643f;
                    OneDevideRotaSpeed = 1000d;
                }
                if (i == 6)
                {
                    Blength = 430;
                    ABdevide = 0.37442f;
                    OneDevideRotaSpeed = 700d;
                }
                if (i == 7)
                {
                    Blength = 281;
                    ABdevide = 0.43416f;
                    OneDevideRotaSpeed = 400d;
                }
                if (i == 14)
                {
                    Blength = 190;
                    ABdevide = 0.43684f;
                    OneDevideRotaSpeed = 300d;
                }
                if (i == 15)
                {
                    Blength = 190;
                    ABdevide = 0.43684f;
                    OneDevideRotaSpeed = 220d;
                }
                for (int u = 0; u < 200; u++)
                {
                    SkyHalo[i, u] = new Vector2(0, Blength).RotatedBy(u / 100d * Math.PI + Main.time / OneDevideRotaSpeed);
                    SkyHalo[i, u].Y *= ABdevide;
                    SkyHalo[i, u] = SkyHalo[i, u].RotatedBy(1 / 60d * Math.PI);
                }
            }
            List<Vertex2D> Vsky = new List<Vertex2D>();
            Vsky.Add(new Vertex2D(SkyBase + new Vector2(1024, -1515), DrawC, new Vector3(1, 0, 0)));
            Vsky.Add(new Vertex2D(SkyBase + new Vector2(1024, 285), DrawC, new Vector3(1, 1, 0)));
            Vsky.Add(new Vertex2D(SkyBase + new Vector2(-1024, 285), DrawC, new Vector3(0, 1, 0)));

            Vsky.Add(new Vertex2D(SkyBase + new Vector2(1024, -1515), DrawC, new Vector3(1, 0, 0)));
            Vsky.Add(new Vertex2D(SkyBase + new Vector2(-1024, -1515), DrawC, new Vector3(0, 0, 0)));
            Vsky.Add(new Vertex2D(SkyBase + new Vector2(-1024, 285), DrawC, new Vector3(0, 1, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/TuskBiomeSky").Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vsky.ToArray(), 0, Vsky.Count / 3);

            Texture2D[] CloudLine = new Texture2D[20];
            CloudLine[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkBlueGreenCloud").Value;
            CloudLine[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkGreenCloud").Value;
            CloudLine[3] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/BlackCloud").Value;
            CloudLine[4] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkGreyCloud").Value;
            CloudLine[5] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkGreyCloud2").Value;
            CloudLine[6] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/LightGreyCloud3").Value;
            CloudLine[7] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/LightGreyCloud4").Value;
            CloudLine[14] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/LightGreyCloud").Value;
            CloudLine[15] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/LightGreyCloud2").Value;
            for (int i = 0; i < 20; i++)
            {
                List<Vertex2D> Vx = new List<Vertex2D>();
                for (int u = 0; u < 200; u++)
                {
                    if (SkyHalo[i, u] == Vector2.Zero)
                    {
                        break;
                    }
                    if (u < 199)
                    {
                        Vx.Add(new Vertex2D(SkyVortexOvalCenter[i] + SkyHalo[i, u] + new Vector2(0, -20), DrawC, new Vector3(u / 200f, 0, 0)));
                        Vx.Add(new Vertex2D(SkyVortexOvalCenter[i] + SkyHalo[i, u + 1] + new Vector2(0, -20), DrawC, new Vector3((u + 1) / 200f, 0, 0)));
                        Vx.Add(new Vertex2D(SkyVortexOvalCenter[i] + new Vector2(0, 20), DrawC, new Vector3((u + 0.5f) / 200f, 1, 0)));
                    }
                    else
                    {
                        Vx.Add(new Vertex2D(SkyVortexOvalCenter[i] + SkyHalo[i, u] + new Vector2(0, -20), DrawC, new Vector3(u / 200f, 0, 0)));
                        Vx.Add(new Vertex2D(SkyVortexOvalCenter[i] + SkyHalo[i, 0] + new Vector2(0, -20), DrawC, new Vector3((u + 1) / 200f, 0, 0)));
                        Vx.Add(new Vertex2D(SkyVortexOvalCenter[i] + new Vector2(0, 20), DrawC, new Vector3((u + 0.5f) / 200f, 1, 0)));
                    }
                }
                Main.graphics.GraphicsDevice.Textures[0] = CloudLine[i];//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            }
            Vector2 FarS = new Vector2(Main.screenWidth / 2f, Main.screenHeight + 80) - TuskBiomeCenterToScreenPosition * 0.04f;
            List<Vertex2D> VskyF = new List<Vertex2D>();
            VskyF.Add(new Vertex2D(FarS + new Vector2(1024, -750), DrawC, new Vector3(1, 0, 0)));
            VskyF.Add(new Vertex2D(FarS + new Vector2(1024, 150), DrawC, new Vector3(1, 1, 0)));
            VskyF.Add(new Vertex2D(FarS + new Vector2(-1024, 150), DrawC, new Vector3(0, 1, 0)));

            VskyF.Add(new Vertex2D(FarS + new Vector2(1024, -750), DrawC, new Vector3(1, 0, 0)));
            VskyF.Add(new Vertex2D(FarS + new Vector2(-1024, -750), DrawC, new Vector3(0, 0, 0)));
            VskyF.Add(new Vertex2D(FarS + new Vector2(-1024, 150), DrawC, new Vector3(0, 1, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/TuskFar").Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VskyF.ToArray(), 0, VskyF.Count / 3);


            Main.spriteBatch.End();

            var texCloseII = MythContent.QuickTexture("TheTusk/Backgrounds/TuskMiddle");
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Rectangle rvcII = GetDrawRect(texCloseII.Size(), 0.27f, 2);
            rvcII.Y -= 0;
            rvcII.X += -360;

            float UpY = rvcII.Y / (float)texCloseII.Height;
            float DownY = (rvcII.Y + rvcII.Height) / (float)texCloseII.Height;
            List<Vertex2D> CloseII = new List<Vertex2D>
            {
                new Vertex2D(new Vector2(0, 0), DrawC, new Vector3(rvcII.X / (float)texCloseII.Width, UpY, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, 0), DrawC, new Vector3((rvcII.X + rvcII.Width) / (float)texCloseII.Width, UpY, 0)),
                new Vertex2D(new Vector2(0, Main.screenHeight), DrawC, new Vector3(rvcII.X / (float)texCloseII.Width, DownY, 0)),

                new Vertex2D(new Vector2(0, Main.screenHeight), DrawC, new Vector3(rvcII.X / (float)texCloseII.Width, DownY, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, 0), DrawC, new Vector3((rvcII.X + rvcII.Width) / (float)texCloseII.Width, UpY, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, Main.screenHeight), DrawC, new Vector3((rvcII.X + rvcII.Width) / (float)texCloseII.Width, DownY, 0))
            };
            Effect bgW = MythContent.QuickEffect("Effects/BackgroundWrapWithColor");
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            bgW.Parameters["uTransform"].SetValue(projection);
            bgW.Parameters["uTime"].SetValue(0.34f);
            bgW.Parameters["uColor"].SetValue(DrawC.ToVector4());
            bgW.CurrentTechnique.Passes[0].Apply();

            if (CloseII.Count > 2)
            {
                Main.graphics.GraphicsDevice.Textures[0] = texCloseII;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, CloseII.ToArray(), 0, 2);
            }
            if (DownY > 1)
            {
                float DrawY = (1 - UpY) / (DownY - UpY) * Main.screenHeight - 5;
                Main.spriteBatch.Draw(texCloseII, new Rectangle(0, (int)DrawY, Main.screenWidth, Main.screenHeight - (int)DrawY), new Rectangle(1000, 1100, 1, 1), DrawC);
            }


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override void Update(GameTime gameTime)
        {
            bool skyActive = TuskGen.TuskLandActive();
            if (skyActive && this.opacity < 1f)
            {
                this.opacity += 0.02f;
                return;
            }
            if (!skyActive && this.opacity > 0f)
            {
                this.opacity -= 0.02f;
            }
        }
        public override float GetCloudAlpha()
        {
            return (1f - this.opacity) * 0.97f + 0.03f;
        }

        private float opacity;
    }
}
