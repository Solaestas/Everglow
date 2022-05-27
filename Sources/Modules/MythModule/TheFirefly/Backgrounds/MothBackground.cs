using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Physics;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds
{
    public class MothBackground : ModSystem
    {     
        //加了个环境光，但还要调整下不然看上去很怪
        public readonly Vector3 ambient = new Vector3(0.001f, 0.001f, 0.05f);
        public List<GHang> GPos = new List<GHang>();
        public List<GHang> GPosSec = new List<GHang>();
        public override void OnModLoad()
        {
            Everglow.HookSystem.AddMethod(DrawBackground, Commons.Core.CallOpportunity.PostDrawBG);
            On.Terraria.Graphics.Light.TileLightScanner.GetTileLight += TileLightScanner_GetTileLight;
        }
        /// <summary>
        /// 荧光悬挂物的数据结构
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
        private void TileLightScanner_GetTileLight(On.Terraria.Graphics.Light.TileLightScanner.orig_GetTileLight orig, Terraria.Graphics.Light.TileLightScanner self, int x, int y, out Vector3 outputColor)
        {
            orig(self, x, y, out outputColor);
            outputColor += ambient;
        }

        public override void PostUpdateEverything()//开启地下背景
        {          
            if (BiomeActive())
            {
                Everglow.HookSystem.DisableDrawBackground = true;
            }
            else
            {
                Everglow.HookSystem.DisableDrawBackground = false;
            }      
        }      
        //判定是否开启地形
        public bool BiomeActive()
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 BiomeCenter = new Vector2(mothLand.FireflyCenterX * 16, (mothLand.FireflyCenterY - 20) * 16);//读取地形信息
            Vector2 v0 = Main.LocalPlayer.Center - BiomeCenter;//距离中心
            v0.Y *= 1.35f;
            v0.X *= 0.9f;//近似于椭圆形，所以xy坐标变换
            return (v0.Length() < 2000);
        }
        /// <summary>
        /// 获取绘制矩形
        /// </summary>
        /// <param name="texSize"></param>
        /// <param name="MoveStep"></param>
        /// <returns></returns>
        public Rectangle GetDrawRec(Vector2 texSize, float MoveStep)
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + (texSize / 2);
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Player localP = Main.LocalPlayer;
            Vector2 deltaPos = localP.Center - new Vector2((mothLand.FireflyCenterX + 34) * 16f, mothLand.FireflyCenterY * 16f);
            deltaPos *= MoveStep;
            return new Rectangle((int)(sampleCenter.X - screenSize.X / 2 + deltaPos.X), (int)(sampleCenter.Y - screenSize.Y / 2 + deltaPos.Y), (int)(screenSize.X), (int)(screenSize.Y));
        }
        /// <summary>
        /// 获取荧光悬挂物点位
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <exception cref="Exception"></exception>
        public void GetGlowPos(string Shapepath)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new Exception("Windows限定");
            }
            using Stream Img = Everglow.Instance.GetFileStream("Sources/Modules/MythModule/TheFirefly/Backgrounds/" + Shapepath);
            Bitmap image = new Bitmap(Img);
            for (int y = image.Height - 1; y > 0 ; y -= 1)
            {
                for (int x = image.Width - 1; x > 0; x -= 1)
                {
                    if (image.GetPixel(x, y).R == 255)
                    {
                        GPos.Add(new GHang(new Vector2(x * 10, y * 10), (image.GetPixel(x, y).G / 4f + 2), image.GetPixel(x, y).B / 255f + 0.5f, Main.rand.Next(5)));
                    }
                }
            }
        }
        /// <summary>
        /// 获取第二层荧光悬挂物点位
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <exception cref="Exception"></exception>
        public void GetGlowPosSec(string Shapepath)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new Exception("Windows限定");
            }
            using Stream Img = Everglow.Instance.GetFileStream("Sources/Modules/MythModule/TheFirefly/Backgrounds/" + Shapepath);
            Bitmap image = new Bitmap(Img);
            for (int y = image.Height - 1; y > 0; y -= 1)
            {
                for (int x = image.Width - 1; x > 0; x -= 1)
                {
                    if (image.GetPixel(x, y).R == 255)
                    {
                        GPosSec.Add(new GHang(new Vector2(x * 10, y * 4.2f), (image.GetPixel(x, y).G / 4f + 2), image.GetPixel(x, y).B / 255f + 0.5f, Main.rand.Next(5)));
                    }
                }
            }
        }
        /// <summary>
        /// 绘制荧光
        /// </summary>
        private void DrawGlow(Vector2 texSize, float MoveStep)
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + (texSize / 2);
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Player localP = Main.LocalPlayer;
            Vector2 deltaPos = localP.Center - new Vector2((mothLand.FireflyCenterX + 34) * 16f, mothLand.FireflyCenterY * 16f);
            deltaPos *= MoveStep;
            Vector2 TexLT = sampleCenter - screenSize / 2f + deltaPos;
            if (GPos.Count <= 1)
            {
                GetGlowPos("GlosPos.bmp");
            }
            float deltaY = localP.Center.Y - (mothLand.FireflyCenterY - 90) * 16f;
            deltaY *= MoveStep * 0.4f;
            if (GPos.Count > 0)
            {
                var texGlow = MythContent.QuickTexture("TheFirefly/Backgrounds/GlowHanging");
                for (int x = 0; x < GPos.Count; x++)
                {
                    Vector2 GP = GPos[x].Pos;
                    GP.Y += deltaY / (GPos[x].Size + 1);
                    Vector2 dPos = GP - TexLT + new Vector2(0, -194);
                    Rectangle sRtTop = new Rectangle(GPos[x].Type * 20, 0, 20, 10);
                    Rectangle sRtLine = new Rectangle(GPos[x].Type * 20, 10, 20, 20);
                    Rectangle sRtDrop = new Rectangle(GPos[x].Type * 20, 65, 20, 35);
                    float Dlength = (float)(GPos[x].Length + Math.Sin(Main.time / 128d + GPos[x].Pos.X / 70d + GPos[x].Pos.Y / 120d) * GPos[x].Length * 0.2f);
                    Main.spriteBatch.Draw(texGlow, dPos, sRtTop, Color.White, 0, new Vector2(10, 0), GPos[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 10 + Dlength) * GPos[x].Size, sRtLine, Color.White, 0, new Vector2(10, 10), new Vector2(1f, Dlength / 10f) * GPos[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 27.5f + Dlength * 2) * GPos[x].Size, sRtDrop, Color.White, 0, new Vector2(10, 17.5f), GPos[x].Size, SpriteEffects.None, 0);
                }
            }
        }
        /// <summary>
        /// 绘制第二层荧光
        /// </summary>
        private void DrawGlowSec(Vector2 texSize, float MoveStep)
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + (texSize / 2);
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Player localP = Main.LocalPlayer;
            Vector2 deltaPos = localP.Center - new Vector2((mothLand.FireflyCenterX + 34) * 16f, mothLand.FireflyCenterY * 16f);
            deltaPos *= MoveStep;
            Vector2 TexLT = sampleCenter - screenSize / 2f + deltaPos;
            if (GPosSec.Count <= 1)
            {
                GetGlowPosSec("GlowHangingMiddle.bmp");
            }
            float deltaY = localP.Center.Y - (mothLand.FireflyCenterY - 90) * 16f;
            deltaY *= MoveStep * 0.4f;
            if (GPosSec.Count > 0)
            {
                var texGlow = MythContent.QuickTexture("TheFirefly/Backgrounds/GlowHangingMiddle");
                for (int x = 0; x < GPosSec.Count; x++)
                {
                    Vector2 GP = GPosSec[x].Pos;
                    GP.Y += deltaY / (GPosSec[x].Size + 1);
                    Vector2 dPos = GP - TexLT + new Vector2(0, -194);
                    Rectangle sRtTop = new Rectangle(GPosSec[x].Type * 10, 0, 10, 3);
                    Rectangle sRtLine = new Rectangle(GPosSec[x].Type * 10, 3, 10, 5);
                    Rectangle sRtDrop = new Rectangle(GPosSec[x].Type * 10, 10, 10, 15);
                    float Dlength = (float)(GPosSec[x].Length + Math.Sin(Main.time / 128d + GPosSec[x].Pos.X / 70d + GPosSec[x].Pos.Y / 120d) * GPosSec[x].Length * 0.2f);
                    Main.spriteBatch.Draw(texGlow, dPos, sRtTop, Color.White, 0, new Vector2(5, 0), GPosSec[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 3 + Dlength) * GPosSec[x].Size, sRtLine, Color.White, 0, new Vector2(5, 2.5f), new Vector2(1f, Dlength / 2.5f) * GPosSec[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 10.5f + Dlength * 2) * GPosSec[x].Size, sRtDrop, Color.White, 0, new Vector2(5, 7.5f), GPosSec[x].Size, SpriteEffects.None, 0);
                }
            }
        }

        private void DrawBackground()
        {
            if (!BiomeActive())
            {
                return;
            }
            var texSky = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflySky");
            var texFar = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyFar");
            var texMiddle = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyMiddle");
            var texMidClose = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyMidClose");
            var texClose = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyClose");
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Rectangle screen = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            Main.spriteBatch.Draw(texSky, screen, GetDrawRec(texSky.Size(), 0), Color.White);
            Main.spriteBatch.Draw(texFar, screen, GetDrawRec(texFar.Size(), 0.03f), Color.White);
            Main.spriteBatch.Draw(texMiddle, screen, GetDrawRec(texMiddle.Size(), 0.17f), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            DrawGlowSec(texClose.Size(), 0.17f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(texMidClose, screen, GetDrawRec(texMidClose.Size(), 0.25f), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            DrawGlow(texClose.Size(), 0.25f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(texClose, screen, GetDrawRec(texClose.Size(), 0.33f), Color.White);
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
            }
            //List<Vector2> L = Commons.Function.BezierCurve.Bezier.GetBezier(oldM, 90);
            //List<Vector2> K = Commons.Function.BezierCurve.Bezier.GetBezier(L, 2000);
            List<Vector2> K = Commons.Function.BezierCurve.Bezier.SmoothPath(oldM);
            // K = Commons.Function.BezierCurve.Bezier.SmoothPath(K);
            // 可多次采样但是效果不明显，而点的数量急剧增加

            if (K.Count >= 2)
            {
                for (int f = 0; f < K.Count - 1; f++)
                {
                    Texture2D t0 = TextureAssets.MagicPixel.Value;
                    float distance = Math.Max(Vector2.Distance(K[f + 1], K[f]) / 4f, 2);
                    for (int i = 0; i < distance; i++)
                    {
                        Vector2 pos = Vector2.Lerp(K[f], K[f + 1], i / distance);
                        Main.spriteBatch.Draw(t0, pos - Main.screenPosition, new Rectangle(0, 0, 4, 4), Color.Red, 0, new Vector2(2), 1, SpriteEffects.None, 0);
                    }
                }
            }



            //for (int f = 0; f < L.Count; f++)
            //{
            //    Texture2D t0 = TextureAssets.MagicPixel.Value;
            //    Main.spriteBatch.Draw(t0, L[f] - Main.screenPosition, new Rectangle(0, 0, 4, 4), Color.Green, 0, new Vector2(2), 2, SpriteEffects.None, 0);
            //}
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        }
        public Vector2[] OldMouseW = new Vector2[30];

            List<Mass> masses = new List<Mass>();
            for (int i = 0; i < 10; i++)
            {
                masses.Add(new Mass())
            }
        }
    }
}

