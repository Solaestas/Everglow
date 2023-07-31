using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
    public class BubbleCoral : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                20
            };
            TileObjectData.newTile.CoordinateWidth = 36;
            TileObjectData.addTile((int)base.Type);
            DustType = 51;
            var modTranslation = base.CreateMapEntryName();
            AddMapEntry(new Color(97, 208, 213), modTranslation);
            // modTranslation.SetDefault("Bubble Coral");
            // modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "气泡珊瑚");
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
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.BlueTreeCoral>());
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 6));
            Main.tile[i, j].TileFrameX = (short)(num * 36);
        }
    }
}
