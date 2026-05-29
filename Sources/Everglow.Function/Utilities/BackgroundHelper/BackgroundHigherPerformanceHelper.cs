using System.Runtime.InteropServices;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;

namespace Everglow.Commons.Utilities.BackgroundHelper;

/// <summary>
/// Use unsafe + Lambda closure for extremely high performance rendering.
/// </summary>
public static class BackgroundHigherPerformanceHelper
{
	// Precompute rotation offset (global static, initialized only once)
	private static readonly Vector2[] _rotOffsets =
	[
		new Vector2(0, -24),
		new Vector2(0, 24),
	];

	/// <summary>
	/// Row-major TriangleStrip rendering, closure capture invariants, unsafe pointer vertex writes
	/// </summary>
	public static void Add_TileBgVertice(
		BackgroundSlideBase bg,
		List<Point> tiles,
		List<Vertex2D> bars)
	{
		// Compute the invariant only once per frame and capture it all into the Lambda closure
		Vector2 screenHalf = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		Vector2 screenCenter = Main.screenPosition + screenHalf;
		Vector2 texSize = bg.Texture.Size();
		Vector2 texMoveBase = (screenCenter - bg.WorldAnchor) / bg.Distance / texSize;
		Vector2 screenPos = Main.screenPosition;

		// Expand the List in advance to avoid array overflow during unsafe writing (estimate the number of vertices, can be enlarged as needed)
		int currentSize = bars.Count;
		int estimatedCapacity = bars.Count + tiles.Count * 6 + 256;
		if (bars.Capacity < estimatedCapacity)
		{
			bars.Capacity = estimatedCapacity;
		}
		CollectionsMarshal.SetCount(bars, estimatedCapacity);
		Span<Vertex2D> span = CollectionsMarshal.AsSpan(bars);

		var visibleTiles = tiles
		   .Select(p => new { Pos = p, World = p.ToWorldCoordinates() })
		   .Where(t => VFXManager.InScreen(t.World, 64))
		   .OrderBy(t => t.Pos.Y)
		   .ThenBy(t => t.Pos.X)
		   .ToList();

		if (visibleTiles.Count == 0)
		{
			CollectionsMarshal.SetCount(bars, currentSize);
			return;
		}

		int lastY = visibleTiles[0].Pos.Y;
		int lastX = visibleTiles[0].Pos.X;
		foreach (var tile in visibleTiles)
		{
			Vector2 basePos = tile.World;
			if (tile.Pos.Y != lastY || tile.Pos.X - lastX > 3)
			{
				// Degenerated vertex: Reuse the last vertex and transparently connect the up and down directions
				// Triangle strip, just like:
				// ↓↗↓↗↓↗↓↗↓↗↓↑↓
				// ↑↓↗↓↗↓↗↓↗↓↗↓↗↓
				// or
				// ↓↗↓↗↓↗↓↗↓↗↓↑↓     ↑↓↗↓↗↓↗↓↗↓↗↓↗↓
				Vertex2D degenerate = span[currentSize - 1];
				degenerate.color = Color.Transparent;

				span[currentSize] = degenerate;
				span[currentSize].position += new Vector2(0, -48);
				currentSize++;

				span[currentSize] = degenerate;
				currentSize++;

				Vector2 finalPos = basePos + new Vector2(24, 24) - Main.screenPosition;
				span[currentSize] = degenerate;
				span[currentSize].position = finalPos + new Vector2(0, -24);
				currentSize++;

				span[currentSize] = degenerate;
				span[currentSize].position = finalPos + new Vector2(0, 24);
				currentSize++;

				lastY = tile.Pos.Y;
			}
			lastX = tile.Pos.X;

			// Add vertex.
			foreach (var offset in _rotOffsets)
			{
				Vector2 pos = basePos + offset + new Vector2(24, 24);

				Vector2 screenOffset = pos - screenCenter;
				Vector2 uv = screenOffset / texSize + texMoveBase;
				uv.X += 0.5f;
				uv.Y += 0.5f;

				Color drawColor = GetColor(bg, pos);
				Vector3 uv3 = new Vector3(uv.X, uv.Y, 0f);
				Vertex2D vtx = new Vertex2D(pos - screenPos, drawColor, uv3);

				span[currentSize] = vtx;
				currentSize++;
			}
		}

		// Update size.
		CollectionsMarshal.SetCount(bars, currentSize);
	}

	private static Color GetColor(BackgroundSlideBase bg, Vector2 worldPos)
	{
		Color c = Color.White;
		switch (bg.UseColorStyle)
		{
			case 0:
				c = Main.ColorOfTheSkies;
				break;
			case 1:
				c = Lighting.GetColor((worldPos + new Vector2(-8)).ToTileCoordinates());
				break;
		}
		return c * bg.Alpha;
	}
}