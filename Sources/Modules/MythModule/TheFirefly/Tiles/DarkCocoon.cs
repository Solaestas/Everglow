namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class DarkCocoon : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            MinPick = 175;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.DarkCocoon>();
            AddMapEntry(new Color(17, 16, 17));
        }
    }
}