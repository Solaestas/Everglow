
using Everglow.Sources.Commons.Function.Vertex;

using Everglow.Sources.Modules.YggdrasilModule.Common;
using Everglow.Sources.Modules.YggdrasilModule.Common.BackgroundManager;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Background
{
    public class MineRoadBackground : ModSystem
    {
        Vector2 BiomeCenter = new Vector2(7200, 180000);
        /// <summary>
        /// 初始化
        /// </summary>
        public override void OnModLoad()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Everglow.HookSystem.AddMethod(DrawBackground, Commons.Core.CallOpportunity.PostDrawBG);
                On.Terraria.Graphics.Light.TileLightScanner.GetTileLight += TileLightScanner_GetTileLight;
                //GetRopePosFir("TreeRope");
                //InitMass_Spring();
            }
        }
        /// <summary>
        /// 环境光的钩子
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="outputColor"></param>
        private void TileLightScanner_GetTileLight(On.Terraria.Graphics.Light.TileLightScanner.orig_GetTileLight orig, Terraria.Graphics.Light.TileLightScanner self, int x, int y, out Vector3 outputColor)
        {
            orig(self, x, y, out outputColor);
            outputColor += BiomeActive() ? new Vector3(0.001f, 0.001f, 0.05f) : Vector3.Zero;
        }
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
            
            if (Main.screenPosition.Y > 172200)
            {
                //if (SubworldSystem.IsActive<YggdrasilWorld>())
                //{
                //    return true;
                //}
            }
            return false;
        }
  
        private void DrawYggdrasilTownBackground(Color baseColor)
        {
            var texSky = YggdrasilContent.QuickTexture("YggdrasilTown/Background/MineRoadBackgroundSky");
            var texClose = YggdrasilContent.QuickTexture("YggdrasilTown/Background/MineRoadBackgroundClose");
            var texC1 = YggdrasilContent.QuickTexture("YggdrasilTown/Background/MineRoadBackgroundC1");
            var texC2 = YggdrasilContent.QuickTexture("YggdrasilTown/Background/MineRoadBackgroundC2");
            var texC3 = YggdrasilContent.QuickTexture("YggdrasilTown/Background/MineRoadBackgroundC3");
            var texBound = YggdrasilContent.QuickTexture("KelpCurtain/Background/KelpCurtainBound");

            BackgroundManager.QuickDrawBG(texSky, GetDrawRect(texSky.Size(), 0f, true), baseColor, 173400, 200000);
            BackgroundManager.QuickDrawBG(texC3, GetDrawRect(texClose.Size(), 0.05f, true), baseColor, 173400, 200000, false, false);
            BackgroundManager.QuickDrawBG(texC2, GetDrawRect(texClose.Size(), 0.10f, true), baseColor, 173400, 200000, false, false);
            BackgroundManager.QuickDrawBG(texC1, GetDrawRect(texClose.Size(), 0.15f, true), baseColor, 173400, 200000, false, true);

            Vector2 DCen = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 deltaPos = DCen - BiomeCenter;
            float MoveStep = 0.15f;
            deltaPos *= MoveStep;
            for (int x = -5; x < 6; x++)
            {
                Vector2 DrawCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f - deltaPos + new Vector2(-650 + texC1.Width * x, -400);
                if (DrawCenter.X >= -60 && DrawCenter.X <= Main.screenWidth + 60)
                {
                    BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.15f, DrawCenter, 20f, 750f, baseColor * 0.06f, 173400, 200000, texC1.Size(), false, false);
                }
                DrawCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f - deltaPos + new Vector2(-1350 + texC1.Width * x, -100);
                if (DrawCenter.X >= -60 && DrawCenter.X <= Main.screenWidth + 60)
                {
                    BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.15f, DrawCenter, 20f, 750f, baseColor * 0.06f, 173400, 200000, texC1.Size(), false, false);
                }
            }
            BackgroundManager.QuickDrawBG(texClose, GetDrawRect(texClose.Size(), 0.35f, true), baseColor, 173400, 200000);

            BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.35f, new Vector2(-650, -400), 60f, 550f, baseColor * 0.12f, 173400, 200000, texClose.Size());
            BackgroundManager.QuickDrawBG(texBound, GetDrawRect(texBound.Size(), 1f, true), baseColor, 173280, 173580, false, false);
        }
        
        /// <summary>
        /// 获取XY向缩放比例
        /// </summary>
        /// <param name="texSize"></param>
        /// <param name="MoveStep"></param>
        /// <returns></returns>
        public static Vector2 GetZoomByScreenSize()
        {
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
            Vector2 sampleTopleft = Vector2.Zero;
            Vector2 sampleCenter = sampleTopleft + (texSize / 2);
            Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
            Vector2 DCen = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Vector2 deltaPos = DCen - BiomeCenter;
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
            return new Rectangle(RX, RY, (int)screenSize.X, (int)screenSize.Y);
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
            DrawYggdrasilTownBackground(baseColor);
        }
    }
}