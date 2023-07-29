using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
    public class GoldGorgonian : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                18
            };
            TileObjectData.newTile.CoordinateWidth = 72;
            TileObjectData.addTile((int)base.Type);
            DustType = 64;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(255, 153, 0), modTranslation);
            modTranslation.SetDefault("GoldGorgonian");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "金柳珊瑚");
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
                Item.NewItem(null, i * 16 + (int)v.X, j * 16 + (int)v.Y, 16, 32, ModContent.ItemType<Items.GoldGorgonianBranch>());
            }
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 4));
            Main.tile[i, j].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 3].TileFrameX = (short)(num * 72);
            Main.tile[i, j + 4].TileFrameX = (short)(num * 72);
        }
    }
}
