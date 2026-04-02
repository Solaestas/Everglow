using Everglow.Commons.CustomTiles;
using Everglow.Commons.Utilities;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class FloorIndicatorTile : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLighted[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18,
		};
		TileObjectData.newTile.CoordinateWidth = 32;

		TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.StyleHorizontal = true;

		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
		TileObjectData.addAlternate(1);
		TileObjectData.addTile(Type);
		SetCustomDefaults();
	}

	public virtual void SetCustomDefaults()
	{
		DustType = DustID.Iron;
		AddMapEntry(new Color(191, 142, 111));
	}

	/// <summary>
	/// Only at this frameY will the tile check for nearby elevators and change its state.
	/// </summary>
	public int IdenticalFrameY = 36;

	public override void NearbyEffects(int i, int j, bool closer)
	{
		CheckChangeState(i, j);
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameY is >= 54 and <= 72)
		{
			Lighting.AddLight(new Vector2(i * 16, j * 16), new Vector3(0.8f, 0.8f, 0.8f));
		}
	}

	public virtual void CheckChangeState(int i, int j)
	{
		var tile = Main.tile[i, j];
		bool hasElevator = ElevatorNearBy(i, j);
		int height = GetMultiTileHeight(i, j);
		int frameCoordY = tile.TileFrameY % (height * 18);
		if (frameCoordY == IdenticalFrameY)
		{
			ChangeState(i, j, hasElevator ? 1 : 0);
		}
	}

	public int GetMultiTileHeight(int i, int j)
	{
		int count = 1;
		int startY = j;
		var check_Tile = TileUtils.SafeGetTile(i, j);
		for (int y = 1; y < j - 1; y++)
		{
			var tile = TileUtils.SafeGetTile(i, j - y);
			if (!tile.HasTile || tile.TileType != Type || tile.TileFrameY != check_Tile.TileFrameY - 18 * y)
			{
				startY = j - y + 1;
				break;
			}
		}
		var start_Tile = TileUtils.SafeGetTile(i, startY);
		for (int y = 1; y < Main.maxTilesY - j - 1; y++)
		{
			var tile = TileUtils.SafeGetTile(i, startY + y);
			if (!tile.HasTile || tile.TileType != Type || tile.TileFrameY != start_Tile.TileFrameY + 18 * y)
			{
				break;
			}
			count = y + 1;
		}
		return count;
	}

	/// <summary>
	/// Switch the tile's state. The tile will only call this method when the elevator is nearby and the tile is at the IdenticalFrameY frameY.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="state">0 for off, 1 for on.</param>
	public virtual void ChangeState(int i, int j, int state)
	{
		int height = GetMultiTileHeight(i, j);
		int frameCoordY = IdenticalFrameY % (height * 18);
		int y0 = j - frameCoordY / 18;
		for (int y = 0; y < height; y++)
		{
			var tile = TileUtils.SafeGetTile(i, y0 + y);
			tile.TileFrameY = (short)((state * height + y) * 18);
		}
	}

	public virtual bool ElevatorNearBy(int i, int j)
	{
		var tile = Main.tile[i, j];
		int maxFrameY = GetMultiTileHeight(i, j) * 18;
		foreach (var entity in ColliderManager.Instance.OfType<Elevator>())
		{
			Vector2 center = entity.Box.Center;
			if (Math.Abs(center.Y - j * 16f - 8) < 64f && tile.TileFrameY % maxFrameY == IdenticalFrameY && Math.Abs(center.X - i * 16f - 8) < entity.Size.X / 2f + 18f)
			{
				return true;
			}
		}
		return false;
	}
}