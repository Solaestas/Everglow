using Everglow.Commons.VFX.Scene;
using SubworldLibrary;

namespace Everglow.Yggdrasil.Common;
[Pipeline(typeof(WCSPipeline))]
public class BoneAndPlatform_foreground : ForegroundVFX
{
	public override void OnSpawn()
	{
		texture = ModAsset.BoneAndPlatform_foreground.Value;
	}
}
