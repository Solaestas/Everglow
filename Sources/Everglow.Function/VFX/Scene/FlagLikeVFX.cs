using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.VFX.Scene;

/// <summary>
/// Texture must sized in any times of 16, such as 32x48, 16x16, 48x64...
/// </summary>
[Pipeline(typeof(WCSPipeline))]
public class FlagLikeVFX : TileVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawPlayers;

	public Vector2[,] Meshgrid;

	public Vector2 AnchorOffset = Vector2.zeroVector;

	public void GenerateMeshgrid()
	{
		int width = Texture.Width / 16;
		int addWidth = 0;
		if (Texture.Width % 16 > 1)
		{
			addWidth = 2;
		}
		if (Texture.Width % 16 == 1)
		{
			addWidth = 1;
		}
		width += addWidth;
		int height = Texture.Height / 16;
		if (Texture.Height % 16 > 0)
		{
			height += 1;
		}
		Meshgrid = new Vector2[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Meshgrid[x, y] = new Vector2(x, y) * 16 + OriginTilePos.ToWorldCoordinates() + AnchorOffset;
			}
		}
	}

	public override void Update()
	{
		if (Meshgrid == null || Meshgrid == default)
		{
			GenerateMeshgrid();
		}
		for (int x = 0; x < Meshgrid.GetLength(0) - 1; x++)
		{
			for (int y = 0; y < Meshgrid.GetLength(1) - 1; y++)
			{
				Vector2 origPos = new Vector2(x, y) * 16 + OriginTilePos.ToWorldCoordinates() + AnchorOffset;
				Meshgrid[x, y] += GetWindPush(Meshgrid[x, y].X, Meshgrid[x, y].Y);
				Meshgrid[x, y] += (origPos - Meshgrid[x, y]) * 0.2f;
			}
		}
		base.Update();
	}

	public override void Draw()
	{
		if (Meshgrid == null || Meshgrid == default)
		{
			GenerateMeshgrid();
		}
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int x = 0; x < Meshgrid.GetLength(0) - 1; x++)
		{
			for (int y = 0; y < Meshgrid.GetLength(1) - 1; y++)
			{
				AddVertex(bars, x, y);
				AddVertex(bars, x + 1, y);
				AddVertex(bars, x, y + 1);

				AddVertex(bars, x, y + 1);
				AddVertex(bars, x + 1, y);
				AddVertex(bars, x + 1, y + 1);
			}
		}
		Ins.Batch.Draw(Texture, bars, PrimitiveType.TriangleList);
	}

	public void AddVertex(List<Vertex2D> bars, int xIndex, int yIndex)
	{
		Vector2 position = Meshgrid[xIndex, yIndex];
		Vector2 texCoord = new Vector2(xIndex, yIndex) / Texture.Size();
		bars.Add(position, Lighting.GetColor(position.ToTileCoordinates()), new Vector3(texCoord, 0));
	}

	public static Vector2 GetWindPush(Vector2 pos)
	{
		return GetWindPush(pos.X, pos.Y);
	}

	public static Vector2 GetWindPush(float x, float y)
	{
		float noise = 0;
		float timeValue = (float)Main.time * 0.08f;
		for (int i = 0; i < 8; i++)
		{
			noise += MathF.Sin(timeValue * MathF.Pow(2, i) + (x + i)) * MathF.Pow(2, -i);
		}
		for (int j = 0; j < 8; j++)
		{
			noise += MathF.Sin(timeValue * MathF.Pow(2, j) + (y + j)) * MathF.Pow(2, -j);
		}

		Vector2 addVel = Vector2.zeroVector;
		foreach (var player in Main.player)
		{
			if (player is not null && player.active)
			{
				if (player.Hitbox.Contains(new Point((int)x, (int)y)))
				{
					addVel += player.velocity * 2.2f;
				}
			}
		}

		return new Vector2(noise * 3f * Main.windSpeedCurrent + Main.windSpeedCurrent * 21f, 0) + addVel;
	}
}