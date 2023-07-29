//using On.Terraria;

namespace Everglow.Ocean.Backgrounds
{
    public class HookBackGroundShallowOcean
    {
        public static void Load()
        {
            Terraria.On_Main.DoDraw_WallsAndBlacks += Main_DrawBackground;
        }
        public static void UnLoad()
        {
            //On.Terraria.Main.DoDraw_WallsAndBlacks -= Main_DrawBackground;
        }
        private static void Main_DrawBackground(Terraria.On_Main.orig_DoDraw_WallsAndBlacks orig, Terraria.Main self)
        {
            Player player = Main.LocalPlayer;
            bool b1 = Main.ActiveWorldFileData.Path.Contains("OcEaNMyTh");
            bool b2 = Main.LocalPlayer.position.X <= Main.maxTilesX * 0.8778 || Main.LocalPlayer.position.X >= Main.maxTilesX * 0.9456;
            if (b1 && b2)
            {
                Color DrawC = Main.ColorOfTheSkies;
                float LigR = DrawC.R / 255f;
                float LigG = DrawC.G / 255f;
                float LigB = DrawC.B / 255f;
                float DeltaSizX = 803;
                float DeltaSizY = 360;
                float MoveSize = 5f;
                Effect ef;
                ef = (Effect)ModContent.Request<Effect>("Everglow/Ocean/Effects/OcenaUBG").Value;
                float SxF0 = (Main.LocalPlayer.Center.X - DeltaSizX * 16f) / MoveSize + 320;//1620
                float SyF0 = (Main.LocalPlayer.Center.Y - DeltaSizY * 16f) / MoveSize - 200;//1210
                float OceanY = (228 * 16 - Main.screenPosition.Y);
                float TrueOceanY = 228 * 16;
                float DeltaPlOceanY = (TrueOceanY - player.Center.Y) / MoveSize;
                /*Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);*/
                Terraria.Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Ocean/Backgrounds/CoralFar").Value, new Vector2(0, OceanY), new Rectangle((int)(SxF0), (int)(Math.Clamp(SyF0 - OceanY, 0, 20000) + DeltaPlOceanY), Terraria.Main.screenWidth, Terraria.Main.screenHeight + 600), new Color(LigR, LigG, LigB, 1f), 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                /*Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                ef.Parameters["uOCY"].SetValue(OceanY);
                ef.CurrentTechnique.Passes["Test"].Apply();*/
                int Lx = (int)(Main.screenPosition.X / 16d);
                int Ly = (int)(Main.screenPosition.Y / 16d);
                for (int x = -20; x < Main.screenWidth / 16 + 20; x++)
                {
                    for (int y = -20; y < Main.screenHeight / 16 + 20; y++)
                    {
                        if (Main.tile[Lx + x, Ly + y].LiquidType == byte.MaxValue)
                        {
                            Lighting.AddLight(Lx + x, Ly + y, LigR * 0.2f, LigG * 0.2f, LigB * 0.2f);
                        }
                        else
                        {
                            Lighting.AddLight(Lx + x, Ly + y, LigR * 0.05f, LigG * 0.05f, LigB * 0.05f);
                        }
                    }
                }
            }
            orig.Invoke(self);
        }
    }
}
