namespace Everglow.Sources.Modules.MythModule.TheTusk.Walls
{
    public class BloodyStoneWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.BloodyStoneWall>();
            AddMapEntry(new Color(61, 13, 11));
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}