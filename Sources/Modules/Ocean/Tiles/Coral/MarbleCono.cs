using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
    public class MarbleCono : ModTile
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
            TileObjectData.newTile.CoordinateWidth = 54;
            TileObjectData.addTile((int)base.Type);
            DustType = 59;
            var modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("Marble Cono");
            // modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "大理石芋螺");
            base.AddMapEntry(new Color(114, 92, 82), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.MarbleCono>());
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 2));
            Main.tile[i, j].TileFrameX = (short)(num * 54);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 54);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 54);
        }
    }
}
