using Terraria.Localization;
using Everglow.Sources.Modules.YggdrasilModule.Common;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles.CyanVine
{
    public class CyanVineStone : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = ModContent.DustType<Dusts.CyanVine>();
            MineResist = 4f;
            ItemDrop = ModContent.ItemType<Items.CyanVineOre>();
            Main.tileSpelunker[Type] = true;
            ModTranslation modTranslation = CreateMapEntryName(null);
            AddMapEntry(new Color(155, 173, 183), modTranslation);
            modTranslation.SetDefault("Cyan Ore");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "青缎矿");
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
