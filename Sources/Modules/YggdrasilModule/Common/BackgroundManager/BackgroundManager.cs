using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.YggdrasilModule.Common.BackgroundManager
{
    public class BackgroundManager
    {
        public class WorldBackground
        {
            public float Layer = 2;//���Ʋ�,����
            public int Priority = 0;//ͬ���»������ȼ�
            public Vector2 Center;//��������
            public Color Color;//��ɫ
            public Rectangle DrawRectangle;//������
            public bool Active;//�Ƿ���ƣ����ȼ����
            public Texture2D Texture;//ͼƬ
            public float scale = 1;//��С
        }
        public class BoardBackground : WorldBackground
        {
            public bool XClamp = false;
            public bool YClamp = true;
        }
        public class PointBackground : WorldBackground
        {
            public float Rotation = 0;
            public Vector2 Velocity;
            public float[] ai = new float[8];
            public void Update()
            {

            }
            public void SpecialDraw(SpriteBatch spriteBatch)
            {

            }
        }

        public static void QuickDrawBG(Texture2D tex, Rectangle drawArea, Color baseColor, int Ymin, int Ymax, bool Xclamp = false, bool Yclamp = true)
        {

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Effect bgW = YggdrasilContent.QuickEffect("Common/BackgroundManager/BackgroundXWarp");
            if (Xclamp && Yclamp)
            {
                bgW = YggdrasilContent.QuickEffect("Common/BackgroundManager/BackgroundXYClamp");
            }
            if (Xclamp && !Yclamp)
            {
                bgW = YggdrasilContent.QuickEffect("Common/BackgroundManager/BackgroundYWarp");
            }
            if (!Xclamp && !Yclamp)
            {
                bgW = YggdrasilContent.QuickEffect("Common/BackgroundManager/BackgroundXYWarp");
            }
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            if(Main.LocalPlayer.gravDir == -1)
            {
                projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, 0, Main.screenHeight, 0, 1);
            }
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
            bgW.Parameters["uTransform"].SetValue(projection);
            bgW.Parameters["uTime"].SetValue(0.34f);
            bgW.CurrentTechnique.Passes[0].Apply();

            //�����������ͼ���޵Ĳ���
            int DrawMaxY = Main.screenHeight;
            int DrawMinY = 0;
            float YSqueezeValueUp = 0f;
            float YSqueezeValueDown = 1f;
            if (Main.screenPosition.Y + Main.screenHeight > Ymax)
            {
                DrawMaxY = Ymax - (int)Main.screenPosition.Y;
                YSqueezeValueDown = (float)DrawMaxY / Main.screenHeight;
            }
            if (Main.screenPosition.Y < Ymin)
            {
                DrawMinY = Ymin - (int)Main.screenPosition.Y;
                YSqueezeValueUp = DrawMinY / (float)Main.screenHeight;
            }

            List<Vertex2D> CloseII = new List<Vertex2D>
            {
                new Vertex2D(new Vector2(0, DrawMinY), baseColor, new Vector3(drawArea.X / (float)tex.Width, (drawArea.Y + (drawArea.Height * YSqueezeValueUp)) / tex.Height, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, DrawMinY), baseColor, new Vector3((drawArea.X + drawArea.Width) / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueUp)/ tex.Height, 0)),
                new Vertex2D(new Vector2(0, DrawMaxY), baseColor, new Vector3(drawArea.X / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueDown) / tex.Height, 0)),

                new Vertex2D(new Vector2(0, DrawMaxY), baseColor, new Vector3(drawArea.X / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueDown) / tex.Height, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, DrawMinY), baseColor, new Vector3((drawArea.X + drawArea.Width) / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueUp) / tex.Height, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, DrawMaxY), baseColor, new Vector3((drawArea.X + drawArea.Width) / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueDown) / tex.Height, 0))
            };
            if (CloseII.Count > 2)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, CloseII.ToArray(), 0, 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void DrawWaterfallInBackground(Vector2 biomeCenter, float moveStep, Vector2 positionToTextureCenter, float Width, float Height, Color baseColor, int Ymin, int Ymax, Vector2 textureSize = new Vector2(), bool Xclamp = false, bool Yclamp = true)
        {
            Vector2 HalfScreenSize = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 ScreenCenter = Main.screenPosition + HalfScreenSize;
            Vector2 deltaPos = ScreenCenter - biomeCenter;
            deltaPos *= moveStep;
            Vector2 DrawCenter = HalfScreenSize - deltaPos + positionToTextureCenter;
            if(Xclamp)
            {
                if (Yclamp)
                {
                    drawWaterfall(DrawCenter, Width, Height, baseColor, Ymin, Ymax);
                }
                else
                {                  
                    Vector2 dCenter = DrawCenter;
                    while (dCenter.Y > Main.screenHeight + 160)
                    {
                        dCenter.Y -= textureSize.Y;
                    }
                    while (dCenter.Y >= -Height && dCenter.Y <= Main.screenHeight + 160)
                    {
                        drawWaterfall(dCenter, Width, Height, baseColor, Ymin, Ymax);
                        dCenter.Y -= textureSize.Y;
                    }

                    dCenter = DrawCenter;
                    if (dCenter.Y >= -Height && dCenter.Y <= Main.screenHeight + 160)
                    {
                        dCenter.Y += textureSize.Y;
                    }
                    while (dCenter.Y < -Height)
                    {
                        dCenter.Y += textureSize.Y;
                    }
                    while (dCenter.Y >= -Height && dCenter.Y <= Main.screenHeight + 160)
                    {
                        drawWaterfall(dCenter, Width, Height, baseColor, Ymin, Ymax);
                        dCenter.Y += textureSize.Y;
                    }
                }
            }
            else if (Yclamp)
            {
                Vector2 dCenter = DrawCenter;
                while (dCenter.X > Main.screenWidth + Width)
                {
                    dCenter.X -= textureSize.X;
                }
                while (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
                {
                    drawWaterfall(dCenter, Width, Height, baseColor, Ymin, Ymax);
                    dCenter.X -= textureSize.X;
                    
                }
                dCenter = DrawCenter;
                if (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
                {
                    dCenter.X += textureSize.X;
                }
                while (dCenter.X < -Width)
                {
                    dCenter.X += textureSize.X;
                }
                while (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
                {
                    drawWaterfall(dCenter, Width, Height, baseColor, Ymin, Ymax);
                    dCenter.X += textureSize.X;
                }
            }
            else
            {
                Vector2 dCenter = DrawCenter;
                while (dCenter.X > Main.screenWidth + Width)
                {
                    dCenter.X -= textureSize.X;
                }
                while (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
                {
                    Vector2 ddCenter = dCenter;
                    while (ddCenter.Y > Main.screenHeight + 160)
                    {
                        ddCenter.Y -= textureSize.Y;
                    }
                    while (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
                    {
                        drawWaterfall(ddCenter, Width, Height, baseColor, Ymin, Ymax);
                        ddCenter.Y -= textureSize.Y;
                    }
                    ddCenter = dCenter;
                    if (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
                    {
                        ddCenter.Y += textureSize.Y;
                    }
                    while (ddCenter.Y < -Height)
                    {
                        ddCenter.Y += textureSize.Y;
                    }
                    while (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
                    {
                        ddCenter.Y += textureSize.Y;
                        drawWaterfall(ddCenter, Width, Height, baseColor, Ymin, Ymax);
                    }
                    dCenter.X -= textureSize.X;
                }
                dCenter = DrawCenter;
                if (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
                {
                    dCenter.X += textureSize.X;
                }
                while (dCenter.X < -Width)
                {
                    dCenter.X += textureSize.X;
                }
                while (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
                {
                    Vector2 ddCenter = dCenter;
                    while (ddCenter.Y > Main.screenHeight + 160)
                    {
                        ddCenter.Y -= textureSize.Y;
                    }
                    while (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
                    {
                        drawWaterfall(ddCenter, Width, Height, baseColor, Ymin, Ymax);
                        ddCenter.Y -= textureSize.Y;
                    }
                    ddCenter = dCenter;
                    if (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
                    {
                        ddCenter.Y += textureSize.Y;
                    }
                    while (ddCenter.Y < -Height)
                    {
                        ddCenter.Y += textureSize.Y;
                    }
                    while (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
                    {
                        ddCenter.Y += textureSize.Y;
                        drawWaterfall(ddCenter, Width, Height, baseColor, Ymin, Ymax);
                    }
                    dCenter.X += textureSize.X;
                }
            }
        }
        public static void drawWaterfall(Vector2 drawCenter, float width, float height, Color baseColor, int Ymin, int Ymax)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            if (Main.LocalPlayer.gravDir == -1)
            {
                projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, 0, Main.screenHeight, 0, 1);
            }
            Effect bgW = YggdrasilContent.QuickEffect("Common/BackgroundManager/BackgroundYWarp");
            bgW.Parameters["uTransform"].SetValue(projection);
            bgW.Parameters["uTime"].SetValue(0.34f);
            bgW.CurrentTechnique.Passes[0].Apply();

            float Time = (float)-Main.timeForVisualEffects / 40f;
            List<Vertex2D> WaterFallVertex = new List<Vertex2D>();
            for (float y = 0; y < height; y += 10f)
            {
                float ColorAlpha = Math.Min((y / 100f), 1);
                if (height - y < 100)
                {
                    ColorAlpha = Math.Max((height - y) / 100f, 0);
                }
                float WorldYCoordinate = drawCenter.Y + y + Main.screenPosition.Y;
                if (WorldYCoordinate > Ymax || WorldYCoordinate < Ymin)
                {
                    ColorAlpha *= 0;
                }
                WaterFallVertex.Add(new Vertex2D(new Vector2(drawCenter.X + width / 2f, drawCenter.Y + y), baseColor * ColorAlpha, new Vector3(0, (float)Math.Pow(y, 0.6) / 10f + Time, 0)));
                WaterFallVertex.Add(new Vertex2D(new Vector2(drawCenter.X - width / 2f, drawCenter.Y + y), baseColor * ColorAlpha, new Vector3(1, (float)Math.Pow(y, 0.6) / 10f + Time, 0)));
            }
            if (WaterFallVertex.Count > 2)
            {
                Main.graphics.GraphicsDevice.Textures[0] = YggdrasilContent.QuickTexture("YggdrasilTown/Background/WaterFall");
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, WaterFallVertex.ToArray(), 0, WaterFallVertex.Count - 2);
            }

            List<Vertex2D> WaterFallVertexII = new List<Vertex2D>();
            for (float y = 0; y < height; y += 10f)
            {
                float ColorAlpha = Math.Min(y / 100f, 1);
                if (height - y < 100)
                {
                    ColorAlpha = Math.Max((height - y) / 100f, 0);
                }
                float WorldYCoordinate = drawCenter.Y + y + Main.screenPosition.Y;
                if (WorldYCoordinate > Ymax || WorldYCoordinate < Ymin)
                {
                    ColorAlpha *= 0;
                }
                WaterFallVertexII.Add(new Vertex2D(new Vector2(drawCenter.X + width / 2f, drawCenter.Y + y), baseColor * ColorAlpha * 1.63f, new Vector3(0, (float)Math.Pow(y, 0.4) / 10f + Time, 0)));
                WaterFallVertexII.Add(new Vertex2D(new Vector2(drawCenter.X - width / 2f, drawCenter.Y + y), baseColor * ColorAlpha * 1.63f, new Vector3(1, (float)Math.Pow(y, 0.4) / 10f + Time, 0)));
            }
            if (WaterFallVertexII.Count > 2)
            {
                Main.graphics.GraphicsDevice.Textures[0] = YggdrasilContent.QuickTexture("YggdrasilTown/Background/WaterFall");
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, WaterFallVertexII.ToArray(), 0, WaterFallVertexII.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}