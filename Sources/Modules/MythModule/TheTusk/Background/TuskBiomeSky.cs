using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Background
{
    public class TuskBiomeSky : CustomSky
    {
        public static bool Open = false;
        public override void Deactivate(params object[] args)
        {
            HasSky = false;
            this.skyActive = false;
        }

        public override void Reset()
        {
            HasSky = false;
            this.skyActive = false;
        }

        public override bool IsActive()
        {
            return this.skyActive || this.opacity > 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            HasSky = true;
            this.skyActive = true;
        }
        public static Vector2 TuskC;
        public static Vector2 TuskM;
        public static Vector2 TuskF;
        public static Vector2 TuskS;
        public static bool HasSky = false;
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Color DrawC = Main.ColorOfTheSkies * opacity;
            Color DrawA = DrawC * Main.atmo;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (maxDepth >= 3E+38f && minDepth < 3E+38f)
            {
                List<VertexBase.CustomVertexInfo> VTsky = new List<VertexBase.CustomVertexInfo>();
                VTsky.Add(new VertexBase.CustomVertexInfo(new Vector2(Main.screenWidth, 0), DrawA, new Vector3(1, 0, 0)));
                VTsky.Add(new VertexBase.CustomVertexInfo(new Vector2(Main.screenWidth, Main.screenHeight), DrawA, new Vector3(1, 1, 0)));
                VTsky.Add(new VertexBase.CustomVertexInfo(new Vector2(0, Main.screenHeight), DrawA, new Vector3(0, 1, 0)));

                VTsky.Add(new VertexBase.CustomVertexInfo(new Vector2(Main.screenWidth, 0), DrawA, new Vector3(1, 0, 0)));
                VTsky.Add(new VertexBase.CustomVertexInfo(new Vector2(0, 0), DrawA, new Vector3(0, 0, 0)));
                VTsky.Add(new VertexBase.CustomVertexInfo(new Vector2(0, Main.screenHeight), DrawA, new Vector3(0, 1, 0)));
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/TuskBiomeSky").Value;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VTsky.ToArray(), 0, VTsky.Count / 3);
            }
            Vector2 SkyBase = new Vector2(Main.screenWidth / 2f, Main.screenHeight * 1.0f - 160f) - TuskS * 0.2f;
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
            List<VertexBase.CustomVertexInfo> Vsky = new List<VertexBase.CustomVertexInfo>();
            Vsky.Add(new VertexBase.CustomVertexInfo(SkyBase + new Vector2(1024, -1515), DrawC, new Vector3(1, 0, 0)));
            Vsky.Add(new VertexBase.CustomVertexInfo(SkyBase + new Vector2(1024, 285), DrawC, new Vector3(1, 1, 0)));
            Vsky.Add(new VertexBase.CustomVertexInfo(SkyBase + new Vector2(-1024, 285), DrawC, new Vector3(0, 1, 0)));

            Vsky.Add(new VertexBase.CustomVertexInfo(SkyBase + new Vector2(1024, -1515), DrawC, new Vector3(1, 0, 0)));
            Vsky.Add(new VertexBase.CustomVertexInfo(SkyBase + new Vector2(-1024, -1515), DrawC, new Vector3(0, 0, 0)));
            Vsky.Add(new VertexBase.CustomVertexInfo(SkyBase + new Vector2(-1024, 285), DrawC, new Vector3(0, 1, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/TuskBiomeSky").Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vsky.ToArray(), 0, Vsky.Count / 3);
            //spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/Backgrounds/TuskMiddleSky").Value, SkyBase + new Vector2(0, -900), new Rectangle(0, 0, 2048, 1800), DrawC, 0, new Vector2(1024, 600), 1.0f, SpriteEffects.None, 0f);
            Texture2D[] CloudLine = new Texture2D[20];
            CloudLine[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/DarkBlueGreenCloud").Value;
            CloudLine[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/DarkGreenCloud").Value;
            CloudLine[3] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/BlackCloud").Value;
            CloudLine[4] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/DarkGreyCloud").Value;
            CloudLine[5] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/DarkGreyCloud2").Value;
            CloudLine[6] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/LightGreyCloud3").Value;
            CloudLine[7] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/LightGreyCloud4").Value;
            CloudLine[14] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/LightGreyCloud").Value;
            CloudLine[15] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/LightGreyCloud2").Value;
            for (int i = 0; i < 20; i++)
            {
                List<VertexBase.CustomVertexInfo> Vx = new List<VertexBase.CustomVertexInfo>();
                for (int u = 0; u < 200; u++)
                {
                    if (SkyHalo[i, u] == Vector2.Zero)
                    {
                        break;
                    }
                    if (u < 199)
                    {
                        Vx.Add(new VertexBase.CustomVertexInfo(SkyVortexOvalCenter[i] + SkyHalo[i, u] + new Vector2(0, -20), DrawC, new Vector3(u / 200f, 0, 0)));
                        Vx.Add(new VertexBase.CustomVertexInfo(SkyVortexOvalCenter[i] + SkyHalo[i, u + 1] + new Vector2(0, -20), DrawC, new Vector3((u + 1) / 200f, 0, 0)));
                        Vx.Add(new VertexBase.CustomVertexInfo(SkyVortexOvalCenter[i] + new Vector2(0, 20), DrawC, new Vector3((u + 0.5f) / 200f, 1, 0)));
                    }
                    else
                    {
                        Vx.Add(new VertexBase.CustomVertexInfo(SkyVortexOvalCenter[i] + SkyHalo[i, u] + new Vector2(0, -20), DrawC, new Vector3(u / 200f, 0, 0)));
                        Vx.Add(new VertexBase.CustomVertexInfo(SkyVortexOvalCenter[i] + SkyHalo[i, 0] + new Vector2(0, -20), DrawC, new Vector3((u + 1) / 200f, 0, 0)));
                        Vx.Add(new VertexBase.CustomVertexInfo(SkyVortexOvalCenter[i] + new Vector2(0, 20), DrawC, new Vector3((u + 0.5f) / 200f, 1, 0)));
                    }
                }
                Main.graphics.GraphicsDevice.Textures[0] = CloudLine[i];//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            }
            Vector2 FarS = new Vector2(Main.screenWidth / 2f, Main.screenHeight + 80) - TuskF * 0.2f;
            List<VertexBase.CustomVertexInfo> VskyF = new List<VertexBase.CustomVertexInfo>();
            VskyF.Add(new VertexBase.CustomVertexInfo(FarS + new Vector2(1024, -750), DrawC, new Vector3(1, 0, 0)));
            VskyF.Add(new VertexBase.CustomVertexInfo(FarS + new Vector2(1024, 150), DrawC, new Vector3(1, 1, 0)));
            VskyF.Add(new VertexBase.CustomVertexInfo(FarS + new Vector2(-1024, 150), DrawC, new Vector3(0, 1, 0)));

            VskyF.Add(new VertexBase.CustomVertexInfo(FarS + new Vector2(1024, -750), DrawC, new Vector3(1, 0, 0)));
            VskyF.Add(new VertexBase.CustomVertexInfo(FarS + new Vector2(-1024, -750), DrawC, new Vector3(0, 0, 0)));
            VskyF.Add(new VertexBase.CustomVertexInfo(FarS + new Vector2(-1024, 150), DrawC, new Vector3(0, 1, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/TuskFar").Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VskyF.ToArray(), 0, VskyF.Count / 3);

            Vector2 MiddleS = new Vector2(Main.screenWidth / 2f, Main.screenHeight + 100) - TuskM * 0.2f;
            List<VertexBase.CustomVertexInfo> VskyM = new List<VertexBase.CustomVertexInfo>();
            VskyM.Add(new VertexBase.CustomVertexInfo(MiddleS + new Vector2(1024, -750), DrawC, new Vector3(1, 0, 0)));
            VskyM.Add(new VertexBase.CustomVertexInfo(MiddleS + new Vector2(1024, 150), DrawC, new Vector3(1, 1, 0)));
            VskyM.Add(new VertexBase.CustomVertexInfo(MiddleS + new Vector2(-1024, 150), DrawC, new Vector3(0, 1, 0)));

            VskyM.Add(new VertexBase.CustomVertexInfo(MiddleS + new Vector2(1024, -750), DrawC, new Vector3(1, 0, 0)));
            VskyM.Add(new VertexBase.CustomVertexInfo(MiddleS + new Vector2(-1024, -750), DrawC, new Vector3(0, 0, 0)));
            VskyM.Add(new VertexBase.CustomVertexInfo(MiddleS + new Vector2(-1024, 150), DrawC, new Vector3(0, 1, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Background/TuskMiddle").Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VskyM.ToArray(), 0, VskyM.Count / 3);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            throw new NotImplementedException();
        }
        public override void Update(GameTime gameTime)
        {
            if (this.skyActive && this.opacity < 1f)
            {
                this.opacity += 0.02f;
                return;
            }
            if (!this.skyActive && this.opacity > 0f)
            {
                this.opacity -= 0.02f;
            }
        }
        public override float GetCloudAlpha()
        {
            return (1f - this.opacity) * 0.97f + 0.03f;
        }

        private bool skyActive;

        private float opacity;
    }
}
