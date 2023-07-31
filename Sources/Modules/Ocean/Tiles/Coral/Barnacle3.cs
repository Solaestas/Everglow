using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
    public class Barnacle3 : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                36,
            };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateWidth = 36;
            TileObjectData.addTile((int)base.Type);
            DustType = 7;
            RegisterItemDrop(ModContent.ItemType<Items.Barnacle>());
            var modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("Barnacle");
            // modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "藤壶");
            base.AddMapEntry(new Color(178, 178, 138), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Barnacle>(), 2, false, 0, false, false);
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            int num = Main.rand.Next(0, 18) * 2;
            int num2 = Main.rand.Next(0, 18) * 2;
            Main.tile[i, j].TileFrameX = (short)num;
            Main.tile[i, j].TileFrameY = (short)num2;
        }
    }
}
