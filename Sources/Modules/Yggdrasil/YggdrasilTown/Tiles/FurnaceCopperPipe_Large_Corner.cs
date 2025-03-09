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
		Main.tileBlendAll[Type] = false;
		Main.tileNoAttach[Type] = true;

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
		int style = -1;

		var upTile = Main.tile[center + new Point(0, -2)];
		var downTile = Main.tile[center + new Point(0, 2)];
		var leftTile = Main.tile[center + new Point(-2, 0)];
		var rightTile = Main.tile[center + new Point(2, 0)];

		if (ConnectRight(rightTile) == 18 && ConnectDown(downTile) == 18)
		{
			style = 0;
			var rightOrigin = center + new Point(2, -1);
			var downOrigin = center + new Point(-1, 2);
			SetFrameStyleH(rightOrigin, false);
			SetFrameStyleV(downOrigin, false);

		}
		if (ConnectLeft(leftTile) == 18 && ConnectDown(downTile) == 0)
		{
			style = 1;
			var leftOrigin = center + new Point(-4, -1);
			var downOrigin = center + new Point(0, 2);
			SetFrameStyleH(leftOrigin, true);
			SetFrameStyleV(downOrigin, false);
		}
		if (ConnectRight(rightTile) == 0 && ConnectUp(upTile) == 18)
		{
			style = 2;
			var rightOrigin = center + new Point(2, 0);
			var upOrigin = center + new Point(-1, -4);
			SetFrameStyleH(rightOrigin, false);
			SetFrameStyleV(upOrigin, true);
		}
		if (ConnectLeft(leftTile) == 0 && ConnectUp(upTile) == 0)
		{
			style = 3;
			var leftOrigin = center + new Point(-4, 0);
			var upOrigin = center + new Point(0, -4);
			SetFrameStyleH(leftOrigin, true);
			SetFrameStyleV(upOrigin, true);
		}
		if (style != -1)
		{
			SetFrame(style, center - new Point(1, 1));
		}
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
				framedTile.TileFrameX = (short)(style * 54 + x * 18);
				framedTile.TileFrameY = (short)(y * 18);
			}
		}
	}

	public void SetFrameStyleH(Point topLeft, bool leftToCorner = false)
	{
		var tile = Main.tile[topLeft];
		int style;
		if(leftToCorner)
		{
			int origStyle = tile.TileFrameX / 54;
			style = 1;
			if (origStyle == 3)
			{
				style = 2;
			}
		}
		else
		{
			int origStyle = tile.TileFrameX / 54;
			style = 1;
			if (origStyle == 3)
			{
				style = 0;
			}
		}
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 2; y++)
			{
				var framedTile = Main.tile[topLeft + new Point(x, y)];
				framedTile.TileFrameX = (short)(style * 54 + x * 18);
				framedTile.TileFrameY = (short)(y * 18);
			}
		}
	}

	public void SetFrameStyleV(Point topLeft, bool upToCorner = false)
	{
		var tile = Main.tile[topLeft];
		int style;
		if (upToCorner)
		{
			int origStyle = tile.TileFrameX / 54;
			style = 1;
			if (origStyle == 3)
			{
				style = 0;
			}
		}
		else
		{
			int origStyle = tile.TileFrameX / 54;
			style = 1;
			if (origStyle == 3)
			{
				style = 2;
			}
		}
		for (int x = 0; x < 2; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				var framedTile = Main.tile[topLeft + new Point(x, y)];
				framedTile.TileFrameX = (short)(style * 36 + x * 18);
				framedTile.TileFrameY = (short)(y * 18);
			}
		}
	}

	public override void PlaceInWorld(int i, int j, Item item) => base.PlaceInWorld(i, j, item);
}