using Terraria.Localization;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles
{
    public class CyanVineStone : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlendAll[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileShine2[Type] = false;
            Main.ugBackTransition = 1000;
            DustType = DustID.Silver;
            ItemDrop = ModContent.ItemType<Items.CyanVineOre>();
            Main.tileSpelunker[Type] = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(155, 173, 183), modTranslation);
            modTranslation.SetDefault("Cyan Ore");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "青缎矿");
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            //SoundEngine.PlaySound(SoundID.NPCHit4, i * 16, j * 16);
        }
    }
}
