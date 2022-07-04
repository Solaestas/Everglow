using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class FireflyWood : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            MinPick = 175;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.GlowWood>();
            AddMapEntry(new Color(37, 46, 47));
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0)
            {
                Point point = new Point(i, j);
                if (tile != null && tile.HasTile)
                {
                    Texture2D valueG = MythContent.QuickTexture("TheFirefly/Tiles/FireflyTreeGlow");
                    Vector2 value2 = point.ToWorldCoordinates();
                    SpriteEffects effects = SpriteEffects.None;
                    Vector2 HalfSize = new Vector2(8);
                    Main.spriteBatch.Draw(valueG, value2 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 1f, 1f, 0), 0f, HalfSize, 1f, effects, 0f);
                }
            }
            return true;
        }
    }
}