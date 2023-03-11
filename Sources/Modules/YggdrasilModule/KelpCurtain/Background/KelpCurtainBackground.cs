
using Everglow.Sources.Commons.Function.Vertex;

using Everglow.Sources.Modules.YggdrasilModule.Common;
using Everglow.Sources.Modules.YggdrasilModule.Common.BackgroundManager;
using Everglow.Sources.Modules.SubWorldModule;

namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Background
{
    public class KelpCurtainBackground : ModSystem
    {
        Vector2 BiomeCenter = new Vector2(9000, 157000);
        /// <summary>
        /// ��ʼ��
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
            outputColor += BiomeActive() ? new Vector3(0.001f, 0.001f, 0.05f) : Vector3.Zero;
        }
        private float alpha = 0f;
        public override void PostUpdateEverything()//�������±���
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
        /// �ж��Ƿ�������
        /// </summary>
        /// <returns></returns>
        public static bool BiomeActive()
        {
            
            if (Main.screenPosition.Y > 148000 && Main.screenPosition.Y < 173200)
            {
                if (SubworldSystem.IsActive<YggdrasilWorld>())
                {
                    return true;
                }
            }
            return false;
        }
  
        private void DrawFarBG(Color baseColor)
        {
            var texSky = YggdrasilContent.QuickTexture("KelpCurtain/Background/KelpCurtainSky");
            var texClose = YggdrasilContent.QuickTexture("KelpCurtain/Background/KelpCurtainClose");
            var texC1 = YggdrasilContent.QuickTexture("KelpCurtain/Background/KelpCurtainMiddle");
            var texC2 = YggdrasilContent.QuickTexture("KelpCurtain/Background/KelpCurtainFar");

            BackgroundManager.QuickDrawBG(texSky, GetDrawRect(texSky.Size(), 0f, true), baseColor, 148000, 173375, true, true);
            BackgroundManager.QuickDrawBG(texC2, GetDrawRect(texC2.Size(), 0.10f, true), baseColor, 148000, 173375, false, false);
            BackgroundManager.QuickDrawBG(texC1, GetDrawRect(texC1.Size(), 0.15f, true), baseColor, 148000, 173375, false, false);
            BackgroundManager.QuickDrawBG(texClose, GetDrawRect(texClose.Size(), 0.35f, true), baseColor, 148000, 173375, false, false);
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
        /// ��ȡ���ƾ���
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
            Rectangle rt = new Rectangle(RX, RY, (int)(screenSize.X), (int)(screenSize.Y));
            return rt;
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
            Color baseColor = Color.White * alpha;
            DrawFarBG(baseColor);
        }
    }
}