using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

[Pipeline(typeof(WCSPipeline))]
public class BloodChurch_Scene_Fountain : BackgroundVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;

	public override void OnSpawn()
	{
		texture = ModAsset.BloodChurch_Scene_1_Fountain.Value;
	}

	public override void Draw()
	{
		List<Vertex2D> bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardRightBottom(originTile.X + 17, originTile.Y + 10, texture, bars);
		Ins.Batch.Draw(texture, bars, PrimitiveType.TriangleList);
	}

	public void DrawWaterFlow()
	{
		Texture2D waterStyle = Commons.ModAsset.Noise_flame_2.Value;
		List<Vertex2D> bars = new List<Vertex2D>();
	}
}