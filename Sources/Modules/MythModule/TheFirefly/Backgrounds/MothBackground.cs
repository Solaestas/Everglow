using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Physics;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using Terraria.GameContent;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.NPCs;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds
{
    public class MothBackground : ModSystem
    {
        //���˸������⣬����Ҫ�����²�Ȼ����ȥ�ܹ�
        public readonly Vector3 ambient = new Vector3(0.001f, 0.001f, 0.05f);
        public List<GHang> GPos = new List<GHang>();
        public List<GHang> GPosSec = new List<GHang>();
        public List<Vector2> RopPosFir = new List<Vector2>();
        public List<float> RopPosFirS = new List<float>();
        public List<int> RopPosFirC = new List<int>();
        private float alpha = 0f;
        private float luminance = 1f;//���������ȣ�bossսʱ�䰵
        private Vector2 RopOffset = Vector2.Zero;//������λ��ƫ����
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public override void OnModLoad()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Everglow.HookSystem.AddMethod(DrawBackground, Commons.Core.CallOpportunity.PostDrawBG);
                On.Terraria.Graphics.Light.TileLightScanner.GetTileLight += TileLightScanner_GetTileLight;
                GetRopePosFir("TreeRope");
                InitMass_Spring();
            }
        }

        /// <summary>
        /// ӫ������������ݽṹ
        /// </summary>
        public struct GHang
        {
            public Vector2 Pos;
            public float Length;
            public float Size;
            public int Type;
            public GHang(Vector2 pos, float length, float size, int type)
            {
                this.Pos = pos;
                this.Length = length;
                this.Size = size;
                this.Type = type;
            }
        }
        /// <summary>
        /// ������Ĺ���
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="outputColor"></param>
        private void TileLightScanner_GetTileLight(On.Terraria.Graphics.Light.TileLightScanner.orig_GetTileLight orig, Terraria.Graphics.Light.TileLightScanner self, int x, int y, out Vector3 outputColor)
        {
            orig(self, x, y, out outputColor);
            outputColor += ambient;
        }
        public override void PostUpdateEverything()//�������±���
        {
            float increase = 0.02f;
            if (BiomeActive() && Main.BackgroundEnabled)
            {
                if (alpha < 1)
                {
                    alpha += increase;
                }
                else
                {
                    alpha = 1;
                }

                Everglow.HookSystem.DisableDrawBackground = true;
            }
            else
            {
                if (alpha > 0)
                {
                    alpha -= increase;
                }
                else
                {
                    alpha = 0;
                    Everglow.HookSystem.DisableDrawBackground = false;
                }
            }
            if (CorruptMoth.CorruputMothNPC != null)//����������bossսʱ�䰵
            {
                if (CorruptMoth.CorruputMothNPC.active)
                {
                    luminance = MathHelper.Lerp(luminance, 0.1f, 0.02f);
                }
                else
                {
                    luminance = MathHelper.Lerp(luminance, 1, 0.02f);
                }
            }
            else
            {
                luminance = MathHelper.Lerp(luminance, 1, 0.02f);
            }
        }
        /// <summary>
        /// �ж��Ƿ�������
        /// </summary>
        /// <returns></returns>
        public static bool BiomeActive()
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 BiomeCenter = new Vector2(mothLand.fireflyCenterX * 16, (mothLand.fireflyCenterY - 20) * 16);//��ȡ������Ϣ
            Vector2 v0 = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f - BiomeCenter;//��������Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f
            v0.Y *= 1.35f;
            v0.X *= 0.9f;//��������Բ�Σ�����xy����任
            return (v0.Length() < 2000);
        }
        /// <summary>
        /// ��ȡӫ���������λ
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <exception cref="Exception"></exception>
        public void GetGlowPos(string Shapepath)
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/" + Shapepath);
            imageData.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        ref var pixel = ref pixelRow[x];
                        if (pixel.R == 255)
                        {
                            GPos.Add(new GHang(new Vector2(x * 10, y * 10), (pixel.G / 4f + 2), pixel.B / 255f + 0.5f, Main.rand.Next(5)));
                        }
                    }
                }
            });

            //int w = colors.GetLength(0);
            //int h = colors.GetLength(1);
            //for (int y = 0; y < h; ++y)
            //{
            //    for (int x = 0; x < w; ++x)
            //    {
            //        Color temp = colors[x, y];
            //        if (temp.R == 255)
            //        {

            //        }
            //    }
            //}
        }
        /// <summary>
        /// ��ȡ�ڶ���ӫ���������λ
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <exception cref="Exception"></exception>
        public void GetGlowPosSec(string Shapepath)
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/" + Shapepath);
            imageData.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        ref var pixel = ref pixelRow[x];
                        if (pixel.R == 255)
                        {
                            GPosSec.Add(new GHang(new Vector2(x * 10, y * 4.2f), (pixel.G / 4f + 2), pixel.B / 255f + 0.5f, Main.rand.Next(5)));
                        }
                    }
                }
            });

            //Color[,] colors = ImageReader.Read("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/" + Shapepath);
            //int w = colors.GetLength(0);
            //int h = colors.GetLength(1);
            //for (int y = 0; y < h; ++y)
            //{
            //    for (int x = 0; x < w; ++x)
            //    {
            //        Color temp = colors[x, y];
            //        if (temp.R == 255)
            //        {
            //            GPosSec.Add(new GHang(new Vector2(x * 10, y * 4.2f), (temp.G / 4f + 2), temp.B / 255f + 0.5f, Main.rand.Next(5)));
            //        }
            //    }
            //}
        }
        /// <summary>
        /// ��ȡ��һ��������λ
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <exception cref="Exception"></exception>
        public void GetRopePosFir(string Shapepath)
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/" + Shapepath);
            imageData.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        ref var pixel = ref pixelRow[x];
                        if (pixel.R == 255)
                        {
                            RopPosFir.Add(new Vector2(x * 5, y * 5f));
                            RopPosFirC.Add(pixel.G + 2);
                            RopPosFirS.Add((pixel.B + 240) / 300f);
                        }
                    }
                }
            });
            //Color[,] colors = ImageReader.Read("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/" + Shapepath);
            //int w = colors.GetLength(0);
            //int h = colors.GetLength(1);
            //for (int y = 0; y < h; ++y)
            //{
            //    for (int x = 0; x < w; ++x)
            //    {
            //        Color temp = colors[x, y];
            //        if (temp.R == 255)
            //        {
            //            RopPosFir.Add(new Vector2(x * 5, y * 5f));
            //            RopPosFirC.Add(temp.G + 2);
            //            RopPosFirS.Add((temp.B + 240) / 300f);
            //        }
            //    }
            //}
        }
        /// <summary>
        /// ����ӫ��
        /// </summary>
        private void DrawGlow(Vector2 texSize, float MoveStep)
        {
            if (GPos.Count <= 1)
            {
                GetGlowPos("GlosPos");
            }
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + (texSize / 2);
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Vector2 DCen = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 deltaPos = DCen - new Vector2((mothLand.fireflyCenterX + 34) * 16f, mothLand.fireflyCenterY * 16f);
            deltaPos *= MoveStep;
            Vector2 TexLT = sampleCenter - screenSize / 2f + deltaPos;
            float deltaY = DCen.Y - (mothLand.fireflyCenterY - 90) * 16f;
            deltaY *= MoveStep * 0.4f;
            if (GPos.Count > 0)
            {
                var texGlow = MythContent.QuickTexture("TheFirefly/Backgrounds/GlowHanging");
                for (int x = 0; x < GPos.Count; x++)
                {
                    Vector2 GP = GPos[x].Pos;
                    GP.Y += deltaY / (GPos[x].Size + 1);
                    Vector2 dPos = GP - TexLT + new Vector2(0, -194);
                    dPos = PointCorrection(dPos);
                    Rectangle sRtTop = new Rectangle(GPos[x].Type * 20, 0, 20, 10);
                    Rectangle sRtLine = new Rectangle(GPos[x].Type * 20, 10, 20, 20);
                    Rectangle sRtDrop = new Rectangle(GPos[x].Type * 20, 65, 20, 35);
                    float Dlength = (float)(GPos[x].Length + Math.Sin(Main.timeForVisualEffects / 128d + GPos[x].Pos.X / 70d + GPos[x].Pos.Y / 120d) * GPos[x].Length * 0.2f);
                    Color color = GetLuminace(Color.White * alpha);
                    Main.spriteBatch.Draw(texGlow, dPos, sRtTop, color, 0, new Vector2(10, 0), GPos[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 10 + Dlength) * GPos[x].Size, sRtLine, color, 0, new Vector2(10, 10), new Vector2(1f, Dlength / 10f) * GPos[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 27.5f + Dlength * 2) * GPos[x].Size, sRtDrop, color, 0, new Vector2(10, 17.5f), GPos[x].Size, SpriteEffects.None, 0);
                }
            }
        }
        /// <summary>
        /// ������Ļ�ֱ������ö�������ϵ�ӫ���������λ
        /// </summary>
        /// <param name="OrigPoint"></param>
        /// <returns></returns>
        private static Vector2 PointCorrection(Vector2 OrigPoint)
        {
            Vector2 ScreenCen = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 dPoss = OrigPoint - ScreenCen;
            Vector2 DSize = GetZoomByScreenSize();
            dPoss.X *= (DSize.X - 1);
            dPoss.Y *= (DSize.Y - 1);
            OrigPoint += dPoss;
            return OrigPoint;
        }
        /// <summary>
        /// ���Ƶڶ���ӫ��
        /// </summary>
        private void DrawGlowSec(Vector2 texSize, float MoveStep)
        {
            if (GPosSec.Count <= 1)
            {
                GetGlowPosSec("GlowHangingMiddlePosition");
            }
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + (texSize / 2);
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Vector2 DCen = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 deltaPos = DCen - new Vector2((mothLand.fireflyCenterX + 34) * 16f, mothLand.fireflyCenterY * 16f);
            deltaPos *= MoveStep;
            Vector2 TexLT = sampleCenter - screenSize / 2f + deltaPos;
            float deltaY = DCen.Y - (mothLand.fireflyCenterY - 90) * 16f;
            deltaY *= MoveStep * 0.14f;
            if (GPosSec.Count > 0)
            {
                var texGlow = MythContent.QuickTexture("TheFirefly/Backgrounds/GlowHangingMiddle");
                for (int x = 0; x < GPosSec.Count; x++)
                {
                    Vector2 GP = GPosSec[x].Pos;
                    GP.Y += deltaY / (GPosSec[x].Size + 1);
                    Vector2 dPos = GP - TexLT + new Vector2(0, -180);
                    dPos = PointCorrection(dPos);
                    Rectangle sRtTop = new Rectangle(GPosSec[x].Type * 10, 0, 10, 3);
                    Rectangle sRtLine = new Rectangle(GPosSec[x].Type * 10, 3, 10, 5);
                    Rectangle sRtDrop = new Rectangle(GPosSec[x].Type * 10, 10, 10, 15);
                    Color color = GetLuminace(Color.White * alpha);
                    float Dlength = (float)(GPosSec[x].Length + Math.Sin(Main.timeForVisualEffects / 128d + GPosSec[x].Pos.X / 70d + GPosSec[x].Pos.Y / 120d) * GPosSec[x].Length * 0.2f);
                    Main.spriteBatch.Draw(texGlow, dPos, sRtTop, color, 0, new Vector2(5, 0), GPosSec[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 3 + Dlength) * GPosSec[x].Size, sRtLine, color, 0, new Vector2(5, 2.5f), new Vector2(1f, Dlength / 2.5f) * GPosSec[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 10.5f + Dlength * 2) * GPosSec[x].Size, sRtDrop, color, 0, new Vector2(5, 7.5f), GPosSec[x].Size, SpriteEffects.None, 0);
                }
            }
        }
        /// <summary>
        /// ��ȡ���ƾ���
        /// </summary>
        /// <param name="texSize"></param>
        /// <param name="MoveStep"></param>
        /// <returns></returns>
        public Rectangle GetDrawRec(Vector2 texSize, float MoveStep, bool Correction)
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + (texSize / 2);
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Vector2 DCen = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 deltaPos = DCen - new Vector2((mothLand.fireflyCenterX + 34) * 16f, mothLand.fireflyCenterY * 16f);
            deltaPos *= MoveStep;
            Vector2 Cor = GetZoomByScreenSize();
            int RX = (int)(sampleCenter.X - screenSize.X / 2f + deltaPos.X);
            int RY = (int)(sampleCenter.Y - screenSize.Y / 2f + deltaPos.Y);
            if (Correction)
            {
                RX = (int)(sampleCenter.X - screenSize.X / 2f / Cor.X + deltaPos.X);
                RY = (int)(sampleCenter.Y - screenSize.Y / 2f / Cor.Y + deltaPos.Y);
                screenSize.X /= Cor.X;
                screenSize.Y /= Cor.Y;
            }
            return new Rectangle(RX, RY, (int)(screenSize.X), (int)(screenSize.Y));
        }
        /// <summary>
        /// ��ȡXY�����ű���
        /// </summary>
        /// <param name="texSize"></param>
        /// <param name="MoveStep"></param>
        /// <returns></returns>
        public static Vector2 GetZoomByScreenSize()
        {
            //return new Vector2(Main.screenWidth / 1366f, Main.screenHeight / 768f);
            return Vector2.One;
        }
        /// <summary>
        /// ��ȡ��Ϊ��ͬ�ֱ��ʵ��µ�λƫ������
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetZoomDelta()
        {
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Vector2 Cor = GetZoomByScreenSize();
            Vector2 v0 = screenSize / 2f - screenSize / 2f / Cor;
            return v0;
        }
        /// <summary>
        /// ��Ȼ�ǻ���������
        /// </summary>
        private void DrawBackground()
        {
            if (alpha <= 0)
            {
                return;
            }
            var texSky = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflySky");
            var texFar = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyFar");
            var texMiddle = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyMiddle");
            var texMidClose = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyMidClose");
            var texClose = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyClose");
            var texCloseII = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyClose2");
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Rectangle screen = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            Vector2 ScreenCen = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 DSize = GetZoomByScreenSize();
            Vector2 ZoomDelta = GetZoomDelta();
            Vector2 DrawPos = ScreenCen;
            Color color0 = Color.White * alpha;
            Main.spriteBatch.Draw(texSky, DrawPos, GetDrawRec(texSky.Size(), 0, true), color0, 0,
                 ScreenCen, DSize, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texFar, DrawPos, GetDrawRec(texSky.Size(), 0.03f, true), color0, 0,
                 ScreenCen, DSize, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texMiddle, DrawPos, GetDrawRec(texSky.Size(), 0.17f, true), color0, 0,
                 ScreenCen, DSize, SpriteEffects.None, 0);
            DrawGlowSec(texClose.Size(), 0.17f);
            Main.spriteBatch.Draw(texMidClose, DrawPos, GetDrawRec(texSky.Size(), 0.25f, false), GetLuminace( color0), 0,
                 ScreenCen, DSize, SpriteEffects.None, 0);
            DrawGlow(texClose.Size(), 0.25f);
            Rectangle rvc = GetDrawRec(texClose.Size(), 0.33f, false);
            RopOffset = new(-150 * 1.12f, 484 + 120 * 0.17f);//ƫ����
            rvc.Y -= 120;
            rvc.X += 150;
            Main.spriteBatch.Draw(texClose, Vector2.Zero, rvc, GetLuminace(color0));


            /*
            OldMouseW[0] = Main.MouseWorld;
            for (int f = OldMouseW.Length - 1; f > 0; f--)
            {
                OldMouseW[f] = OldMouseW[f - 1];
            }
            List<Vector2> oldM = new List<Vector2>();
            for (int f = 0; f < OldMouseW.Length; f++)
            {
                if (OldMouseW[f] != Vector2.Zero)
                {
                    oldM.Add(OldMouseW[f]);
                }
            }*/
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
            //InitMass_Spring();
            float gravity = 1.0f;
            List<Vertex2D> Vertices = new List<Vertex2D>();
            for (int i = 0; i < RopPosFir.Count; i++)
            {
                masses[i][0].position = Vector2.Zero;
                float deltaTime = 1;
                foreach (var spring in springs[i])
                {
                    spring.ApplyForce(deltaTime);
                }
                List<Vector2> massPositions = new List<Vector2>();
                foreach (var massJ in masses[i])
                {
                    massJ.Update(deltaTime);
                    massPositions.Add(massJ.position);
                }
                List<Vector2> massPositionsSmooth = new List<Vector2>();
                massPositionsSmooth = Commons.Function.BezierCurve.Bezier.SmoothPath(massPositions);
                if (massPositionsSmooth.Count > 0)
                {
                    DrawRope(massPositionsSmooth, RopPosFir[i] + RopOffset, Vertices);
                }
            }
            if (Vertices.Count > 2)
            {
                Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertices.ToArray(), 0, Vertices.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < RopPosFir.Count; i++)
            {
                Vector2 TexLT = GetRopeMove(new Vector2(800, 400), 0.33f);
                foreach (var massJ in masses[i])
                {
                    Vector2 DrawP = massJ.position + RopPosFir[i] + RopOffset;
                    massJ.force += new Vector2(0.02f + 0.02f * (float)(Math.Sin(Main.timeForVisualEffects / 72f + DrawP.X / 13d + DrawP.Y / 4d)), 0) * (Main.windSpeedCurrent + 1f) * 2f;
                    //mass.force -= mass.velocity * 0.1f;
                    // �������ٶȣ��ɵ�
                    massJ.force += new Vector2(0, gravity) * massJ.mass;
                    Texture2D t0 = MythContent.QuickTexture("TheFirefly/Backgrounds/Drop");
                    int FiIdx = masses[i].FindIndex(mass => mass.position == massJ.position);
                    Vector2 FinalDrawP = PointCorrection(DrawP - TexLT);
                    float Scale = massJ.mass * 2f;
                    if (FiIdx > 0)
                    {
                        Vector2 v0 = massJ.position - masses[i][FiIdx - 1].position;
                        float Rot = (float)(Math.Atan2(v0.Y, v0.X)) - (float)(Math.PI / 2d);
                        Color color = GetLuminace( new Color(0, 0.15f * FiIdx, 1f / 5f * FiIdx, 0) * alpha);
                        for (int z = 0; z < FiIdx; z++)
                        {
                            Main.spriteBatch.Draw(t0, DrawP - TexLT, null, color, Rot, t0.Size() / 2f, Scale, SpriteEffects.None, 0);
                        }
                    }
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Rectangle rvcII = GetDrawRec(texCloseII.Size(), 0.57f, false);
            rvcII.Y -= 300;
            rvcII.X += 300;
            Color colorCloseII = GetLuminace(Color.White * alpha);
            List<Vertex2D> CloseII = new List<Vertex2D>
            {
                new Vertex2D(new Vector2(0, 0), colorCloseII, new Vector3(rvcII.X / (float)texCloseII.Width, rvcII.Y / (float)texCloseII.Height, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, 0), colorCloseII, new Vector3((rvcII.X + rvcII.Width) / (float)texCloseII.Width, rvcII.Y / (float)texCloseII.Height, 0)),
                new Vertex2D(new Vector2(0, Main.screenHeight), colorCloseII, new Vector3(rvcII.X / (float)texCloseII.Width, (rvcII.Y + rvcII.Height) / (float)texCloseII.Height, 0)),

                new Vertex2D(new Vector2(0, Main.screenHeight), colorCloseII, new Vector3(rvcII.X / (float)texCloseII.Width, (rvcII.Y + rvcII.Height) / (float)texCloseII.Height, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, 0), colorCloseII, new Vector3((rvcII.X + rvcII.Width) / (float)texCloseII.Width, rvcII.Y / (float)texCloseII.Height, 0)),
                new Vertex2D(new Vector2(Main.screenWidth, Main.screenHeight), colorCloseII, new Vector3((rvcII.X + rvcII.Width) / (float)texCloseII.Width, (rvcII.Y + rvcII.Height) / (float)texCloseII.Height, 0))
            };
            Effect bgW = MythContent.QuickEffect("Effects/BackgroundWrap");
            bgW.CurrentTechnique.Passes[0].Apply();

            if (CloseII.Count > 2)
            {
                Main.graphics.GraphicsDevice.Textures[0] = texCloseII;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, CloseII.ToArray(), 0, CloseII.Count - 2);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public Vector2[] OldMouseW = new Vector2[30];
        private List<List<Mass>> masses = new List<List<Mass>>();
        private List<List<Spring>> springs = new List<List<Spring>>();
        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void InitMass_Spring()
        {
            masses.Clear();
            springs.Clear();
            for (int j = 0; j < RopPosFir.Count; j++)
            {
                masses.Add(new List<Mass>());
                springs.Add(new List<Spring>());
                for (int i = 0; i < RopPosFirC[j]; i++)
                {
                    float x = i == RopPosFirC[j] - 1 ? 1.3f : 1f;
                    masses[j].Add(new Mass(RopPosFirS[j] * Main.rand.NextFloat(0.45f, 0.55f) * x,
                        Main.MouseScreen + new Vector2(0, 6 * i), i == 0));
                }
                for (int i = 1; i < RopPosFirC[j]; i++)
                {
                    springs[j].Add(new Spring(0.3f, 20, 0.05f, masses[j][i - 1], masses[j][i]));
                }
            }
        }
        /// <summary>
        /// У��ӫ������λ��
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        private static Vector2 GetRopeMove(Vector2 Size, float move)
        {
            Vector2 DCen = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + Size;
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Vector2 deltaPos = DCen - new Vector2(mothLand.fireflyCenterX * 16f, mothLand.fireflyCenterY * 16f);
            deltaPos *= move;
            Vector2 TexLT = sampleCenter - screenSize / 2f + deltaPos;
            return TexLT;
        }

        /// <summary>
        /// ����ǰ���ӵĶ���Mesh���ݴ���vertices��
        /// </summary>
        /// <param name="massPositionsSmooth"></param>
        /// <param name="offset"></param>
        /// <param name="vertices"></param>
        private void DrawRope(List<Vector2> massPositionsSmooth, Vector2 offset, List<Vertex2D> vertices)
        {
            if (vertices.Count != 0)
            {
                // ����һ�����㣬������һ��Rope Mesh���˻�������
                vertices.Add(vertices.Last());
            }
            int count = massPositionsSmooth.Count;
            float baseWidth = 4;
            offset -= GetRopeMove(new Vector2(800, 400), 0.33f);
            // count �������1
            for (int i = 0; i < count; i++)
            {
                Vector2 dir;
                if (i == 0)
                {
                    dir = massPositionsSmooth[1] - massPositionsSmooth[0];

                }
                else
                {
                    dir = massPositionsSmooth[i] - massPositionsSmooth[i - 1];
                }

                Vector2 normalDir = Vector2.Normalize(new Vector2(-dir.Y, dir.X));
                float width = baseWidth * (count - i - 1) / (count - 1);
                Color color = GetLuminace(new Color(11, 9, 25) * alpha);
                var vertex1 = new Vertex2D(massPositionsSmooth[i] + offset + normalDir * width,color , Vector3.Zero);
                if (i == 0)
                {
                    // ������һ���˻������ζ���
                    vertices.Add(vertex1);
                }
                vertices.Add(vertex1);
                vertices.Add(new Vertex2D(massPositionsSmooth[i] + offset - normalDir * width, color, Vector3.Zero));
            }
        }
        private Color GetLuminace(Color color)
        {
            if (luminance != 1)
            {
                byte a = color.A;
                color *= luminance;
                color.A = a;
            }
            return color;
        }
    }
}

