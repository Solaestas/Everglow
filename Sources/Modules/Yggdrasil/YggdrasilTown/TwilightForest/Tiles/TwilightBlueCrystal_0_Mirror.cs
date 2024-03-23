using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;
[Pipeline(typeof(ScreenReflectionPipeline))]
public class TwilightBlueCrystal_0_Mirror : Tile_MirrorFaceVFX
{
	public override void OnSpawn()
	{
		texture = ModAsset.TwilightBlueCrystal_0_Mirror.Value;
	}
}
