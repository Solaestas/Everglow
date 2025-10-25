using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

[Pipeline(typeof(WCSPipeline))]
public class TwilightCastle_RoomScene_Background : BackgroundVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;

	public Point Offset = new Point(0, 0);

	public override void OnSpawn()
	{
	}

	public override void Draw()
	{
		List<Vertex2D> bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardRightBottom(originTile.X, originTile.Y, texture, bars);
		Ins.Batch.Draw(texture, bars, PrimitiveType.TriangleList);
	}
}