using Terraria.Localization;


namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Tiles
{
    public class DragonScaleWood : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlendAll[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileShine2[Type] = false;
            Main.tileMerge[Type][ModContent.TileType<OldMoss>()] = true;
            Main.tileMerge[Type][ModContent.TileType<YggdrasilDirt>()] = true;
            //Main.tileFrameImportant[(int)base.Type] = false;
            Main.ugBackTransition = 1000;
            DustType = DustID.BorealWood;
            MinPick = 300;
            HitSound = SoundID.Grass;
            
            ItemDrop = ModContent.ItemType<Items.DragonScaleWood>();
            //Main.tileSpelunker[(int)base.Type] = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(86, 62, 44), modTranslation);
            modTranslation.SetDefault("");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

    }
}
