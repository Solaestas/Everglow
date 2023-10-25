namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;
[Pipeline(typeof(WCSPipeline))]
public class StoneBridge_fence : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;
	public Vector2 position;
	public StoneBridge_fence() { }
	public override void Update()
	{
	}
	public override void Draw()
	{
		Ins.Batch.Draw(ModAsset.StoneBridge_fence.Value, position, Lighting.GetColor((int)position.X / 16, (int)position.Y / 16));
	}
}
