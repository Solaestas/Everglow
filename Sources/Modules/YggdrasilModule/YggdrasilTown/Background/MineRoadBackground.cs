using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.TheFirefly.NPCs.Bosses;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Background
{
    public class MineRoadBackground : ModSystem
    {
        private float alpha = 0f;
        public override void PostUpdateEverything()//开启地下背景
        {
            const float increase = 0.02f;
            if (BiomeActive() && Main.BackgroundEnabled)
            {
                if (alpha < 1)
                {
                    alpha += increase;
                }
                else
                {
                    alpha = 1;
                    Everglow.HookSystem.DisableDrawBackground = true;
                }

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
                }
                Everglow.HookSystem.DisableDrawBackground = false;
            }
        }
        /// <summary>
        /// 判定是否开启地形
        /// </summary>
        /// <returns></returns>
        public static bool BiomeActive()
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            Vector2 BiomeCenter = new Vector2(mothLand.fireflyCenterX * 16, (mothLand.fireflyCenterY - 20) * 16);//读取地形信息
            Vector2 v0 = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f - BiomeCenter;//距离中心Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f
            v0.Y *= 1.35f;
            v0.X *= 0.9f;//近似于椭圆形，所以xy坐标变换
            return (v0.Length() < 2000);
        }
  
        private void DrawFarBG(Color baseColor)
        {
            var texSky = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflySky");
            var texFar = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyFar");
            var texMiddle = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyMiddle");
            var texMiddleGlow = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyMiddleGlow");
            var texMidClose = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyMidClose");
            var texClose = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyClose");
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Vector2 ScreenCen = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 DSize = GetZoomByScreenSize();
            Vector2 DrawPos = ScreenCen;

            Main.spriteBatch.Draw(texMidClose, DrawPos, GetDrawRect(texSky.Size(), 0.25f, false), baseColor, 0,
                 ScreenCen, DSize, SpriteEffects.None, 0);

        }
        /// <summary>
        /// 获取XY向缩放比例
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
        /// 获取绘制矩形
        /// </summary>
        /// <param name="texSize"></param>
        /// <param name="MoveStep"></param>
        /// <returns></returns>
        public Rectangle GetDrawRect(Vector2 texSize, float MoveStep, bool Correction)
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
        private void DrawCloseBG(Color baseColor)
        {

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>
        /// 当然是绘制主体啦
        /// </summary>
        private void DrawBackground()
        {
            if (alpha <= 0)
            {
                return;
            }
            Color baseColor = Color.White * alpha;
            DrawFarBG(baseColor);
            DrawCloseBG(baseColor);
        }
    }
}