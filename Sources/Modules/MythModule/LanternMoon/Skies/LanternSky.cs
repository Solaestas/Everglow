using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Skies
{
    public class LanternSky : CustomSky
    {
        public static bool Open = false;
        public override void Deactivate(params object[] args)
        {
            this.skyActive = false;
        }

        public override void Reset()
        {
            this.skyActive = false;
        }

        public override bool IsActive()
        {
            return this.skyActive || this.opacity > 0f;
        }
        public override void Activate(Vector2 position, params object[] args)
        {
            TimeLeft = 600;
            this.skyActive = true;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 3E+38f && minDepth < 3E+38f)
            {
                spriteBatch.Draw(Common.MythContent.QuickTexture("LanternMoon/Skies/LanternSky"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(255, 255, 255, 255));
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (this.skyActive && this.opacity < 1f)
            {
                this.opacity += 0.02f;
                return;
            }
            if (!this.skyActive && this.opacity > 0f)
            {
                this.opacity -= 0.02f;
            }
            TimeLeft--;
            if(TimeLeft <= 0)
            {
                Deactivate();
            }
        }
        public override float GetCloudAlpha()
        {
            return (1f - this.opacity) * 0.97f + 0.03f;
        }

        private bool skyActive;

        private float opacity;

        public int TimeLeft = 600;
    }
}
