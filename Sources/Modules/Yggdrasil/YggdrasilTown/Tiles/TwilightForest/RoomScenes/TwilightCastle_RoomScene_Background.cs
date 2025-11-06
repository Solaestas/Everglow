using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

[Pipeline(typeof(WCSPipeline_PointWrap))]
public class TwilightCastle_RoomScene_Background : BackgroundVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;

	public bool FlipHorizontally(int i, int j)
	{
		int leftSolid = 0;
		int rightSolid = 0;
		for (int x = 1; x < 15; x++)
		{
			Tile tileLeft = YggdrasilWorldGeneration.SafeGetTile(i - x, j);
			Tile tileRight = YggdrasilWorldGeneration.SafeGetTile(i + x, j);
			if (tileLeft.HasTile && tileLeft.TileType == ModContent.TileType<GreenRelicBrick>())
			{
				leftSolid++;
			}
			if (tileRight.HasTile && tileRight.TileType == ModContent.TileType<GreenRelicBrick>())
			{
				rightSolid++;
			}
		}
		if (rightSolid > leftSolid)
		{
			return false;
		}
		return true;
	}

	public Point Offset = new Point(0, 0);

	/// <summary>
	/// Allow to make a custon draw for this vfx.
	/// </summary>
	public delegate void CustomDrawVFX(TwilightCastle_RoomScene_Background backgrouond);

	public event CustomDrawVFX CustomDraw;

	public override void OnSpawn()
	{
	}

	public override void Draw()
	{
		CustomDraw?.Invoke(this);
	}

	public override void Kill()
	{
		UnregisterCustomDraw(CustomDraw);
		base.Kill();
	}

	public void RegisterCustomLogic(CustomDrawVFX customDraw)
	{
		CustomDraw += customDraw;
	}

	public void UnregisterCustomDraw(CustomDrawVFX customDraw)
	{
		CustomDraw -= customDraw;
	}
}