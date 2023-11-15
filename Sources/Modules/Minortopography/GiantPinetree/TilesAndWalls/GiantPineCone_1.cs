using Terraria.Audio;
using Terraria.ObjectData;

namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class GiantPineCone_1 : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 4;
		TileObjectData.newTile.Width = 5;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16
		};
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.addTile(Type);
		HitSound = new SoundStyle("Everglow/Minortopography/GiantPinetree/Sounds/PineconeCollapse");
		AddMapEntry(new Color(119, 77, 63));
	}
	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield return new Item(ModContent.ItemType<Items.PineNut>(), Main.rand.Next(27, 54));
	}
}