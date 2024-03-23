using Everglow.Commons.VFX.Scene;
using SubworldLibrary;

namespace Everglow.Yggdrasil.Common;
[Pipeline(typeof(WCSPipeline))]
public class StoneBridge_fence : BackgroundVFX
{
	public override void OnSpawn()
	{
		texture = ModAsset.StoneBridge_fence.Value;
	}
	public override void Update()
	{
		if (!SubworldSystem.IsActive<YggdrasilWorld>())
		{
			Active = false;
		}
		base.Update();
	}
	public override void Draw()
	{
		base.Draw();
	}
}
