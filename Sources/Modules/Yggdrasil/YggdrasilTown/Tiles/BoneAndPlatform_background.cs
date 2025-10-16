using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

[Pipeline(typeof(WCSPipeline))]
public class BoneAndPlatform_background : BackgroundVFX
{
	public override void OnSpawn()
	{
		texture = ModAsset.BoneAndPlatform_background.Value;
	}
}