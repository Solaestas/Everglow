using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
    public class LargeBlueStarfish : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile((int)base.Type);
            DustType = 7;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("Large Blue Starfish");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "大蓝色海星");
            base.AddMapEntry(new Color(43, 36, 136), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 48, 48, ModContent.ItemType<Items.LargeBlueStarfish>(), 1, false, 0, false, false);
        }
    }
}
