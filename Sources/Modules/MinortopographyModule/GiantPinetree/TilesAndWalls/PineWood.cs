namespace Everglow.Sources.Modules.MinortopographyModule.GiantPinetree.TilesAndWalls
{
    public class PineWood : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][TileID.PineTree] = true;
            Main.tileMerge[TileID.PineTree][Type] = true;
            DustType = DustID.Ebonwood;
            HitSound = SoundID.Dig;           
            ItemDrop = ItemID.PineTreeBlock;
            AddMapEntry(new Color(45, 15, 3));
        }
    }
}
