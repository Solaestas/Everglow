using Terraria.DataStructures;
using Terraria.ObjectData;
namespace Everglow.CagedDomain.Tiles;

public class WhiteLotusBonsai : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileLavaDeath[Type] = true;
		Main.tileFrameImportant[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 12;
		TileObjectData.newTile.Width = 6;
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
			16,
			16,
			16,
			16,
			16
		};
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.Origin = new Point16(3, 10);
		TileObjectData.addTile(Type);
		DustType = 1;
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(90, 90, 90), modTranslation);
		HitSound = SoundID.DD2_SkeletonHurt;
	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
		Main.tile[i, j].TileFrameX += (short)(item.placeStyle * 108);
	}
}
