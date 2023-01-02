using Everglow.Sources.Modules.MythModule.LanternMoon.NPCs.LanternGhostKing;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.Common
{
    public class DrawBossUI
    {
        private static void Main_DrawUI(On.Terraria.Main.orig_DrawInterface_33_MouseText orig, Terraria.Main self)
        {
            orig.Invoke(self);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (LanternGhostKing.VagueStre > 0f)
            {
                float x0 = LanternGhostKing.VagueStre / 0.08f;
                Texture2D BossUI = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/LanternMoon/LanternGhostKingEN").Value;
                if (Language.ActiveCulture.Name == "zh-Hans")
                {
                    BossUI = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/LanternMoon/LanternGhostKingCH").Value;
                }
                float fx0 = (float)x0 * x0 * x0;
                Vector2 vpos = Main.LocalPlayer.Center - Main.screenPosition;
                Vector2 vu = new Vector2(Main.screenWidth / 2f - 162, Main.screenHeight / 2f - BossUI.Height / 2f * fx0 + 200);
                Vector2 vv = new Vector2(Main.screenWidth / 2f - 162, Main.screenHeight / 2f + 200);
                Vector2 Dc = new Vector2(BossUI.Width / 2f, BossUI.Height / 2f);
                Rectangle rec1 = new Rectangle(0, 0, BossUI.Width, (int)(BossUI.Height / 2f * fx0) + 1);
                Rectangle rec2 = new Rectangle(0, (int)(BossUI.Height / 2f * (2 - fx0)), BossUI.Width, (int)(BossUI.Height / 2f * fx0) + 1);
                Main.spriteBatch.Draw(BossUI, vu, rec1, Color.White, 0, Dc, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(BossUI, vv, rec2, Color.White, 0, Dc, 1f, SpriteEffects.None, 0f);
            }
        }
        public static void Load()
        {
            On.Terraria.Main.DrawInterface_33_MouseText += Main_DrawUI;
        }
        public static void Unload()
        {
            //On.Terraria.Main.DrawInterface_33_MouseText -= Main_DrawUI;
        }
    }
}