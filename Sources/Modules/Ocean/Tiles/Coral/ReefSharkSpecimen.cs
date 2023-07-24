using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace MythMod.OceanMod.Tiles
{
    public class ReefSharkSpecimen : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            TileObjectData.newTile.Height = 7;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 198;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData((Terraria.Enums.AnchorType)1, 1, 1);
            TileObjectData.addTile((int)base.Type);
            DustType = 7;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("ReefSharkSpecimen");
            base.AddMapEntry(new Color(99, 71, 50), modTranslation);
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "礁鲨标本");
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 48, 48, ModContent.ItemType<OceanMod.Items.ReefSharkSpecimen>(), 1, false, 0, false, false);
        }
    }
}
