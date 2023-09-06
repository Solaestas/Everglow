//using System.Drawing;
using Terraria.UI;

namespace Everglow.Ocean.UIs
{
    public class OceanWorld : UIState
    {
        private bool living = true;
        public static bool Open = true;
        private OceanWorldMain OceanWorldMain = new OceanWorldMain();
        public override void OnInitialize()
        {
            OceanWorldMain = new OceanWorldMain();
            OceanWorldMain.Width.Set(422, 0);
            OceanWorldMain.Height.Set(424, 0);
            OceanWorldMain.Left.Set(Main.screenWidth * 0.5f - 315, 0);
            OceanWorldMain.Top.Set(Main.screenHeight * 0.5f - 318, 0);

            OceanWorldMain.Activate();
            OceanWorldMain.Append(OceanWorldMain);
            OceanWorldMain.Draw(Main.spriteBatch);

            Append(OceanWorldMain);

        }
        Vector2 offset;
        public override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            float shopx2 = innerDimensions.X;
            float shopy2 = innerDimensions.Y;
            float shopx = innerDimensions.X;
            float shopy = innerDimensions.Y;
        }
    }
    public class OceanWorldMain : UIElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            Player player = Main.player[Main.myPlayer];
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            float shopx = innerDimensions.X;
            float shopy = innerDimensions.Y;
            spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Ocean/UIImages/heatmapBlue").Value, new Vector2(shopx, shopy), null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }
        public override void OnActivate()
        {
            base.OnActivate();
        }
    }
}
