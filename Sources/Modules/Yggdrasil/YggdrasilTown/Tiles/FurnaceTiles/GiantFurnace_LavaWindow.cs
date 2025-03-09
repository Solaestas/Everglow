using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

[Pipeline(typeof(GiantFurnace_LavaWindowPipeline), typeof(BloomPipeline))]
public class GiantFurnace_LavaWindow : BackgroundVFX
{
	public override void Update()
	{
		base.Update();
	}

	public override void OnSpawn()
	{
		texture = ModAsset.GiantFurnace_LavaWindow.Value;
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(texture);
		var bars = new List<Vertex2D>();
		Color drawColor = new Color(1f, 0.7f, 0.4f, 1f);
		bars.Add(position + new Vector2(0, 0), drawColor, new Vector3(0, 0, 0));
		bars.Add(position + new Vector2(texture.Width, 0), drawColor, new Vector3(1, 0, 0));
		bars.Add(position + new Vector2(0, texture.Height), drawColor, new Vector3(0, 1, 0));
		bars.Add(position + new Vector2(texture.Width, texture.Height), drawColor, new Vector3(1, 1, 0));

		if (bars.Count <= 0)
		{
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));

			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}