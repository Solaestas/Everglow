using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

[Pipeline(typeof(WCSPipeline))]
public class StoneCave_Scene_Far : BackgroundVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;

	public override void OnSpawn()
	{
		texture = ModAsset.StoneCave_Scene_Far.Value;
	}

	public override void Draw()
	{
		List<Vertex2D> bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardRightBottom(originTile.X, originTile.Y, texture, bars);
		Ins.Batch.Draw(texture, bars, PrimitiveType.TriangleList);
	}
}