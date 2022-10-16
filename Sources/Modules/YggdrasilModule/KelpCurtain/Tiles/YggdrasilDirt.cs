using Terraria.Localization;

namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Tiles
{
    public class YggdrasilDirt : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlendAll[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<DragonScaleWood>()] = true;
            Main.tileMerge[Type][ModContent.TileType<OldMoss>()] = true;
            Main.ugBackTransition = 1000;
            DustType = DustID.Dirt;
            MinPick = 50;
            HitSound = SoundID.Dig;
            ItemDrop = ModContent.ItemType<Items.YggdrasilDirt>();
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(53, 29, 26), modTranslation);
            modTranslation.SetDefault("");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        public override bool CanExplode(int i, int j)
        {
            return true;
        }
    }
}
