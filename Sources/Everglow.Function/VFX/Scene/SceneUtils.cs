using System.Collections.Generic;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.VFX.Scene;

public class SceneUtils
{
	/// <summary>
	/// Fill draw vertices that cover a tile.(TriangleList)
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="texture"></param>
	/// <param name="startCoordX"></param>
	/// <param name="startCoordY"></param>
	/// <param name="bars"></param>
	public static void DrawTileCover(int i, int j, Texture2D texture, float startCoordX, float startCoordY, List<Vertex2D> bars)
	{
		Vector2 drawPos0 = new Point(i, j).ToWorldCoordinates() + new Vector2(8);
		Vector2 drawPos1 = drawPos0 + new Vector2(16, 0);
		Vector2 drawPos2 = drawPos0 + new Vector2(0, 16);
		Vector2 drawPos3 = drawPos0 + new Vector2(16, 16);

		Color light0 = Lighting.GetColor(drawPos0.ToTileCoordinates());
		Color light1 = Lighting.GetColor(drawPos1.ToTileCoordinates());
		Color light2 = Lighting.GetColor(drawPos2.ToTileCoordinates());
		Color light3 = Lighting.GetColor(drawPos3.ToTileCoordinates());

		Vector2 size = new Vector2(16) / texture.Size();

		bars.Add(drawPos0, light0, new Vector3(startCoordX, startCoordY, 0));
		bars.Add(drawPos1, light1, new Vector3(startCoordX + size.X, startCoordY, 0));
		bars.Add(drawPos3, light3, new Vector3(startCoordX + size.X, startCoordY + size.Y, 0));

		bars.Add(drawPos0, light0, new Vector3(startCoordX, startCoordY, 0));
		bars.Add(drawPos3, light3, new Vector3(startCoordX + size.X, startCoordY + size.Y, 0));
		bars.Add(drawPos2, light2, new Vector3(startCoordX, startCoordY + size.Y, 0));
	}

	/// <summary>
	/// Make sure the width and the height of the texture are the multiples of 16.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="texture"></param>
	/// <param name="bars"></param>
	public static void DrawMultiSceneTowardRightBottom(int i, int j, Texture2D texture, List<Vertex2D> bars)
	{
		for (int x = 0; x < texture.Width; x += 16)
		{
			for (int y = 0; y < texture.Height; y += 16)
			{
				DrawTileCover(i + x / 16, j + y / 16, texture, x / (float)texture.Width, y / (float)texture.Height, bars);
			}
		}
	}
}