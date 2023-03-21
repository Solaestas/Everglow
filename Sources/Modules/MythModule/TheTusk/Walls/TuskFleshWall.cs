namespace Everglow.Sources.Modules.MythModule.TheTusk.Walls
{
    public class TuskFleshWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Blood;
            AddMapEntry(new Color(25, 0, 4));
        }
    }
}