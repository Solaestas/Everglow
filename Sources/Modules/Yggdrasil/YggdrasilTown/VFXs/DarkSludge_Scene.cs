using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline), typeof(BloomPipeline))]
public class DarkSludge_Scene : ForegroundVFX
{
	public float[] ai;
	public int MinX;
	public int MaxX;

	public override CodeLayer DrawLayer => CodeLayer.PostDrawPlayers;

	public override void OnSpawn()
	{
	}

	public override void Update()
	{
		if (Ins.VisualQuality.Low)
		{
			Active = false;
			SceneVFXSystem.TilePointHasScene[(originTile.X, originTile.Y)] = false;
			return;
		}
		if (MaxX == 0)
		{
			MinX = originTile.X;
			MaxX = originTile.X;
			while (MinX > 0)
			{
				if (TileUtils.SafeGetTile(MinX, originTile.Y).TileType == ModContent.TileType<DarkSludge>() && TileUtils.SafeGetTile(MinX, originTile.Y - 1).TileType != ModContent.TileType<DarkSludge>())
				{
					MinX--;
				}
				else
				{
					break;
				}
			}
			while (MaxX < Main.maxTilesX)
			{
				if (TileUtils.SafeGetTile(MaxX, originTile.Y).TileType == ModContent.TileType<DarkSludge>() && TileUtils.SafeGetTile(MaxX, originTile.Y - 1).TileType != ModContent.TileType<DarkSludge>())
				{
					MaxX++;
				}
				else
				{
					break;
				}
			}
		}
	}

	public override void Draw()
	{
		List<Vertex2D> bars = [];
		List<Vertex2D> bars2 = [];
		if (MinX >= MaxX)
		{
			for (int x = 0; x < 4; x++)
			{
				bars.Add(Vector2.zeroVector, Color.Transparent, new Vector3(0, 0, 1));
				bars2.Add(Vector2.zeroVector, Color.Transparent, new Vector3(0, 0, 1));
			}
		}
		int coeDistance = 12;
		for (int i = MinX; i <= MaxX; i += coeDistance)
		{
			Vector2 position = new Point(i, originTile.Y).ToVector2() * 16;
			Color color = Lighting.GetColor(new Point(i, originTile.Y));
			for (int x = 0; x < 4; x++)
			{
				bars.Add(position + new Vector2(0 + x * 4 * coeDistance, NoiseWave(x * 4 * coeDistance + i * 16) * 12), color, new Vector3(0, 0, 1));
				bars.Add(position + new Vector2(4 * coeDistance + x * 4 * coeDistance, NoiseWave(x * 4 * coeDistance + 4 * coeDistance + i * 16) * 12), color, new Vector3(0, 0, 1));

				bars.Add(position + new Vector2(0 + x * 4 * coeDistance, NoiseWave(x * 4 * coeDistance + i * 16) * 12 + 4), color, new Vector3(0, 0.4f, 1));
				bars.Add(position + new Vector2(4 * coeDistance + x * 4 * coeDistance, NoiseWave(x * 4 * coeDistance + 4 * coeDistance + i * 16) * 12 + 4), color, new Vector3(0, 0.4f, 1));
			}

			for (int x = 0; x < 4; x++)
			{
				bars2.Add(position + new Vector2(0 + x * 4 * coeDistance, NoiseWave(x * 4 * coeDistance + i * 16) * 12 + 4), color, new Vector3(0, 0.4f, 1));
				bars2.Add(position + new Vector2(4 * coeDistance + x * 4 * coeDistance, NoiseWave(x * 4 * coeDistance + 4 * coeDistance + i * 16) * 12 + 4), color, new Vector3(0, 0.4f, 1));

				bars2.Add(position + new Vector2(0 + x * 4 * coeDistance, 16), color, new Vector3(0, 1f, 1));
				bars2.Add(position + new Vector2(4 * coeDistance + x * 4 * coeDistance, 16), color, new Vector3(0, 1f, 1));
			}
		}
		Ins.Batch.Draw(ModAsset.DarkSludge_Liquid.Value, bars, PrimitiveType.TriangleStrip);
		Ins.Batch.Draw(ModAsset.DarkSludge_Liquid.Value, bars2, PrimitiveType.TriangleStrip);
	}

	public float NoiseWave(float x)
	{
		x *= 0.012f;
		//x += ;
		float value = 0;

		int octaves = 4;
		float lacunarity = 2f;
		float gain = 0.5f;

		// Initial values
		float amplitude = 0.5f;
		float frequency = 1;

		// Loop of octaves
		for (int i = 0; i < octaves; i++)
		{
			value += amplitude * MathF.Sin(frequency * x + (float)Main.time * 0.04f * MathF.Sin(i * MathHelper.PiOver2));
			frequency *= lacunarity;
			amplitude *= gain;
		}
		return value;
	}
}