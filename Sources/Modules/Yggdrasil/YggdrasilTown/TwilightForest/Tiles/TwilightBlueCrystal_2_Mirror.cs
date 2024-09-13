using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;
[Pipeline(typeof(ScreenReflectionPipeline))]
public class TwilightBlueCrystal_2_Mirror : Tile_MirrorFaceVFX
{
	public bool FlipH = false;
	public override void OnSpawn()
	{
		DepthZ = -15;
	}
}

