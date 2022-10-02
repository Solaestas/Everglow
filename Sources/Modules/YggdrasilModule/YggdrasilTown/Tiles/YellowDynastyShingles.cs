using Terraria.Localization;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles
{
    public class YellowDynastyShingles : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlendAll[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileShine2[Type] = false;
            Main.ugBackTransition = 1000;
            DustType = 9;
            MinPick = 0;
            HitSound = SoundID.Grass;
            
            ItemDrop = ModContent.ItemType<Items.YellowDynastyShingles>();
            Main.tileSpelunker[Type] = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(229, 128, 4), modTranslation);
            modTranslation.SetDefault("Yellow Dynasty Shingles");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "黄王朝瓦");
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
