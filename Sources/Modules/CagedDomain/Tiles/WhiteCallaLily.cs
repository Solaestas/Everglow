using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Everglow.CagedDomain.Tiles;

public class WhiteCallaLily : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileLavaDeath[Type] = true;
		Main.tileFrameImportant[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 8;
		TileObjectData.newTile.Width = 8;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			18,
		};
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.Origin = new Point16(4, 7);
		TileObjectData.addTile(Type);
		DustType = DustID.Grass;
		AddMapEntry(new Color(24, 61, 58));
		HitSound = SoundID.DD2_SkeletonHurt;
	}

	public override void PlaceInWorld(int i, int j, Item item)
	{
		base.PlaceInWorld(i, j, item);
	}
}