using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

[Pipeline(typeof(WCSPipeline))]
public class GiantFurnace_Body : TileVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;

	public override void Update()
	{
		MaxDiatanceOutOfScreen = 1500;
		base.Update();
	}

	public override void OnSpawn()
	{
		Texture = ModAsset.GiantFurnace_Body.Value;
	}

	public override void Draw()
	{
		var bars = new List<Vertex2D>();
		float width = 15;
		float height = 26;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Color lightColor0 = Lighting.GetColor((int)Position.X / 16 + i * 2, (int)Position.Y / 16 + j * 2);
				Color lightColor1 = Lighting.GetColor((int)Position.X / 16 + i * 2 + 2, (int)Position.Y / 16 + j * 2);
				Color lightColor2 = Lighting.GetColor((int)Position.X / 16 + i * 2, (int)Position.Y / 16 + j * 2 + 2);
				Color lightColor3 = Lighting.GetColor((int)Position.X / 16 + i * 2 + 2, (int)Position.Y / 16 + j * 2 + 2);

				bars.Add(Position + new Vector2(i, j) * 32, lightColor0, new Vector3(i / width, j / height, 0));
				bars.Add(Position + new Vector2(i + 1, j) * 32, lightColor1, new Vector3((i + 1) / width, j / height, 0));
				bars.Add(Position + new Vector2(i, j + 1) * 32, lightColor2, new Vector3(i / width, (j + 1) / height, 0));

				bars.Add(Position + new Vector2(i, j + 1) * 32, lightColor2, new Vector3(i / width, (j + 1) / height, 0));
				bars.Add(Position + new Vector2(i + 1, j) * 32, lightColor1, new Vector3((i + 1) / width, j / height, 0));
				bars.Add(Position + new Vector2(i + 1, j + 1) * 32, lightColor3, new Vector3((i + 1) / width, (j + 1) / height, 0));
			}
		}

		if (bars.Count <= 0)
		{
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));

			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(Position, Color.Transparent, new Vector3(0, 0, 0));
		}
		Ins.Batch.Draw(Texture, bars, PrimitiveType.TriangleList);
	}
}