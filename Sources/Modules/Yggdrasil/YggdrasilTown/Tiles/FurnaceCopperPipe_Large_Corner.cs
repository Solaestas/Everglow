using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class FurnaceCopperPipe_Large_Corner : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileMergeDirt[Type] = false;
		Main.tileNoAttach[Type] = true;
		Main.tileBlendAll[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 3;

		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new Point16(1, 1);
		TileObjectData.addTile(Type);
		DustType = DustID.Copper;
		HitSound = default;

		AddMapEntry(new Color(132, 84, 58));
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		var tile = Main.tile[i, j];
		Point center = new Point(i - (tile.TileFrameX % 54) / 18 + 1, j - (tile.TileFrameY % 54) / 18 + 1);
		int style = 0;

		var upTile = Main.tile[center + new Point(0, -3)];
		var downTile = Main.tile[center + new Point(0, 3)];
		var leftTile = Main.tile[center + new Point(-3, 0)];
		var rightTile = Main.tile[center + new Point(0, 3)];

		if(ConnectLeft(leftTile) == 18 && ConnectDown(downTile) == 0)
		{
			style = 1;
		}
		if (ConnectRight(rightTile) == 0 && ConnectUp(upTile) == 18)
		{
			style = 2;
		}
		if (ConnectLeft(leftTile) == 0 && ConnectUp(upTile) == 0)
		{
			style = 3;
		}
		SetFrame(style, center - new Point(1, 1));
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}

	public int ConnectUp(Tile tile)
	{
		if (tile.TileType == ModContent.TileType<FurnaceCopperPipe_Large_V>() && tile.TileFrameY == 36)
		{
			return tile.TileFrameX % 36;
		}
		return -1;
	}

	public int ConnectDown(Tile tile)
	{
		if (tile.TileType == ModContent.TileType<FurnaceCopperPipe_Large_V>() && tile.TileFrameY == 0)
		{
			return tile.TileFrameX % 36;
		}
		return -1;
	}

	public int ConnectLeft(Tile tile)
	{
		if (tile.TileType == ModContent.TileType<FurnaceCopperPipe_Large_H>() && tile.TileFrameX % 54 == 36)
		{
			return tile.TileFrameY;
		}
		return -1;
	}

	public int ConnectRight(Tile tile)
	{
		if (tile.TileType == ModContent.TileType<FurnaceCopperPipe_Large_H>() && tile.TileFrameX % 54 == 0)
		{
			return tile.TileFrameY;
		}
		return -1;
	}

	public void SetFrame(int style, Point topLeft)
	{
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				var framedTile = Main.tile[topLeft + new Point(x, y)];
				framedTile.TileFrameX = (short)(style % 2 * 54 + x * 18);
				framedTile.TileFrameY = (short)((style - (style % 2)) / 2 * 54 + y * 18);
			}
		}
	}

	public override void PlaceInWorld(int i, int j, Item item) => base.PlaceInWorld(i, j, item);
}