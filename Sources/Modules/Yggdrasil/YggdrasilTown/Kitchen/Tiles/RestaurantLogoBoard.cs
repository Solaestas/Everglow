using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;

public class RestaurantLogoBoard : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
		TileObjectData.newTile.Height = 7;
		TileObjectData.newTile.Width = 12;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16,
			16,
			16,
		};
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(153, 132, 123));
		MinPick = 99999999;
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		noBreak = true;
		return false;
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}