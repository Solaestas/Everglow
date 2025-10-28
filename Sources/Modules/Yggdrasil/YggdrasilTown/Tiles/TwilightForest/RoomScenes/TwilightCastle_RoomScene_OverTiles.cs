using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

[Pipeline(typeof(WCSPipeline))]
public class TwilightCastle_RoomScene_OverTiles : BackgroundVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawTiles;

	public Point Offset = new Point(0, 0);

	/// <summary>
	/// Allow to make a custon draw for this vfx.
	/// </summary>
	public delegate void CustomDrawVFX(TwilightCastle_RoomScene_OverTiles overtileDraw);

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
		UnregisterCustomLogic(CustomDraw);
		base.Kill();
	}

	public void RegisterCustomLogic(CustomDrawVFX customDraw)
	{
		CustomDraw += customDraw;
	}

	public void UnregisterCustomLogic(CustomDrawVFX customDraw)
	{
		CustomDraw -= customDraw;
	}
}