using Everglow.Commons.CustomTiles;

namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class WinchTile<TElevator> : ModTile
	where TElevator : Elevator, new()
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlendAll[Type] = true;
		Main.tileBlockLight[Type] = true;

		AddMapEntry(new Color(112, 75, 75));

		DustType = DustID.Iron;
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		// Skip closer updates.
		if (closer)
		{
			return;
		}

		Point winchTileCoord = new Point(i, j);
		Tile winchTile = Main.tile[winchTileCoord];
		if (ColliderManager.Instance.OfType<TElevator>()
			.Any(r => r.WinchCoord == winchTileCoord))
		{
			winchTile.TileFrameY = 18;
		}
		else
		{
			var newElevator = ColliderManager.Instance.Add<TElevator>(new Vector2(i, j + 15) * 16 - new Vector2(48, 8));
			newElevator.WinchTileType = Type;
			newElevator.WinchCoord = winchTileCoord;
			winchTile.TileFrameY = 0;
		}
	}
}