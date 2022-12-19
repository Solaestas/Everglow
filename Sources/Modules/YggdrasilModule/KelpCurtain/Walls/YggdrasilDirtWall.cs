namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Walls
{
    public class YggdrasilDirtWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.WoodFurniture;
            AddMapEntry(new Color(25, 14, 12));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
