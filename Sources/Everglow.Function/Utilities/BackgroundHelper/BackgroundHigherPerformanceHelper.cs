using System.Runtime.CompilerServices;
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
	public static void Add_TileBgVertice_UnsafeLambda(
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
		bars.Capacity = Math.Max(bars.Capacity, tiles.Count * 6 + 256);

		// Lambda closure: Capture all invariants and eliminate parameter passing overhead
		void AddVertex(Vector2 pos)
		{
			Vector2 screenOffset = pos - screenCenter;
			Vector2 uv = screenOffset / texSize + texMoveBase;
			uv.X += 0.5f;
			uv.Y += 0.5f;

			Color drawColor = GetColor(bg, pos);
			Vector3 uv3 = new Vector3(uv.X, uv.Y, 0f);
			Vertex2D vtx = new Vertex2D(pos - screenPos, drawColor, uv3);

			unsafe
			{
				ref Vertex2D[] itemArray = ref ListAccessor.GetItems(bars);
				ref int size = ref ListAccessor.GetSize(bars);
				itemArray[size] = vtx;
				size++;
			}
		}

		void NextRow(Vector2 pos)
		{
			unsafe
			{
				ref Vertex2D[] itemArray = ref ListAccessor.GetItems(bars);
				ref int size = ref ListAccessor.GetSize(bars);
				ref Vertex2D lastRef = ref itemArray[size - 1];
				Vertex2D degenerate = lastRef;
				degenerate.color = Color.Transparent;
				ref Vertex2D degRef0 = ref itemArray[size];
				degRef0 = degenerate;
				degRef0.position += new Vector2(0, -48);
				size++;
				ref Vertex2D degRef1 = ref itemArray[size];
				degRef1 = degenerate;
				size++;
				Vector2 finalPos = pos + new Vector2(24, 24) - Main.screenPosition;
				ref Vertex2D degRef2 = ref itemArray[size];
				degRef2 = degenerate;
				degRef2.position = finalPos + new Vector2(0, -24);
				size++;
				ref Vertex2D degRef3 = ref itemArray[size];
				degRef3.position = finalPos + new Vector2(0, 24);
				size++;
			}
		}

		var visibleTiles = tiles
			.Select(p => new { Pos = p, World = p.ToWorldCoordinates() })
			.Where(t => VFXManager.InScreen(t.World, 64))
			.OrderBy(t => t.Pos.Y)
			.ThenBy(t => t.Pos.X)
			.ToList();

		if (visibleTiles.Count == 0)
		{
			return;
		}

		int lastY = visibleTiles[0].Pos.Y;

		foreach (var tile in visibleTiles)
		{
			Vector2 basePos = tile.World;
			if (tile.Pos.Y != lastY)
			{
				// Degenerated vertex: Reuse the last vertex and transparently connect the up and down directions
				NextRow(basePos);
				lastY = tile.Pos.Y;
			}

			foreach (var offset in _rotOffsets)
			{
				AddVertex(basePos + offset + new Vector2(24, 24));
			}
		}
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