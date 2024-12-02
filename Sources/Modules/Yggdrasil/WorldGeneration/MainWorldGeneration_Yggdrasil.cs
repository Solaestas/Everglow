using Everglow.Yggdrasil.Common.Blocks;
using Terraria.DataStructures;
using Terraria.ObjectData;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.WorldGeneration;

public class MainWorldGeneratioin_Yggdrasil
{
	public static void BuildYggdrasilPylonRelic()
	{
		Point16 yggdrasilPylonPoint = YggdrasilPylonPos();
		string mapIOPath = ModAsset.PylonToYggdrasil_16x16_Path;
		var mapIO = new Commons.TileHelper.MapIO(yggdrasilPylonPoint.X, yggdrasilPylonPoint.Y);
		int mapIOHeight = mapIO.ReadHeight(ModIns.Mod.GetFileStream(mapIOPath));
		QuickBuild(yggdrasilPylonPoint.X, yggdrasilPylonPoint.Y - mapIOHeight / 2 - 5, mapIOPath);

		var pylonBottom = new Point(yggdrasilPylonPoint.X + WorldGen.genRand.Next(8, 16), yggdrasilPylonPoint.Y - mapIOHeight / 2 + 3);
		ushort PylonType = (ushort)ModContent.TileType<YggdrasilWorldPylon>();
		for (int a = 0; a < 12; a++)
		{
			pylonBottom.Y++;
			if (SafeGetTile(pylonBottom.X, pylonBottom.Y).HasTile)
			{
				pylonBottom.Y -= 1;
				break;
			}
		}

		TileObject.CanPlace(pylonBottom.X, pylonBottom.Y, PylonType, 0, 0, out var tileObject);
		TileObject.Place(tileObject);
		TileObjectData.CallPostPlacementPlayerHook(pylonBottom.X, pylonBottom.Y, PylonType, 0, 0, 0, tileObject);
	}

	/// <summary>
	/// Get a flat area near world center(the origin spawn point).
	/// </summary>
	/// <returns></returns>
	public static Point16 YggdrasilPylonPos()
	{
		int pointX = (int)(WorldGen.genRand.Next(80, 160) * (WorldGen.genRand.Next(2) - 0.5f) * 2 - 20 + Main.maxTilesX / 2);
		int pointY = 160;

		while (!IsTileSmooth(new Point(pointX, pointY)) || !AreaNoChest(pointX, pointX + 16, pointY - 13, pointY + 3))
		{
			pointX = (int)(WorldGen.genRand.Next(80, 240) * (WorldGen.genRand.Next(2) - 0.5f) * 2 - 20 + Main.maxTilesX / 2);
			for (int y = 160; y < Main.maxTilesY / 3; y++)
			{
				if (SafeGetTile(pointX, y).HasTile && SafeGetTile(pointX, y).TileType != TileID.Trees)
				{
					if (SafeGetTile(pointX, y - 1).HasTile)
					{
						y -= 2;
					}
					pointY = y;
					break;
				}
			}
		}
		return new Point16(pointX, pointY);
	}

	public static bool AreaNoChest(int x0, int x1, int y0, int y1)
	{
		for (int i = x0; i < x1; i++)
		{
			for (int j = y0; j < y1; j++)
			{
				Tile tile = SafeGetTile(i, j);
				if (tile.TileType == 21 || tile.TileType == 467 || TileID.Sets.BasicChest[tile.TileType])
				{
					return false;
				}
			}
		}
		return true;
	}

	/// <summary>
	/// Is the tile smooth in a horizental width range.
	/// </summary>
	/// <param name="point"></param>
	/// <param name="width"></param>
	/// <returns></returns>
	public static bool IsTileSmooth(Point point, int width = 16)
	{
		if (point.X > Main.maxTilesX - 20 || point.Y > Main.maxTilesY - 20 || point.X < 20 || point.Y < 20)
		{
			return false;
		}

		int x = point.X;
		int y = point.Y;
		var leftTile = SafeGetTile(x, y);
		var rightTile = SafeGetTile(x + width, y);
		var leftTileUp = SafeGetTile(x, y - 1);
		var rightTileUp = SafeGetTile(x + width, y - 1);
		if ((!leftTileUp.HasTile || leftTileUp.TileType == 3) && (!rightTileUp.HasTile || rightTileUp.TileType == 3))
		{
			if (leftTile.HasTile && rightTile.HasTile)
			{
				if ((leftTile.TileType == TileID.Grass || leftTile.TileType == TileID.Dirt) && (rightTile.TileType == TileID.Grass || rightTile.TileType == TileID.Dirt))
				{
					return true;
				}
			}
		}
		return false;
	}
}