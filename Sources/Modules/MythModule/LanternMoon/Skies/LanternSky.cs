using Terraria.Graphics.Effects;
using Everglow.Sources.Modules.MythModule.Common;

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
            StarPos = Vector2.Zero;
            OldStar = new Vector2[240];
            HitTimer = 0;
            MoonLight = 0;
            this.skyActive = true;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 3E+38f && minDepth < 3E+38f)
            {
                Texture2D LightE = Common.MythContent.QuickTexture("VisualTextures/LightEffect");
                Main.spriteBatch.Draw(LightE, StarPos, null, new Color(0.3f, 0.21f, 0, 0), -(float)(Math.Sin(Main.time / 26d)) + 0.6f, new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d))) * 0.05f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(LightE, StarPos, null, new Color(1f, 0.7f, 0, 0), (float)(Math.Sin(Main.time / 12d + 2)) + 1.6f, new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d))) * 0.05f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(LightE, StarPos, null, new Color(0.3f, 0.21f, 0, 0), (float)Math.PI / 2f + (float)(Main.time / 9d), new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d + 1.57))) * 0.05f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(LightE, StarPos, null, new Color(1f, 0.7f, 0, 0), (float)(Main.time / 26d), new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d + 3.14))) * 0.05f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(LightE, StarPos, null, new Color(1f, 0.7f, 0, 0), -(float)(Main.time / 26d), new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d + 4.71))) * 0.05f, SpriteEffects.None, 0);
                //spriteBatch.Draw(Common.MythContent.QuickTexture("LanternMoon/Skies/LanternSky"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(255, 255, 255, 255));
            }
            Texture2D LMoon = Common.MythContent.QuickTexture("LanternMoon/Skies/LanternMoon");
            float HalfMaxTime = Main.dayTime ? 27000 : 16200;
            float rotation = (float)(Main.time / HalfMaxTime) - 7.3f;
            if((StarPos - Common.MythContent.GetSunPos()).Length() < 30)
            {
                HitTimer++;
                MoonLight = Math.Clamp((HitTimer * HitTimer) / 500f,0,1);
                spriteBatch.Draw(LMoon, Common.MythContent.GetSunPos(), new Rectangle(0, Main.moonPhase * 25, 50, 50), new Color(MoonLight, MoonLight, MoonLight, MoonLight), rotation, new Vector2(25), Main.ForcedMinimumZoom, SpriteEffects.None, 0);
            }
            if(StarPos == Vector2.Zero)
            {
                StarPos = new Vector2(Main.screenWidth, Main.screenHeight * 2) - Common.MythContent.GetSunPos();
                StarVel =  Vector2.Normalize(Common.MythContent.GetSunPos() - StarPos).RotatedBy(0.6);
            }
            Vector2 StarAcc = Vector2.Normalize((Common.MythContent.GetSunPos() - StarVel * 4f) - StarPos);
            StarVel = StarVel * 0.99f + StarAcc * 0.0095f;
            StarPos += StarVel;
            List<VertexBase.Vertex2D> bars = new List<VertexBase.Vertex2D>();
            float width = 6;
            if (TimeLeft < 60)
            {
                width = TimeLeft / 10f;
            }
            OldStar[0] = StarPos;
            for (int x = OldStar.Length - 1; x > 0;x--)
            {
                OldStar[x] = OldStar[x - 1];
            }
            int TrueL = 0;
            for (int i = 1; i < OldStar.Length; ++i)
            {
                TrueL++;
                if (OldStar[i] == Vector2.Zero)
                    break;
            }
            for (int i = 1; i < OldStar.Length; ++i)
            {
                if (OldStar[i] == Vector2.Zero)
                    break;
                var normalDir = OldStar[i - 1] - OldStar[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new VertexBase.Vertex2D(OldStar[i] + normalDir * width, new Color(255, 0, 0, 0), new Vector3(factor, 1, w)));
                bars.Add(new VertexBase.Vertex2D(OldStar[i] + normalDir * -width, new Color(255, 0, 0, 0), new Vector3(factor, 0, w)));
            }
            List<VertexBase.Vertex2D> Vx = new List<VertexBase.Vertex2D>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new VertexBase.Vertex2D((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(StarVel) * 30, new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
                Vx.Add(bars[1]);
                Vx.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    Vx.Add(bars[i]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 1]);

                    Vx.Add(bars[i + 1]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 3]);
                }

            }
            if (Vx.Count > 2)
            {
                Texture2D t = Common.MythContent.QuickTexture("LanternMoon/Projectiles/LBloodEffect");
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
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
        private Vector2 StarPos = Vector2.Zero;

        private Vector2 StarVel;

        private Vector2[] OldStar = new Vector2[240];

        private bool skyActive;

        private float opacity;

        private float MoonLight;

        private int HitTimer;

        public int TimeLeft = 600;
    }
}
