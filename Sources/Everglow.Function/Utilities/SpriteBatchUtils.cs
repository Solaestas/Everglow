using Everglow.Commons.Vertex;
using MathNet.Numerics.Distributions;
using Spine;

namespace Everglow.Commons.Utilities;

public static class SpriteBatchUtils
{
	public static List<Vertex2D> DrawCurveStrip_EnvironmentLight(List<Vector2> curve, float width, float coord_x_min, float coord_x_max, float coord_y_min = 0, float coord_y_max = 1, bool curveHasScreenPos = false)
	{
		if(curve.Count < 2)
		{
			return [];
		}
		Vector2 lightSamplingOffset = Vector2.zeroVector;
		if(!curveHasScreenPos)
		{
			lightSamplingOffset = Main.screenPosition;
		}
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i < curve.Count; i++)
		{
			Vector2 pos = curve[i];
			Vector2 dir;
			if(i == 0)
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

	public static void AddVertexWithEnv_Light(List<Vertex2D> bars, Vector2 worldPos, Vector3 texCoord, bool removeScreenPos = true)
	{
		Vector2 offset = Vector2.Zero;
		if(removeScreenPos)
		{
			offset = -Main.screenPosition;
		}
		bars.Add(worldPos + offset, GetEnv_Light(worldPos + Main.screenPosition + offset), texCoord);
	}
}