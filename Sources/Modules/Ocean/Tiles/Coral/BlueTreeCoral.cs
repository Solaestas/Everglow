using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
    public class BlueTreeCoral : ModTile
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
                18
            };
            TileObjectData.newTile.CoordinateWidth = 36;
            TileObjectData.addTile((int)base.Type);
            DustType = 59;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(28, 59, 204), modTranslation);
            modTranslation.SetDefault("BlueTreeCoral");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "蓝海树珊瑚");
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
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.Alcyonarian>());
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 6));
            Main.tile[i, j].TileFrameX = (short)(num * 36);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 36);
            Main.tile[i, j + 2].TileFrameX = (short)(num * 36);
        }
    }
}