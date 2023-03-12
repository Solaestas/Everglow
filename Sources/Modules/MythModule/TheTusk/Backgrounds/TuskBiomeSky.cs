﻿using Terraria.Graphics.Effects;
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
            rocks = new List<Rock>();
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



        //物体相关系统
        private class Rock
        {
            public Vector3 pos;
            public Vector3 velocity;
            public int style;
        }
        public class RedLightning
        {
            public Vector3 pos;
            public bool sub;
            public float rotation;
            public List<Vector2> nodes=new();
            public int timeleft;
            public int maxTimeleft;
            public Vertex3D_2[] GetVertices(float maxWidth,Color c)
            {
                List<Vertex3D_2> vertices = new();
                for (int i = 0; i < nodes.Count - 1; ++i)
                {
                    var normalDir = nodes[i] - nodes[i + 1];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = i / (float)nodes.Count;
                    var w = 1;
                    float width = MathHelper.Lerp(maxWidth, 0,factor);
                    width *= (float) timeleft / maxTimeleft;
                    if (sub)
                        width*=0.5f;
                    Vector2 posV2 = nodes[i] + normalDir * width;
                    vertices.Add(new Vertex3D_2(new Vector3(posV2, pos.Z), new Vector3(factor, 1, w),c));
                    posV2 = nodes[i] - normalDir * width;
                    vertices.Add(new Vertex3D_2(new Vector3(posV2, pos.Z), new Vector3(factor, 0, w),c));
                }
                return vertices.ToArray();
            }
            public void Create()
            {
                timeleft = maxTimeleft;
                int counts = Main.rand.Next(15,80);
                if(sub)
                    counts = Main.rand.Next(5, 12);
                Vector2 vec = new Vector2(pos.X, pos.Y);
                for (int i=0;i<counts;i++)
                {
                    nodes.Add(vec);
                    vec += (rotation + Main.rand.NextFloatDirection()*0.5f).ToRotationVector2()*Main.rand.Next(90,150);
                    if(!sub&&i<12&&Main.rand.NextBool(15))
                    {
                        RedLightning l = new RedLightning() { pos = new Vector3(vec,pos.Z), rotation = rotation + Main.rand.NextFloatDirection() * 1f, maxTimeleft = maxTimeleft-10 ,sub=true };
                        l.Create();
                        lightnings.Add(l);
                    }
                }
            }
        }
        List<Rock> rocks = new List<Rock>();
        private static List<RedLightning> lightnings = new();

        private void CreateAndDrawLightning(Vector3 cloudCenter)
        {
            if (Main.rand.NextBool(300) && !Main.gamePaused)
            {
                Vector2 offset = Main.rand.NextVector2Unit() * Main.rand.Next(700, 3000);
                Vector3 pos = cloudCenter + new Vector3(offset.X,0,offset.Y);
                RedLightning l = new RedLightning() { pos = pos, rotation = 1.57f, maxTimeleft = 60 };
                l.Create();
                lightnings.Add(l);
                
            }
            if (!Main.gamePaused)
            {
                for (int i = 0; i < lightnings.Count; i++)
                {
                    RedLightning lightning = lightnings[i];
                    lightning.timeleft--;
                    if (!lightning.sub&&lightning.pos.Z<8000)
                    {
                        float a = MathHelper.Clamp( 1-(lightning.pos.Z) / 8000f,0f,1f);
                        Main.ColorOfTheSkies = Color.Lerp(Main.ColorOfTheSkies, new Color(0.8f, 0.6f, 0.6f), 0.5f*a*lightning.timeleft / 60f);//闪电背景颜色
                    }
                    if (lightning.timeleft <= 0)
                        lightnings.Remove(lightning);
                }
            }
            for (int i = 0; i < lightnings.Count; i++)
            {
                RedLightning lightning = lightnings[i];
                float alpha = 0;
                if(lightning.pos.Z>7000)
                {
                    alpha = (lightning.pos.Z - 7000)/10000f;
                }
                Vertex3D_2[] vertices = lightning.GetVertices(90,new Color(1,1f-alpha,1f-alpha,0f));
                Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("TheTusk/Backgrounds/Lightning");
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length -2);

            }
        }

        private void CreateAndDrawRocks(Vector3 cloudCenter)
        {
            
            if(Main.rand.NextBool(45)&&!Main.gamePaused)
            {
                Vector3 pos = cloudCenter + new Vector3(Main.rand.Next(-500,500),+3000, Main.rand.Next(-500, 500));
                rocks.Add(new Rock() { pos=pos,velocity=new Vector3(Main.rand.NextFloat(-2,2),-Main.rand.NextFloat(1,3), Main.rand.NextFloat(-2, 2)),style=1+Main.rand.Next(5)});
            }
            if (!Main.gamePaused)
            {
                for (int i = 0; i < rocks.Count; i++)//Update 更新 以及石块的ai
                {
                    Rock rock = rocks[i];

                    rock.pos += rock.velocity;

                    Vector2 vecToCenter = new Vector2(cloudCenter.X - rock.pos.X, cloudCenter.Z - rock.pos.Z);
                    if (vecToCenter.Length() > 320)//向心的加速
                    {
                        Vector2 velxz = new Vector2(rock.velocity.X, rock.velocity.Z);
                        Vector2 acc = 2.8f * Vector2.Normalize(vecToCenter) * velxz.LengthSquared() / vecToCenter.Length();
                        rock.velocity.X += acc.X;
                        rock.velocity.Z += acc.Y;
                    }
                    if (rock.pos.Y < cloudCenter.Y - 100)
                        rocks.Remove(rock);

                }
            }
            for (int i = 0; i < rocks.Count; i++)//Draw
            {
                Color color = Main.ColorOfTheSkies*opacity;

                float scale = 5f;
                Rock rock = rocks[i];
                if (rock.pos.Y<cloudCenter.Y+300)
                {
                    float alpha = (cloudCenter.Y+300 - rock.pos.Y)/400f;
                    if(alpha>0)
                        color *= (1 - alpha);
                }
              
                Texture2D tex = MythContent.QuickTexture("TheTusk/Backgrounds/Stone" + rock.style);//贴图
                List<Vertex3D_2> vertices = new();
                vertices.Add(new(rock.pos, new Vector3(0, 0, 0), color));
                vertices.Add(new(rock.pos + new Vector3(tex.Width, 0, 0) * scale, new Vector3(1, 0, 0), color));
                vertices.Add(new(rock.pos + new Vector3(0, tex.Height, 0) * scale, new Vector3(0, 1, 0), color));

                vertices.Add(new(rock.pos + new Vector3(tex.Width, 0, 0) * scale, new Vector3(1, 0, 0), color));
                vertices.Add(new(rock.pos + new Vector3(0, tex.Height, 0) * scale, new Vector3(0, 1, 0), color));
                vertices.Add(new(rock.pos + new Vector3(tex.Width, tex.Height, 0) * scale, new Vector3(1, 1, 0), color));
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count / 3);
            }
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

            #region #1：背景光
            int yoffset = (int)Main.screenPosition.Y / 50;
            Texture2D tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/TuskBiomeSky").Value;
            Main.spriteBatch.Draw(tex, new Rectangle(0, -yoffset, Main.screenWidth, Main.screenHeight + yoffset), DrawC * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f));
            #endregion
            #region #2：风暴
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            //计算矩阵
            Vector3 camPos = new Vector3(Main.screenWidth / 2+Main.screenPosition.X, Main.screenHeight / 2 + Main.screenPosition.Y, -300);
            int lookup = 50;//往上看
            Matrix matrix = Matrix.CreateLookAt(camPos, new Vector3(camPos.X, camPos.Y-lookup, 1), Vector3.Down);
            matrix *= Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 5, Main.graphics.GraphicsDevice.Viewport.AspectRatio, 1, 15000);

            Effect eff = MythContent.QuickEffect("Effects/DrawPrim3D");
            eff.Parameters["uTransform"].SetValue(matrix);
            eff.CurrentTechnique.Passes[0].Apply();

            Vector2 SkyBase = new Vector2(Main.screenWidth / 2f, Main.screenHeight * 1.0f - 160f) - TuskBiomeCenterToScreenPosition * 0.01f;
            Vector2 SkyVortex = SkyBase + new Vector2(1210, 161)/*图心偏移坐标*/ - new Vector2(1024, 600)/*绘制中心*/;
            SkyVortex = TuskBiomeCenter;
            float Blength = 1800;
            float ABdevide = 0.32085f;
            double OneDevideRotaSpeed = 2500d;

            Texture2D[] CloudLine = new Texture2D[10];
            CloudLine[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkBlueGreenCloud").Value;
            CloudLine[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkGreenCloud").Value;
            CloudLine[3] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkGreyCloud").Value;
            CloudLine[4] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkGreyCloud").Value;
            CloudLine[5] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/DarkGreyCloud2").Value;
            CloudLine[6] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/LightGreyCloud3").Value;
            CloudLine[7] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/LightGreyCloud4").Value;
            CloudLine[8] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/LightGreyCloud2").Value;
            CloudLine[9] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/LightGreyCloud").Value;

            for (int i = 3; i < 10; i++)
            {
                
                #region decide values
                if (i == 1)
                {
                    Blength = 1640;
                    OneDevideRotaSpeed = 10500d;
                }
                if (i == 2)
                {
                    Blength = 1450;
                    OneDevideRotaSpeed = 7000d;
                }
                if (i == 3)
                {
                    Blength = 1230;
                    OneDevideRotaSpeed = 4300d;
                }
                if (i == 4)
                {
                    Blength = 801;
                    OneDevideRotaSpeed = 2200d;
                }
                if (i == 5)
                {
                    Blength = 611;
                    OneDevideRotaSpeed = 1200d;
                }
                if (i == 6)
                {
                    Blength = 430;
                    OneDevideRotaSpeed = 1000d;
                }
                if (i == 7)
                {
                    Blength = 300;
                    OneDevideRotaSpeed = 600d;
                }
                if (i == 8)
                {
                    Blength = 210;
                    OneDevideRotaSpeed = 400d;
                }
                if (i == 9)
                {
                    Blength = 160;
                    OneDevideRotaSpeed = 220d;
                }
                #endregion
                List<Vertex3D_2> Vx = new List<Vertex3D_2>();
                float counts = 30;

                Vector3 center = new Vector3(SkyVortex.X + 1000, SkyVortex.Y - 3250 + (float)Math.Pow(i, 1f) * 130, 6000);//云的中心位置

                if(i==7)
                {
                    CreateAndDrawLightning(center);
                }
                if (i == 9)
                {
                    CreateAndDrawRocks(center+new Vector3(0,-400,0));
                }
                for (int u = 0; u <= counts; u++)
                {
                    Matrix rot = Matrix.CreateRotationY((float)(1f / OneDevideRotaSpeed * Main.timeForVisualEffects + u * MathHelper.TwoPi / counts));
                    Vector3 offset = Vector3.Transform(Vector3.UnitX,rot) * Blength*7;

                    Color c = DrawC;
                    if (i == 9)
                        c *= 0.9f;

                    if (i == 8)
                        c *= 0.68f;

                    if (i == 7)
                        c *= 0.7f;

                    if (i == 6)
                        c *= 0.6f;

                    float n = 0.9f- i*0.075f;//内层顶点的距离
                    if (i == 6)
                        n -= 0.1f;
                    Vx.Add(new Vertex3D_2(center + offset*1f, new Vector3(u / counts, 0, 0),c));
                    Vx.Add(new Vertex3D_2(center + offset*n, new Vector3(u / counts, 0.6f, 0) ,c));
                }
                Main.graphics.GraphicsDevice.Textures[0] = CloudLine[i];//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vx.ToArray(), 0, Vx.Count -2);
            }
            #endregion
            #region #3：前景
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


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
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
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
