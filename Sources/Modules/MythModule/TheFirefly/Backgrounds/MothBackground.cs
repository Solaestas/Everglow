using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds
{
    public class MothBackground : ModSystem
    {     
        //加了个环境光，但还要调整下不然看上去很怪
        public readonly Vector3 ambient = new Vector3(0.001f, 0.001f, 0.05f);
        public List<GHang> GPos = new List<GHang>();
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
            Vector2 deltaPos = localP.Center - new Vector2(mothLand.FireflyCenterX * 16f, mothLand.FireflyCenterY * 16f);
            deltaPos *= MoveStep;
            Vector2 Move = new Vector2((int)deltaPos.X, (int)(deltaPos.Y));
            return new Rectangle((int)(sampleCenter.X - screenSize.X / 2 + Move.X), (int)(sampleCenter.Y - screenSize.Y / 2 + Move.Y), (int)(screenSize.X), (int)(screenSize.Y));
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
            for (int y = 0; y < image.Height; y += 1)
            {
                for (int x = 0; x < image.Width; x += 1)
                {
                    if (image.GetPixel(x, y).R == 255)
                    {
                        GPos.Add(new GHang(new Vector2(x, y), (image.GetPixel(x, y).G / 4f + 2), image.GetPixel(x, y).B / 255f + 0.5f, Main.rand.Next(5)));
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
            Vector2 deltaPos = localP.Center - new Vector2(mothLand.FireflyCenterX * 16f, mothLand.FireflyCenterY * 16f);
            deltaPos *= MoveStep;
            Vector2 Move = deltaPos;
            Vector2 TexLT = sampleCenter - screenSize / 2f + Move;
            if (GPos.Count <= 1)
            {
                GetGlowPos("GlosPos.bmp");
            }
            if(GPos.Count > 0)
            {
                var texGlow = MythContent.QuickTexture("TheFirefly/Backgrounds/GlowHanging");
                for (int x = 0; x < GPos.Count; x++)
                {
                    Vector2 dPos = GPos[x].Pos - TexLT;
                    Rectangle sRtTop = new Rectangle(GPos[x].Type * 20, 0, 20, 10);
                    Rectangle sRtLine = new Rectangle(GPos[x].Type * 20, 10, 20, 20);
                    Rectangle sRtDrop = new Rectangle(GPos[x].Type * 20, 65, 20, 35);
                    float Dlength = (float)(GPos[x].Length + Math.Sin(Main.time / 178d + GPos[x].Pos.X / 70d + GPos[x].Pos.Y / 120d) * GPos[x].Length * 0.1f);
                    Main.spriteBatch.Draw(texGlow, dPos, sRtTop, Color.White, 0, new Vector2(10, 0), GPos[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 10 + Dlength) * GPos[x].Size, sRtLine, Color.White, 0, new Vector2(10, 10), new Vector2(1f, Dlength / 10f) * GPos[x].Size, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texGlow, dPos + new Vector2(0, 27.5f + Dlength * 2) * GPos[x].Size, sRtDrop, Color.White, 0, new Vector2(10, 17.5f), GPos[x].Size, SpriteEffects.None, 0);
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
            var texClose = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyClose");
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Rectangle screen = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            Main.spriteBatch.Draw(texSky, screen, GetDrawRec(texSky.Size(), 0), Color.White);
            Main.spriteBatch.Draw(texFar, screen, GetDrawRec(texFar.Size(), 0.03f), Color.White);
            Main.spriteBatch.Draw(texMiddle, screen, GetDrawRec(texMiddle.Size(), 0.17f), Color.White);
            Main.spriteBatch.Draw(texClose, screen, GetDrawRec(texClose.Size(), 0.25f), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            DrawGlow(texClose.Size(), 0.25f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        }
    }
}

