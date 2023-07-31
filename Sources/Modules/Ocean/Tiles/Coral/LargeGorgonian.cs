using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
    public class LargeGorgonian : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                20
            };
            TileObjectData.newTile.CoordinateWidth = 72;
            TileObjectData.addTile((int)base.Type);
            DustType = 60;
            var modTranslation = base.CreateMapEntryName();
            AddMapEntry(new Color(227, 83, 56), modTranslation);
            // modTranslation.SetDefault("Large Gorgonian");
            // modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "柳珊瑚");
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            for (int x = 0; x < 3; x++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 60f)).RotatedByRandom(3.14159);
                Item.NewItem(null, i * 16 + (int)v.X, j * 16 + (int)v.Y, 16, 32, ModContent.ItemType<Items.GorgonianPiece>());
            }
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 6));
            Main.tile[i, j].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 72);
        }
    }
}
