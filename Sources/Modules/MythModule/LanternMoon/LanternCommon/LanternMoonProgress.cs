using Terraria.Localization;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.LanternCommon
{
    public class LanternMoonProgress : ModSystem//灯笼月
    {
        public override void PostUpdateInvasions()
        {
            if (PreWavePoint[0] == 0)//共计40波，每一波需要的分数
            {
                PreWavePoint[0] = 100;
                PreWavePoint[1] = 150;
                PreWavePoint[2] = 200;
                PreWavePoint[3] = 250;
                PreWavePoint[4] = 300;
                PreWavePoint[5] = 400;
                PreWavePoint[6] = 500;
                PreWavePoint[7] = 500;
                PreWavePoint[8] = 600;//3000
                PreWavePoint[9] = 600;
                PreWavePoint[10] = 600;
                PreWavePoint[11] = 800;//5000
                PreWavePoint[12] = 1000;
                PreWavePoint[13] = 1000;//7000
                PreWavePoint[14] = 3000;//10000

                PreWavePoint[15] = 1200;
                PreWavePoint[16] = 1350;
                PreWavePoint[17] = 1400;
                PreWavePoint[18] = 1500;
                PreWavePoint[19] = 1750;
                PreWavePoint[20] = 2000;//19200
                PreWavePoint[21] = 2250;
                PreWavePoint[22] = 2750;
                PreWavePoint[23] = 3300;//27500
                PreWavePoint[24] = 6000;//33500

                PreWavePoint[25] = 4500;
                PreWavePoint[26] = 5300;
                PreWavePoint[27] = 6100;
                PreWavePoint[28] = 6800;
                PreWavePoint[29] = 7500;
                PreWavePoint[30] = 9000;
                PreWavePoint[31] = 11500;
                PreWavePoint[32] = 12600;
                PreWavePoint[33] = 15000;
                PreWavePoint[34] = 20000;//131800

                PreWavePoint[35] = 123500;
                PreWavePoint[36] = 167800;
                PreWavePoint[37] = 235500;
                PreWavePoint[38] = 341400;//1000000
                PreWavePoint[39] = 0;
            }
            if (OnLanternMoon)
            {
                if (Wave == 0)
                {
                    WavePoint = Point;
                }
                if (WavePoint > PreWavePoint[Wave])
                {
                    if (Wave == 0)
                    {
                        Main.NewText("Wave 2:", new Color(175, 75, 255));
                    }
                    if (Wave == 1)
                    {
                        Main.NewText("Wave 3:", new Color(175, 75, 255));
                    }
                    if (Wave == 2)
                    {
                        Main.NewText("Wave 4:", new Color(175, 75, 255));
                    }
                    if (Wave == 3)
                    {
                        Main.NewText("Wave 5:", new Color(175, 75, 255));
                    }
                    if (Wave == 4)
                    {
                        Main.NewText("Wave 6:", new Color(175, 75, 255));
                    }
                    if (Wave == 5)
                    {
                        Main.NewText("Wave 7:", new Color(175, 75, 255));
                    }
                    WavePoint -= PreWavePoint[Wave];
                    Wave++;
                }
            }
            if (Main.dayTime)
            {
                OnLanternMoon = false;
            }
        }
        public int Wave = 0;
        public int[] PreWavePoint = new int[40];
        public int Point = 0;
        public int WavePoint = 0;
        public bool OnLanternMoon = false;
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (!OnLanternMoon)
            {
                return;
            }
            Main.invasionProgressMax = PreWavePoint[Wave];
            Main.invasionProgress = WavePoint;
            if (OnLanternMoon)
            {
                Main.invasionProgressMode = 2;
                Main.invasionProgressNearInvasion = true;
                Main.invasionProgressWave = Wave + 1;
            }
            if (Main.invasionProgressMode == 2 && Main.invasionProgressNearInvasion && Main.invasionProgressDisplayLeft < 160)
            {
                Main.invasionProgressDisplayLeft = 160;
            }
            if (!Main.gamePaused && Main.invasionProgressDisplayLeft > 0)
            {
                Main.invasionProgressDisplayLeft--;
            }
            if (Main.invasionProgressDisplayLeft > 0)
            {
                Main.invasionProgressAlpha += 0.05f;
            }
            else
            {
                Main.invasionProgressAlpha -= 0.05f;
            }
            if (Main.invasionProgressMode == 0)
            {
                Main.invasionProgressDisplayLeft = 0;
                Main.invasionProgressAlpha = 0f;
            }
            if (Main.invasionProgressAlpha < 0f)
            {
                Main.invasionProgressAlpha = 0f;
            }
            if (Main.invasionProgressAlpha > 1f)
            {
                Main.invasionProgressAlpha = 1f;
            }
            if (Main.invasionProgressAlpha <= 0f)
            {
                return;
            }
            float num = 0.5f + Main.invasionProgressAlpha * 0.5f;
            Texture2D value = MythContent.QuickTexture("UIimages/LanternMoon");
            string text = " Lantern Moon ";
            /*if (Language.ActiveCulture.Name == "zh-Hans")
            {
                text = " 灯笼月 ";
            }*/
            Color c = Color.White;

            if (Main.invasionProgressWave > 0)
            {
                int num2 = (int)(200f * num);
                int num3 = (int)(45f * num);
                Vector2 vector = new Vector2((float)(Main.screenWidth - 120), (float)(Main.screenHeight - 40));
                Rectangle r = new Rectangle((int)vector.X - num2 / 2, (int)vector.Y - num3 / 2, num2, num3);
                Utils.DrawInvBG(Main.spriteBatch, r, new Color(63, 65, 151, 255) * 0.785f);
                string text2;
                if (Main.invasionProgressMax == 0)
                {
                    text2 = WavePoint.ToString();
                }
                else
                {
                    text2 = (int)((float)Main.invasionProgress * 100f / (float)Main.invasionProgressMax) + "%";
                }
                text2 = Language.GetTextValue("Game.WaveMessage", Main.invasionProgressWave, text2);
                Texture2D value2 = TextureAssets.ColorBar.Value;
                Texture2D value3 = TextureAssets.ColorBlip.Value;
                float num4 = MathHelper.Clamp((float)Main.invasionProgress / (float)Main.invasionProgressMax, 0f, 1f);
                if (Main.invasionProgressMax == 0)
                {
                    num4 = 1f;
                }
                float num5 = 169f * num;
                float num6 = 8f * num;
                Vector2 vector2 = vector + Vector2.UnitY * num6 + Vector2.UnitX * 1f;
                Utils.DrawBorderString(Main.spriteBatch, text2, vector2, Color.White * Main.invasionProgressAlpha, num, 0.5f, 1f, -1);
                Main.spriteBatch.Draw(value2, vector, null, Color.White * Main.invasionProgressAlpha, 0f, new Vector2((float)(value2.Width / 2), 0f), num, SpriteEffects.None, 0f);
                vector2 += Vector2.UnitX * (num4 - 0.5f) * num5;
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 241, 51) * Main.invasionProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(num5 * num4, num6), SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 165, 0, 127) * Main.invasionProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num6), SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * Main.invasionProgressAlpha, 0f, new Vector2(0f, 0.5f), new Vector2(num5 * (1f - num4), num6), SpriteEffects.None, 0f);
            }
            else
            {
                int num7 = (int)(200f * num);
                int num8 = (int)(45f * num);
                Vector2 vector3 = new Vector2((float)(Main.screenWidth - 120), (float)(Main.screenHeight - 40));
                Rectangle r2 = new Rectangle((int)vector3.X - num7 / 2, (int)vector3.Y - num8 / 2, num7, num8);
                Utils.DrawInvBG(Main.spriteBatch, r2, new Color(63, 65, 151, 255) * 0.785f);
                string text3;
                if (Main.invasionProgressMax == 0)
                {
                    text3 = Main.invasionProgress.ToString();
                }
                else
                {
                    text3 = (int)((float)Main.invasionProgress * 100f / (float)Main.invasionProgressMax) + "%";
                }
                text3 = Language.GetTextValue("Game.WaveCleared", text3);
                Texture2D value4 = TextureAssets.ColorBar.Value;
                Texture2D value5 = TextureAssets.ColorBlip.Value;
                if (Main.invasionProgressMax != 0)
                {
                    Main.spriteBatch.Draw(value4, vector3, null, Color.White * Main.invasionProgressAlpha, 0f, new Vector2((float)(value4.Width / 2), 0f), num, SpriteEffects.None, 0f);
                    float num9 = MathHelper.Clamp((float)Main.invasionProgress / (float)Main.invasionProgressMax, 0f, 1f);
                    Vector2 vector4 = FontAssets.MouseText.Value.MeasureString(text3);
                    float num10 = num;
                    if (vector4.Y > 22f)
                    {
                        num10 *= 22f / vector4.Y;
                    }
                    float num11 = 169f * num;
                    float num12 = 8f * num;
                    Vector2 vector5 = vector3 + Vector2.UnitY * num12 + Vector2.UnitX * 1f;
                    Utils.DrawBorderString(Main.spriteBatch, text3, vector5 + new Vector2(0f, -4f), Color.White * Main.invasionProgressAlpha, num10, 0.5f, 1f, -1);
                    vector5 += Vector2.UnitX * (num9 - 0.5f) * num11;
                    Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 241, 51) * Main.invasionProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(num11 * num9, num12), SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 165, 0, 127) * Main.invasionProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num12), SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * Main.invasionProgressAlpha, 0f, new Vector2(0f, 0.5f), new Vector2(num11 * (1f - num9), num12), SpriteEffects.None, 0f);
                }
            }
            Vector2 vector6 = FontAssets.MouseText.Value.MeasureString(text);
            float num13 = 120f;
            if (vector6.X > 200f)
            {
                num13 += vector6.X - 200f;
            }
            Rectangle r3 = Utils.CenteredRectangle(new Vector2((float)Main.screenWidth - num13, (float)(Main.screenHeight - 80)), (vector6 + new Vector2((float)(value.Width + 12), 6f)) * num);
            Utils.DrawInvBG(Main.spriteBatch, r3, c);
            Main.spriteBatch.Draw(value, r3.Left() + Vector2.UnitX * num * 8f, null, Color.White * Main.invasionProgressAlpha, 0f, new Vector2(0f, (float)(value.Height / 2)), num * 0.8f, SpriteEffects.None, 0f);
            Utils.DrawBorderString(Main.spriteBatch, text, r3.Right() + Vector2.UnitX * num * -22f, Color.White * Main.invasionProgressAlpha, num * 0.9f, 1f, 0.4f, -1);
        }
    }
}
