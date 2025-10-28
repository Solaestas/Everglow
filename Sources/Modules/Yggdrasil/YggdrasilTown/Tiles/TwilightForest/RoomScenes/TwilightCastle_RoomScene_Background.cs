using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.IRProbe;
using static Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.IRProbe.IRProbe_Normal_Laser;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

[Pipeline(typeof(WCSPipeline))]
public class TwilightCastle_RoomScene_Background : BackgroundVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;

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