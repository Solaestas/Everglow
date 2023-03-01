namespace Everglow.Sources.Modules.MythModule.TheFirefly.Walls
{
    public class DarkCocoonWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.DarkCocoonWall>();
            AddMapEntry(new Color(10, 10, 10));
        }
    }
}