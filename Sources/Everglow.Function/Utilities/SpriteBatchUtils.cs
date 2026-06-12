using System.Runtime.InteropServices;
using Everglow.Commons.Vertex;
using MathNet.Numerics;

namespace Everglow.Commons.Utilities;

public static class SpriteBatchUtils
{
	public static List<Vertex2D> DrawCurveStrip_EnvironmentLight(List<Vector2> curve, float width, float coord_x_min, float coord_x_max, float coord_y_min = 0, float coord_y_max = 1, bool curveHasScreenPos = false)
	{
		if (curve.Count < 2)
		{
			return [];
		}
		Vector2 lightSamplingOffset = Vector2.zeroVector;
		if (!curveHasScreenPos)
		{
			lightSamplingOffset = Main.screenPosition;
		}
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i < curve.Count; i++)
		{
			Vector2 pos = curve[i];
			Vector2 dir;
			if (i == 0)
			{
				dir = curve[i + 1] - curve[i];
			}
			else
			{
				dir = curve[i] - curve[i - 1];
			}
			dir = dir.NormalizeSafe();
			Vector2 normal = new Vector2(dir.Y, -dir.X) * width / 2f;
			float value = i / (float)(curve.Count - 1);
			float coordX = float.Lerp(coord_x_min, coord_x_max, value);
			AddVertexWithEnv_Light(bars, pos + normal + lightSamplingOffset, new Vector3(coordX, coord_y_min, 0));
			AddVertexWithEnv_Light(bars, pos - normal + lightSamplingOffset, new Vector3(coordX, coord_y_max, 0));
		}
		return bars;
	}

	public static Color GetEnv_Light(Vector2 worldPosition)
	{
		return Lighting.GetColor(worldPosition.ToTileCoordinates());
	}

	public static void AddVertexWithEnv_Light(List<Vertex2D> bars, Vector2 worldPos, Vector3 texCoord, bool removeScreenPos = true, float colorFade = 1f)
	{
		Vector2 offset = Vector2.Zero;
		if (removeScreenPos)
		{
			offset = -Main.screenPosition;
		}
		bars.Add(worldPos + offset, GetEnv_Light(worldPos + Main.screenPosition + offset) * colorFade, texCoord);
	}

	public static void AddVerticesForCircleRing(List<Vertex2D> bars, Vector2 position, float radius, float width, Color drawColor, float coorx_x_min, float coord_x_max, float coord_z = 0)
	{
		int currentSize = bars.Count;
		int count = (int)(radius / 4 + 10);
		CollectionsMarshal.SetCount(bars, count * 2 + 256);
		Span<Vertex2D> span = CollectionsMarshal.AsSpan(bars);
		for (int k = 0; k <= count; k++)
		{
			float value = k / (float)count;
			float coordX = (float)Utils.Lerp(coorx_x_min, coord_x_max, value);
			Vector2 pos0 = position + new Vector2(0, radius + width / 2f).RotatedBy(value * MathHelper.TwoPi);
			Vector2 pos1 = position + new Vector2(0, radius - width / 2f).RotatedBy(value * MathHelper.TwoPi);
			float maxCoordY = 1;
			if (radius < width / 2f)
			{
				pos1 = position;
				maxCoordY = 0.5f + radius / (width / 2f);
			}
			span[currentSize] = new Vertex2D(pos0, drawColor, new Vector3(coordX, 0, coord_z));
			currentSize++;
			span[currentSize] = new Vertex2D(pos1, drawColor, new Vector3(coordX, maxCoordY, coord_z));
			currentSize++;
		}
		CollectionsMarshal.SetCount(bars, currentSize);
	}
}