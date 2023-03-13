namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Walls
{
    public class StoneDragonScaleWoodWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.WoodFurniture;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("");
            ItemDrop = ModContent.ItemType <Items.StoneDragonScaleWoodWall> ();
            AddMapEntry(new Color(24, 0, 0));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
