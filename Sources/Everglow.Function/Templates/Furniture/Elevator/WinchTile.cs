namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class WinchTile : ModTile
{
	// Add Elevator.
	public override void NearbyEffects(int i, int j, bool closer)
	{
		// Tile tile = Main.tile[i, j];
		// if (!CheckEmpty(i - 2, j + 1, 3, 3))
		// {
		// return;
		// }
		// if (!CheckEmpty(i - 2, j + 1, 3, 3))
		// {
		// return;
		// }

		// bool hasLift = false;
		// foreach (var boxEntity in ColliderManager.Instance.OfType<YggdrasilElevator>())
		// {
		// if (boxEntity is YggdrasilElevator elevator)
		// {
		// if (elevator.WinchCoord == new Point(i, j))
		// {
		// hasLift = true;
		// break;
		// }
		// }
		// }
		// if (!hasLift)
		// {
		// ColliderManager.Instance.Add(new YggdrasilElevator() { Position = new Vector2(i, j + 15) * 16 - new Vector2(48, 8), WinchCoord = new Point(i, j) });
		// tile.TileFrameY = 0;
		// }
		// if (hasLift)
		// {
		// tile.TileFrameY = 18;
		// }
	}

	public bool CheckEmpty(int x, int y, int width, int height)
	{
		for (int i = x; i < x + width; i++)
		{
			for (int j = y; j < y + height; j++)
			{
				if (Elevator.SafeGetTile(i, j).HasTile)
				{
					return false;
				}
			}
		}
		return true;
	}
}