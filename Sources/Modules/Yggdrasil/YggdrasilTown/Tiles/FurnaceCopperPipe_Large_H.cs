using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class FurnaceCopperPipe_Large_H : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileMergeDirt[Type] = false;
		Main.tileNoAttach[Type] = true;
		Main.tileBlendAll[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.Height = 2;
		TileObjectData.newTile.Width = 3;

		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
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
		Point topLeft = new Point(i - (tile.TileFrameX % 54) / 18, j - tile.TileFrameY / 18);
		int style = 3;

		var leftTile = Main.tile[topLeft + new Point(-1, 0)];
		var rightTile = Main.tile[topLeft + new Point(3, 0)];
		if (ConnectLeft(leftTile))
		{
			if (ConnectLeft(rightTile))
			{
				style = 1;
			}
			else
			{
				style = 2;
			}
			int leftStyle = (leftTile.TileFrameX - 36) / 54;
			if (leftStyle == 3)
			{
				SetFrame(0, topLeft - new Point(3, 0));
			}
			if (leftStyle == 2)
			{
				SetFrame(1, topLeft - new Point(3, 0));
			}
		}
		else if (ConnectLeft(rightTile))
		{
			style = 0;
		}

		if (ConnectLeft(rightTile))
		{
			int rightStyle = rightTile.TileFrameX / 54;
			if (rightStyle == 3)
			{
				SetFrame(2, topLeft + new Point(3, 0));
			}
			if (rightStyle == 0)
			{
				SetFrame(1, topLeft + new Point(3, 0));
			}
		}
		SetFrame(style, topLeft);
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}

	public bool ConnectLeft(Tile tile)
	{
		return tile.TileType == Type && tile.TileFrameX % 54 == 36 && tile.TileFrameY == 0;
	}

	public bool ConnectRight(Tile tile)
	{
		return tile.TileType == Type && tile.TileFrameX % 54 == 0 && tile.TileFrameY == 0;
	}

	public void SetFrame(int style, Point topLeft)
	{
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

	public override void PlaceInWorld(int i, int j, Item item) => base.PlaceInWorld(i, j, item);
}