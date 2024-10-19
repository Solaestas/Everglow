using Everglow.Yggdrasil.Common.Blocks;
using Terraria.DataStructures;
using Terraria.ObjectData;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.WorldGeneration;

public class MainWorldGeneratioin_Yggdrasil
{
	public static void BuildYggdrasilPylonRelic()
	{
		Point16 sbpp = YggdrasilPylonPos();
		string Path = ModAsset.PylonToYggdrasil_16x16_Path;
		var mapIO = new Commons.TileHelper.MapIO(sbpp.X, sbpp.Y);
		int Height = mapIO.ReadHeight(ModIns.Mod.GetFileStream(Path));
		QuickBuild(sbpp.X, sbpp.Y - Height / 2, Path);

		var pylonBottom = new Point(sbpp.X + WorldGen.genRand.Next(8, 16), sbpp.Y - Height / 2 + 8);
		ushort PylonType = (ushort)ModContent.TileType<YggdrasilWorldPylon>();
		for (int a = 0; a < 12; a++)
		{
			pylonBottom.Y++;
			if (SafeGetTile(pylonBottom.X, pylonBottom.Y].HasTile)
			{
				pylonBottom.Y -= 1;
				break;
			}
		}
		for (int i = -2; i <= 2; i++)
		{
			var PylonTile = SafeGetTile(pylonBottom.X + i, pylonBottom.Y + 1);
			PylonTile.TileType = TileID.GrayBrick;
			PylonTile.HasTile = true;
			PylonTile.Slope = SlopeType.Solid;
			PylonTile.IsHalfBlock = false;
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
		int PoX = (int)(WorldGen.genRand.Next(80, 160) * (WorldGen.genRand.Next(2) - 0.5f) * 2 - 20 + Main.maxTilesX / 2);
		int PoY = 160;

		while (!IsTileSmooth(new Point(PoX, PoY)))
		{
			PoX = (int)(WorldGen.genRand.Next(80, 240) * (WorldGen.genRand.Next(2) - 0.5f) * 2 - 20 + Main.maxTilesX / 2);
			for (int y = 160; y < Main.maxTilesY / 3; y++)
			{
				if (SafeGetTile(PoX, y).HasTile && SafeGetTile(PoX, y).TileType != TileID.Trees)
				{
					PoY = y;
					break;
				}
			}
		}
		return new Point16(PoX, PoY);
	}

	/// <summary>
	/// Is the tile smooth in a horizental width range.
	/// </summary>
	/// <param name="point"></param>
	/// <param name="Width"></param>
	/// <returns></returns>
	public static bool IsTileSmooth(Point point, int Width = 16)
	{
		if (point.X > Main.maxTilesX - 20 || point.Y > Main.maxTilesY - 20 || point.X < 20 || point.Y < 20)
		{
			return false;
		}

		int x = point.X;
		int y = point.Y;
		var LeftTile = SafeGetTile(x, y);
		var RightTile = SafeGetTile(x + Width, y);
		var LeftTileUp = SafeGetTile(x, y - 1);
		var RightTileUp = SafeGetTile(x + Width, y - 1);
		if (!LeftTileUp.HasTile && !RightTileUp.HasTile)
		{
			if (LeftTile.HasTile && RightTile.HasTile)
			{
				if(LeftTile.TileType == TileID.Grass && RightTile.TileType == TileID.Grass)
				{
					return true;
				}
			}
		}
		return false;
	}
}