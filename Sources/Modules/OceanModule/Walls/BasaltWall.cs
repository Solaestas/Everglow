namespace Everglow.Sources.Modules.OceanModule.Walls
{
    public class BasaltWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = 240;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("");
            AddMapEntry(new Color(1, 1, 1));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
