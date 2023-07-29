using Terraria.Localization;

namespace Everglow.Ocean.Tiles
{
    public class DarkSeaOre : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[(int)base.Type] = true;
            Main.tileMergeDirt[(int)base.Type] = false;
            Main.tileBlendAll[(int)base.Type] = false;
            Main.tileBlockLight[(int)base.Type] = true;
            Main.tileShine2[(int)base.Type] = false;
            Main.ugBackTransition = 1000;
            DustType = DustID.BorealWood;
            MinPick = 240;
            SoundType = SoundID.Grass;
            SoundStyle = 2;
            ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Ores.DarkSeaOre>();
            Main.tileSpelunker[(int)base.Type] = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(3, 0, 105), modTranslation);
            modTranslation.SetDefault("Dark Sea Ore");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "深焚石");
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
