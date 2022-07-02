namespace Everglow.Sources.Modules.MythModule.TheFirefly
{
    internal class TileSpin
    {
        public static Dictionary<(int, int), Vector2> TileRotation = new Dictionary<(int, int), Vector2>();
        public void Update(int i, int j)
        {
            if (TileRotation.ContainsKey((i, j)) && !Main.gamePaused)
            {
                float rot;
                float Omega;
                Omega = TileRotation[(i, j)].X;
                rot = TileRotation[(i, j)].Y;
                Omega = Omega * 0.75f - rot * 0.13f;
                TileRotation[(i, j)] = new Vector2(Omega, rot + Omega);
                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                {
                    TileRotation.Remove((i, j));
                }
            }
        }
        public void DrawRotatedTile(int i, int j, Texture2D tex, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
        {
            float rot = 0;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            var tile = Main.tile[i, j];
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Color c = Lighting.GetColor(i, j);
            if(specialColor)
            {
                c = color;
            }
            if (TileRotation.ContainsKey((i, j)))
            {
                rot = TileRotation[(i, j)].Y;
                Main.spriteBatch.Draw(tex, new Vector2(i * 16  + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX - 18, 0, 54, 48), c, rot, new Vector2(27, 0), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX - 18, 0, 54, 48), c, rot, new Vector2(27, 0), 1f, SpriteEffects.None, 0f);
            }
        }

        public void DrawRotatedTile(int i, int j, Texture2D tex, Rectangle sourceRectangle, Vector2 origin, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
        {
            float rot = 0;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Color c = Lighting.GetColor(i, j);
            if (specialColor)
            {
                c = color;
            }
            if (TileRotation.ContainsKey((i, j)))
            {
                rot = TileRotation[(i, j)].Y;
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot, origin, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot, origin, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
