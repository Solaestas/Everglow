using Everglow.Sources.Commons.Core.ModuleSystem;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Sources.Modules.ZY.InvasionSystem
{
    [ModuleDependency(typeof(InvasionSystem))]
    internal abstract class Invasion : IModule
    {
        public abstract string Name { get; }
        public const int VanillaCount = 5;
        private static int modInvasionCount = 114;
        public bool active = false;
        public int progress;
        public int progressMax;
        public int wave;
        public int InvasionID { get; private set; }
        public virtual Vector2 BarCenter => new Vector2(Main.screenWidth - 120, Main.screenHeight - 40);
        public virtual Asset<Texture2D> ColorBar => TextureAssets.ColorBar;
        public virtual Asset<Texture2D> Icon => TextureAssets.MagicPixel;
        public virtual string Text => "Invasion";
        public void Load()
        {
            InvasionID = modInvasionCount++;
        }

        public void Unload()
        {

        }
        public virtual void Update()
        {
            Main.invasionProgressMax = progressMax;
            Main.invasionProgress = progress;
            Main.invasionProgressMode = InvasionID;
            Main.invasionProgressNearInvasion = true;
            Main.invasionProgressDisplayLeft = 160;
            Main.invasionProgressAlpha = 1;
            Main.invasionProgressWave = wave;
        }
        public virtual void DrawBar()
        {
            float fadinAlpha = 0.5f + Main.invasionProgressAlpha * 0.5f;
            int barSizeX = (int)(200f * fadinAlpha);
            int barSizeY = (int)(45f * fadinAlpha);
            string waveStage = Main.invasionProgressMax == 0 ?
                wave.ToString() :
                (int)(Main.invasionProgress * 100f / Main.invasionProgressMax) + "%";
            float textSizeX = 169f * fadinAlpha;
            float textSizeY = 8f * fadinAlpha;
            Vector2 barCenter = BarCenter;
            if (Main.invasionProgressWave > 0)
            {
                waveStage = Language.GetTextValue("Game.WaveMessage", Main.invasionProgressWave, waveStage);
            }
            else
            {
                waveStage = Language.GetTextValue("Game.WaveCleared", waveStage);
            }

            float waveRate = Main.invasionProgressMax == 0 ? 1 :
                    MathHelper.Clamp(Main.invasionProgress / (float)Main.invasionProgressMax, 0f, 1f);

            Vector2 textPosition = barCenter + Vector2.UnitY * textSizeY + Vector2.UnitX * 1f;
            //进度条背景
            Utils.DrawInvBG(Main.spriteBatch,
                new Rectangle((int)barCenter.X - barSizeX / 2, (int)barCenter.Y - barSizeY / 2, barSizeX, barSizeY),
                new Color(63 * 0.785f, 65 * 0.785f, 151 * 0.785f, 255 * 0.785f));
            //进度条文字
            Utils.DrawBorderString(Main.spriteBatch, waveStage, textPosition, Color.White * Main.invasionProgressAlpha, fadinAlpha, 0.5f, 1f, -1);
            //进度条外框
            Main.spriteBatch.Draw(ColorBar.Value, barCenter, null, Color.White * Main.invasionProgressAlpha, 0f, new Vector2(ColorBar.Value.Width / 2, 0f), fadinAlpha, SpriteEffects.None, 0f);

            textPosition += Vector2.UnitX * (waveRate - 0.5f) * textSizeX;

            //进度条，由两短一长的像素条组成，有点……
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,
                textPosition,
                new Rectangle(0, 0, 1, 1),
                new Color(255, 241, 51) * Main.invasionProgressAlpha,
                0f,
                new Vector2(1f, 0.5f),
                new Vector2(textSizeX * waveRate, textSizeY),
                SpriteEffects.None,
                0f);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,
                textPosition,
                new Rectangle(0, 0, 1, 1),
                new Color(255, 165, 0, 127) * Main.invasionProgressAlpha,
                0f,
                new Vector2(1f, 0.5f),
                new Vector2(2f, textSizeY),
                SpriteEffects.None,
                0f);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,
                textPosition,
                new Rectangle(0, 0, 1, 1),
                Color.Black * Main.invasionProgressAlpha,
                0f,
                new Vector2(0f, 0.5f),
                new Vector2(textSizeX * (1f - waveRate), textSizeY),
                SpriteEffects.None,
                0f);
        }
        public virtual void DrawIcon()
        {
            float fadinAlpha = 0.5f + Main.invasionProgressAlpha * 0.5f;
            Color BGColor = Color.White;
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(Text);
            float OffsetX = 120f;
            if (textSize.X > 200f)
            {
                OffsetX += textSize.X - 200f;
            }
            Rectangle BGRectangle = Utils.CenteredRectangle(
                new Vector2(Main.screenWidth - OffsetX, Main.screenHeight - 80),
                (textSize + new Vector2(Icon.Value.Width + 12, 6f)) * fadinAlpha);
            Utils.DrawInvBG(Main.spriteBatch,
                BGRectangle,
                BGColor);
            Main.spriteBatch.Draw(Icon.Value,
                BGRectangle.Left() + Vector2.UnitX * fadinAlpha * 8f,
                null,
                Color.White * Main.invasionProgressAlpha,
                0f,
                new Vector2(0f, Icon.Value.Height / 2),
                fadinAlpha * 0.8f,
                SpriteEffects.None,
                0f);
            Utils.DrawBorderString(Main.spriteBatch,
                Text,
                BGRectangle.Right() + Vector2.UnitX * fadinAlpha * -22f,
                Color.White * Main.invasionProgressAlpha,
                fadinAlpha * 0.9f,
                1f,
                0.4f,
                -1);
        }
        public virtual void Draw()
        {
            DrawBar();
            DrawIcon();
        }
        public virtual void Begin()
        {

        }
        public virtual void End()
        {

        }
    }
}
