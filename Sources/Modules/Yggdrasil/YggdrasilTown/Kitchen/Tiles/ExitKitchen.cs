using Everglow.SubSpace;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;

public class ExitKitchen : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
		TileObjectData.newTile.Height = 5;
		TileObjectData.newTile.Width = 2;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16,
		};
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(0, 0, 0));
		MinPick = 99999999;
	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}

	public override bool RightClick(int i, int j)
	{
		RoomManager.ExitALevelOfRoom();
		return base.RightClick(i, j);
	}

	public override void MouseOver(int i, int j)
	{
		Main.instance.MouseText("Exit Canteen.");
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